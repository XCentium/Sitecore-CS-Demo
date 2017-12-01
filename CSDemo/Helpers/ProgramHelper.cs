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

            if (program == null && ConfigurationManager.AppSettings["DebugMode"] == "1")
            {
                var id = Guid.Parse(GetSelectedProgramId());

                return new ProgramModel
                {
                    ID = Guid.Parse(GetSelectedProgramId()),
                    QuarterlyOrderPriceLimit = GetProgramQuarterlyOrderPriceLimit(id),
                    QuarterlyOrderWeightLimit = GetProgramQuarterlyOrderWeightLimit(id)
                };
            }

            if (program != null)
            {
                program.QuarterlyOrderPriceLimit = GetProgramQuarterlyOrderPriceLimit(program.ID);
                program.QuarterlyOrderWeightLimit = GetProgramQuarterlyOrderWeightLimit(program.ID);
            }

            return program;
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
        }
    }
}