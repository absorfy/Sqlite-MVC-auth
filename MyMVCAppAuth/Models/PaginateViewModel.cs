namespace MyMVCAppAuth.Models;

public class PaginateViewModel
{
    public int Page { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    
    public HeroesOrderByEnum OrderBy { get; set; } = HeroesOrderByEnum.Name;
    public bool IsAscending { get; set; } = true;
}