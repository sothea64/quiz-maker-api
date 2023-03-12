using Microsoft.EntityFrameworkCore.Migrations;

namespace quiz_maker_api.Migrations
{
    public partial class v00001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Quiz",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAlreadyUsed",
                table: "Question",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Question",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "Question",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ShowInQuizTime",
                table: "Question",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAlreadyUsed",
                table: "AnswerSet",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowInQuizTime",
                table: "AnswerSet",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "IsAlreadyUsed",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ShowInQuizTime",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "IsAlreadyUsed",
                table: "AnswerSet");

            migrationBuilder.DropColumn(
                name: "ShowInQuizTime",
                table: "AnswerSet");
        }
    }
}
