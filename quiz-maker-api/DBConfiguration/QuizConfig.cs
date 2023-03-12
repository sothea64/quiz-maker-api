using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using quiz_maker_models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_maker_api.DBConfiguration
{
    public class QuizConfig : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.Ignore(x => x.Questions);
            builder.Property(x => x.Code).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500).IsRequired();
            builder.Property(x => x.QuizDuration).HasColumnType("decimal(5,2)");
            builder.ConfigBaseModel();
        }
    }

    public class QuestionConfig : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.Ignore(x => x.AnswerSets);
            builder.Property(x => x.QuestionTitle).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Explaination).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Score).HasColumnType("decimal(5,2)");
            builder.ConfigBaseModel();
        }
    }

    public class AnswerSetConfig : IEntityTypeConfiguration<AnswerSet>
    {
        public void Configure(EntityTypeBuilder<AnswerSet> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(150).IsRequired();
            builder.Property(x => x.ScoreWeight).HasColumnType("decimal(5,2)");
            builder.Property(x => x.AnswerTip).HasMaxLength(500).IsRequired();
            builder.ConfigBaseModel();
        }
    }

    public class QuizResultConfig : IEntityTypeConfiguration<QuizResult>
    {
        public void Configure(EntityTypeBuilder<QuizResult> builder)
        {
            builder.Property(x => x.TotalScore).HasColumnType("decimal(5,2)");
            builder.Ignore(x => x.QuizResultAnswers);
            builder.Property(x => x.ScoredBy).HasMaxLength(100).IsRequired();
            builder.ConfigBaseModel();
        }
    }

    public class QuestionAnswerResultConfig : IEntityTypeConfiguration<QuizResultAnswer>
    {
        public void Configure(EntityTypeBuilder<QuizResultAnswer> builder)
        {
            builder.Property(x => x.Score).HasColumnType("decimal(5,2)");
            builder.Property(x => x.Feedback).HasMaxLength(600).IsRequired();
            builder.Property(x => x.AnswerText).HasMaxLength(500).IsRequired();
            builder.ConfigBaseModel();
        }
    }
}
