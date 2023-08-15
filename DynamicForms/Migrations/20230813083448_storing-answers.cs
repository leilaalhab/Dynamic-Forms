using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynamicForms.Migrations
{
    /// <inheritdoc />
    public partial class storinganswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Progresses_ProgressId",
                table: "Answer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answer",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Answer_ProgressId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "DoubleValue",
                table: "Answer");

            migrationBuilder.RenameTable(
                name: "Answer",
                newName: "TextAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "TextValue",
                table: "TextAnswers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextAnswers",
                table: "TextAnswers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DoubleValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoubleValue = table.Column<double>(type: "float", nullable: false),
                    InputId = table.Column<int>(type: "int", nullable: false),
                    ProgressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoubleValues", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoubleValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TextAnswers",
                table: "TextAnswers");

            migrationBuilder.RenameTable(
                name: "TextAnswers",
                newName: "Answer");

            migrationBuilder.AlterColumn<string>(
                name: "TextValue",
                table: "Answer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Answer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "DoubleValue",
                table: "Answer",
                type: "float",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answer",
                table: "Answer",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_ProgressId",
                table: "Answer",
                column: "ProgressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Progresses_ProgressId",
                table: "Answer",
                column: "ProgressId",
                principalTable: "Progresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
