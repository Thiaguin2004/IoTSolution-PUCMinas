using IoTSolution.Models;

public class PagedLeiturasViewModel
{
    public int TotalItems { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public List<LeiturasModel> Items { get; set; }
}