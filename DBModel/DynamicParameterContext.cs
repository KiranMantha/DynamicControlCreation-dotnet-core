using DynamicControlCreation.Models.TemplatesAndParameters;
using Microsoft.EntityFrameworkCore;

namespace DynamicControlCreation.DBModel
{
  public class DynamicParameterContext : DbContext
  {
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=DynamicParameters.db");        
    }

    public virtual DbSet<Template> Templates { get; set; }
    public virtual DbSet<TemplateParameters> TemplateParameters { get; set; }
    public virtual DbSet<TemplateParameterValues> TemplateParameterValues { get; set; }
    public virtual DbSet<TemplateParameterDefaults> TemplateParameterDefaults { get; set; }
  }
}