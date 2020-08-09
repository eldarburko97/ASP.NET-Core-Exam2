using Microsoft.EntityFrameworkCore.Migrations;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class BBBB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TakmicenjeUcesnik_DodjeljenPredmet_DodjeljenPredmetId",
                table: "TakmicenjeUcesnik");

            migrationBuilder.RenameColumn(
                name: "DodjeljenPredmetId",
                table: "TakmicenjeUcesnik",
                newName: "OdjeljenjeStavkaId");

            migrationBuilder.RenameIndex(
                name: "IX_TakmicenjeUcesnik_DodjeljenPredmetId",
                table: "TakmicenjeUcesnik",
                newName: "IX_TakmicenjeUcesnik_OdjeljenjeStavkaId");

            migrationBuilder.AddForeignKey(
                name: "FK_TakmicenjeUcesnik_OdjeljenjeStavka_OdjeljenjeStavkaId",
                table: "TakmicenjeUcesnik",
                column: "OdjeljenjeStavkaId",
                principalTable: "OdjeljenjeStavka",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TakmicenjeUcesnik_OdjeljenjeStavka_OdjeljenjeStavkaId",
                table: "TakmicenjeUcesnik");

            migrationBuilder.RenameColumn(
                name: "OdjeljenjeStavkaId",
                table: "TakmicenjeUcesnik",
                newName: "DodjeljenPredmetId");

            migrationBuilder.RenameIndex(
                name: "IX_TakmicenjeUcesnik_OdjeljenjeStavkaId",
                table: "TakmicenjeUcesnik",
                newName: "IX_TakmicenjeUcesnik_DodjeljenPredmetId");

            migrationBuilder.AddForeignKey(
                name: "FK_TakmicenjeUcesnik_DodjeljenPredmet_DodjeljenPredmetId",
                table: "TakmicenjeUcesnik",
                column: "DodjeljenPredmetId",
                principalTable: "DodjeljenPredmet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
