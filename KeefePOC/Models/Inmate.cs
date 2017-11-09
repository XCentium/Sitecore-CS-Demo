using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeefePOC.Models
{
    public class Inmate
    {
        public string Id { get; set; }
        
        public List<string> Restrictions { get; set; }
    }
}
