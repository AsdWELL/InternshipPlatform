namespace InternshipPlatform.Application.Dtos.Pagination
{
    public class PageRequestParameters
    {
        private const int MaxPageSize = 100;
        
        public int PageIndex { get; set; } = 0;

        private int _pageSize = 20;

        public int PageSize
        {
            get => Math.Min(_pageSize, MaxPageSize);
            set => _pageSize = value;
        }
    }
}