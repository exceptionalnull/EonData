import { Component, Input, OnInit } from '@angular/core';
import { Item } from '../../models/item.interface';
import { NmsDataService } from '../../services/nms-data.service';
import { Recipe } from '../../models/recipe.interface';
import { Observable, forkJoin, of } from 'rxjs';

@Component({
  selector: 'nms-recipe-node',
  templateUrl: './recipe-node.component.html',
  styleUrls: ['./recipe-node.component.scss']
})
export class RecipeNodeComponent implements OnInit {
  @Input() itemId: number = 0;
  itemInfo: Item | undefined;
  recipes: Recipe[] | undefined;
  selectedRecipe: number = 0;
  isIntermediate: boolean = false;
  ingredients: Observable<number[]> | undefined;

  constructor(private dataService: NmsDataService) { }

  ngOnInit() {
    forkJoin({
      item: this.dataService.getItem(this.itemId),
      recipes: this.dataService.getRecipesByItem(this.itemId)
    }).subscribe(({ item, recipes }) => {
      this.itemInfo = item;
      this.recipes = recipes;

      if ((this.recipes?.length ?? 0) > 0) {
        this.selectedRecipe = this.recipes[0].recipeId;
        this.isIntermediate = true;
        this.pickedRecipe();
      }
    });
  }

  getIngredientIds(): Observable<number[]> {
    return of(this.recipes?.find(rcp => rcp.recipeId == this.selectedRecipe)?.sources.map(rsrc => rsrc.itemId) ?? []);
  }

  pickedRecipe() {
    this.ingredients = this.getIngredientIds();
  }
}
