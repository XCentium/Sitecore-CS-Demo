using System;

namespace CSDemo.Contracts
{
    public interface IEditableBase
    {
        Guid ID { get; set; }

        string Path { get; set; }
    }
}
