using KeefePOC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeefePOC.Interfaces.Repositories
{
    public interface IProgramRepository
    {
        List<Program> GetPrograms();
        List<Program> GetPrograms(string state);

        Program GetProgram(string id);
    }
}
