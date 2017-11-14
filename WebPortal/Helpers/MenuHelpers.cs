using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WebPortal.Models;

namespace WebPortal.Helpers
{
    public static class MenuHelpers
    {
        public static MvcHtmlString RenderDashMenu(this HtmlHelper helper, MenuViewModel model)
        {
            var result = new StringBuilder();
            if (model != null && model.RootMenu != null)
                foreach (var item in model.RootMenu)
                {
                    if (model.SubMenu.Count(p => p.ParentId == item.Id) > 0)
                    {
                        result.AppendLine("<li class='has-sub'>");
                        result.AppendLine("<a href='#" + item.Name + "' title='" + item.Description +
                                          "'>" + item.Name +
                                          "</a>");
                        result.AppendLine("<ul class='has-sub' role='menu'>");
                        foreach (var cp in model.SubMenu.Where(p => p.ParentId == item.Id))
                        {
                            result.AppendLine("<li>");
                            result.AppendLine("<a href='" + cp.Url + "' class='menu-btn'  title='" +
                                              cp.Description + "'>" + cp.Name + "</a>");

                            if (model.RootMenu.Count(p => p.ParentId == cp.Id) > 0)
                            {
                                result.AppendLine(" <ul class='has-sub' role='menu'>");
                                result.AppendLine(RenderSubMenu(cp.Id, item.Id, model.SubMenu.ToList()));
                                result.AppendLine("</ul>");
                            }
                            else
                            {
                                result.AppendLine("</li>");
                            }
                        }
                        result.AppendLine("</ul>");
                    }
                    else
                    {
                        result.AppendLine("<li >");
                        result.AppendLine("<a href='" + item.Url + "'  class='menu-btn' title='" +
                                          item.Description + "'>" + item.Name +
                                          "</a>");
                    }
                    result.AppendLine("</li>");
                }

            return new MvcHtmlString(result.ToString());
        }

        public static MvcHtmlString RenderMenu(this HtmlHelper helper, MenuViewModel model)
        {
            var result = new StringBuilder();
            if (model != null && model.RootMenu != null)
                foreach (var item in model.RootMenu)
                {
                    if (model.SubMenu.Count(p => p.ParentId == item.Id) > 0)
                    {
                        result.AppendLine("<li class='dropdown'>");
                        result.AppendLine("<a href='#" + item.Name +
                                          "' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-expanded='false' title='" +
                                          item.Description + "'>" + item.Name +
                                          " <span class='caret'></span></a>");
                        result.AppendLine("<ul class='dropdown-menu' role='menu'>");
                        foreach (var cp in model.SubMenu.Where(p => p.ParentId == item.Id))
                        {
                            result.AppendLine("<li>");
                            result.AppendLine("<a href='" + cp.Url + "' title='" +
                                              cp.Description + "'>" + cp.Name + "</a>");

                            if (model.RootMenu.Count(p => p.ParentId == cp.Id) > 0)
                            {
                                result.AppendLine(" <ul class='dropdown-menu' role='menu'>");
                                result.AppendLine(RenderSubMenu(cp.Id, item.Id, model.SubMenu.ToList()));
                                result.AppendLine("</ul>");
                            }
                            else
                            {
                                result.AppendLine("</li>");
                            }
                        }
                        result.AppendLine("</ul>");
                    }
                    else
                    {
                        result.AppendLine("<li>");
                        result.AppendLine("<a href='" + item.Url + "' title='" +
                                          item.Description + "'>" + item.Name +
                                          "</a>");
                    }
                    result.AppendLine("</li>");
                }

            return new MvcHtmlString(result.ToString());
        }

        private static string RenderSubMenu(int itemId, int itemParentId, List<MenuModel> subMenus)
        {
            var result = new StringBuilder();

            if (subMenus != null)
                foreach (var item in subMenus.Where(p => p.ParentId == itemParentId))
                {
                    result.AppendLine("<a href='/" + item.Controller + '/' + item.Action + "' class='menu-btn' title='" +
                                      item.Description + "'>" + item.Name +
                                      "</a>");
                    if (subMenus.Count(p => p.ParentId == itemId) > 0)
                    {
                        result.AppendLine(" <ul class='dropdown-menu has-sub' role='menu'>");

                        result.AppendLine(RenderSubMenu(item.Id, itemId, subMenus));

                        result.AppendLine("</ul>");
                    }
                    else
                    {
                        result.AppendLine("</li>");
                    }
                }

            return result.ToString();
        }
    }
}