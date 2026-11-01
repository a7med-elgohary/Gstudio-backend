using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GStudio.Backend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSocialMediaLinksToSiteContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BehanceUrl",
                table: "SiteContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "SiteContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramUrl",
                table: "SiteContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "SiteContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TikTokUrl",
                table: "SiteContents",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BehanceUrl",
                table: "SiteContents");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "SiteContents");

            migrationBuilder.DropColumn(
                name: "InstagramUrl",
                table: "SiteContents");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "SiteContents");

            migrationBuilder.DropColumn(
                name: "TikTokUrl",
                table: "SiteContents");
        }
    }
}
