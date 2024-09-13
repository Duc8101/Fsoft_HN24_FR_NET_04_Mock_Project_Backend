namespace Phone_Shop.Common.Paging
{
    public class Pagination<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalElement {  get; set; }
        public List<T> List { get; set; } = new List<T>();
        public int NumberPage => (int) Math.Ceiling((double)TotalElement / PageSize);
    }
}
