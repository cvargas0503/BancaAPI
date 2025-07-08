namespace BancaAPI.Application.DTOs.Common
{ 
        public class PaginatedResult<T>
        {
            public int Total { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
            public List<T> Items { get; set; } = new();
        }

}
