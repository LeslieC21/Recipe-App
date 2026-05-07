import { Component, Input, signal, inject, EventEmitter, Output, DestroyRef } from '@angular/core';

import { RecipeModel } from '../../../core/models/RecipeModel';
import { CapitalizePipe } from '../../../core/services/CapitalizePipe';
import { RecipeService } from '../../../core/services/RecipeService';

@Component({
  selector: 'app-recipe',
  imports: [CapitalizePipe],
  templateUrl: './recipe.html',
  styleUrl: './recipe.css',
})
export class Recipe {
  @Input({ required: true }) recipe!: RecipeModel;
  @Input({ required: true }) viewAlone!: boolean;
  @Output() expandedRecipe = new EventEmitter<string>();
  @Output() updateRecipe = new EventEmitter<void>();

  RService = inject(RecipeService);
  destroyRef = inject(DestroyRef);

  imageURL = signal<string | null>(null);
  showRecipeDetails = signal<Boolean>(false);

  toggleRecipeDetails() {
    if(this.viewAlone != true)
      this.showRecipeDetails.update(v => !v);
  }

  async toggleFavoriteRecipe(event: Event) {
    console.log("toggleFavoriteRecipe called");
    console.log("isFavorite:", this.recipe.isFavorite);

    if (this.recipe.isFavorite) {
      // Recipe is a user favorite - Remove from favorites
      this.recipe.isFavorite = false;
      const subscription = this.RService.deleteFromFavorites(this.recipe.recipeId).subscribe({
        complete: () => {
          console.log("complete fired");
          console.log("Emitting:", this.recipe.recipeId);
          this.updateRecipe.emit()
        }
      });
      this.destroyRef.onDestroy(() => {
        subscription.unsubscribe();
      })
    } else if (this.recipe.isFavorite == false) {
      // Recipe was not a user favorite - Add to favorites
      this.recipe.isFavorite = true;
      const subscription = this.RService.addToFavorites(this.recipe.recipeId).subscribe({
        complete: () => {
          console.log("complete fired");
          console.log("Emitting:", this.recipe.recipeId);
          this.updateRecipe.emit()
        }
      });
      this.destroyRef.onDestroy(() => {
        subscription.unsubscribe();
      });
    } else if (this.recipe.isFavorite == null){
      // We arent logged in since we didnt get favorites

    }
  }

  toggleFullscreen() {
    if (this.viewAlone == true) {
      this.showRecipeDetails.set(false);
      this.expandedRecipe.emit('');
    } else {
      this.showRecipeDetails.set(true);
      this.expandedRecipe.emit(this.recipe.recipeId);
    }
  }

  ngOnInit() {
    // If we are viewing the recipe in full screen
    if (this.viewAlone == true) {
      this.showRecipeDetails.set(true);
    }

    // Convert recipe image from blob to temporary link to the data
    if (this.recipe?.image instanceof Blob) {
      const url = URL.createObjectURL(this.recipe.image);
      this.imageURL.set(`url(${url})`);
    }
  }
  // LOOK AT s3 for saving images instead of uploading them to the db
}
