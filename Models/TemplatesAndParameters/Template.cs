using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DynamicControlCreation.Models.TemplatesAndParameters
{
  public class Template
  {
    [Key]
    public int TemplateID { get; set; }
    public string Name { get; set; }
    public List<TemplateParameters> Parameters { get; set; }
  }
}