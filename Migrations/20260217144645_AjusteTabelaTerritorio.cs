using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizarTerritorios.Migrations
{
    /// <inheritdoc />
    public partial class AjusteTabelaTerritorio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quadras",
                table: "Territorios");

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeQuadras",
                table: "Territorios",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeQuadras",
                table: "Territorios");

            migrationBuilder.AddColumn<string>(
                name: "Quadras",
                table: "Territorios",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
