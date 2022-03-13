namespace CleemyTest.Controllers
{
    public class ControllerProblemClass
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Type { get; set; }
        public int StatusCode { get; set; }

        public ControllerProblemClass(string title, string detail, string type, int statusCode)
        {
            Title = title;
            Detail = detail;    
            Type = type;
            StatusCode = statusCode;
        }
    }
}
