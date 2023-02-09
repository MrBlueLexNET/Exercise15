namespace Lms.Core.ResourceParameters
{
    public interface IGamesResourceParameters
    {
        string? Name { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string? SearchQuery { get; set; }
    }
}