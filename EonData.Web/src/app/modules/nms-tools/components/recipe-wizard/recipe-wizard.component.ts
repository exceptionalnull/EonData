import { Component, OnInit } from '@angular/core';
import { NmsDataService } from '../../services/nms-data.service';
import { Item } from '../../models/item.interface';
import { Observable } from 'rxjs';


@Component({
  selector: 'nms-recipe-wizard',
  templateUrl: './recipe-wizard.component.html',
  styleUrls: ['./recipe-wizard.component.scss']
})
export class RecipeWizardComponent implements OnInit {
  selectedItem: number = 0;
  selectedItems: number[] = [];
  items$?: Observable<Item[] | null>;
  
  constructor(private dataService: NmsDataService) { }

  ngOnInit() {
    this.items$ = this.dataService.getCraftableItems();
  }

  pickedItem() {
    if (!this.selectedItems.includes(this.selectedItem)) {
      this.selectedItems.push(this.selectedItem);
    }
  }
}
