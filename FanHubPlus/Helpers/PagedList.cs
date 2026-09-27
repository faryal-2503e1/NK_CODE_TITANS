using Microsoft.EntityFrameworkCore;

namespace FanHubPlus.Helpers
{
    public interface IPagedList
    {
        int Page { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
    }

    public class PagedList<T> : List<T>, IPagedList
    {
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }

        public PagedList(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            AddRange(items);
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
            TotalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
        }

        // Runs COUNT plus a single Skip/Take query, so only one page is ever loaded into memory.
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int page, int pageSize = 10)
        {
            if (page < 1) page = 1;

            var totalCount = await source.CountAsync();
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            if (page > totalPages) page = totalPages;

            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, totalCount, page, pageSize);
        }

        // For results that were already grouped/merged in memory (small, bounded sets only).
        public static PagedList<T> Create(IList<T> source, int page, int pageSize = 10)
        {
            if (page < 1) page = 1;
            var totalPages = Math.Max(1, (int)Math.Ceiling(source.Count / (double)pageSize));
            if (page > totalPages) page = totalPages;

            return new PagedList<T>(source.Skip((page - 1) * pageSize).Take(pageSize), source.Count, page, pageSize);
        }
    }
}
