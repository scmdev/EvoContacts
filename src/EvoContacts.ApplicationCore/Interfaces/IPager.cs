﻿using EvoContacts.ApplicationCore.Entities;
using EvoContacts.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoContacts.ApplicationCore.Interfaces
{
    public interface IPager<T>
    {
        List<T> Items { get; set; }

        int Page { get; set; }
        int PageSize { get; set; }
        int NumItems { get; } //could be less than PageSize if last page
        int TotalRecords { get; set; }
        int TotalPages { get; }
    }
}
