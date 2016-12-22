using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Dapper.Extensions.Repository.SqlGenerator
{
    internal enum SelectType
    {
        All,
        Top,
        Count
    }
}
