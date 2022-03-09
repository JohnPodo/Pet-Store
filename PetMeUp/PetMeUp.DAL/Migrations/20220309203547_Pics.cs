using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetMeUp.DAL.Migrations
{
    public partial class Pics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PicId",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PicId",
                table: "Families",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_PicId",
                table: "Groups",
                column: "PicId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_PicId",
                table: "Families",
                column: "PicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Pics_PicId",
                table: "Families",
                column: "PicId",
                principalTable: "Pics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Pics_PicId",
                table: "Groups",
                column: "PicId",
                principalTable: "Pics",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Families_Pics_PicId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Pics_PicId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "Pics");

            migrationBuilder.DropIndex(
                name: "IX_Groups_PicId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Families_PicId",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "PicId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "PicId",
                table: "Families");
        }
    }
}
