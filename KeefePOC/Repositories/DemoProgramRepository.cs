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
        List<State> DemoStates = new List<State>();
        List<Program> DemoPrograms = new List<Program>();

        public DemoProgramRepository()
        {
            DemoStates.Add(new State() { Code = "CA", Name = "California" });
            DemoStates.Add(new State() { Code = "OH", Name = "Ohio" });

            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe CA Facility", State = "CA", ProgramType = ProgramType.Jail, ExternalId = "1" });
            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe OH Facility", State = "OH", ProgramType = ProgramType.Jail, ExternalId = "2" });

            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe CA Hospital", State = "CA", ProgramType = ProgramType.Hospital, ExternalId = "3" });
            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe OH Hospital", State = "OH", ProgramType = ProgramType.Hospital, ExternalId = "4" });

            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe CA Music", State = "CA", ProgramType = ProgramType.Doc, ExternalId = "5" });
            DemoPrograms.Add(new Program() { CatalogId = Guid.NewGuid(), Name = "Keefe OH Music", State = "OH", ProgramType = ProgramType.Doc, ExternalId = "6" });

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

        public List<State> GetStates()
        {
            return DemoStates;
        }
    }
}
