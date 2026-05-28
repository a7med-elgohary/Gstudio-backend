using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GStudio.Backend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tagline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameMeaning = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DecisionMakers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndustryTraits = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deadline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoundingDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LaunchDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetAudience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Competitors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoTypes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Colors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedTraits = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LikedLogosReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dislikes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRequests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectRequests");
        }
    }
}
