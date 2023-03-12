using Microsoft.EntityFrameworkCore.Metadata.Builders;
using quiz_maker_models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_maker_api.DBConfiguration
{
    public static class ConfigsHelper
    {
        public static void ConfigBaseModel<T>(this EntityTypeBuilder<T> builder) where T : BaseModel
        {
            builder.Property(x => x.CreatedBy).HasMaxLength(150).IsRequired();
        }
    }
}
