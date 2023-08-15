using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynamicForms.Migrations
{
    /// <inheritdoc />
    public partial class storinganswers2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DoubleValues",
                table: "DoubleValues");

            migrationBuilder.RenameTable(
                name: "DoubleValues",
                newName: "DoubleAnswers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoubleAnswers",
                table: "DoubleAnswers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DoubleAnswers",
                table: "DoubleAnswers");

            migrationBuilder.RenameTable(
                name: "DoubleAnswers",
                newName: "DoubleValues");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoubleValues",
                table: "DoubleValues",
                column: "Id");
        }
    }
}
