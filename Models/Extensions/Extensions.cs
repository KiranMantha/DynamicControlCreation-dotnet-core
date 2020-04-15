using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DynamicControlCreation.Models.Extensions
{
  public static class Extensions
  {
    //public static IHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression)
    //{
    //    var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

    //    var enumType = Nullable.GetUnderlyingType(metadata.ModelType) ?? metadata.ModelType;

    //    var enumValues = Enum.GetValues(enumType).Cast<object>();

    //    var items = from enumValue in enumValues
    //                select new SelectListItem
    //                {
    //                    Text = enumValue.ToString(),
    //                    Value = ((int)enumValue).ToString(),
    //                    Selected = enumValue.Equals(metadata.Model)
    //                };

    //    return html.DropDownListFor(expression, items, "Select", null);
    //}

    public static IHtmlContent EnumDropDownListFor<TModel, TProperty, TEnum>(
                this IHtmlHelper<TModel> htmlHelper,
                Expression<Func<TModel, TProperty>> expression,
                TEnum selectedValue)
    {
      IEnumerable<object> values = Enum.GetValues(typeof(TEnum))
                                  .Cast<object>();
      IEnumerable<SelectListItem> items = from value in values
                                          select new SelectListItem()
                                          {
                                            Text = value.ToString(),
                                            Value = ((int)value).ToString(),
                                            Selected = (value.Equals(selectedValue))
                                          };
      return htmlHelper.DropDownListFor(expression, items);
      //return HtmlHelperSelectExtensions.DropDownListFor(htmlHelper, expression, items);
    }
  }
}