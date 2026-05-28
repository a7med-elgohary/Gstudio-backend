using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GStudio.Backend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectRequestFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "ProjectRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Images",
                table: "ProjectRequests");
        }
    }
}
