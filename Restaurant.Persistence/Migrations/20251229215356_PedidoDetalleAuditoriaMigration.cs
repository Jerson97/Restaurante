using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PedidoDetalleAuditoriaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "actualizadopor",
                table: "pedidodetalles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "creadopor",
                table: "pedidodetalles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "eliminado",
                table: "pedidodetalles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "fechaactualizacion",
                table: "pedidodetalles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "fechacreacion",
                table: "pedidodetalles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "actualizadopor",
                table: "pedidodetalles");

            migrationBuilder.DropColumn(
                name: "creadopor",
                table: "pedidodetalles");

            migrationBuilder.DropColumn(
                name: "eliminado",
                table: "pedidodetalles");

            migrationBuilder.DropColumn(
                name: "fechaactualizacion",
                table: "pedidodetalles");

            migrationBuilder.DropColumn(
                name: "fechacreacion",
                table: "pedidodetalles");
        }
    }
}
