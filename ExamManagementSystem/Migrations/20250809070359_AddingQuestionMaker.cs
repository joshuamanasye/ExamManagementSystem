using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddingQuestionMaker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionMakerId",
                table: "Exams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_QuestionMakerId",
                table: "Exams",
                column: "QuestionMakerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Users_QuestionMakerId",
                table: "Exams",
                column: "QuestionMakerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Users_QuestionMakerId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_QuestionMakerId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "QuestionMakerId",
                table: "Exams");
        }
    }
}
