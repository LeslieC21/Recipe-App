import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { UnitModel } from '../models/UnitModel';
import { IngredientModel } from '../models/IngredientModel';
import { TagModel } from '../models/TagModel';
import { RecipeModel } from '../models/RecipeModel';
import { CreateTagRequest } from '../models/RequestModels/CreateTagRequest';
import { CreateIngredientRequest } from '../models/RequestModels/CreateIngredientRequest';
import { GetRecipeByFiltersRequest } from '../models/RequestModels/GetRecipeByFiltersRequest';
import { AddDeleteFavoriteRecipe } from '../models/RequestModels/AddDeleteFavoriteRecipe';
 

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

  getUserFavoriteRecipes() {
    return this.httpClient.get<RecipeModel[]>(`/Recipe/Find/FavoriteRecipes`);
  }

  getRecipeByName(name: string) {
    return this.httpClient.get<RecipeModel[]>(`/Recipe/Find/RecipeName/${name}`);
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
  getRecipesByFilter(tagsFilter: string[], ingredientsFilter: string[]) {
    const getReq: GetRecipeByFiltersRequest = ({
      ingredients: ingredientsFilter,
      tags: tagsFilter
    })

    console.log(getReq);

    return this.httpClient.post<RecipeModel[]>('/Recipe/Find/Recipe/Filters', getReq)
  }

  newRecipeTag(name: string) {
    const addReq: CreateTagRequest = ({
      Name: name,
      Type: this.TAGTYPE_RECIPE
    });

    return this.httpClient.post('/Recipe/New/Tag', addReq);
  }

  newIngredientTag(name: string) {
    const addReq: CreateTagRequest = ({
      Name: name,
      Type: this.TAGTYPE_INGREDIENT
    });

    return this.httpClient.post('/Recipe/New/Tag', addReq);
  }

  // Post Method to create a new ingredient
  newIngredient(addReq: CreateIngredientRequest) {
    return this.httpClient.post('/Recipe/New/Ingredient', addReq);
  }

  // Post Method to create a new recipe
  newRecipe(addReq: FormData) {
    return this.httpClient.post('/Recipe/New/Recipe', addReq);
  }

  // Post Method to add a favorite Recipe
  addToFavorites(recipeId: string) {
    const addReq: AddDeleteFavoriteRecipe = ({
      recipeId: recipeId
    })

    return this.httpClient.post('/Recipe/New/FavoriteRecipe', addReq);
  }

  // Delete Method to remove a favorite recipe
  deleteFromFavorites(recipeId: string) {
    const deleteReq: AddDeleteFavoriteRecipe = ({
      recipeId: recipeId
    });

    return this.httpClient.delete(`/Recipe/Delete/FavoriteRecipe/${recipeId}`);
  }
}
