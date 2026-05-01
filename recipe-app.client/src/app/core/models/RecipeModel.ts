import { RecipeIngredientResponse } from './RecipeIngredientModel'

export interface RecipeModel {
  recipeId: string,
  image?: Blob | string | undefined,
  name: string,
  tags: string[],
  instructions: string,
  ingredients: RecipeIngredientResponse[]
}
