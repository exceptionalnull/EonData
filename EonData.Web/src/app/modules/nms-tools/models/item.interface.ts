export interface Item {
  itemId: number;
  itemName: string;
  abbrev: string;
  category: string;
  rarity: string;
  price: number;
  isCraftable?: boolean;
  icon: string;
}
