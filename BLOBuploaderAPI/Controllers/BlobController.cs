using BLOBuploaderAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BLOBuploaderAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly IBlobHelper _blobHelper;

        public BlobController(IBlobHelper blobHelper)
        {
            _blobHelper = blobHelper ?? throw new NullReferenceException(nameof(blobHelper));
        }

        [HttpPost]
        [Route("docx")]
        public String PostBlob([FromForm] IFormFile docxFile, String email)
        {
            var emailResult = _blobHelper.SetContainersEmail(email.Trim());
            if (!emailResult.IsSuccessful)
                return emailResult.ErrorMessage;

            var fileResult = _blobHelper.UploadDocxFileToBlob(docxFile);
            if (!fileResult.IsSuccessful)
                return fileResult.ErrorMessage; 

            return "Ok";
        }
    }
}
