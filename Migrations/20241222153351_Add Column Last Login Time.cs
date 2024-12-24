using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginLogout.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnLastLoginTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDatetime",
                table: "UserAccounts",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginDatetime",
                table: "UserAccounts");
        }
    }
}
