import { Component } from '@angular/core';

import { Recipe } from './recipe/recipe';
import { RecipeModel } from '../../core/models/RecipeModel';

@Component({
  selector: 'app-recipes',
  imports: [Recipe],
  templateUrl: './recipes.html',
  styleUrl: './recipes.css',
})
export class Recipes {
  exampleData1 = <RecipeModel>{
    recipeId: "1",
    recipeImgUrl: "ExampleRecipe.jpg",
    recipeName: "Scrambled Eggs",
    recipeType: "Breakfast",
    recipeSteps: [
      "Place a small skillet over medium heat and add 1tbsp of butter",
      "Crack 2 large eggs into the skillet.",
      "Stir the eggs gently for about 1 minute, until it seems the egg gets thicker.",
      "Remove your eggs from the skillet, place onto a plate and add salt and pepper as desired."
    ],
    recipeIngredients: [
      ["Eggs", "2", ""],
      ["Butter", "1", "TBSP"],
      ["Salt", "1", "TSP"],
      ["Pepper", "1", "TSP"]
    ]
  }
}
