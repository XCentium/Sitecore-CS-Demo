using System;
using System.Configuration;
using System.Web;
using CSDemo.Models;
using Sitecore.Data;

namespace CSDemo.Helpers
{
    public class ProgramHelper
    {
        public static string GetSelectedProgramId()
        {
            if (ConfigurationManager.AppSettings["DebugMode"] == "1")
            {
                return "{41EAB12C-4403-473E-ABE3-EAB0A12287D1}";
            }

            return GetSelectedProgram()?.ID.ToString();
        }

        public static ProgramModel GetSelectedProgram()
        {
            var program = HttpContext.Current?.Session["SELECTED_PROGRAM"] as ProgramModel;

            var cookie = HttpContext.Current.Request.Cookies["SELECTED_PROGRAM"];

            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                if(ConfigurationManager.AppSettings["DebugMode"] == "1")
                {
                    var id = Guid.Parse(GetSelectedProgramId());

                    return new ProgramModel
                    {
                        ID = Guid.Parse(GetSelectedProgramId()),
                        QuarterlyOrderPriceLimit = GetProgramQuarterlyOrderPriceLimit(id),
                        QuarterlyOrderWeightLimit = GetProgramQuarterlyOrderWeightLimit(id)
                    };
                }
            }

            var programSaved = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<ProgramModel>(cookie.Value);

            //if (program == null && ConfigurationManager.AppSettings["DebugMode"] == "1")
            //{
            //    var id = Guid.Parse(GetSelectedProgramId());

            //    return new ProgramModel
            //    {
            //        ID = Guid.Parse(GetSelectedProgramId()),
            //        QuarterlyOrderPriceLimit = GetProgramQuarterlyOrderPriceLimit(id),
            //        QuarterlyOrderWeightLimit = GetProgramQuarterlyOrderWeightLimit(id)
            //    };
            //}

            if (programSaved != null)
            {
                programSaved.QuarterlyOrderPriceLimit = GetProgramQuarterlyOrderPriceLimit(programSaved.ID);
                programSaved.QuarterlyOrderWeightLimit = GetProgramQuarterlyOrderWeightLimit(programSaved.ID);
            }

            return programSaved;
        }

        private static double GetProgramQuarterlyOrderWeightLimit(Guid programId)
        {
            var item = Sitecore.Context.Database.GetItem(ID.Parse(programId));
            var program = GlassHelper.Cast<ProgramModel>(item);

            if (program != null)
            {
                return program.QuarterlyOrderWeightLimit;
            }

            return -1;
        }

        private static decimal GetProgramQuarterlyOrderPriceLimit(Guid programId)
        {
            var item = Sitecore.Context.Database.GetItem(ID.Parse(programId));
            var program = GlassHelper.Cast<ProgramModel>(item);

            if (program != null)
            {
                return program.QuarterlyOrderPriceLimit;
            }

            return -1;
        }

        public static void SaveSelectedProgram(ProgramModel program)
        {
            HttpContext.Current.Session["SELECTED_PROGRAM"] = program;

            if (program != null)
            {
                var cookie = HttpContext.Current.Request.Cookies["SELECTED_PROGRAM"];
                if (cookie != null)
                {
                    cookie.Value = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(program);
                    cookie.Expires = DateTime.Now.AddDays(365);
                    HttpContext.Current.Response.SetCookie(cookie); // updates existing cookie, cookies.add.. can cause multiple cookies
                }
                else
                {
                    var newCookie = new HttpCookie("SELECTED_PROGRAM")
                    {
                        Expires = DateTime.Now.AddDays(365),
                        Value = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(program)
                    };
                    HttpContext.Current.Response.SetCookie(newCookie);
                }
            }
        }
    }
}