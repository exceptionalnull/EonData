import { Component } from '@angular/core';

import nmsItems from '../../data/items.json'
import nmsRecipes from '../../data/recipes.json';


@Component({
  selector: 'app-recipe-wizard',
  templateUrl: './recipe-wizard.component.html',
  styleUrls: ['./recipe-wizard.component.scss']
})
export class RecipeWizardComponent {
  selectedItem = 0;
  items = nmsItems;
  recipes = nmsRecipes;

  pickedItem(itemId: number) {
  }
}
