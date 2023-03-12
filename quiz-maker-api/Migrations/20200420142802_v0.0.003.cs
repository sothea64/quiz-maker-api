using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace quiz_maker_api.Migrations
{
    public partial class v00003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionAnswerResult");

            migrationBuilder.DropColumn(
                name: "AlreadyCorrect",
                table: "QuizResult");

            migrationBuilder.DropColumn(
                name: "CorrectByUserId",
                table: "QuizResult");

            migrationBuilder.DropColumn(
                name: "CorrectedBy",
                table: "QuizResult");

            migrationBuilder.DropColumn(
                name: "IsAlreadyUsed",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ShowInQuizTime",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "IsAlreadyUsed",
                table: "AnswerSet");

            migrationBuilder.DropColumn(
                name: "PossibleCorrectTextAnswer",
                table: "AnswerSet");

            migrationBuilder.DropColumn(
                name: "ShowInQuizTime",
                table: "AnswerSet");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalScore",
                table: "QuizResult",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<bool>(
                name: "IsScored",
                table: "QuizResult",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ScoredBy",
                table: "QuizResult",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ScoredByUserId",
                table: "QuizResult",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "QuizDuration",
                table: "Quiz",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Quiz",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Quiz",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Quiz",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RandomQuestions",
                table: "Quiz",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "Question",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionTitle",
                table: "Question",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Explaination",
                table: "Question",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequire",
                table: "Question",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AnswerSet",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnswerTip",
                table: "AnswerSet",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ScoreWeight",
                table: "AnswerSet",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "QuizResultAnswer",
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
                    Feedback = table.Column<string>(maxLength: 600, nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AnswerText = table.Column<string>(maxLength: 500, nullable: false),
                    SelectedAnswerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizResultAnswer", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizResultAnswer");

            migrationBuilder.DropColumn(
                name: "IsScored",
                table: "QuizResult");

            migrationBuilder.DropColumn(
                name: "ScoredBy",
                table: "QuizResult");

            migrationBuilder.DropColumn(
                name: "ScoredByUserId",
                table: "QuizResult");

            migrationBuilder.DropColumn(
                name: "RandomQuestions",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "IsRequire",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "AnswerTip",
                table: "AnswerSet");

            migrationBuilder.DropColumn(
                name: "ScoreWeight",
                table: "AnswerSet");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalScore",
                table: "QuizResult",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AddColumn<bool>(
                name: "AlreadyCorrect",
                table: "QuizResult",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CorrectByUserId",
                table: "QuizResult",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CorrectedBy",
                table: "QuizResult",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "QuizDuration",
                table: "Quiz",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Quiz",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Quiz",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Quiz",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "Question",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<string>(
                name: "QuestionTitle",
                table: "Question",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Explaination",
                table: "Question",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AddColumn<bool>(
                name: "IsAlreadyUsed",
                table: "Question",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowInQuizTime",
                table: "Question",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AnswerSet",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150);

            migrationBuilder.AddColumn<bool>(
                name: "IsAlreadyUsed",
                table: "AnswerSet",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PossibleCorrectTextAnswer",
                table: "AnswerSet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowInQuizTime",
                table: "AnswerSet",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "QuestionAnswerResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    AnswerString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    QuizResultId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SelectedAnswerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswerResult", x => x.Id);
                });
        }
    }
}
