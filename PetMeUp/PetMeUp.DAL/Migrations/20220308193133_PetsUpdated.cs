using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetMeUp.DAL.Migrations
{
    public partial class PetsUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Species_Families_FamilyId",
                table: "Species");

            migrationBuilder.RenameColumn(
                name: "FamilyId",
                table: "Species",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Species_FamilyId",
                table: "Species",
                newName: "IX_Species_GroupId");

            migrationBuilder.AddColumn<double>(
                name: "MaximumFemaleHeight",
                table: "Species",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaximumMaleWeight",
                table: "Species",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinimumFemaleHeight",
                table: "Species",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinimumMaleWeight",
                table: "Species",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "Species",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamilyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_FamilyId",
                table: "Groups",
                column: "FamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Species_Groups_GroupId",
                table: "Species",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Species_Groups_GroupId",
                table: "Species");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropColumn(
                name: "MaximumFemaleHeight",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "MaximumMaleWeight",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "MinimumFemaleHeight",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "MinimumMaleWeight",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "Origin",
                table: "Species");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Species",
                newName: "FamilyId");

            migrationBuilder.RenameIndex(
                name: "IX_Species_GroupId",
                table: "Species",
                newName: "IX_Species_FamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Species_Families_FamilyId",
                table: "Species",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
