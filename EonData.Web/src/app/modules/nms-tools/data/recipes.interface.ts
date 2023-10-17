interface RecipeSource {
  itemId: number;
  quantity: number;
}

export interface Recipe {
  recipeId: number;
  recipeName: string;
  createsItemId: number;
  sources: RecipeSource[];
  isRefined: boolean;
  duration: number;
}
