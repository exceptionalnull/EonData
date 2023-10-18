import { Component, Input } from '@angular/core';
import { Item } from '../../models/item.interface';
import { NmsDataService } from '../../services/nms-data.service';
import { Recipe } from '../../models/recipe.interface';

@Component({
  selector: 'nms-recipe-node',
  templateUrl: './recipe-node.component.html',
  styleUrls: ['./recipe-node.component.scss']
})
export class RecipeNodeComponent {
  @Input() itemId: number = 0;
  itemInfo: Item | undefined;
  recipes: Recipe[] | undefined;
  selectedRecipe: number = 0;

  constructor(private dataService: NmsDataService) {
    this.itemInfo = dataService.getItem(this.itemId);
    this.recipes = dataService.getRecipesByItem(this.itemId);
  }

  pickedRecipe() {

  }
}
