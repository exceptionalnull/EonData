import { Component } from '@angular/core';
import { NmsDataService } from '../../services/nms-data.service';
import { Item } from '../../models/item.interface';


@Component({
  selector: 'nms-recipe-wizard',
  templateUrl: './recipe-wizard.component.html',
  styleUrls: ['./recipe-wizard.component.scss']
})
export class RecipeWizardComponent {
  selectedItem: number = 0;
  selectedItems: number[] = [];
  items: Item[] = [];
  
  constructor(private dataService: NmsDataService) {
    this.items = dataService.items;
  }

  pickedItem() {
    if (!this.selectedItems.includes(this.selectedItem)) {
      this.selectedItems.push(this.selectedItem);
    }
  }
}
