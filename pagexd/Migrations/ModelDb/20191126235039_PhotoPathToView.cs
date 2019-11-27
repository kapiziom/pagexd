using Microsoft.EntityFrameworkCore.Migrations;

namespace pagexd.Migrations.ModelDb
{
    public partial class PhotoPathToView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PathForView",
                table: "Photos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathForView",
                table: "Photos");
        }
    }
}
