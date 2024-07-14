using System;
using System.Collections.Generic;

namespace iCare.AutoReference {
    public interface IProvideReference {
        object RefService { get; }
        IEnumerable<Type> RefTypes { get; }
        string RefID { get; }
    }
}