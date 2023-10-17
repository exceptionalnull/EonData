import { Component } from '@angular/core';

import nmsItems from '../../data/items.json'
import { Item } from '../../data/items.interface'

import nmsRecipes from '../../data/recipes.json';
import { Recipe } from '../../data/recipes.interface'

@Component({
  selector: 'app-recipe-wizard',
  templateUrl: './recipe-wizard.component.html',
  styleUrls: ['./recipe-wizard.component.scss']
})
export class RecipeWizardComponent {
  selectedItem: number = 0;
  selectedItems: number[] = [];
  items: Item[] = nmsItems;
  recipes: Recipe[] = nmsRecipes;
  itemCache: Item[] = [];

  getItem(itemId: number): Item | undefined {
    if (itemId in this.itemCache) {
      return this.itemCache[itemId];
    }
    else {
      const result = this.items.find(itm => itm.itemId == itemId);
      if (result != undefined) {
        this.itemCache[itemId] = result;
        return result;
      }
    }
    return undefined;
  }

  *getItemDetails() {
    for (const itemId of this.selectedItems) {
      yield this.getItem(itemId);
    }
  }

  pickedItem(itemId: number) {
    if (!this.selectedItems.includes(itemId)) {
      this.selectedItems.push(itemId);
    }
  }
}
