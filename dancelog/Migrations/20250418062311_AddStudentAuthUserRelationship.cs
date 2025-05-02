using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dancelog.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentAuthUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthUserId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "AuthUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_AuthUserId",
                table: "Students",
                column: "AuthUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AuthUsers_AuthUserId",
                table: "Students",
                column: "AuthUserId",
                principalTable: "AuthUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_AuthUsers_AuthUserId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_AuthUserId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AuthUserId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "AuthUsers");
        }
    }
}
