using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpendWise.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NameOfMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_permissions_permissions_PermissionId",
                table: "permissions");

            migrationBuilder.DropIndex(
                name: "IX_permissions_PermissionId",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "PermissionId",
                table: "permissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PermissionId",
                table: "permissions",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "PermissionId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_permissions_PermissionId",
                table: "permissions",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_permissions_permissions_PermissionId",
                table: "permissions",
                column: "PermissionId",
                principalTable: "permissions",
                principalColumn: "Id");
        }
    }
}
