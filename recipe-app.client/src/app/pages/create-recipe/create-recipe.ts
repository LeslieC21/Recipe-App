import { Component, signal } from '@angular/core';
import { form, required, FormField } from '@angular/forms/signals';

@Component({
  selector: 'app-create-recipe',
  imports: [FormField],
  templateUrl: './create-recipe.html',
  styleUrl: './create-recipe.css',
})
export class CreateRecipe {
  // Form and Model
  recipeModel = signal({
    recipeName: '',
    recipeTotalSteps: 0,
    recipeSteps: [],
    recipeTotalTags: 0,
    recipeTags: [],
    recipeTotalIngredients: 0,
    recipeIngredients: []
  })

  recipeForm = form(this.recipeModel, (schemaPath) => {
    required(schemaPath.recipeName);
    required(schemaPath.recipeSteps);
    required(schemaPath.recipeTags);
    required(schemaPath.recipeIngredients);
  })

  addSteps() {
    this.recipeModel().recipeSteps.length = this.recipeModel().recipeTotalSteps;
    this.recipeModel().recipeTags.length = this.recipeModel().recipeTotalTags;
    this.recipeModel().recipeIngredients.length = this.recipeModel().recipeTotalIngredients;
  }

  submitRecipeForm(event: Event) {
    event.preventDefault();

    if (this.recipeForm().invalid())
      return;
  }
}
