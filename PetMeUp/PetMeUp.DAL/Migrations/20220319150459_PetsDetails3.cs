using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetMeUp.DAL.Migrations
{
    public partial class PetsDetails3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "Pics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pics_PetId",
                table: "Pics",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pics_Pets_PetId",
                table: "Pics",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pics_Pets_PetId",
                table: "Pics");

            migrationBuilder.DropIndex(
                name: "IX_Pics_PetId",
                table: "Pics");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "Pics");
        }
    }
}
