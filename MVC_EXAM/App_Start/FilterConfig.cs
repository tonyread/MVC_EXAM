using MVC_EXAM.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_EXAM.App_Start
{
    public class FilterConfig
    {

        public static void RegisterGlobalFilters(GlobalFilterCollection filter)
        {
            filter.Add(new TransactionFilter());
            filter.Add(new HandleErrorAttribute());
        }
    }
}