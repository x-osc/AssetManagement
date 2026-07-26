namespace AssetManagement.Common
{
    class SortHelper
    {
        public static string? NextOrder(string? currentSort, string? currentOrder, string column)
        {
            if (currentSort != column)
                return "asc";

            return currentOrder switch
            {
                null => "asc",
                "asc" => "desc",
                "desc" => null,
                _ => null
            };
        }
    }
}
