using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using UCRS.Models;

namespace UCRS.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString Login(this HtmlHelper htmlHelper, LoginStudentViewModel student)
        {
            // Get viewmodel property - Reflection
            string fullName = student.GetType().FullName;
            PropertyInfo[] viewModelProperty = Type.GetType(fullName).GetProperties();

            // Add Fieldset
            var fieldset = new TagBuilder("fieldset");
            // Add Legend
            var propertyLegend = new TagBuilder("legend");
            string legendText = "Please enter your " + viewModelProperty[0].Name.ToLower();
            for (int i = 1; i < viewModelProperty.Length; i++)
            {
                legendText += " and " + viewModelProperty[i].Name.ToLower();
            }
            propertyLegend.SetInnerText(legendText);
            fieldset.InnerHtml = propertyLegend.ToString();

            // Add Properties
            for (int i = 0; i < viewModelProperty.Length; i++)
            {
                // Property label
                var propertyLabelDiv = new TagBuilder("div");
                propertyLabelDiv.AddCssClass("editor-label");
                var propertyLabelLabel = new TagBuilder("label");
                propertyLabelLabel.MergeAttribute("for", viewModelProperty[i].Name);

                // get DataAnotation [Display Name] for Property  
                DisplayAttribute propertyDisplayAttribute = (DisplayAttribute)viewModelProperty[i]
                                        .GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();
                string displayName = propertyDisplayAttribute.Name;
                propertyLabelLabel.SetInnerText(displayName);
                propertyLabelDiv.InnerHtml = propertyLabelLabel.ToString();
                // Property input
                var propertyInputDiv = new TagBuilder("div");
                propertyInputDiv.AddCssClass("editor-field");
                var propertyInput = new TagBuilder("input");
                propertyInput.AddCssClass("text-box single-line");
                propertyInput.MergeAttribute("id", viewModelProperty[i].Name);
                if (viewModelProperty[i].Name.ToLower() == "password")
                {
                    propertyInput.MergeAttribute("type", "password");
                }
                else
                {
                    propertyInput.MergeAttribute("type", "text");
                }
                propertyInput.MergeAttribute("value", Decode(GetPropValue(student, viewModelProperty[i].Name)));
                propertyInput.MergeAttribute("name", viewModelProperty[i].Name);
                propertyInputDiv.InnerHtml = propertyInput.ToString();
                // Add class delta
                var classDelta = new TagBuilder("div");
                classDelta.AddCssClass("delta");
                fieldset.InnerHtml += propertyLabelDiv.ToString() + propertyInputDiv.ToString() + classDelta.ToString();
            }
            // Add Button
            var inputButton = new TagBuilder("input");
            inputButton.MergeAttribute("id", "formBtn");
            inputButton.MergeAttribute("class", "formBtn");
            inputButton.MergeAttribute("type", "submit");
            inputButton.MergeAttribute("value", "Log In");
            fieldset.InnerHtml += inputButton.ToString();

            MvcHtmlString result = MvcHtmlString.Create(fieldset.ToString());
            return result;
        }

        public static string GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null).ToString();
        }

        public static string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }
    }
}