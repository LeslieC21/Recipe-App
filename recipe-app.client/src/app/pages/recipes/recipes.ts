import { Component, DestroyRef, OnInit, inject, signal } from '@angular/core';
import { map, switchMap }  from 'rxjs'

import { Recipe } from './recipe/recipe';
import { RecipeModel } from '../../core/models/RecipeModel';
import { RecipeIngredientResponse } from '../../core/models/RecipeIngredientModel';
import { RecipeService } from '../../core/services/RecipeService';

@Component({
  selector: 'app-recipes',
  imports: [Recipe],
  templateUrl: './recipes.html',
  styleUrl: './recipes.css',
})
export class Recipes implements OnInit {
  // Injects
  RService = inject(RecipeService);
  destroyRef = inject(DestroyRef);

  viewSingleRecipe = signal<string | null>(null);
  recipes = signal<RecipeModel[]>([]);
  exampleData1 = <RecipeModel>{
    recipeId: "1",
    name: "Scrambled Eggs",
    tags: ["Breakfast"],
    instructions: "Place a small skillet over medium heat and add 1tbsp of butter.|Crack 2 large eggs into the skillet.|Stir the eggs gently for about 1 minute, until it seems the egg gets thicker.|Remove your eggs from the skillet, place onto a plate and add salt and pepper as desired.",
    ingredients: [
      {
        ingredientId: "1",
        ingredientName: "Eggs",
        ingredientQuantity: 2,
        ingredientUnitName: "",
        ingredientUnitAbbreviation: ""
      },
      {
        ingredientId: "2",
        ingredientName: "Butter",
        ingredientQuantity: 1,
        ingredientUnitName: "Tablespoon",
        ingredientUnitAbbreviation: "TBSP"
      },
      {
        ingredientId: "3",
        ingredientName: "Salt",
        ingredientQuantity: 1,
        ingredientUnitName: "Teaspoon",
        ingredientUnitAbbreviation: "TSP"
      },
      {
        ingredientId: "4",
        ingredientName: "Pepper",
        ingredientQuantity: 1,
        ingredientUnitName: "Teaspoon",
        ingredientUnitAbbreviation: "TSP"
      }
    ]
  }

  // Method that holds the API request
  async getRecipes() {
    const subscription = this.RService.getRecipes()
      .pipe(
        map(switchMap => {
          console.log(switchMap);
          switchMap.forEach((recipe) => {
            if (recipe.image) {
              const base64 = recipe.image as string;
              const binary = atob(base64);
              const bytes = Uint8Array.from(binary, c => c.charCodeAt(0));
              recipe.image = new Blob([bytes], { type: 'image/jpeg' });
              console.log('is Blob:', recipe.image instanceof Blob);
            }
          })
          console.log(switchMap);
          return switchMap;
        })
      )
      .subscribe(x => {
        this.recipes.set(x);
    });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }

  // Event from child component that is triggered when a user selects a recipe to view
  toggleSingleRecipeView(recipeId: string) {
    if (recipeId == '') {
      this.viewSingleRecipe.set(null);
      return;
    }

    this.viewSingleRecipe.set(recipeId);
  }

  ngOnInit() {
    // Method to get all recipes from db
    this.getRecipes();
  }
}
