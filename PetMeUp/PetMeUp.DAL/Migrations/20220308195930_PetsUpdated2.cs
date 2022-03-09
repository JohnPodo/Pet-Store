using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetMeUp.DAL.Migrations
{
    public partial class PetsUpdated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Life_Span",
                table: "Species",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Life_Span",
                table: "Species");
        }
    }
}
