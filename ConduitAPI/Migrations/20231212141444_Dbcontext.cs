using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConduitAPI.Migrations
{
    public partial class Dbcontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Conten",
                schema: "article",
                table: "Article",
                newName: "Content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                schema: "article",
                table: "Article",
                newName: "Conten");
        }
    }
}
