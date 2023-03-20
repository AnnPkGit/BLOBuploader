using BLOBuploaderAPI.Helpers.Result.WebSpaceManager.Helpers.Result;

namespace BLOBuploaderAPI.Helpers
{
    public interface IBlobHelper
    {
        MyResult UploadDocxFileToBlob(IFormFile file);

        MyResult SetContainersEmail(String email);
    }
}
