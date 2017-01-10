using CSDemo.Models.Article;
using System.Collections.Generic;

namespace CSDemo.Contracts.Article
{
    interface IAccordion
    {
        #region Properties
        IEnumerable<AccordionData> Data { get; set; }

        string Title { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        #endregion

    }
}
