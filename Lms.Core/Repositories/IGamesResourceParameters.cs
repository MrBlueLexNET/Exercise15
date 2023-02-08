namespace Lms.Api.ResourceParameters
{
    public interface IGamesResourceParameters
    {
        string? MainCategory { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string? SearchQuery { get; set; }
    }
}