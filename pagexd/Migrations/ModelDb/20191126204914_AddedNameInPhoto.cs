using Microsoft.EntityFrameworkCore.Migrations;

namespace pagexd.Migrations.ModelDb
{
    public partial class AddedNameInPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Photos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Photos");
        }
    }
}
