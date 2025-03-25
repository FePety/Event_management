namespace Event_management.Core.Models
{
    // Defines a generic API response class that inherits from BaseResponse.
    // The class holds a property `Data` of type T, which represents the data returned by the API.
    public class ApiResponse<T> : BaseResponse
    {
        // The Data property stores the response data of type T.
        public T Data { get; set; }

        // Constructor that initializes the Data property with the provided value.
        public ApiResponse(T data)
        {
            Data = data;
        }
    }
}
