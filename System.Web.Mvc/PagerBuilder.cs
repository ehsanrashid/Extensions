namespace System.Web.Mvc
{
    using Routing;
    using Ajax;
    using Collections.Generic;
    using Text;
    using Globalization;

    public class PagerBuilder
    {
        const String CopyrightText = "\r\n<!--ASP.NET MvcPager 1.3 for ASP.NET MVC 1.0 Copyright © 2009-2010 Webdiyer (http://en.webdiyer.com)-->\r\n";
        const String JqueryScriptCheckItemKey = "_MvcPager_CheckjQueryScript";
        const String ScriptPageIndexName = "*_MvcPager_PageIndex_*";
        const String JqCheckScript = "if(typeof(jQuery)==\"undefined\"){alert(\"jQuery library not detected, please make sure it loaded prior to MvcPager!\");}";
        const String GoToPageScript = "function _MvcPager_GoToPage(_pib,_mp){var pageIndex;if(_pib.tagName==\"SELECT\"){pageIndex=_pib.options[_pib.selectedIndex].value;}else{pageIndex=_pib.value;var r=new RegExp(\"^\\\\s*(\\\\d+)\\\\s*$\");if(!r.test(pageIndex)){alert(\"%InvalidPageIndexErrorMessage%\");return;}else if(RegExp.$1<1||RegExp.$1>_mp){alert(\"%PageIndexOutOfRangeErrorMessage%\");return;}}var _hl=document.getElementById(_pib.id+'link').childNodes[0];var _lh=_hl.href;_hl.href=_lh.replace('" + ScriptPageIndexName + "',pageIndex);%ClickHandler%;_hl.href=_lh;}";
        const String KeyDownScript = "function _MvcPager_Keydown(e){var _kc,_pib;if(window.event){_kc=e.keyCode;_pib=e.srcElement;}else if(e.which){_kc=e.which;_pib=e.target;}var validKey=(_kc==8||_kc==46||_kc==37||_kc==39||(_kc>=48&&_kc<=57)||(_kc>=96&&_kc<=105));if(!validKey){if(_kc==13){ _MvcPager_GoToPage(_pib,%TotalPageCount%);}if(e.preventDefault){e.preventDefault();}else{event.returnValue=false;}}}";


        readonly HtmlHelper _htmlHelper;
        readonly AjaxHelper _ajaxHelper;
        readonly String _actionName;
        readonly String _controllerName;
        readonly String _routeName;

        readonly int _NoOfPages = 1;
        readonly int _pageIndex;
        readonly PagerOptions _pagerOptions;
        readonly RouteValueDictionary _routeValues;

        readonly int _startPageIndex = 1;
        readonly int _endPageIndex = 1;
        readonly bool _msAjaxPaging;
        readonly AjaxOptions _ajaxOptions;

        IDictionary<String, object> _htmlAttributes;

        /// <summary>
        /// used when PagedList is null
        /// </summary>
        public PagerBuilder(HtmlHelper htmlHelper, AjaxHelper ajaxHelper, PagerOptions pagerOptions, IDictionary<String, object> htmlAttributes)
        {
            if (default(PagerOptions) == pagerOptions) pagerOptions = new PagerOptions();
            
            _msAjaxPaging = (default(AjaxHelper) != ajaxHelper);
            _htmlHelper = htmlHelper;
            _ajaxHelper = ajaxHelper;
            _pagerOptions = pagerOptions;
            _htmlAttributes = htmlAttributes;
        }


        public PagerBuilder(HtmlHelper htmlHelper, AjaxHelper ajaxHelper, String actionName, String controllerName, int noOfPages, int pageIndex, PagerOptions pagerOptions, String routeName, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<String, object> htmlAttributes)
        {
            _msAjaxPaging = (default(AjaxHelper) != ajaxHelper);

            if (actionName.IsNullOrEmpty())
            {
                actionName = _msAjaxPaging
                    ? (String) ajaxHelper.ViewContext.RouteData.Values["action"]
                    : (String) htmlHelper.ViewContext.RouteData.Values["action"];
            }
            if (controllerName.IsNullOrEmpty())
            {
                controllerName = _msAjaxPaging
                    ? (String) ajaxHelper.ViewContext.RouteData.Values["controller"]
                    : (String) htmlHelper.ViewContext.RouteData.Values["controller"];
            }

            if (null == pagerOptions) pagerOptions = new PagerOptions();

            _htmlHelper = htmlHelper;
            _ajaxHelper = ajaxHelper;
            _actionName = actionName;
            _controllerName = controllerName;
            _NoOfPages = noOfPages;
            _pageIndex = pageIndex;
            _pagerOptions = pagerOptions;
            _routeName = routeName;
            _routeValues = routeValues;
            _ajaxOptions = ajaxOptions;
            _htmlAttributes = htmlAttributes;

            // start page index
            _startPageIndex = pageIndex - (pagerOptions.NumericPagerItemCount / 2);
            if (_startPageIndex + pagerOptions.NumericPagerItemCount > _NoOfPages)
                _startPageIndex = _NoOfPages + 1 - pagerOptions.NumericPagerItemCount;
            if (_startPageIndex < 1) _startPageIndex = 1;

            // end page index
            _endPageIndex = _startPageIndex + _pagerOptions.NumericPagerItemCount - 1;
            if (_endPageIndex > _NoOfPages) _endPageIndex = _NoOfPages;
        }

        //non Ajax pager builder
        public PagerBuilder(HtmlHelper htmlHelper, String actionName, String controllerName, int noOfPages, int pageIndex, PagerOptions pagerOptions, String routeName, RouteValueDictionary routeValues, IDictionary<String, object> htmlAttributes)
            : this(htmlHelper, null, actionName, controllerName, noOfPages, pageIndex, pagerOptions, routeName, routeValues, null, htmlAttributes) { }

        //jQuery Ajax pager builder
        public PagerBuilder(HtmlHelper htmlHelper, String actionName, String controllerName, int noOfPages, int pageIndex, PagerOptions pagerOptions, String routeName, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<String, object> htmlAttributes)
            : this(htmlHelper, null, actionName, controllerName, noOfPages, pageIndex, pagerOptions, routeName, routeValues, ajaxOptions, htmlAttributes) { }

        //Microsoft Ajax pager builder
        public PagerBuilder(AjaxHelper ajaxHelper, String actionName, String controllerName, int noOfPages, int pageIndex, PagerOptions pagerOptions, String routeName, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<String, object> htmlAttributes)
            : this(null, ajaxHelper, actionName, controllerName, noOfPages, pageIndex, pagerOptions, routeName, routeValues, ajaxOptions, htmlAttributes) { }


        // ---

        void AddPrevious(ICollection<PagerItem> collection)
        {
            var pagerItem = new PagerItem(_pagerOptions.PrevPageText, _pageIndex - 1, _pageIndex == 1, PagerItemType.PrevPage);
            if (!pagerItem.Disabled || (pagerItem.Disabled && _pagerOptions.ShowDisabledPagerItems))
                collection.Add(pagerItem);
        }

        void AddFirst(ICollection<PagerItem> collection)
        {
            PagerItem pagerItem = new PagerItem(_pagerOptions.FirstPageText, 1, _pageIndex == 1, PagerItemType.FirstPage);
            if (!pagerItem.Disabled || (pagerItem.Disabled && _pagerOptions.ShowDisabledPagerItems))
                collection.Add(pagerItem);
        }

        void AddMoreBefore(ICollection<PagerItem> collection)
        {
            if (_startPageIndex > 1 && _pagerOptions.ShowMorePagerItems)
            {
                var index = _startPageIndex - 1;
                if (index < 1) index = 1;
                PagerItem item = new PagerItem(_pagerOptions.MorePageText, index, false, PagerItemType.MorePage);
                collection.Add(item);
            }
        }

        void AddPageNumbers(ICollection<PagerItem> collection)
        {
            for (var pageIndex = _startPageIndex; pageIndex <= _endPageIndex; pageIndex++)
            {
                var text = pageIndex.ToString();
                if (pageIndex == _pageIndex && _pagerOptions.CurrentPageNumberFormatString.IsNotNullOrEmpty())
                    text = String.Format(_pagerOptions.CurrentPageNumberFormatString, text);
                else if (_pagerOptions.PageNumberFormatString.IsNotNullOrEmpty())
                    text = String.Format(_pagerOptions.PageNumberFormatString, text);

                var pagerItem = new PagerItem(text, pageIndex, false, PagerItemType.NumericPage);
                collection.Add(pagerItem);
            }
        }

        void AddMoreAfter(ICollection<PagerItem> collection)
        {
            if (_endPageIndex < _NoOfPages)
            {
                var index = _startPageIndex + _pagerOptions.NumericPagerItemCount;
                if (index > _NoOfPages) { index = _NoOfPages; }
                var item = new PagerItem(_pagerOptions.MorePageText, index, false, PagerItemType.MorePage);
                collection.Add(item);
            }
        }

        void AddNext(ICollection<PagerItem> collection)
        {
            var pagerItem = new PagerItem(_pagerOptions.NextPageText, _pageIndex + 1, _pageIndex >= _NoOfPages, PagerItemType.NextPage);
            if (!pagerItem.Disabled || (pagerItem.Disabled && _pagerOptions.ShowDisabledPagerItems))
                collection.Add(pagerItem);
        }

        void AddLast(ICollection<PagerItem> collection)
        {
            var pagerItem = new PagerItem(_pagerOptions.LastPageText, _NoOfPages, _pageIndex >= _NoOfPages, PagerItemType.LastPage);
            if (!pagerItem.Disabled || (pagerItem.Disabled && _pagerOptions.ShowDisabledPagerItems))
                collection.Add(pagerItem);
        }

        /// <summary>
        /// generate paging url
        /// </summary>
        /// <param name="pageIndex">page index to generate navigate url</param>
        /// <returns>navigated url for pager item</returns>
        String GenerateUrl(int pageIndex)
        {
            //return null if  page index larger than total page count or page index is current page index
            if (pageIndex > _NoOfPages || pageIndex == _pageIndex) return null;

            var routeValues = _routeValues ?? new RouteValueDictionary();

            // set route value of page index parameter name in url, pageIndex==0 is a special case
            routeValues[_pagerOptions.PageIndexParameterName] = (pageIndex == 0) ? ScriptPageIndexName : pageIndex.ToString();

            var reqQuery = _htmlHelper.ViewContext.HttpContext.Request.QueryString;
            foreach (String key in reqQuery.Keys)
            {
                if (key != _pagerOptions.PageIndexParameterName)
                    routeValues[key] = reqQuery[key];
            }
            // Add action
            routeValues["action"] = _actionName;

            // Add controller
            routeValues["controller"] = _controllerName;

            // Return link
            var urlHelper = new UrlHelper(_htmlHelper.ViewContext.RequestContext);

            return _routeName.IsNullOrEmpty()
                ? urlHelper.RouteUrl(routeValues)
                : urlHelper.RouteUrl(_routeName, routeValues);
        }

        String BuildGoToPageSection(ref String pagerScript)
        {
            const String ctrlIndexName = "_MvcPager_ControlIndex";
            var viewContext = _msAjaxPaging
                                      ? _ajaxHelper.ViewContext
                                      : _htmlHelper.ViewContext;

            int ctrlIndex;
            if (int.TryParse((String) viewContext.HttpContext.Items[ctrlIndexName], out ctrlIndex))
                ++ctrlIndex;
            viewContext.HttpContext.Items[ctrlIndexName] = ctrlIndex.ToString();

            var controlId = "_MvcPager_Ctrl" + ctrlIndex;
            var scriptLink = GenerateAnchor(new PagerItem("0", 0, false, PagerItemType.NumericPage));
            var linkClickTrigger = "_hl.click()";
            var browserName = viewContext.HttpContext.Request.Browser.Browser.ToLower();
            if (browserName.Contains("safari") || browserName.Contains("firefox"))
                linkClickTrigger =
                    "var evt=document.createEvent('MouseEvents');evt.initEvent('click',true,true);_hl.dispatchEvent(evt)";

            if (ctrlIndex == 0)
            {
                pagerScript += KeyDownScript.Replace("%TotalPageCount%", _NoOfPages.ToString()) + GoToPageScript.Replace("%InvalidPageIndexErrorMessage%",
                                                                      _pagerOptions.InvalidPageIndexErrorMessage).Replace("%PageIndexOutOfRangeErrorMessage%",
                                                       _pagerOptions.PageIndexOutOfRangeErrorMessage).Replace("%ClickHandler%", linkClickTrigger);
            }
            String onChangeScript = null;
            if (!_pagerOptions.ShowGoButton)
                onChangeScript = " onchange=\"_MvcPager_GoToPage(this," + _NoOfPages + ")\"";

            StringBuilder piBuilder = new StringBuilder();
            if (_pagerOptions.PageIndexBoxType == PageIndexBoxType.DropDownList)
            {
                // start page index
                var startIndex = _pageIndex - (_pagerOptions.MaximumPageIndexItems / 2);
                if (startIndex + _pagerOptions.MaximumPageIndexItems > _NoOfPages)
                    startIndex = _NoOfPages + 1 - _pagerOptions.MaximumPageIndexItems;
                if (startIndex < 1) startIndex = 1;

                // end page index
                var endIndex = startIndex + _pagerOptions.MaximumPageIndexItems - 1;
                if (endIndex > _NoOfPages) endIndex = _NoOfPages;
                piBuilder.AppendFormat("<select id=\"{0}\"{1}>", controlId + "_pib", onChangeScript);
                for (var i = startIndex; i <= endIndex; i++)
                {
                    piBuilder.AppendFormat("<option value=\"{0}\"", i);
                    if (i == _pageIndex)
                        piBuilder.Append(" selected=\"selected\"");
                    piBuilder.AppendFormat(">{0}</option>", i);
                }
                piBuilder.Append("</select>");
            }
            else
                piBuilder.AppendFormat(
                    "<input type=\"text\" id=\"{0}\" value=\"{1}\" onkeydown=\"_MvcPager_Keydown(event)\"{2}/>",
                    controlId + "_pib", _pageIndex, onChangeScript);
            String outHtml;
            if (_pagerOptions.PageIndexBoxWrapperFormatString.IsNotNullOrEmpty())
            {
                outHtml = String.Format(_pagerOptions.PageIndexBoxWrapperFormatString, piBuilder);
                piBuilder = new StringBuilder(outHtml);
            }

            if (_pagerOptions.ShowGoButton)
                piBuilder.AppendFormat(
                    "<input type=\"button\" value=\"{0}\" onclick=\"_MvcPager_GoToPage(document.getElementById('{1}')," + _NoOfPages + ")\"/>",
                    _pagerOptions.GoButtonText
                    , controlId + "_pib");
            piBuilder.AppendFormat("<span id=\"{0}\" style=\"display:none;width:0px;height:0px\">{1}</span>",
                                   controlId + "_piblink", scriptLink);
            if (_pagerOptions.GoToPageSectionWrapperFormatString.IsNotNullOrEmpty() ||
                _pagerOptions.PagerItemWrapperFormatString.IsNotNullOrEmpty())
            {
                outHtml = String.Format(_pagerOptions.GoToPageSectionWrapperFormatString ?? _pagerOptions.PagerItemWrapperFormatString, piBuilder);
            }
            else
                outHtml = piBuilder.ToString();
            return outHtml;
        }

        String GenerateAnchor(PagerItem item)
        {
            if (_msAjaxPaging)
            {
                var routeValues = GetCurrentRouteValues(_ajaxHelper.ViewContext);
                if (item.PageIndex == 0)
                    routeValues[_pagerOptions.PageIndexParameterName] = ScriptPageIndexName;
                else
                    routeValues[_pagerOptions.PageIndexParameterName] = item.PageIndex;

                return _routeName.IsNullOrEmpty()
                        ? _ajaxHelper.RouteLink(item.Text, routeValues, _ajaxOptions).ToString()
                        : _ajaxHelper.RouteLink(item.Text, _routeName, routeValues, _ajaxOptions).ToString();

            }
            String url = GenerateUrl(item.PageIndex);
            if (_pagerOptions.UseJqueryAjax)
            {
                StringBuilder ehBuilder = new StringBuilder();
                //ignore OnSuccess property
                if (_ajaxOptions.OnFailure.IsNotNullOrEmpty() || _ajaxOptions.OnBegin.IsNotNullOrEmpty())
                {
                    ehBuilder.Append("$.ajax({url:$(this).attr(\'href\'),success:function(data,status,xhr){$(\'#");
                    ehBuilder.Append(_ajaxOptions.UpdateTargetId).Append("\').html(data);}");
                    if (_ajaxOptions.OnFailure.IsNotNullOrEmpty())
                        ehBuilder.Append(",error:").Append(HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnFailure));
                    if (_ajaxOptions.OnBegin.IsNotNullOrEmpty())
                        ehBuilder.Append(",beforeSend:").Append(HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnBegin));
                    if (_ajaxOptions.OnComplete.IsNotNullOrEmpty())
                        ehBuilder.Append(",complete:").Append(HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnComplete));
                    ehBuilder.Append("});return false;");
                }
                else
                {
                    ehBuilder.Append("$(\'#").Append(_ajaxOptions.UpdateTargetId);
                    ehBuilder.Append("\').load($(this).attr(\'href\')");
                    if (_ajaxOptions.OnComplete.IsNotNullOrEmpty())
                        ehBuilder.Append(",").Append(HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnComplete));
                    ehBuilder.Append(");return false;");
                }

                return url.IsNullOrEmpty()
                           ? _htmlHelper.Encode(item.Text)
                           : String.Format(CultureInfo.InvariantCulture,
                                           "<a href=\"{0}\" onclick=\"{1}\">{2}</a>",
                                           GenerateUrl(item.PageIndex), ehBuilder, item.Text);
            }

            return String.Format("<a href=\"{0}\" onclick=\"{1}\"></a>",
                                    url, "window.open(this.attributes.getNamedItem('href').value,'_self')");

        }

        String GeneratePagerElement(PagerItem item)
        {
            //pager item link
            String url = GenerateUrl(item.PageIndex);
            if (item.Disabled) //first,last,next or previous page
                return CreateWrappedPagerElement(item, String.Format("<a disabled=\"disabled\">{0}</a>", item.Text));
            return CreateWrappedPagerElement(item, url.IsNullOrEmpty()
                                                 ? _htmlHelper.Encode(item.Text)
                                                 : String.Format("<a href='{0}'>{1}</a>", url, item.Text));
        }

        String GenerateJqAjaxPagerElement(PagerItem item)
        {
            if (item.Disabled)
                return CreateWrappedPagerElement(item, String.Format("<a disabled=\"disabled\">{0}</a>", item.Text));
            return CreateWrappedPagerElement(item, GenerateAnchor(item));
        }

        String GenerateMsAjaxPagerElement(PagerItem item)
        {
            if (item.PageIndex == _pageIndex && !item.Disabled) //current page index
                return CreateWrappedPagerElement(item, item.Text);
            if (item.Disabled)
                return CreateWrappedPagerElement(item, String.Format("<a disabled=\"disabled\">{0}</a>", item.Text));

            // return null if current page index less than 1 or large than total page count
            if (item.PageIndex < 1 || item.PageIndex > _NoOfPages)
                return null;

            return CreateWrappedPagerElement(item, GenerateAnchor(item));
        }

        String CreateWrappedPagerElement(PagerItem item, String el)
        {
            String navStr = el;
            switch (item.Type)
            {
            case PagerItemType.FirstPage:
            case PagerItemType.LastPage:
            case PagerItemType.NextPage:
            case PagerItemType.PrevPage:
                if (_pagerOptions.NavigationPagerItemWrapperFormatString.IsNotNullOrEmpty() ||
                    _pagerOptions.PagerItemWrapperFormatString.IsNotNullOrEmpty())
                    navStr = String.Format(
                            _pagerOptions.NavigationPagerItemWrapperFormatString ??
                            _pagerOptions.PagerItemWrapperFormatString, el);
                break;
            case PagerItemType.MorePage:
                if (_pagerOptions.MorePagerItemWrapperFormatString.IsNotNullOrEmpty() ||
                    _pagerOptions.PagerItemWrapperFormatString.IsNotNullOrEmpty())
                    navStr = String.Format(
                            _pagerOptions.MorePagerItemWrapperFormatString ??
                            _pagerOptions.PagerItemWrapperFormatString, el);
                break;
            case PagerItemType.NumericPage:
                if (item.PageIndex == _pageIndex &&
                    (_pagerOptions.CurrentPagerItemWrapperFormatString.IsNotNullOrEmpty() ||
                     _pagerOptions.PagerItemWrapperFormatString.IsNotNullOrEmpty())) //current page
                    navStr = String.Format(
                            _pagerOptions.CurrentPagerItemWrapperFormatString ??
                            _pagerOptions.PagerItemWrapperFormatString, el);
                else if (_pagerOptions.NumericPagerItemWrapperFormatString.IsNotNullOrEmpty() ||
                         _pagerOptions.PagerItemWrapperFormatString.IsNotNullOrEmpty())
                    navStr = String.Format(
                            _pagerOptions.NumericPagerItemWrapperFormatString ??
                            _pagerOptions.PagerItemWrapperFormatString, el);
                break;
            }

            return navStr + _pagerOptions.SeparatorHtml;
        }

        RouteValueDictionary GetCurrentRouteValues(ViewContext viewContext)
        {
            var routeValues = _routeValues ?? new RouteValueDictionary();
            var rq = viewContext.HttpContext.Request.QueryString;
            foreach (String key in rq.Keys)
            {
                // add other url query String parameters (not include PageIndexParameterName parameter value and X-Requested-With=XMLHttpRequest ajax parameter) to route value collection
                if (key != _pagerOptions.PageIndexParameterName &&
                    (key.ToLower() != "x-requested-with" && rq[key].ToLower() != "xmlhttprequest"))
                {
                    routeValues[key] = rq[key];
                }
            }
            // action
            routeValues["action"] = _actionName;
            // controller
            routeValues["controller"] = _controllerName;
            return routeValues;
        }

        /// <summary>
        /// render paging control
        /// </summary>
        /// <returns></returns>
        public String RenderPager()
        {
            //return null if total page count less than or equal to 1
            if (_NoOfPages <= 1 && _pagerOptions.AutoHide) return CopyrightText;
            //Display error message if pageIndex out of range
            if ((_pageIndex > _NoOfPages && _NoOfPages > 0) || _pageIndex < 1)
            {
                return String.Format("{0}<div style=\"color:red;font-weight:bold\">{1}</div>{0}",
                    CopyrightText, _pagerOptions.PageIndexOutOfRangeErrorMessage);
            }


            var pagerItems = new List<PagerItem>();
            //First page
            if (_pagerOptions.ShowFirstLast) AddFirst(pagerItems);

            // Prev page
            if (_pagerOptions.ShowPrevNext)
                AddPrevious(pagerItems);


            if (_pagerOptions.ShowNumericPagerItems)
            {
                if (_pagerOptions.AlwaysShowFirstLastPageNumber && _startPageIndex > 1)
                    pagerItems.Add(new PagerItem("1", 1, false, PagerItemType.NumericPage));

                // more page before numeric page buttons
                if (_pagerOptions.ShowMorePagerItems)
                    AddMoreBefore(pagerItems);

                // numeric page
                AddPageNumbers(pagerItems);

                // more page after numeric page buttons
                if (_pagerOptions.ShowMorePagerItems)
                    AddMoreAfter(pagerItems);

                if (_pagerOptions.AlwaysShowFirstLastPageNumber && _endPageIndex < _NoOfPages)
                    pagerItems.Add(new PagerItem(_NoOfPages.ToString(), _NoOfPages, false,
                                                 PagerItemType.NumericPage));
            }

            // Next page
            if (_pagerOptions.ShowPrevNext) AddNext(pagerItems);

            //Last page
            if (_pagerOptions.ShowFirstLast) AddLast(pagerItems);

            var sb = new StringBuilder();
            if (_msAjaxPaging)
            {
                foreach (PagerItem item in pagerItems)
                {
                    sb.Append(GenerateMsAjaxPagerElement(item));
                }
            }
            else if (_pagerOptions.UseJqueryAjax)
            {
                foreach (PagerItem item in pagerItems)
                {
                    sb.Append(GenerateJqAjaxPagerElement(item));
                }
            }
            else
            {
                foreach (PagerItem item in pagerItems)
                {
                    sb.Append(GeneratePagerElement(item));
                }
            }

            var tb = new TagBuilder(_pagerOptions.ContainerTagName);
            if (_pagerOptions.Id.IsNotNullOrEmpty())
                tb.GenerateId(_pagerOptions.Id);
            if (_pagerOptions.CssClass.IsNotNullOrEmpty() )
                tb.AddCssClass(_pagerOptions.CssClass);
            if (_pagerOptions.HorizontalAlign.IsNotNullOrEmpty())
            {
                var strAlign = "text-align:" + _pagerOptions.HorizontalAlign.ToLower();
                if (_htmlAttributes == null)
                    _htmlAttributes = new RouteValueDictionary { { "style", strAlign } };
                else
                {
                    if (_htmlAttributes.Keys.Contains("style"))
                        _htmlAttributes["style"] += ";" + strAlign;
                }
            }
            tb.MergeAttributes(_htmlAttributes, true);
            String pagerScript = String.Empty;
            if (_pagerOptions.UseJqueryAjax &&
                (String) _htmlHelper.ViewContext.HttpContext.Items[JqueryScriptCheckItemKey] != "1")
            {
                pagerScript = JqCheckScript;
                _htmlHelper.ViewContext.HttpContext.Items[JqueryScriptCheckItemKey] = "1";
            }
            if (_pagerOptions.ShowPageIndexBox)
            {
                sb.Append(BuildGoToPageSection(ref pagerScript));
            }
            else
                sb.Length -= _pagerOptions.SeparatorHtml.Length;
            tb.InnerHtml = sb.ToString();

            if (pagerScript.IsNotNullOrEmpty())
                pagerScript = String.Concat("<script language=\"javascript\" type=\"text/javascript\">", pagerScript, "</script>");
            return CopyrightText + pagerScript + tb.ToString(TagRenderMode.Normal) + CopyrightText;
        }

    }
}
