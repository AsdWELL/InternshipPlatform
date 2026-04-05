using InternshipPlatform.Application.Dtos.Pagination;

namespace InternshipPlatform.Application.Mappers
{
    public static class PagedResponseMapper
    {
        public static PagedResponse<R> ToPagedResponse<T, R>(
            this PagedResult<T> pagedResult,
            PageRequestParameters parameters,
            Func<T, R> mapItemToResponse)
        {
            return new PagedResponse<R>
            {
                PageIndex = parameters.PageIndex,
                PageSize = parameters.PageSize,
                TotalCount = pagedResult.TotalCount,
                Items = pagedResult.Items.Select(x => mapItemToResponse(x)).ToList()
            };
        }
    }
}