using MedicalApi.Models;

namespace MedicalApi.Helpers;

public static class ResponseHelper
{
    public static ApiResponse<T> Success<T>(string message, T? data = default)
    {
        return new ApiResponse<T>
        {
            Status = 200,
            Error = false,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Created<T>(string message, T? data = default)
    {
        return new ApiResponse<T>
        {
            Status = 201,
            Error = false,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Error<T>(string message, int status = 400)
    {
        return new ApiResponse<T>
        {
            Status = status,
            Error = true,
            Message = message,
            Data = default
        };
    }
}