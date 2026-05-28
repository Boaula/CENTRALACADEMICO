using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academico.Migrations
{
    /// <inheritdoc />
    public partial class ResetSenhaToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetSenhaToken",
                table: "Professores",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetSenhaTokenExpiraEm",
                table: "Professores",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetSenhaToken",
                table: "Alunos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetSenhaTokenExpiraEm",
                table: "Alunos",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetSenhaToken",
                table: "Professores");

            migrationBuilder.DropColumn(
                name: "ResetSenhaTokenExpiraEm",
                table: "Professores");

            migrationBuilder.DropColumn(
                name: "ResetSenhaToken",
                table: "Alunos");

            migrationBuilder.DropColumn(
                name: "ResetSenhaTokenExpiraEm",
                table: "Alunos");
        }
    }
}
