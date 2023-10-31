namespace ShoppingList.WebMVC.Models
{
    public class ErrorResponse
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string traceId { get; set; }
        public Dictionary<string, List<string>> errors { get; set; }
    }
}
