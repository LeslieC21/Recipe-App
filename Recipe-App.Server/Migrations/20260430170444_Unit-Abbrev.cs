using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipe_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class UnitAbbrev : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "Units");
        }
    }
}
