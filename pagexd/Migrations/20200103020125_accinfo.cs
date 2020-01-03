using Microsoft.EntityFrameworkCore.Migrations;

namespace pagexd.Migrations
{
    public partial class accinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccInfo",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccInfo",
                table: "AspNetUsers");
        }
    }
}
