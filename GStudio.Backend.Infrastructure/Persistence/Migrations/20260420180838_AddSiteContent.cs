using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GStudio.Backend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeroVideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeroVideoUrlMobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeroImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutTagline = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutTitleLine1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutTitleLine2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutTitleAccent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactWhatsapp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteContents", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteContents");
        }
    }
}
