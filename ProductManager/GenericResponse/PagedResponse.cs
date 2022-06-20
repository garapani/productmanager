namespace ProductManager.GenericResponse
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }        
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, int totalCount)
        {
            PageNumber = pageNumber;
            TotalCount = totalCount;
            PageSize = pageSize;
            this.Data = data;            
        }
    }
}
