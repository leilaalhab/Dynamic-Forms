using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynamicForms.Migrations
{
    /// <inheritdoc />
    public partial class storinganswers3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TextValue",
                table: "TextAnswers",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "DoubleValue",
                table: "DoubleAnswers",
                newName: "Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "TextAnswers",
                newName: "TextValue");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "DoubleAnswers",
                newName: "DoubleValue");
        }
    }
}
