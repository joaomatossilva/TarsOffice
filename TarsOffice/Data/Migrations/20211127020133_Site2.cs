using Microsoft.EntityFrameworkCore.Migrations;

namespace TarsOffice.Data.Migrations
{
    public partial class Site2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Site_SiteId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Site_SiteId",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Site",
                table: "Site");

            migrationBuilder.RenameTable(
                name: "Site",
                newName: "Sites");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sites",
                table: "Sites",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Sites_SiteId",
                table: "Bookings",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Sites_SiteId",
                table: "Teams",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Sites_SiteId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Sites_SiteId",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sites",
                table: "Sites");

            migrationBuilder.RenameTable(
                name: "Sites",
                newName: "Site");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Site",
                table: "Site",
                column: "Id");

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
    }
}
