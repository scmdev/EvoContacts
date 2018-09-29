using EvoContacts.ApplicationCore.Enums;
using EvoContacts.ApplicationCore.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvoContacts.ApplicationCore.Models
{
    public class Result
    {
        [JsonIgnore]
        public string ErrorMessage { get; set; }

        [JsonIgnore]
        public bool RequestFailed { get { return !string.IsNullOrEmpty(ErrorMessage); } }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }
    }

    public class ListResult<T> : Result
    {
        public ListResult()
        {
            Items = new List<T>();
        }

        public List<T> Items { get; set; }

        public int TotalRecords { get { return Items.Count; } }
    }

    public class PagedListResult<T> : Result, IPager<T>
    {
        public PagedListResult(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;

            Items = new List<T>();
        }

        public List<T> Items { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get { return (TotalRecords + PageSize - 1) / PageSize; } }
    }
}
