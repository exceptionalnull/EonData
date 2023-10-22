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
  itemInfo?: Item;
  recipes: Recipe[] | null = null;
  selectedRecipe: number = 0;
  isIntermediate: boolean = false;
  ingredients: Observable<number[]> | undefined;

  constructor(private dataService: NmsDataService) { }

  ngOnInit() {
    forkJoin({
      item: this.dataService.getItemById(this.itemId),
      recipes: this.dataService.getRecipesByItem(this.itemId)
    }).subscribe(({ item, recipes }) => {
      this.itemInfo = item ?? NmsDataService.UNKNOWN_ITEM;
      this.recipes = recipes;

      if (this.recipes != null && this.recipes.length > 0) {
        this.selectedRecipe = this.recipes[0].recipeId;
        this.isIntermediate = true;
        this.pickedRecipe();
      }
    });
  }

  isLoadingData(): boolean {
    if (this.itemInfo == undefined || this.itemInfo == null || this.recipes == undefined) {
      return true;
    }
    else {
      return false;
    }
  }

  getIngredientIds(): Observable<number[]> {
    return of(this.recipes?.find(rcp => rcp.recipeId == this.selectedRecipe)?.sources.map(rsrc => rsrc.itemId) ?? []);
  }

  pickedRecipe() {
    this.ingredients = this.getIngredientIds();
  }
}
