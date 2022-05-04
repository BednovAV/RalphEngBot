using System.Collections.Generic;

namespace Entities.Navigation
{
    public class Page<T>
    {
        public int Number { get; set; }
        public List<T> Data { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
