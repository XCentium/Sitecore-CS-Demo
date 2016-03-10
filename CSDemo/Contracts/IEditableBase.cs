using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDemo.Contracts
{
    public interface IEditableBase
    {
        Guid ID { get; set; }

        string Path { get; set; }
    }
}
