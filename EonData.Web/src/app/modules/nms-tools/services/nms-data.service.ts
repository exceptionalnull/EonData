import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs'

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

  getItem(itemId: number): Observable<Item | undefined> {
    return of(this.items.find(itm => itm.itemId == itemId));
  }

  getRecipe(recipeId: number): Observable<Recipe | undefined> {
    return of(this.recipes.find(rcp => rcp.recipeId == recipeId));
  }

  getRecipesByItem(itemId: number | undefined): Observable<Recipe[]> {
    let result: Recipe[] = [];
    if (itemId != undefined) {
      result = this.recipes.filter(rcp => rcp.createsItemId == itemId);
    }
    return of(result);
  }
}
