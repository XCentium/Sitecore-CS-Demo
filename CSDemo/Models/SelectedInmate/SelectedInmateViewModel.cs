using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.SelectedInmate
{
    public class SelectedInmateViewModel
    {
        public bool IsHippa { get; set; }
        public KeefePOC.Models.Inmate SelectedInmate { get; set; }
    }
}