import { Component, Input, OnInit } from '@angular/core';
import { Item } from '../../models/item.interface';
import { NmsDataService } from '../../services/nms-data.service';
import { Recipe } from '../../models/recipe.interface';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

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
    this.itemInfo = this.dataService.getItem(this.itemId);
    this.recipes = this.dataService.getRecipesByItem(this.itemId);

    if (this.recipes !== undefined) {
      this.selectedRecipe = this.recipes[0].recipeId;
      this.isIntermediate = true;
    }
  }

  pickedRecipe() {

  }
}
