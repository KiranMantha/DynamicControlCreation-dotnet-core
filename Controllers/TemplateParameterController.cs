using System.Collections.Generic;
using DynamicControlCreation.DBModel;
using DynamicControlCreation.Models.TemplatesAndParameters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DynamicControlCreation.Controllers
{
  public class TemplateParameterController : Controller
  {
    public ActionResult Index(int ID)
    {
      TemplateParameters _TemplateParameter = new TemplateParameters();
      if (ID != 0)
      {
        using (var ctx = new DynamicParameterContext())
        {
          _TemplateParameter = ctx.TemplateParameters.Where(m => (m.TemplateParameterID == ID)).FirstOrDefault();

          //Get Template Parameter Defaults Based On TemplateParameterID
          _TemplateParameter.ParameterDefaults = ctx.TemplateParameterDefaults.Where(m => (m.TemplateParameterID == ID)).ToList();
          if (_TemplateParameter.ParameterDefaults.Count > 0)
          {
            if (_TemplateParameter.AllowMultiple)
              _TemplateParameter.Values = _TemplateParameter.ParameterDefaults.Select(m => m.Value).ToList();
            else
              _TemplateParameter.Values.Add(_TemplateParameter.ParameterDefaults.Select(m => m.Value).LastOrDefault());
          }

          //Get Template Parameter Values Based On TemplateParameterID
          _TemplateParameter.ParameterValues = ctx.TemplateParameterValues.Where(m => (m.TemplateParameterID == ID)).ToList();
          if (_TemplateParameter.ParameterValues.Count > 0)
          {
            var av = new List<SelectListItem>();
            foreach (string s in _TemplateParameter.Values)
            {
              var y = _TemplateParameter.ParameterValues.FirstOrDefault(x => x.Value == s);
              if (y != null)
                av.Add(new SelectListItem() { Text = y.Label, Value = y.Value, Selected = true });
            }

            av.AddRange(_TemplateParameter.ParameterValues.Where(i => !_TemplateParameter.Values.Contains(i.Value)).Select(y => new SelectListItem() { Text = y.Label, Value = y.Value, Selected = false }));

            _TemplateParameter.AvailableValues = av;
          }
        }
      }
      return View(_TemplateParameter);
    }

    public ActionResult Create(int TID, int ID = 0)
    {
      TemplateParameters _TemplateParameter = new TemplateParameters();
      if (ID != 0)
      {
        using (var ctx = new DynamicParameterContext())
        {
          _TemplateParameter = ctx.TemplateParameters.Where(m => (m.TemplateParameterID == ID && m.TemplateID == TID)).FirstOrDefault();
          _TemplateParameter.ParameterValues = ctx.TemplateParameterValues.Where(m => (m.TemplateParameterID == ID)).ToList();
          _TemplateParameter.ParameterDefaults = ctx.TemplateParameterDefaults.Where(m => (m.TemplateParameterID == ID)).ToList();
          _TemplateParameter.Types = (TemplateParameterType)_TemplateParameter.Type;
        }
      }
      _TemplateParameter.TemplateID = TID;
      return View(_TemplateParameter);
    }

    [HttpPost]
    public ActionResult Create(TemplateParameters _TemplateParameters)
    {
      using (var ctx = new DynamicParameterContext())
      {
        //Check If New Model Or Existing Model
        if (_TemplateParameters.TemplateParameterID != 0)
        {
          TemplateParameters temp = ctx.TemplateParameters.Where(m => m.TemplateParameterID == _TemplateParameters.TemplateParameterID).FirstOrDefault();
          temp.Name = _TemplateParameters.Name;
          temp.Value = _TemplateParameters.Value;
          temp.Type = _TemplateParameters.Type;
          temp.AllowMultiple = _TemplateParameters.AllowMultiple;
        }
        else
        {
          ctx.TemplateParameters.Add(_TemplateParameters);
        }

        //Add Parameter Values
        if (_TemplateParameters.ParameterValues.Count > 0)
        {
          //Remove All Template Parameter Values From DB Based On Template Parameter ID
          var tpv = ctx.TemplateParameterValues.Where(x => x.TemplateParameterID == _TemplateParameters.TemplateParameterID);
          if (tpv.Any())
            ctx.TemplateParameterValues.RemoveRange(tpv);

          //Remove Deleted Values On ClientSide
          _TemplateParameters.ParameterValues.RemoveAll(x => x.Deleted == true);

          //Add New Template Parameter Values To DB          
          foreach (TemplateParameterValues t in _TemplateParameters.ParameterValues)
          {
            ctx.TemplateParameterValues.Add(t);
          }
        }

        //Add Parameter Defaults
        if (_TemplateParameters.ParameterDefaults.Count > 0)
        {
          //Remove All Template Parameter Values From DB Based On Template Parameter ID
          var tpd = ctx.TemplateParameterDefaults.Where(x => x.TemplateParameterID == _TemplateParameters.TemplateParameterID);
          if (tpd.Any())
            ctx.TemplateParameterDefaults.RemoveRange(tpd);

          //Remove Deleted Values On ClientSide
          _TemplateParameters.ParameterDefaults.RemoveAll(x => x.Deleted == true);

          //Add New Template Parameter Values To DB
          foreach (TemplateParameterDefaults t in _TemplateParameters.ParameterDefaults)
          {
            if (_TemplateParameters.ParameterValues.Any(x => x.Value == t.Value))
              ctx.TemplateParameterDefaults.Add(t);
            else
            {
              ModelState.AddModelError("", "Some of the default values are not found in available values. please check once.");
              break;
            }
          }
        }

        //Save Changes To DB
        if (ModelState.IsValid)
          ctx.SaveChanges();
        else
          return View(_TemplateParameters);
      }
      return RedirectToAction("Index", new { ID = _TemplateParameters.TemplateParameterID });
    }

    [HttpPost]
    public ActionResult AddAvailableValues(TemplateParameters _TemplateParameters)
    {
      _TemplateParameters.ParameterValues.Add(new TemplateParameterValues() { TemplateParameterID = _TemplateParameters.TemplateParameterID });
      return View("Create", _TemplateParameters);
    }

    [HttpPost]
    public ActionResult AddDefaultValues(TemplateParameters _TemplateParameters)
    {
      if ((_TemplateParameters.ParameterDefaults.Count == 0) || (_TemplateParameters.ParameterDefaults.Count > 0 && _TemplateParameters.AllowMultiple == true))
      {
        _TemplateParameters.ParameterDefaults.Add(new TemplateParameterDefaults() { TemplateParameterID = _TemplateParameters.TemplateParameterID });
        return View("Create", _TemplateParameters);
      }
      else
      {
        return Content("100");
      }
      return View();
    }
  }
}