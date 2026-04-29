import { IngredientInfoModel } from './IngredientInfoModel';

export interface CreateRecipeModel {
  Name: string,
  Image: string,
  Instructions: string,
  Tags: string[],
  Ingredients: IngredientInfoModel[]
}
