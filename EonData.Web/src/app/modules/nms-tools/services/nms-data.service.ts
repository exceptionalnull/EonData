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

  constructor() { }

  getItem(itemId: number): Item | undefined {
    return this.items.find(itm => itm.itemId == itemId);
  }

  getRecipe(recipeId: number) {
    this.recipes.find(rcp => rcp.recipeId == recipeId);
  }

  getRecipesByItem(itemId: number | undefined): Recipe[] | undefined {
    if (itemId != undefined) {
      return this.recipes.filter(rcp => rcp.createsItemId == itemId);
    }
    return undefined;
  }
}
