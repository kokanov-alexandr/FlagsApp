using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlagsApp.Migrations
{
    /// <inheritdoc />
    public partial class AddFlagGraphicElement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlagGraphicElements",
                columns: table => new
                {
                    FlagId = table.Column<int>(type: "int", nullable: false),
                    GraphicElementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlagGraphicElements", x => new { x.FlagId, x.GraphicElementId });
                    table.ForeignKey(
                        name: "FK_FlagGraphicElements_Flags_FlagId",
                        column: x => x.FlagId,
                        principalTable: "Flags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlagGraphicElements_GraphicElements_GraphicElementId",
                        column: x => x.GraphicElementId,
                        principalTable: "GraphicElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlagGraphicElements_GraphicElementId",
                table: "FlagGraphicElements",
                column: "GraphicElementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlagGraphicElements");
        }
    }
}
