import { Component, signal, inject, DestroyRef } from '@angular/core';
import { form, required, FormField } from '@angular/forms/signals';
import { Subscription } from 'rxjs';

import { IngredientInfoModel } from '../../core/models/IngredientInfoModel';
import { UnitModel } from '../../core/models/UnitModel';
import { IngredientModel } from '../../core/models/IngredientModel';
import { TagModel } from '../../core/models/TagModel';
import { RecipeService } from '../../core/services/RecipeService';
import { CapitalizePipe } from '../../core/services/CapitalizePipe';
import { Recipe } from '../recipes/recipe/recipe';

interface RecipeModel {
  recipeName: string;
  recipeImage: File | null;
  recipeTotalSteps: number;
  recipeSteps: string[];
  recipeTotalTags: number;
  recipeTags: string[];
  recipeTotalIngredients: number;
  recipeIngredients: IngredientInfoModel[];
}

@Component({
  selector: 'app-create-recipe',
  imports: [FormField, CapitalizePipe],
  templateUrl: './create-recipe.html',
  styleUrl: './create-recipe.css',
})
export class CreateRecipe {
  // Injects
  RService = inject(RecipeService);
  destoryRef = inject(DestroyRef);

  // Arrays For Units/Tags/Ingredients For the Dropdowns
  units = signal<UnitModel[]>([]);
  tags = signal<TagModel[]>([]);
  ingredients = signal<IngredientModel[]>([]);

  // Form and Model
  recipeModel = signal<RecipeModel>({
    recipeName: '',
    recipeImage: null,
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
    const input = event.target as HTMLInputElement;
    var file = input.files?.[0] ?? null;

    // Update the value
    this.recipeModel.update(current => {
      return { ...current, recipeImage: file }
    });
  }

  onFileRemove() {
    // Remove the file in recipeImageURL
    this.recipeModel.update(current => {
      return { ...current, recipeImage: null }
    })
  }

  addSteps() {
    // Set the array length to total steps
    // then set the array values to an empty string, since the input is a textarea
    this.recipeModel().recipeSteps.length = this.recipeModel().recipeTotalSteps;
    // Only give empty string values to the indexs that are undefined aka they didnt recieve user input
    for (let step of this.recipeModel().recipeSteps) {
      if (step == undefined) {
        step = '';
      }
    }
  }

  addTags() {
    // Just set the array lengths
    this.recipeModel().recipeTags.length = this.recipeModel().recipeTotalTags;
    for (let tag of this.recipeModel().recipeTags) {
      if (tag == undefined) {
        tag = '';
      }
    }
  }

  addIngredients() {
    // Set the array length
    this.recipeModel().recipeIngredients.length = this.recipeModel().recipeTotalIngredients;
    for (let ing of this.recipeModel().recipeIngredients) {
      if (ing == undefined) {
        ing = {
          IngredientId: '',
          Quantity: 0,
          UnitId: ''
        }
      }
    }
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
      ingredients[index] = { ...ingredients[index], UnitId: value };
      return { ...current, recipeIngredients: ingredients }
    });
  }

  submitRecipeForm(event: Event) {
    // ADD IMAGE TO API CALL LATER
    event.preventDefault();

    if (this.recipeForm().invalid()) {
      console.log("Couldnt be submitted... ;c");
      return;
    }

    // Add all the instructions into one string - seperated by | 
    var recipeInstructions = this.recipeModel().recipeSteps.join("|");
    console.log(recipeInstructions);

    // Build FormData to send in the API Req
    const formData = new FormData();
    formData.append('Name', this.recipeForm.recipeName().value());
    formData.append('Instructions', recipeInstructions);

    // Check if the image exists
    const image = this.recipeModel().recipeImage;
    if (image)
      formData.append('Image', image);

    // Add Tags
    this.recipeModel().recipeTags.forEach(tag => {
      formData.append('Tags', tag);
    });

    // Add Ingredients
    this.recipeModel().recipeIngredients.forEach((ingredient, index) => {
      formData.append(`Ingredients[${index}][IngredientId]`, ingredient.IngredientId);
      formData.append(`Ingredients[${index}][Quantity]`, ingredient.Quantity.toString());
      formData.append(`Ingredients[${index}][UnitId]`, ingredient.UnitId);
    });

    console.log("We Submitted... ");
    for (let [key, value] of formData.entries()) {
      console.log(key, value);
    }


    // API Call
    // Server is using [FromForm] this means we need to send multipart/form-data instead of JSON.
    //const subscription = this.RService.newRecipe(formData).subscribe(x => console.log(x));
    //this.destroySubscription(subscription);
  }

  // Method to populate the drop downs for units
  getUnits() {
    const subscription = this.RService.getUnits().subscribe((x) => {
      this.units.set(x);
    });
    this.destroySubscription(subscription);
  }

  // Method to populate the drop downs for ingredient
  getIngredients() {
    const subscription = this.RService.getIngredients().subscribe(x => {
      this.ingredients.set(x);
      console.log(this.ingredients())
    });
    this.destroySubscription(subscription);
  }

  // Method to populate the drop downs for tags
  getRecipeTags() {
    const subscription = this.RService.getTagsByRecipeType().subscribe(x => {
      this.tags.set(x);
    });
    this.destroySubscription(subscription);
  }

  // Method that is called by other methods to destroy their subscription
  destroySubscription(subscription: Subscription) {
    this.destoryRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }

  ngOnInit() {
    // Populate all dropdowns - Units/Tags/Ingredients
    this.getUnits();
    this.getIngredients();
    this.getRecipeTags();
  }
}
