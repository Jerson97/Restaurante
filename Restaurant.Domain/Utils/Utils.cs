using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Utils
{
    public static class Utils
    {
        public static int GetOffset(int pageSize, int pageNumber)
        {
            return (pageNumber - 1) * pageSize;
        }
    }
}
