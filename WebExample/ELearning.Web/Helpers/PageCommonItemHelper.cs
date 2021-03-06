﻿using ELearning.DataAccess.Tools;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Web.Helpers
{
    public static class PageCommonItemHelper
    {
        /// <summary>
        /// 针对分页器 ListPageParameter 实例做的在页面上处理用于存放相关资料的便捷处理方法
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="pageParameter"></param>
        /// <returns></returns>
        public static HtmlString SetListPageParameter(this IHtmlHelper helper, ListPageParameter pageParameter)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append("<input type='hidden' name='对应的类型' id='lionTypeID' value='" + pageParameter.ObjectTypeID + "' />");
            htmlContent.Append("<input type='hidden' name='当前页码' id='lionPageIndex' value='" + pageParameter.PageIndex + "' />");
            htmlContent.Append("<input type='hidden' name='每页数据条数' id='lionPageSize' value='" + pageParameter.PageSize + "' /> ");
            htmlContent.Append("<input type='hidden' name='分页数量' id='lionPageAmount' value='" + pageParameter.PageAmount +"' />");
            htmlContent.Append("<input type='hidden' name='相关的对象的总数' id='lionObjectAmount' value='" + pageParameter.ObjectAmount +"' />");
            htmlContent.Append("<input type='hidden' name='当前的检索关键词' id='lionKeyword' value='" + pageParameter.Keyword + "' />");
            htmlContent.Append("<input type='hidden' name='排序属性' id='lionSortProperty' value='" + pageParameter.SortProperty + "' /> ");
            htmlContent.Append("<input type='hidden' name='排序方向' id='lionSortDesc' value='" + pageParameter.SortDesc + "' />");
            htmlContent.Append("<input type='hidden' name='当前焦点对象 ID' id='lionSelectedObjectID' value='" + pageParameter.SelectedObjectID + "' />");
            htmlContent.Append("<input type='hidden' name='当前是否为检索' id='lionIsSearch' value='" + pageParameter.IsSearch + "' />");

            return new HtmlString(htmlContent.ToString());

        }

        /// <summary>
        /// 将普通的字符串渲染为 html 页面效果的简单转换处理
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static HtmlString SetHtml(this IHtmlHelper helper, string Content)
        {
            return new HtmlString(Content);
        }

        public static HtmlString SetAdditionalRowForTable(this IHtmlHelper helper, int rowAmount,int colAmount)
       {
            var htmlContent = new StringBuilder();
            for (int i = 0; i < rowAmount; i++)
            {
                htmlContent.Append("<tr>");
                for (int j = 0; j < colAmount; j++)
                {
                    if(j==0)
                        htmlContent.Append("<td> # </td>");
                    else
                        htmlContent.Append("<td> </td>");
                }
                htmlContent.Append("<tr>");
            } 
            return new HtmlString(htmlContent.ToString());
        }

    }
}
