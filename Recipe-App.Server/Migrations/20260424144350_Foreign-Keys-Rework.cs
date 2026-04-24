using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipe_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeysRework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTags_Recipe_fk_RecipeId",
                table: "RecipeTags");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTags_Tags_fk_TagId",
                table: "RecipeTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeTags",
                table: "RecipeTags");

            migrationBuilder.DropIndex(
                name: "IX_RecipeTags_fk_RecipeId",
                table: "RecipeTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "RecipeTagsId",
                table: "RecipeTags");

            migrationBuilder.DropColumn(
                name: "Ingredients",
                table: "Recipe");

            migrationBuilder.RenameColumn(
                name: "fk_TagId",
                table: "RecipeTags",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "fk_RecipeId",
                table: "RecipeTags",
                newName: "RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeTags_fk_TagId",
                table: "RecipeTags",
                newName: "IX_RecipeTags_TagId");

            migrationBuilder.AlterColumn<string>(
                name: "IngredientId",
                table: "RecipeIngredients",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeTags",
                table: "RecipeTags",
                columns: new[] { "RecipeId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients",
                columns: new[] { "RecipeId", "IngredientId" });

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
                name: "FK_RecipeIngredients_Recipe_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Tags_TagsTagId",
                table: "RecipeIngredients",
                column: "TagsTagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTags_Recipe_RecipeId",
                table: "RecipeTags",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTags_Tags_TagId",
                table: "RecipeTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Ingredients_IngredientId1",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Recipe_RecipeId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Tags_TagsTagId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTags_Recipe_RecipeId",
                table: "RecipeTags");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTags_Tags_TagId",
                table: "RecipeTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeTags",
                table: "RecipeTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeIngredients",
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
                name: "TagId",
                table: "RecipeTags",
                newName: "fk_TagId");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "RecipeTags",
                newName: "fk_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeTags_TagId",
                table: "RecipeTags",
                newName: "IX_RecipeTags_fk_TagId");

            migrationBuilder.AddColumn<string>(
                name: "RecipeTagsId",
                table: "RecipeTags",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "IngredientId",
                table: "RecipeIngredients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Ingredients",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeTags",
                table: "RecipeTags",
                column: "RecipeTagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTags_fk_RecipeId",
                table: "RecipeTags",
                column: "fk_RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTags_Recipe_fk_RecipeId",
                table: "RecipeTags",
                column: "fk_RecipeId",
                principalTable: "Recipe",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTags_Tags_fk_TagId",
                table: "RecipeTags",
                column: "fk_TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
