using System.Collections.Generic;
using System.Linq;
using DynamicControlCreation.DBModel;
using DynamicControlCreation.Models.TemplatesAndParameters;
using Microsoft.AspNetCore.Mvc;

namespace DynamicControlCreation.Controllers
{
  public class TemplateController : Controller
  {
    public ActionResult Index()
    {
      List<Template> lstTemplates = new List<Template>();
      using (var ctx = new DynamicParameterContext())
      {
        lstTemplates = ctx.Templates.ToList();
      }
      return View(lstTemplates);
    }
    public ActionResult Create(int id = 0)
    {
      Template _Template = new Template();
      if (id != 0)
      {
        using (var ctx = new DynamicParameterContext())
        {
          _Template = ctx.Templates.Where(m => m.TemplateID == id).FirstOrDefault();
          var TemplateParameters = ctx.TemplateParameters.Where(m => m.TemplateID == id);
          if (TemplateParameters.Any())
            _Template.Parameters = TemplateParameters.ToList();
        }
      }
      return View(_Template);
    }

    [HttpPost]
    public ActionResult Create(Template _Template)
    {
      using (var ctx = new DynamicParameterContext())
      {
        if (_Template.TemplateID != 0)
        {
          Template temp = ctx.Templates.Where(m => m.TemplateID == _Template.TemplateID).FirstOrDefault();
          temp.Name = _Template.Name;
        }
        else
        {
          ctx.Templates.Add(_Template);
        }
        ctx.SaveChanges();

      }
      return RedirectToAction("Index");
    }
  }
}