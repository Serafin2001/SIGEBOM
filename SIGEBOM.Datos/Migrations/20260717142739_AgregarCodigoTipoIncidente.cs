using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEBOM.Datos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCodigoTipoIncidente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "TiposIncidentes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "TiposIncidentes");
        }
    }
}
