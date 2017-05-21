using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_EXAM.InfraStructure
{
    public class PagedData<T>:IEnumerable<T>
    {
        private readonly IEnumerable<T> _currentItem;
        public int TotalCount { get; private set; }
        public int Page { get; private set; }
        public int PerPage { get; private set; }
        public int TotalPages { get; private set; }

        public bool HasNextPage { get; private set; }
        public bool HasPreviousPage { get; private set; }

        public PagedData(IEnumerable<T> currentItems, int totalcount, int page, int perpage)
        {
            _currentItem = currentItems;
            TotalCount = totalcount;
            Page = page;
            PerPage = perpage;
            TotalPages = (int)Math.Ceiling((float)TotalCount / PerPage);
            HasNextPage = Page < TotalPages;
            HasPreviousPage = Page > 1;
        }
        public int NextPage()
        {
            if (!HasNextPage)
            {
                throw new InvalidOperationException();
            }
            return Page + 1;
        }
        public int PreviousPage()
        {
            if (!HasPreviousPage)
            {
                throw new InvalidOperationException();
            }
            return Page - 1;
        }   
        public IEnumerator<T> GetEnumerator()
        {
            return _currentItem.GetEnumerator();
        }

      IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}