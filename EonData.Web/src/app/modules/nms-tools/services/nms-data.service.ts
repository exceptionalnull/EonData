import { Injectable } from '@angular/core';
import { Observable, map, of } from 'rxjs'

import nmsItems from '../data/items.json'
import nmsRecipes from '../data/recipes.json';

import { Item } from '../models/item.interface'
import { Recipe } from '../models/recipe.interface'

@Injectable({
  providedIn: 'root'
})
export class NmsDataService {
  private readonly items: Item[] = nmsItems;
  private readonly recipes: Recipe[] = nmsRecipes;

  static readonly UNKNOWN_ITEM: Item = {
    itemId: 0,
    icon: "unk",
    itemName: "Unknown",
    category: "Unknown",
    rarity: "Unknown",
    abbrev: "Unk",
    isCraftable: false,
    price: 0
  };

  static readonly PLACEHOLDER_ICON: string = "plac";

  constructor() { }

  getItems(): Observable<Item[]> {
    return of(this.items);
  }

  getItemById(itemId: number): Observable<Item | null> {
    return this.getItems().pipe(map(itms => itms.find(itm => itm.itemId == itemId) ?? null));
  }

  getCraftableItems(): Observable<Item[] | null> {
    return this.getItems().pipe(map(itms => itms.filter(itm => itm.isCraftable) ?? null));
  }

  getRecipes(): Observable<Recipe[]> {
    return of(this.recipes);
  }

  getRecipeById(recipeId: number): Observable<Recipe | null> {
    return this.getRecipes().pipe(map(rcps => rcps.find(rcp => rcp.recipeId == recipeId) ?? null));
  }

  getRecipesByItem(itemId: number | undefined): Observable<Recipe[] | null> {
    return this.getRecipes().pipe(map(rcps => rcps.filter(rcp => rcp.createsItemId == itemId) ?? null));
  }
}
