namespace AssetManagement.Common
{
    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public class SortFilter
    {
        public required string Sort { get; set; }
        public required SortDirection SortDirection { get; set; }
    }
}
