using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetMeUp.DAL.Migrations
{
    public partial class PetsDetails2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Species_SpecieId",
                table: "Pets");

            migrationBuilder.AlterColumn<int>(
                name: "SpecieId",
                table: "Pets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Species_SpecieId",
                table: "Pets",
                column: "SpecieId",
                principalTable: "Species",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Species_SpecieId",
                table: "Pets");

            migrationBuilder.AlterColumn<int>(
                name: "SpecieId",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Species_SpecieId",
                table: "Pets",
                column: "SpecieId",
                principalTable: "Species",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
