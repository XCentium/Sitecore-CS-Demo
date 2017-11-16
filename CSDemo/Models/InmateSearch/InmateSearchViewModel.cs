using KeefePOC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSDemo.Models.InmateSearch
{
    public class InmateSearchViewModel
    {
        public Facility SelectedFacility { get; set; }

        [Display(Name = "Inmate Number")]
        public string InmateNumber { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "First Name")]
        public string LastName { get; set; }

        public string SelectedInmateId { get; set; }


        public string ErrorMessage { get; set; }
        public bool HasError { get { return !string.IsNullOrEmpty(ErrorMessage); } }
    }
}