using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogService.Migrations
{
    /// <inheritdoc />
    public partial class AddModifiedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Posts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Comments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Comments");
        }
    }
}
