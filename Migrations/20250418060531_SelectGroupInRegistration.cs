using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dancelog.Migrations
{
    /// <inheritdoc />
    public partial class SelectGroupInRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "AuthUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthUsers_GroupId",
                table: "AuthUsers",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthUsers_Groups_GroupId",
                table: "AuthUsers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthUsers_Groups_GroupId",
                table: "AuthUsers");

            migrationBuilder.DropIndex(
                name: "IX_AuthUsers_GroupId",
                table: "AuthUsers");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "AuthUsers");
        }
    }
}
