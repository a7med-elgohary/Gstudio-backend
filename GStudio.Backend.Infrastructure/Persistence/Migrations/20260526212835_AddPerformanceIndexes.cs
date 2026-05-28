using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GStudio.Backend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Works",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Num",
                table: "Services",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ProjectRequests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "ProjectRequests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Works_Category",
                table: "Works",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Works_CreatedAt",
                table: "Works",
                column: "CreatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Services_Num",
                table: "Services",
                column: "Num");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRequests_Category",
                table: "ProjectRequests",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRequests_CreatedAt",
                table: "ProjectRequests",
                column: "CreatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRequests_Status",
                table: "ProjectRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLogos_CreatedAt",
                table: "ClientLogos",
                column: "CreatedAt",
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Works_Category",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Works_CreatedAt",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Services_Num",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_ProjectRequests_Category",
                table: "ProjectRequests");

            migrationBuilder.DropIndex(
                name: "IX_ProjectRequests_CreatedAt",
                table: "ProjectRequests");

            migrationBuilder.DropIndex(
                name: "IX_ProjectRequests_Status",
                table: "ProjectRequests");

            migrationBuilder.DropIndex(
                name: "IX_ClientLogos_CreatedAt",
                table: "ClientLogos");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Works",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Num",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ProjectRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "ProjectRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
