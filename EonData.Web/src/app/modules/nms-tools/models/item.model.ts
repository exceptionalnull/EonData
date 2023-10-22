import { Item } from "./item.interface";

export class ItemModel implements Item {
  itemId: number = 0;
  itemName: string = "Unknown";
  abbrev: string = "Unk";
  category: string = "Unknown";
  rarity: string = "Unknown";
  price: number = 0;
  isCraftable: boolean | null = false;
  icon: string = "unk";

  constructor(init?: Partial<ItemModel>) {
    if (init != undefined && init != null) {
      // assign initial property values
      Object.assign(this, init);

      // set placeholder icon if one isn't set on the actual object
      if (this.icon == "" && this.itemId != 0) {
        this.icon = "plac";
      }
    }
  }
}
