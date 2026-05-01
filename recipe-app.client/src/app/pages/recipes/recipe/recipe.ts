import { Component, Input, signal, inject, EventEmitter, Output } from '@angular/core';
import { NgOptimizedImage } from '@angular/common';
import { DomSanitizer } from '@angular/platform-browser';

import { RecipeModel } from '../../../core/models/RecipeModel';
import { CapitalizePipe } from '../../../core/services/CapitalizePipe';

@Component({
  selector: 'app-recipe',
  imports: [CapitalizePipe, NgOptimizedImage],
  templateUrl: './recipe.html',
  styleUrl: './recipe.css',
})
export class Recipe {
  @Input({ required: true }) recipe!: RecipeModel;
  @Input({ required: true }) viewAlone!: boolean;
  @Output() expandedRecipe = new EventEmitter<string>();

  sanitizer = inject(DomSanitizer);
  imageURL = signal<string | null>(null);

  showRecipeDetails = signal<Boolean>(false);
  isFavorite = signal<Boolean>(false);

  toggleRecipeDetails() {
    this.showRecipeDetails.update(v => !v);
  }

  toggleFavoriteRecipe() {
    this.isFavorite.update(f => !f);
    console.log(this.isFavorite());
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
