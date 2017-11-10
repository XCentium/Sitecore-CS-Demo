using KeefePOC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSDemo.Models.LandingPage
{
    public class HomeViewModel
    {
        public HomeViewModel(List<State> states)
        {
            States = new SelectList(states, nameof(State.Code), nameof(State.Name));
        }

        public SelectList States { get; set; }
        public SelectList Programs { get; set; } = new SelectList(new List<object>());

        [Display(Name = "State")]
        [Required(ErrorMessage = "Please select a state")]
        public string SelectedState { get; set; }

        [Display(Name = "Program")]
        [Required(ErrorMessage = "Please select a program")]
        public string SelectedProgram { get; set; }

        public string ErrorMessage { get; set; }
        public bool HasError { get { return !string.IsNullOrEmpty(ErrorMessage); } }

    }
}