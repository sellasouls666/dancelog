using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dancelog.Migrations
{
    /// <inheritdoc />
    public partial class FixLessonReturn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_AuthUsers_CompletedByTeacherId1",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_CompletedByTeacherId1",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "CompletedByTeacherId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "CompletedByTeacherId1",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Lessons");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompletedByTeacherId",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CompletedByTeacherId1",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Lessons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CompletedByTeacherId1",
                table: "Lessons",
                column: "CompletedByTeacherId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_AuthUsers_CompletedByTeacherId1",
                table: "Lessons",
                column: "CompletedByTeacherId1",
                principalTable: "AuthUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
