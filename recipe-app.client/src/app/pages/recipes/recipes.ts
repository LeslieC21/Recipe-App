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
    recipeName: "Cereal",
    recipeType: "Breakfast",
    recipeSteps: [
      "Step 1: Pour boxed cereal into bowl",
      "Step 2: Pour Milk into bowl"
    ]
  }
}
