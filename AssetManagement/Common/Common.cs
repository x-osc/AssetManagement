namespace AssetManagement.Common
{
    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public class SortFilter
    {
        public required string SortBy { get; set; }
        public required SortDirection SortDirection { get; set; }
    }
}
