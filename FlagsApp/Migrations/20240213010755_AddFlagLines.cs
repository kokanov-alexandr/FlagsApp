using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagsApp.Migrations
{
    /// <inheritdoc />
    public partial class AddFlagLines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlagLines",
                columns: table => new
                {
                    FlagId = table.Column<int>(type: "int", nullable: false),
                    LinesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlagLines", x => new { x.FlagId, x.LinesId });
                    table.ForeignKey(
                        name: "FK_FlagLines_Flags_FlagId",
                        column: x => x.FlagId,
                        principalTable: "Flags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlagLines_Lines_LinesId",
                        column: x => x.LinesId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlagLines_LinesId",
                table: "FlagLines",
                column: "LinesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlagLines");
        }
    }
}
