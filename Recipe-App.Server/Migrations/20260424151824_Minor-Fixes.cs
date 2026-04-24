using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipe_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class MinorFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Ingredients_IngredientId1",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Tags_TagsTagId",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_TagsTagId",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_IngredientId1",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "TagsTagId",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "IngredientId1",
                table: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "IngredientTag",
                table: "Ingredients",
                newName: "TagId");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "RecipeIngredients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TagsTagId",
                table: "Ingredients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_TagsTagId",
                table: "Ingredients",
                column: "TagsTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Tags_TagsTagId",
                table: "Ingredients",
                column: "TagsTagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Tags_TagsTagId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_IngredientId",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_TagsTagId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "TagsTagId",
                table: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "Ingredients",
                newName: "IngredientTag");

            migrationBuilder.AddColumn<string>(
                name: "TagsTagId",
                table: "RecipeIngredients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IngredientId1",
                table: "Ingredients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_TagsTagId",
                table: "RecipeIngredients",
                column: "TagsTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_IngredientId1",
                table: "Ingredients",
                column: "IngredientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Ingredients_IngredientId1",
                table: "Ingredients",
                column: "IngredientId1",
                principalTable: "Ingredients",
                principalColumn: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Tags_TagsTagId",
                table: "RecipeIngredients",
                column: "TagsTagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
