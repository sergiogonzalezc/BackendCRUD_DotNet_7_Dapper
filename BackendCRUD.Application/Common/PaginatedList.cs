using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace BackendCRUD.Application.Common
{
    /// <summary>
    /// permite paginar. Usa tipo genérico 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //public class PaginatedList<T>
    //{
    //    public IReadOnlyCollection<T> Items { get; }
    //    public int PageNumber { get; }
    //    public int TotalPages { get; }
    //    public int TotalCount { get; }

    //    public PaginatedList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    //    {
    //        PageNumber = pageNumber;
    //        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    //        TotalCount = count;
    //        Items = items;
    //    }

    //    public bool HasPreviousPage => PageNumber > 1;

    //    public bool HasNextPage => PageNumber < TotalPages;

    //    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    //    {
    //        var count = await source.CountAsync();
    //        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

    //        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    //    }
    //}

    public class PaginatedList<TEntity>(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
    where TEntity : class
    {
        public int PageIndex { get; } = pageIndex;
        public int PageSize { get; } = pageSize;
        public long Count { get; } = count;
        public IEnumerable<TEntity> Data { get; } = data;
    }


}
