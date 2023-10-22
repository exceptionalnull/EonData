import { RecipeSource } from "./recipe-source.interface";

export interface Recipe {
  recipeId: number;
  recipeName: string;
  createsItemId: number;
  sources: RecipeSource[];
  isRefined: boolean;
  duration: number;
  createsQuantity: number;
}
