namespace CrmImobiliaria.Shared
{
    public class Result<T>
    {
        public T? Value { get; }
        public string? Error { get; }
        public bool IsSuccess { get; }

        private Result(bool isSuccess, T? value, string? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Failure(string error) => new(false, default, error);
    }
}
