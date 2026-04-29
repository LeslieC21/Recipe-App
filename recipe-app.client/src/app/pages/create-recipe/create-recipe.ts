import { Component, signal } from '@angular/core';
import { form, required, FormField } from '@angular/forms/signals';

import { IngredientInfoModel } from '../../core/models/IngredientInfoModel';
import { CreateRecipeModel } from '../../core/models/CreateRecipeModel';

interface RecipeModel {
  recipeName: string;
  recipeImageUrl: string;
  recipeTotalSteps: number;
  recipeSteps: string[];
  recipeTotalTags: number;
  recipeTags: string[];
  recipeTotalIngredients: number;
  recipeIngredients: IngredientInfoModel[];
}

@Component({
  selector: 'app-create-recipe',
  imports: [FormField],
  templateUrl: './create-recipe.html',
  styleUrl: './create-recipe.css',
})
export class CreateRecipe {
  // Form and Model
  recipeModel = signal<RecipeModel>({
    recipeName: '',
    recipeImageUrl: 'No File Selected...',
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

  onFileSelected(event: Event) {
    //const input = (event.target as HTMLInputElement).value;
    //console.log(input)
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    // Update the value
    this.recipeModel.update(current => {
      return { ...current, recipeImageUrl: file?.name ?? 'No File Selected...'}
    });
  }

  onFileRemove() {
    // Remove the file name in recipeImageURL
    this.recipeModel.update(current => {
      return { ...current, recipeImageUrl: 'No File Selected...'}
    })
  }

  addSteps() {
    // Set the array length to total steps
    // then set the array values to an empty string, since the input is a textarea
    this.recipeModel().recipeSteps.length = this.recipeModel().recipeTotalSteps;
    this.recipeModel().recipeSteps.fill("");
  }

  addTags() {
    // Just set the array lengths - then set the array values to the first option in the api responces
    this.recipeModel().recipeTags.length = this.recipeModel().recipeTotalTags;
    this.recipeModel().recipeTags.fill("");
  }

  addIngredients() {
    this.recipeModel().recipeIngredients.length = this.recipeModel().recipeTotalIngredients;
    this.recipeModel().recipeIngredients.fill({
      IngredientId: '',
      Quantity: 0,
      Unit: ''
    })
  }

  updateSteps(index: number, event: Event) {
    // Set the value in the textarea to the appropriate index in the array
    const value = (event.target as HTMLTextAreaElement).value;
    this.recipeModel.update(current => {
      const steps = [...current.recipeSteps];
      steps[index] = value;
      return { ...current, recipeSteps: steps }
    });
  }

  updateTags(index: number, event: Event) {
    // Set the value in the select to the appropriate index in the array
    const value = (event.target as HTMLSelectElement).value;
    this.recipeModel.update(current => {
      const tags = [...current.recipeTags];
      tags[index] = value;
      return { ...current, recipeTags: tags }
    });
  }

  updateIngredientID(index: number, event: Event) {
    // Set the value in the select to the appropriate index in the array
    const value = (event.target as HTMLSelectElement).value;
    this.recipeModel.update(current => {
      const ingredients = [...current.recipeIngredients];
      ingredients[index] = { ...ingredients[index], IngredientId: value };
      return { ...current, recipeIngredients: ingredients }
    });
  }

  updateIngredientQuantity(index: number, event: Event) {
    // Set the value in the input to the appropriate index in the array
    const value = Number((event.target as HTMLInputElement).value);
    this.recipeModel.update(current => {
      const ingredients = [...current.recipeIngredients];
      ingredients[index] = { ...ingredients[index], Quantity: value };
      return { ...current, recipeIngredients: ingredients }
    });
  }

  updateIngredientUnit(index: number, event: Event) {
    // Set the value in the select to the appropriate index in the array
    const value = (event.target as HTMLSelectElement).value;
    this.recipeModel.update(current => {
      const ingredients = [...current.recipeIngredients];
      ingredients[index] = { ...ingredients[index], Unit: value };
      return { ...current, recipeIngredients: ingredients }
    });
  }

  submitRecipeForm(event: Event) {
    // ADD IMAGE TO API CALL LATER
    event.preventDefault();

    if (this.recipeForm().invalid())
      return;

    // Add all the instructions into one string - seperated by | 
    var recipeInstructions = '';
    for (let step of this.recipeModel().recipeSteps) {
      // If we are at the last step dont put a divider
      if (this.recipeModel().recipeSteps.indexOf(step) == this.recipeModel().recipeSteps.length-1)
        recipeInstructions += step;
      else
        recipeInstructions += step + "|";
    }

    // Create the model
    const createRecipeModel: CreateRecipeModel = {
      Name: this.recipeForm.recipeName().value(),
      Image: this.recipeForm.recipeImageUrl().value(),
      Instructions: recipeInstructions,
      Tags: this.recipeForm.recipeTags().value(),
      Ingredients: this.recipeForm.recipeIngredients().value()
    };

    // API Call
    // Server is using [FromForm] this means we need to send multipart/form-data instead of JSON.
    // Instead of JSON.stringify() use FormData
  }
}
