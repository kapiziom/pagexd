using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pagexd.Migrations.ModelDb
{
    public partial class comment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_PageUser_PageUserId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "PageUser");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PageUserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "PageUserId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "CreationDate",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditDate",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostIDref",
                table: "Comments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "EditDate",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "PostIDref",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Comments");

            migrationBuilder.AddColumn<Guid>(
                name: "PageUserId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PageUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PageUserId",
                table: "Comments",
                column: "PageUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_PageUser_PageUserId",
                table: "Comments",
                column: "PageUserId",
                principalTable: "PageUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
