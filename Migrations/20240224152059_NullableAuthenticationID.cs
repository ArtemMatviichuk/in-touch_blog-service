using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogService.Migrations
{
    /// <inheritdoc />
    public partial class NullableAuthenticationID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_AuthenticationId",
                table: "UserProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "AuthenticationId",
                table: "UserProfiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_AuthenticationId",
                table: "UserProfiles",
                column: "AuthenticationId",
                unique: true,
                filter: "[AuthenticationId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_AuthenticationId",
                table: "UserProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "AuthenticationId",
                table: "UserProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_AuthenticationId",
                table: "UserProfiles",
                column: "AuthenticationId",
                unique: true);
        }
    }
}
