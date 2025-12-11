using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Restaurant.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fechacreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fechaactualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    creadopor = table.Column<int>(type: "integer", nullable: true),
                    actualizadopor = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categorias", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    pinhash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "platos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    precio = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    categoriaid = table.Column<int>(type: "integer", nullable: false),
                    fechacreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fechaactualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    creadopor = table.Column<int>(type: "integer", nullable: true),
                    actualizadopor = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_platos", x => x.id);
                    table.ForeignKey(
                        name: "fk_platos_categorias_categoriaid",
                        column: x => x.categoriaid,
                        principalTable: "categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mesas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numeromesa = table.Column<int>(type: "integer", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    mozoid = table.Column<int>(type: "integer", nullable: true),
                    fechacreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fechaactualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    creadopor = table.Column<int>(type: "integer", nullable: true),
                    actualizadopor = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mesas", x => x.id);
                    table.ForeignKey(
                        name: "fk_mesas_usuarios_mozoid",
                        column: x => x.mozoid,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "usuarioroles",
                columns: table => new
                {
                    usuarioid = table.Column<int>(type: "integer", nullable: false),
                    rolid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuarioroles", x => new { x.usuarioid, x.rolid });
                    table.ForeignKey(
                        name: "fk_usuarioroles_roles_rolid",
                        column: x => x.rolid,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_usuarioroles_usuarios_usuarioid",
                        column: x => x.usuarioid,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pedidos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tipopedido = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    mesaid = table.Column<int>(type: "integer", nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    clientenombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    clientetelefono = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    fechaentrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    total = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    mozoid = table.Column<int>(type: "integer", nullable: true),
                    repartidorid = table.Column<int>(type: "integer", nullable: true),
                    fechacreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fechaactualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    creadopor = table.Column<int>(type: "integer", nullable: true),
                    actualizadopor = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pedidos", x => x.id);
                    table.ForeignKey(
                        name: "fk_pedidos_mesas_mesaid",
                        column: x => x.mesaid,
                        principalTable: "mesas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pedidos_usuarios_mozoid",
                        column: x => x.mozoid,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pedidos_usuarios_repartidorid",
                        column: x => x.repartidorid,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "direcciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pedidoid = table.Column<int>(type: "integer", nullable: false),
                    nombrecliente = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    telefono = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    direccioncompleta = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    referencia = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_direcciones", x => x.id);
                    table.ForeignKey(
                        name: "fk_direcciones_pedidos_pedidoid",
                        column: x => x.pedidoid,
                        principalTable: "pedidos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pedidodetalles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pedidoid = table.Column<int>(type: "integer", nullable: false),
                    platoid = table.Column<int>(type: "integer", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false),
                    precio = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pedidodetalles", x => x.id);
                    table.ForeignKey(
                        name: "fk_pedidodetalles_pedidos_pedidoid",
                        column: x => x.pedidoid,
                        principalTable: "pedidos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pedidodetalles_platos_platoid",
                        column: x => x.platoid,
                        principalTable: "platos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_direcciones_pedidoid",
                table: "direcciones",
                column: "pedidoid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_mesas_mozoid",
                table: "mesas",
                column: "mozoid");

            migrationBuilder.CreateIndex(
                name: "ix_pedidodetalles_pedidoid",
                table: "pedidodetalles",
                column: "pedidoid");

            migrationBuilder.CreateIndex(
                name: "ix_pedidodetalles_platoid",
                table: "pedidodetalles",
                column: "platoid");

            migrationBuilder.CreateIndex(
                name: "ix_pedidos_mesaid",
                table: "pedidos",
                column: "mesaid");

            migrationBuilder.CreateIndex(
                name: "ix_pedidos_mozoid",
                table: "pedidos",
                column: "mozoid");

            migrationBuilder.CreateIndex(
                name: "ix_pedidos_repartidorid",
                table: "pedidos",
                column: "repartidorid");

            migrationBuilder.CreateIndex(
                name: "ix_platos_categoriaid",
                table: "platos",
                column: "categoriaid");

            migrationBuilder.CreateIndex(
                name: "ix_roles_nombre",
                table: "roles",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_usuarioroles_rolid",
                table: "usuarioroles",
                column: "rolid");

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_email",
                table: "usuarios",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "direcciones");

            migrationBuilder.DropTable(
                name: "pedidodetalles");

            migrationBuilder.DropTable(
                name: "usuarioroles");

            migrationBuilder.DropTable(
                name: "pedidos");

            migrationBuilder.DropTable(
                name: "platos");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "mesas");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
