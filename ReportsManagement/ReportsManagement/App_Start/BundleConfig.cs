using System.Web;
using System.Web.Optimization;

namespace ReportsManagement
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Content/js/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery-2.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery.datatables").Include(
                        "~/Content/js/jquery.dataTables.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatables-bootstrap3").Include(
                        "~/Content/js/datatables-bootstrap3.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatables.responsive").Include(
                        "~/Content/js/datatables.responsive.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/js/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                        "~/Content/css/bootstrap.min.css"));
        }
    }
}