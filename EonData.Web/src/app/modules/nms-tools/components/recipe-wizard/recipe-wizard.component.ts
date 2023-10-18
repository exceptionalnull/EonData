import { Component } from '@angular/core';
import { NmsDataService } from '../../services/nms-data.service';


@Component({
  selector: 'app-recipe-wizard',
  templateUrl: './recipe-wizard.component.html',
  styleUrls: ['./recipe-wizard.component.scss']
})
export class RecipeWizardComponent {
  selectedItem: number = 0;
  selectedItems: number[] = [];
  selectedRecipe: number = 0;
  selectedRecipes: number[] = [];
  nmsData: NmsDataService;
  
  constructor(private dataService: NmsDataService) {
    this.nmsData = dataService;
  }

  *getItemDetails() {
    for (const itemId of this.selectedItems) {
      yield this.nmsData.getItem(itemId);
    }
  }

  pickedItem() {
    if (!this.selectedItems.includes(this.selectedItem)) {
      this.selectedItems.push(this.selectedItem);
    }
  }

  pickedRecipe(itemId: number | undefined, recipeId: number) {
    if (itemId != undefined) {
      this.selectedRecipes[itemId] = recipeId;
    }
  }
}
