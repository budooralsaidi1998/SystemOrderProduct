﻿using SystemProductOrder.models;

namespace SystemProductOrder.DTO
{
    
        public class PagedResult<T>
        {
            public List<Product> Items { get; set; } // The list of items for the current page
            public int TotalItems { get; set; } // Total number of items across all pages
            public int PageSize { get; set; } // Number of items per page
            public int CurrentPage { get; set; } // The current page number

            public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize); // Calculate total pages
        }
    
}