namespace WeatherAppServer.Models
{
    public record GenericResponse<T>(bool IsSuccess, T? Result, string? Error) where T: class
    {
        public static GenericResponse<T> Success(T result)
        {
            return new GenericResponse<T>(true, result, null);
        }

        public static GenericResponse<T> Failed(string error)
        {
            return new GenericResponse<T>(false, null, error);
        }
    }
}
