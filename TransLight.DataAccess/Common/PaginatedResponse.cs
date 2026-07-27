using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransLight.DataAccess.Common
{
    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}