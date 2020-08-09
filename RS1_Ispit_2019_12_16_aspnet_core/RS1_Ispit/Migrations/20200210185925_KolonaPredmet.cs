using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class KolonaPredmet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PredmetId",
                table: "PopravniIspit",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspit_PredmetId",
                table: "PopravniIspit",
                column: "PredmetId");

            migrationBuilder.AddForeignKey(
                name: "FK_PopravniIspit_Predmet_PredmetId",
                table: "PopravniIspit",
                column: "PredmetId",
                principalTable: "Predmet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PopravniIspit_Predmet_PredmetId",
                table: "PopravniIspit");

            migrationBuilder.DropIndex(
                name: "IX_PopravniIspit_PredmetId",
                table: "PopravniIspit");

            migrationBuilder.DropColumn(
                name: "PredmetId",
                table: "PopravniIspit");
        }
    }
}
