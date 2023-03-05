namespace AgileApp.Models.Common
{
    public class Response
    {
        public bool IsSuccess { get; set; }

        public string Error { get; set; }

        public static Response Succeeded()
            => new Response
            {
                IsSuccess = true
            };

        public static Response Failed(string error)
            => new Response
            {
                IsSuccess = false,
                Error = error
            };
    }

    public class Response<T>
    {
        public bool IsSuccess { get; set; }

        public string Error { get; set; }

        public T Data { get; set; }

        public static Response<T> Succeeded(T data)
            => new Response<T>
            {
                IsSuccess = true,
                Data = data
            };

        public static Response<T> Failed(string error)
            => new Response<T>
            {
                IsSuccess = false,
                Error = error
            };
    }
}
