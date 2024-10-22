﻿namespace tienda_catalogo_api.Endpoints.Commons.Responses;

public class PaginatedResponse<T>
{
    public IEnumerable<T> Data { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool HasNext => Page * PageSize < TotalCount;
    public bool HasPrevious => Page > 1;
}