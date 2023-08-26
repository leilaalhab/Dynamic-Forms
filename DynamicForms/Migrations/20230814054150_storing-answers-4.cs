using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynamicForms.Migrations
{
    /// <inheritdoc />
    public partial class storinganswers4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Choices_Inputs_InputId",
                table: "Choices");

            migrationBuilder.DropIndex(
                name: "IX_Choices_InputId",
                table: "Choices");

            migrationBuilder.CreateTable(
                name: "SendChoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InputId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendChoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SendChoice_Inputs_InputId",
                        column: x => x.InputId,
                        principalTable: "Inputs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SendChoice_InputId",
                table: "SendChoice",
                column: "InputId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SendChoice");

            migrationBuilder.CreateIndex(
                name: "IX_Choices_InputId",
                table: "Choices",
                column: "InputId");

            migrationBuilder.AddForeignKey(
                name: "FK_Choices_Inputs_InputId",
                table: "Choices",
                column: "InputId",
                principalTable: "Inputs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
