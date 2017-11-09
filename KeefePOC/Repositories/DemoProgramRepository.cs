using KeefePOC.Interfaces.Repositories;
using KeefePOC.Models;
using KeefePOC.Models.Enumerations;
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
            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe CA Facility", State = "CA", ProgramType = ProgramType.Jail });
            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe OH Facility", State = "OH", ProgramType = ProgramType.Jail });

            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe CA Hospital", State = "CA", ProgramType = ProgramType.Hospital });
            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe OH Hospital", State = "OH", ProgramType = ProgramType.Hospital });

            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe CA Music", State = "CA", ProgramType = ProgramType.Doc });
            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe OH Music", State = "OH", ProgramType = ProgramType.Doc });

        }

        public Program GetProgram(string id)
        {
            return DemoPrograms.First();
        }

        public List<Program> GetPrograms()
        {
            return DemoPrograms;
        }

        public List<Program> GetPrograms(string stateCode)
        {
            return DemoPrograms.Where(s => s.State.Equals(stateCode, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
