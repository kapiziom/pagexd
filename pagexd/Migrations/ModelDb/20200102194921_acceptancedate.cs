using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pagexd.Migrations.ModelDb
{
    public partial class acceptancedate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptanceDate",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptanceDate",
                table: "Posts");
        }
    }
}
