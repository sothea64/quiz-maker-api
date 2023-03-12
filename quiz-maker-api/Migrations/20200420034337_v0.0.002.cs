using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace quiz_maker_api.Migrations
{
    public partial class v00002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionAnswerResult",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    QuizResultId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    Feedback = table.Column<string>(nullable: true),
                    Score = table.Column<decimal>(nullable: false),
                    AnswerString = table.Column<string>(nullable: true),
                    SelectedAnswerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswerResult", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizResult",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    QuizId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    FinishedTime = table.Column<DateTime>(nullable: false),
                    TotalScore = table.Column<decimal>(nullable: false),
                    AlreadyCorrect = table.Column<bool>(nullable: false),
                    CorrectedBy = table.Column<string>(nullable: true),
                    CorrectByUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizResult", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionAnswerResult");

            migrationBuilder.DropTable(
                name: "QuizResult");
        }
    }
}
