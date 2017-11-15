using KeefePOC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.InmateSearch
{
    public class SelectInmateViewModel
    {
        public Program SelectedProgram { get; set; }
        public Facility SelectedFacility { get; set; }
        public KeefePOC.Models.Inmate SelectedInmate { get; set; }

        public string SelectedInmateId { get; set; }
    }
}