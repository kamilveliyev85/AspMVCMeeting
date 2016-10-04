using System.Web;
using System.Web.Optimization;

namespace AspMVCMeeting
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //BEGIN Master Page bundles
            bundles.Add(new ScriptBundle("~/bundles/masterPageGlobaljs").Include("~/assets/global/plugins/jquery.min.js",
                 "~/assets/global/plugins/bootstrap/js/bootstrap.min.js",
                 "~/assets/global/plugins/js.cookie.min.js",
                 "~/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                 "~/assets/global/plugins/jquery.blockui.min.js",
                 "~/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
                 "~/assets/global/scripts/app.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/masterPageLayoutjs").Include("~/assets/layouts/layout/scripts/layout.min.js",
            "~/assets/layouts/layout/scripts/demo.min.js",
            "~/assets/layouts/global/scripts/quick-sidebar.min.js",
            "~/assets/layouts/global/scripts/quick-nav.min.js"));

            bundles.Add(new StyleBundle("~/bundles/masterGlobalcss").Include("~/assets/global/plugins/font-awesome/css/font-awesome.min.css",
               "~/assets/global/plugins/simple-line-icons/simple-line-icons.min.css",
               "~/assets/global/plugins/bootstrap/css/bootstrap.min.css",
               "~/assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css",
               "~/assets/global/css/components.min.css",
               "~/assets/global/css/plugins.min.css"));

            bundles.Add(new StyleBundle("~/bundles/masterPageLayoutcss").Include("~/assets/layouts/layout/css/layout.min.css",
                "~/assets/layouts/layout/css/themes/darkblue.min.css",
                "~/assets/layouts/layout/css/custom.min.css"));

            //END Master Page bundles
        }
    }
}
