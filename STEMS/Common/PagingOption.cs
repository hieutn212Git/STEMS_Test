namespace Common
{
    public class PagingOption
    {
        public PagingOption()
        {
            PageIndex = 1;
            PageSize = 100;
        }

        /// <summary>
        /// Default pageSize = 10
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Default pageIndex = 1
        /// </summary>
        /// </summary>
        public int PageIndex { get; set; }

        public Dictionary<string, string> Sorter { get; set; } = new Dictionary<string, string>();

    }
}
