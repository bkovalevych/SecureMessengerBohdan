namespace SecureMessengerBohdan.Application.Wrappers
{
    public class PagingList<T>
    {
        public long TotalCount { get; set; }
        
        public int Skip { get; set; }

        public int Take { get; set; }

        public List<T> Items { get; set; } = new List<T>();
    }
}
