import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { UnitModel } from '../models/UnitModel';
import { IngredientModel } from '../models/IngredientModel';
import { TagModel } from '../models/TagModel';
import { RecipeModel } from '../models/RecipeModel';


@Injectable({
  providedIn: 'root'
})

export class RecipeService {
  // HttpClient Inject
  private httpClient = inject(HttpClient);

  // Constants
  private TAGTYPE_NOTYPE = 0;
  private TAGTYPE_RECIPE = 1;
  private TAGTYPE_INGREDIENT = 2;

  // -- GETS --
  // Get Method to return ALL recipes
  getRecipes() {
    return this.httpClient.get<RecipeModel[]>('/Recipe/Find/AllRecipes');
  }

  getUnits() {
    return this.httpClient.get<UnitModel[]>('/Recipe/Find/Units');
  }

  getIngredients() {
    return this.httpClient.get<IngredientModel[]>('/Recipe/Find/AllIngredients');
  }

  getTags() {
    return this.httpClient.get<TagModel[]>('/Recipe/Find/AllTags');
  }

  getTagsByRecipeType() {
    return this.httpClient.get<TagModel[]>(`/Recipe/Find/TagsType/${this.TAGTYPE_RECIPE}`);
  }

  getTagsByIngredientType() {
    return this.httpClient.get<TagModel[]>(`/Recipe/Find/TagsType/${this.TAGTYPE_INGREDIENT}`);
  }

  // -- POSTS --
  // Post Method to create a new Tag
  newTag(addReq: string) {
    return this.httpClient.post('/Recipe/New/Tag', addReq);
  }

  // Post Method to create a new ingredient
  newIngredient(addReq: string) {
    return this.httpClient.post('Recipe/New/Ingredient', addReq);
  }

  // Post Method to create a new recipe
  newRecipe(addReq: FormData) {
    return this.httpClient.post('Recipe/New/Recipe', addReq);
  }
}
