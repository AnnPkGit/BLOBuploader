using System.Net.Mail;

namespace BLOBuploaderAPI.Helpers
{
    public class Validator : IValidator
    {
        private const String _docxExtension = ".docx";
        public bool IsEmailValid(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool IsDocxValid(IFormFile file)
        {
            return _docxExtension.Equals(Path.GetExtension(file.FileName));
        }
    }
}
