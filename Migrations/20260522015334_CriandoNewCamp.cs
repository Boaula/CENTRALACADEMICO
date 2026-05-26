using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academico.Migrations
{
    /// <inheritdoc />
    public partial class CriandoNewCamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Periodo",
                keyValue: null,
                column: "Periodo",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Periodo",
                table: "Disciplinas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CargaHoraria",
                table: "Disciplinas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CodigoDiario",
                table: "Disciplinas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CodigoDisciplina",
                table: "Disciplinas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Grau",
                table: "Disciplinas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Horario",
                table: "Disciplinas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LocalAula",
                table: "Disciplinas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Disciplinas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeEtapas",
                table: "Disciplinas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalAulas",
                table: "Disciplinas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Turno",
                table: "Disciplinas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CargaHoraria",
                table: "Disciplinas");

            migrationBuilder.DropColumn(
                name: "CodigoDiario",
                table: "Disciplinas");

            migrationBuilder.DropColumn(
                name: "CodigoDisciplina",
                table: "Disciplinas");

            migrationBuilder.DropColumn(
                name: "Grau",
                table: "Disciplinas");

            migrationBuilder.DropColumn(
                name: "Horario",
                table: "Disciplinas");

            migrationBuilder.DropColumn(
                name: "LocalAula",
                table: "Disciplinas");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Disciplinas");

            migrationBuilder.DropColumn(
                name: "QuantidadeEtapas",
                table: "Disciplinas");

            migrationBuilder.DropColumn(
                name: "TotalAulas",
                table: "Disciplinas");

            migrationBuilder.DropColumn(
                name: "Turno",
                table: "Disciplinas");

            migrationBuilder.AlterColumn<string>(
                name: "Periodo",
                table: "Disciplinas",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
