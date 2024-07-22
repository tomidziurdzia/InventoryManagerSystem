namespace Application.DTO.Response;

public class ServiceResponse(bool Flag, string Message)
{
    public bool Flag { get; set; }
    public string Message { get; set; }
}