﻿using System.Web;
using System.Web.Optimization;

namespace Dauber
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                         "~/Scripts/jquery.blockUI.js",
                         "~/Scripts/sweetalert.min.js"
                        ));

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
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Content/sweetalert.css",
                      "~/Content/zocial.css"));

            bundles.Add(new StyleBundle("~/Content/onepagewonder").Include(
                "~/Content/one-page-wonder.css"));

            bundles.Add(new StyleBundle("~/Content/Testimonials").Include(
                     "~/Content/testimonials.css"));

            bundles.Add(new ScriptBundle("~/bundles/ClientsIndex").Include(
              "~/Scripts/clientsIndex.js"));

            bundles.Add(new ScriptBundle("~/bundles/PlanChange").Include(
             "~/Scripts/planChange.js"));

            bundles.Add(new ScriptBundle("~/bundles/CardChange").Include(
           "~/Scripts/cardChange.js"));

            bundles.Add(new ScriptBundle("~/bundles/Testimonials").Include(
          "~/Scripts/testimonials.js"));
        }
    }
}
