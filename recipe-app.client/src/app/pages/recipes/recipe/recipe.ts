import { Component, Input, signal } from '@angular/core';

import { RecipeModel } from '../../../core/models/RecipeModel';

@Component({
  selector: 'app-recipe',
  imports: [],
  templateUrl: './recipe.html',
  styleUrl: './recipe.css',
})
export class Recipe {
  @Input({ required: true }) recipe!: RecipeModel;
  showRecipeDetails = signal<Boolean>(false);
  isFavorite = signal<Boolean>(false);

  toggleRecipeDetails() {
    this.showRecipeDetails.update(v => !v);
  }

  toggleFavoriteRecipe() {
    this.isFavorite.update(f => !f);
    console.log(this.isFavorite());
  }

  // LOOK AT s3 for saving images instead of uploading them to the db
}
