using Microsoft.EntityFrameworkCore.Migrations;

namespace MaCoCo.Data.Migrations
{
    public partial class category2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "parentId",
                table: "Category",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "parentId",
                table: "Category");
        }
    }
}
