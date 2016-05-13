using System.Web;
using System.Web.Optimization;

namespace ubakip
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Site.css",
                      "~/Content/rating.css",                                          
                      "~/Content/slider.css",
                       "~/Content/custom.css",
                      "~/Content/ngDialog-theme-default.min.css",
                      "~/Content/ngDialog.min.css"));

            bundles.Add(new StyleBundle("~/Content/light").Include(
              "~/Content/lightTheme.css"
            ));

            bundles.Add(new StyleBundle("~/Content/dark").Include(
                 "~/Content/darkTheme.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                       "~/Scripts/angular.min.js",
                        "~/Scripts/angularbase.js"));

            bundles.Add(new ScriptBundle("~/bundles/interact").Include(
                        "~/Scripts/interact.js"));
        }
    }
}
