using EvoContacts.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvoContacts.ApplicationCore.Models
{
    public class PagedList<T> : IPager<T>
    {
        public List<T> Items { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get { return (TotalRecords + PageSize - 1) / PageSize; } }
    }
}
