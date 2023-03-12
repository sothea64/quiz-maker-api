using Microsoft.EntityFrameworkCore.Migrations;

namespace quiz_maker_api.Migrations
{
    public partial class v00005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ScoreWeight",
                table: "QuizResultAnswer",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoreWeight",
                table: "QuizResultAnswer");
        }
    }
}
