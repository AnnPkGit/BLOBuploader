namespace BLOBuploaderAPI.Helpers
{
    public interface IValidator
    {
        bool IsEmailValid(String email);

        bool IsDocxValid(IFormFile file);
    }
}
