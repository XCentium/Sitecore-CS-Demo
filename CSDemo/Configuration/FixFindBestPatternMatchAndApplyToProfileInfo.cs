using Sitecore.Cintel;
using Sitecore.Cintel.Commons;
using Sitecore.Cintel.Configuration;
using Sitecore.Cintel.Reporting;
using Sitecore.Cintel.Reporting.Processors;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using System;
using System.Collections.Generic;
using System.Data;

namespace CSDemo.Configuration
{
    public class FixFindBestPatternMatchAndApplyToProfileInfo : ReportProcessorBase
    {
        public override void Process(ReportProcessorArgs args)
        {
            DataTable resultTable = GrabPreliminaryResultsFromCurrentReport(args);
            Assert.IsNotNull(resultTable, "Result table for {0} could not be found.", args.ReportParameters.ViewName);
            ApplyPatternsToResultTable(args, resultTable);
        }

        private static ViewParameters GetParametersForRetrievingBestPattern(ReportProcessorArgs args, DataRow row)
        {
            ViewParameters viewParameters = new ViewParameters();
            viewParameters.SortFields = new List<SortCriterion>()
      {
        new SortCriterion(Sitecore.Cintel.Reporting.Contact.ProfilePatternMatch.Schema.PatternGravityShare.Name, SortDirection.Desc)
      };
            viewParameters.PageSize = 1;
            viewParameters.ViewName = "profile-pattern-matches";
            viewParameters.ViewEntityId = DataRowExtensions.Field<Guid>(row, Sitecore.Cintel.Reporting.Contact.ProfileInfo.Schema.ProfileId.Name).ToString();
            viewParameters.ContactId = args.ReportParameters.ContactId;
            viewParameters.AdditionalParameters = new Dictionary<string, object>()
      {
        {
          "VisitId",
          (object) DataRowExtensions.Field<Guid>(row, Sitecore.Cintel.Reporting.Contact.ProfileInfo.Schema.LatestVisitId.Name).ToString()
        }
      };
            return viewParameters;
        }

        private static DataTable GrabPreliminaryResultsFromCurrentReport(ReportProcessorArgs args)
        {
            return args.ResultTableForView;
        }

        private bool ApplyPatternToOneProfile(ReportProcessorArgs args, DataRow profileRow)
        {
            bool flag = true;
            if (DataRowExtensions.Field<Guid>(profileRow, Sitecore.Cintel.Reporting.Contact.ProfileInfo.Schema.ProfileId.Name) == Guid.Empty)
                flag = false;
            ViewParameters retrievingBestPattern = GetParametersForRetrievingBestPattern(args, profileRow);
            DataTable dataTable = CustomerIntelligenceManager.ViewProvider.GenerateContactView(retrievingBestPattern).Data.Dataset[retrievingBestPattern.ViewName];
            if (dataTable.Rows != null && dataTable.Rows.Count != 0)
            {
                if (!this.TryFillData<Guid>(profileRow, Sitecore.Cintel.Reporting.Contact.ProfileInfo.Schema.BestMatchedPatternId, dataTable.Rows[0], Sitecore.Cintel.Reporting.Contact.ProfilePatternMatch.Schema.PatternId.Name) || !this.TryFillData<string>(profileRow, Sitecore.Cintel.Reporting.Contact.ProfileInfo.Schema.BestMatchedPatternDisplayName, dataTable.Rows[0], Sitecore.Cintel.Reporting.Contact.ProfilePatternMatch.Schema.PatternDisplayName.Name) || !this.TryFillData<double>(profileRow, Sitecore.Cintel.Reporting.Contact.ProfileInfo.Schema.BestMatchedPatternGravityShare, dataTable.Rows[0], Sitecore.Cintel.Reporting.Contact.ProfilePatternMatch.Schema.PatternGravityShare.Name))
                    flag = false;
            }
            else
            {
                flag = false;
            }

            return flag;
        }

        private void ApplyPatternsToResultTable(ReportProcessorArgs args, DataTable resultTable)
        {
            bool flag = false;
            foreach (DataRow profileRow in DataTableExtensions.AsEnumerable(resultTable))
                flag = !this.ApplyPatternToOneProfile(args, profileRow);
            if (!flag)
                return;
            ReportProcessorBase.LogNotificationForView(args.ReportParameters.ViewName, new NotificationMessage()
            {
                Id = 13,
                MessageType = NotificationTypes.Error,
                Text = Translate.Text("One or more data entries are missing due to invalid data")
            });
        }
    }
}