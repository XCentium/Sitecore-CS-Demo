using KeefePOC.Interfaces.Repositories;
using KeefePOC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeefePOC.Repositories
{
    public class DemoProgramRepository : IProgramRepository
    {
        List<Program> DemoPrograms = new List<Program>();

        public DemoProgramRepository()
        {
            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe CA" });
            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe OH" });
            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe Music" });

        }

        public Program GetProgram(string id)
        {
            return DemoPrograms.First();
        }

        public List<Program> GetPrograms()
        {
            return DemoPrograms;
        }
    }
}
