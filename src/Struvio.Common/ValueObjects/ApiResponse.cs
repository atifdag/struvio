namespace Struvio.Common.ValueObjects;

/// <summary>
/// ApiResponse, bir API yanıtını temsil eder.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T>
{
    // İşlemin başarılı olup olmadığını belirtir.
    public bool IsSuccess { get; set; }

    // Başarılı bir yanıt durumunda döndürülecek veriyi tutar.
    public T? Data { get; set; }

    // Yanıt ile ilgili mesajı tutar.
    public string? Message { get; set; }

    // Hata kodunu tutar.
    public ErrorCodes? ErrorCode { get; set; }

    // Başarılı bir yanıt oluşturur.
    public static ApiResponse<T> Success(T data)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Data = data
        };
    }

    // Başarılı bir yanıt oluşturur, sadece mesaj ile.
    public static ApiResponse<T> Success(string message)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Message = message
        };
    }

    // Başarılı bir yanıt oluşturur, veri ve mesaj ile.
    public static ApiResponse<T> Success(T data, string message)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message
        };
    }

    // Hatalı bir yanıt oluşturur.
    public static ApiResponse<T> Error(string message, ErrorCodes? errorCode = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
            ErrorCode = errorCode
        };
    }

    // Hatalı bir yanıt oluşturur, veri, mesaj ve hata kodu ile.
    public static ApiResponse<T> Error(T data, string message, ErrorCodes? errorCode = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Data = data,
            Message = message,
            ErrorCode = errorCode
        };
    }
}

// ApiResponse sınıfı, hata kodu ve mesajı tutan bir yanıtı temsil eder.
public class ApiResponse
{
    // İşlemin başarılı olup olmadığını belirtir.
    public bool IsSuccess { get; set; }

    // Yanıt ile ilgili mesajı tutar.
    public string? Message { get; set; }

    // Hata kodunu tutar.
    public ErrorCodes? ErrorCode { get; set; }

    // Başarılı bir yanıt oluşturur.
    public static ApiResponse Success(string message)
    {
        return new ApiResponse
        {
            IsSuccess = true,
            Message = message
        };
    }

    // Hatalı bir yanıt oluşturur.
    public static ApiResponse Error(string message, ErrorCodes? errorCode = null)
    {
        return new ApiResponse
        {
            IsSuccess = false,
            Message = message,
            ErrorCode = errorCode
        };
    }
}