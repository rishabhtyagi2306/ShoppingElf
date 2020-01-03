using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace ShoppingELF.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Code removed for brevity
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/dist/shoppingELF/runtime*",
                      "~/Scripts/libs/polyfills*",
                      //"~/Scripts/libs/vendor*",
                      "~/Scripts/libs/main*"));
        }
    }
}