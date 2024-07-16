using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Interfaces
{
    public interface IEntityIdentity<TPrimeryKey>
    {
        #region Properties
        public TPrimeryKey Id { get; set; }
        #endregion
    }
}
