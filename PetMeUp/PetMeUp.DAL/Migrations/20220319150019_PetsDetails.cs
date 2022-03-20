using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetMeUp.DAL.Migrations
{
    public partial class PetsDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Families_FamilyId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Species_PetSpecieId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Species_Groups_GroupId",
                table: "Species");

            migrationBuilder.DropIndex(
                name: "IX_Pets_PetSpecieId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "PetSpecieId",
                table: "Pets");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Species",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PicId",
                table: "Species",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Pets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpecieId",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Vat",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "FamilyId",
                table: "Groups",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Species_PicId",
                table: "Species",
                column: "PicId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_SpecieId",
                table: "Pets",
                column: "SpecieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Families_FamilyId",
                table: "Groups",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Species_SpecieId",
                table: "Pets",
                column: "SpecieId",
                principalTable: "Species",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Species_Groups_GroupId",
                table: "Species",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Species_Pics_PicId",
                table: "Species",
                column: "PicId",
                principalTable: "Pics",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Families_FamilyId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Species_SpecieId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Species_Groups_GroupId",
                table: "Species");

            migrationBuilder.DropForeignKey(
                name: "FK_Species_Pics_PicId",
                table: "Species");

            migrationBuilder.DropIndex(
                name: "IX_Species_PicId",
                table: "Species");

            migrationBuilder.DropIndex(
                name: "IX_Pets_SpecieId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "PicId",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "SpecieId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Vat",
                table: "Pets");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Species",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PetSpecieId",
                table: "Pets",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FamilyId",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_PetSpecieId",
                table: "Pets",
                column: "PetSpecieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Families_FamilyId",
                table: "Groups",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Species_PetSpecieId",
                table: "Pets",
                column: "PetSpecieId",
                principalTable: "Species",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Species_Groups_GroupId",
                table: "Species",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
