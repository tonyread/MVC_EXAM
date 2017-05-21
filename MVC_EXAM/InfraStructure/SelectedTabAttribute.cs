using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_EXAM.InfraStructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method) ]
    public class SelectedTabAttribute:ActionFilterAttribute
    {
        private readonly string _selectedTab;
        public SelectedTabAttribute(string SelectedTab){

            _selectedTab = SelectedTab;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.SelectedTab = _selectedTab;
        }
    }
}