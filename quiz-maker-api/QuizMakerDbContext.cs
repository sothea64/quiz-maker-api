using Microsoft.EntityFrameworkCore;
using quiz_maker_api.DBHelpers;
using quiz_maker_models.Models;

namespace quiz_maker_api
{
    public class QuizMakerDbContext : DbContext
    {
        public QuizMakerDbContext(DbContextOptions<QuizMakerDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        /*
         * Company Section
         */
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }

        /*
         * Quiz Section
         */
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AnswerSet> AnswerSets { get; set; }

        /*
         * Quiz Result Section
         */
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<QuizResultAnswer> QuestionAnswerResults { get; set; }

        /*
         * Security Section
         */
        public DbSet<User> Users { get; set; }
    }
}
