import { Injectable } from '@angular/core';

import nmsItems from '../data/items.json'
import nmsRecipes from '../data/recipes.json';

import { Item } from '../models/item.interface'
import { Recipe } from '../models/recipe.interface'

@Injectable({
  providedIn: 'root'
})
export class NmsDataService {
  items: Item[] = nmsItems;
  recipes: Recipe[] = nmsRecipes;
  private itemCache: Item[] = [];
  private recipeCache: Recipe[] = [];

  constructor() { }

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

  getRecipe(recipeId: number) {

  }

  getRecipesByItem(itemId: number | undefined): Recipe[] | undefined {
    if (itemId != undefined) {
      return this.recipes.filter(rcp => rcp.createsItemId == itemId);
    }
    return undefined;
  }
}
