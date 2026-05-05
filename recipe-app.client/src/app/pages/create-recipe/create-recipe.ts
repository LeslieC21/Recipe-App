import { Component, signal, inject, DestroyRef } from '@angular/core';
import { form, required, FormField, debounce } from '@angular/forms/signals';
import { Subscription } from 'rxjs';

import { IngredientInfoModel } from '../../core/models/IngredientInfoModel';
import { CreateIngredientRequest } from '../../core/models/RequestModels/CreateIngredientRequest';
import { UnitModel } from '../../core/models/UnitModel';
import { IngredientModel } from '../../core/models/IngredientModel';
import { TagModel } from '../../core/models/TagModel';
import { RecipeService } from '../../core/services/RecipeService';
import { CapitalizePipe } from '../../core/services/CapitalizePipe';

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

  // Constants
  readonly MODALTYPE_RESPONSE = 0;
  readonly MODALTYPE_CREATETAG = 1;
  readonly MODALTYPE_CREATEINGREDIENT = 2;

  // Arrays For Units/Tags/Ingredients For the Dropdowns
  units = signal<UnitModel[]>([]);
  recipeTags = signal<TagModel[]>([]);
  ingredientTags = signal<TagModel[]>([]);
  ingredients = signal<IngredientModel[]>([]);

  // Signals for modal logic
  showModal = signal<boolean>(false);
  modalVersion = signal<number>(this.MODALTYPE_RESPONSE);
  modalTitle = signal<string>('Title');
  modalBody = signal<string>('Body');
  saveModalSuccess = signal<boolean>(false);

  // Forms and Models
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

  tagModel = signal({
    tagName: '',
    isRecipeTag: true
  })

  ingredientModel = signal<CreateIngredientRequest>({
    Name: '',
    TagId: ''
  })

  recipeForm = form(this.recipeModel, (schemaPath) => {
    required(schemaPath.recipeName);
    debounce(schemaPath.recipeName, 300);
    required(schemaPath.recipeSteps);
    required(schemaPath.recipeTags);
    required(schemaPath.recipeIngredients);
    debounce(schemaPath.recipeSteps, 300);
  })

  tagForm = form(this.tagModel, (schemaPath) => {
    required(schemaPath.tagName);
  })

  ingredientForm = form(this.ingredientModel, (schemaPath) => {
    required(schemaPath.Name);
    required(schemaPath.TagId);
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
    console.log(value);
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
      if (ingredient.UnitId != '')
        formData.append(`Ingredients[${index}][UnitId]`, ingredient.UnitId);
    });

    console.log("We Submitted... ");
    for (let [key, value] of formData.entries()) {
      console.log(key, value);
    }
    console.log(this.recipeModel());

    // API Call
    // Server is using [FromForm] this means we need to send multipart/form-data instead of JSON.
    const subscription = this.RService.newRecipe(formData).subscribe(x => {
      this.saveModalSuccess.set(x as boolean);
      this.showModal.set(true);
      this.modalVersion.set(this.MODALTYPE_RESPONSE);
      if (x) {
        this.modalTitle.set('Success!');
        this.modalBody.set(`A new recipe ${this.recipeModel().recipeName} was created successfully!`);
      }
      else {
        this.modalTitle.set('Uh oh! Something went wrong...');
        this.modalBody.set(`The recipe ${this.recipeModel().recipeName} could not be created. Please try again later.`);
      }
    });
    this.destroySubscription(subscription);
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

  // Method to populate the drop downs for recipe tags
  getRecipeTags() {
    const subscription = this.RService.getTagsByRecipeType().subscribe(x => {
      this.recipeTags.set(x);
    });
    this.destroySubscription(subscription);
  }

  // Method to populate the drop down for ingredient tags
  getIngredientTags() {
    const subscription = this.RService.getTagsByIngredientType().subscribe(x => {
      this.ingredientTags.set(x);
    });
    this.destroySubscription(subscription);
  }

  // Method that is called by other methods to destroy their subscription
  destroySubscription(subscription: Subscription) {
    this.destoryRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }

  // Method to create a new tag
  createNewTag() {
    //Check if tag is supposed to be a recipe or ingredient tag
    if (this.tagModel().isRecipeTag) {
      const subscription = this.RService.newRecipeTag(this.tagModel().tagName).subscribe(x => {
        this.saveModalSuccess.set(x as boolean);
        this.modalVersion.set(this.MODALTYPE_RESPONSE);
        if (x) {
          this.modalTitle.set('Success!');
          this.modalBody.set(`A new recipe tag ${this.tagModel().tagName} was created successfully!`);
        }
        else {
          this.modalTitle.set('Uh oh! Something went wrong...');
          this.modalBody.set(`The recipe tag ${this.tagModel().tagName} could not be created. Please try again later.`);
        }
        this.getRecipeTags();
      });
      this.destroySubscription(subscription);
    } else {
      const subscription = this.RService.newIngredientTag(this.tagModel().tagName).subscribe(x => {
        this.saveModalSuccess.set(x as boolean);
        this.modalVersion.set(this.MODALTYPE_RESPONSE);
        if (x) {
          this.modalTitle.set('Success!');
          this.modalBody.set(`A new ingredient tag ${this.tagModel().tagName} was created successfully!`);
        }
        else {
          this.modalTitle.set('Uh oh! Something went wrong...');
          this.modalBody.set(`The ingredient tag ${this.tagModel().tagName} could not be created. Please try again later.`);
        }
        this.getIngredientTags();
      });
      this.destroySubscription(subscription);
    }
  }

  // Method to create a new ingredient
  createNewIngredient() {
    console.log(this.ingredientModel());

    const subscription = this.RService.newIngredient(this.ingredientModel()).subscribe(x => {
      this.saveModalSuccess.set(x as boolean);
      this.modalVersion.set(this.MODALTYPE_RESPONSE);
      if (x) {
        this.modalTitle.set('Success!');
        this.modalBody.set(`A new ingredient ${this.ingredientModel().Name} was created successfully!`);
      }
      else {
        this.modalTitle.set('Uh oh! Something went wrong...');
        this.modalBody.set(`The ingredient ${this.ingredientModel().Name} could not be created. Please try again later.`);
      }
      this.getIngredients();
    });
    this.destroySubscription(subscription);
  }

  // Method to add Tags to db
  addTag(event: Event) {
    event.preventDefault();
    this.modalVersion.set(this.MODALTYPE_CREATETAG);
    this.modalTitle.set('Add A Tag');
    this.modalBody.set('Let create a new tag! You can create either a recipe tag or an ingredient tag. ' +
      'A Recipe Tag could be Breakfast/Dessert/Spicy and an ingredient tag could be Poultry/Grain. ' +
      'Recipes can have many tags while ingredients may only have one. Think carefully!');
    this.showModal.set(true);
  }

  // Method to add Ingredients to db
  addIngredient(event: Event) {
    event.preventDefault();
    this.modalVersion.set(this.MODALTYPE_CREATEINGREDIENT);
    this.modalTitle.set('Add An Ingredient');
    this.modalBody.set('Lets create a new Ingredient! Each ingredient must have a name and a single tag!');
    this.showModal.set(true);

    // Get all tags
    this.getIngredientTags();
  }

  // Method to handle Modal Save
  saveModal() {
    if (this.modalVersion() === this.MODALTYPE_CREATETAG && this.tagForm().valid())
      this.createNewTag();
    else if(this.modalVersion() === this.MODALTYPE_CREATEINGREDIENT && this.ingredientForm().valid())
      this.createNewIngredient();

    console.log(this.tagForm().valid());
  }

  // Method to close modal
  closeModal() {
    this.showModal.set(false);
  }

  ngOnInit() {
    // Populate all dropdowns - Units/Tags/Ingredients
    this.getUnits();
    this.getIngredients();
    this.getRecipeTags();
  }
}
