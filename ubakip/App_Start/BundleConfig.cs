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
                      "~/Content/custom.css"));
        }
    }
}
