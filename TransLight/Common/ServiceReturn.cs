namespace TransLight.Services.Common
{
    public class ServiceReturn<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public T? Data { get; set; }
    }
}
