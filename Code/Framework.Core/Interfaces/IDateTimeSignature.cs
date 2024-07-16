using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Interfaces
{
    public interface IDateTimeSignature : ICreationTimeSignature
    {
        #region Properties
        DateTime? FirstModificationDate { get; set; }
        DateTime? LastModificationDate { get; set; }
        #endregion
    }
}
