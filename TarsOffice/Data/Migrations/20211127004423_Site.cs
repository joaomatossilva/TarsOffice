using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TarsOffice.Data.Migrations
{
    public partial class Site : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SiteId",
                table: "Teams",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SiteId",
                table: "Bookings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Site",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    EmailDomain = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Site", x => x.Id);
                });

            // Manual adding data
            migrationBuilder.InsertData(
                table: "Site",
                columns: new[] { "Id", "Name", "EmailDomain" },
                values: new object[] { Guid.NewGuid(), "Public", "*" });
            
            var farfetchGuid = Guid.NewGuid();
            migrationBuilder.InsertData(
                table: "Site",
                columns: new[] { "Id", "Name", "EmailDomain" },
                values: new object[] { farfetchGuid, "Farfetch", "farfetch.com" });

            //no need to worry about SQL injection, since parameter is safe
            migrationBuilder.Sql($"UPDATE Bookings SET SiteId = '{farfetchGuid}'");
            migrationBuilder.Sql($"UPDATE Teams SET SiteId = '{farfetchGuid}'");

            //drop default
            migrationBuilder.AlterColumn<Guid>(
                name: "SiteId",
                table: "Bookings",
                oldDefaultValue: true,
                defaultValue: null);

            migrationBuilder.AlterColumn<Guid>(
                name: "SiteId",
                table: "Teams",
                oldDefaultValue: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SiteId",
                table: "Teams",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SiteId",
                table: "Bookings",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Site_SiteId",
                table: "Bookings",
                column: "SiteId",
                principalTable: "Site",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Site_SiteId",
                table: "Teams",
                column: "SiteId",
                principalTable: "Site",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Site_SiteId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Site_SiteId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "Site");

            migrationBuilder.DropIndex(
                name: "IX_Teams_SiteId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_SiteId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Bookings");
        }
    }
}
