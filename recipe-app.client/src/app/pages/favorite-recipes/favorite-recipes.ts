import { Component, DestroyRef, inject, signal } from '@angular/core';
import { form, debounce, FormField } from '@angular/forms/signals';
import { map, catchError } from 'rxjs'

import { Recipe } from '../recipes/recipe/recipe';
import { RecipeModel } from '../../core/models/RecipeModel';
import { TagModel } from '../../core/models/TagModel';
import { IngredientModel } from '../../core/models/IngredientModel';
import { RecipeService } from '../../core/services/RecipeService';
import { AuthService } from '../../core/services/AuthService';

interface SearchModel {
  recipeName: string;
  recipeTags: string[];
  recipeIngredients: string[];
}

@Component({
  selector: 'app-favorite-recipes',
  imports: [FormField, Recipe],
  templateUrl: './favorite-recipes.html',
  styleUrl: './favorite-recipes.css',
})
export class FavoriteRecipes {
  // Injects
  RService = inject(RecipeService);
  AService = inject(AuthService);
  destroyRef = inject(DestroyRef);

  viewSingleRecipe = signal<string | null>(null);
  isLoading = signal<boolean>(false);
  recipes = signal<RecipeModel[]>([]);
  tags = signal<TagModel[]>([]);
  ingredients = signal<IngredientModel[]>([]);
  showSearchCriteria = signal<boolean>(false);
  message = signal<string>('');

  // Search Forms
  searchModel = signal<SearchModel>({
    recipeName: '',
    recipeTags: [],
    recipeIngredients: []
  })

  searchForm = form(this.searchModel, (schemaPath) => {
    debounce(schemaPath.recipeName, 300);
  })

  // Method that holds the API request
  getFavoriteRecipes() {
    this.isLoading.set(true);
    this.message.set("Loading Recipes");
    const subscription = this.RService.getUserFavoriteRecipes()
      .pipe(
        catchError((error) => {
          if (error.status === 401) {
            this.message.set("Log in to see your favorite recipes");
          }

          return ([]);
        }),
        map(switchMap => {
          switchMap.forEach((recipe) => {
            if (recipe.image) {
              const base64 = recipe.image as string;
              const binary = atob(base64);
              const bytes = Uint8Array.from(binary, c => c.charCodeAt(0));
              recipe.image = new Blob([bytes], { type: 'image/jpeg' });
            }
          })
          return switchMap;
        })
      )
      .subscribe(x => {
        this.recipes.set(x);
        this.isLoading.set(false);
        if (this.recipes.length == 0) {
          this.message.set("No Favorite Recipes");
        }
      });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }

  removeRecipeFromFavorites() {
    // Update recipes
    this.getFavoriteRecipes();
  }

  // Event from child component that is triggered when a user selects a recipe to view
  toggleSingleRecipeView(recipeId: string) {
    if (recipeId == '') {
      this.viewSingleRecipe.set(null);
      return;
    }

    this.viewSingleRecipe.set(recipeId);
  }

  // Event from checkbox
  toggleSearchItemTag(event: Event, value: string) {
    const checked = (event.target as HTMLInputElement).checked;
    const tags = this.searchModel().recipeTags;

    if (checked) {
      if (!tags.includes(value))
        tags.push(value);
    } else {
      const idx = tags.indexOf(value);
      if (idx > -1)
        tags.splice(idx, 1);
    }

    this.doFilterSearch(undefined);
  }

  // Event from checkbox
  toggleSearchItemIngredient(event: Event, value: string) {
    const checked = (event.target as HTMLInputElement).checked;
    const tags = this.searchModel().recipeIngredients;

    // if we just checked a new item
    if (checked) {
      if (!tags.includes(value))
        tags.push(value);
    } else {    // We unchecked an item
      const idx = tags.indexOf(value);
      if (idx > -1)
        tags.splice(idx, 1);
    }

    this.doFilterSearch(undefined);
  }

  // Method to get all recipe tags
  async getSearchTags() {
    const subscription = this.RService.getTagsByRecipeType().subscribe(x => {
      this.tags.set(x);
    });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }

  // Method to get all ingredients
  async getSearchIngredients() {
    const subscription = this.RService.getIngredients().subscribe(x => {
      this.ingredients.set(x);
    });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }

  toggleSearchCriteria(event: Event) {
    event.preventDefault();

    // Update to the opposite value
    this.showSearchCriteria.update(s => !s);

    // If we are now looking at the search options
    if (this.showSearchCriteria()) {
      // Set a timeout to allow DOM elements to be rendered
      setTimeout(() => {
        // Check which searches were marked before it closed
        // Go through each tag that is checked
        console.log(this.searchModel().recipeTags);
        for (let tag of this.searchModel().recipeTags) {
          const element = document.getElementById(tag) as HTMLInputElement;
          if (element)
            element.checked = true;
        }

        // Go through each ingredient that is checked
        for (let ingredient of this.searchModel().recipeIngredients) {
          const element = document.getElementById(ingredient) as HTMLInputElement;
          if (element)
            element.checked = true;
        }
      }, 0);
    }
  }

  doNameSearch(event: Event) {
    event.preventDefault();

    console.log("Changed");
    if (this.searchModel().recipeName == '') {
      this.getFavoriteRecipes();
      return;
    }

    const subscription = this.RService.getRecipeByName(this.searchModel().recipeName).subscribe(x => {
      this.recipes.set(x);
    });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    })
  }

  // Method to search recipes by their ingredients or tags
  doFilterSearch(event: Event | undefined) {
    if (event)
      event.preventDefault();

    const subscription = this.RService.getRecipesByFilter(this.searchModel().recipeTags, this.searchModel().recipeIngredients)
      .subscribe(x => {
        this.recipes.set(x);
      })
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    })
  }

  clearFilters(event: Event) {
    event.preventDefault();

    // Go through each tag that is checked
    for (let tag of this.searchModel().recipeTags) {
      const element = document.getElementById(tag) as HTMLInputElement;
      element.checked = false;
    }

    // Go through each ingredient that is checked
    for (let ingredient of this.searchModel().recipeIngredients) {
      const element = document.getElementById(ingredient) as HTMLInputElement;
      element.checked = false;
    }

    // set both signal arrays to empty
    this.searchModel().recipeIngredients = [];
    this.searchModel().recipeTags = [];

    const subscription = this.RService.getRecipes().subscribe(x => {
      this.recipes.set(x);
    });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    })
  }

  ngOnInit() {
    // Method to get all recipes from db
    this.getFavoriteRecipes();
    this.getSearchTags();
    this.getSearchIngredients();
  }
}
