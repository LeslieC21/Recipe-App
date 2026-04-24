using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipe_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class Fixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Tags_TagsTagId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_TagsTagId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "TagsTagId",
                table: "Ingredients");

            migrationBuilder.AlterColumn<string>(
                name: "TagId",
                table: "Ingredients",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_TagId",
                table: "Ingredients",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Tags_TagId",
                table: "Ingredients",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Tags_TagId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_TagId",
                table: "Ingredients");

            migrationBuilder.AlterColumn<string>(
                name: "TagId",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "TagsTagId",
                table: "Ingredients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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
        }
    }
}
