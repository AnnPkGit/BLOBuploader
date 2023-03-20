using Azure.Storage.Blobs;
using BLOBuploaderAPI.Helpers.Constants;
using BLOBuploaderAPI.Helpers.Result.WebSpaceManager.Helpers.Result;

namespace BLOBuploaderAPI.Helpers
{
    public class BlobHelper : IBlobHelper
    {
        private readonly BlobServiceClient _blobServiceClient;

        private readonly IConfiguration _configuration;

        private readonly IValidator _validator;

        private readonly String _containerName;

        private const String _emailKey = "Email";

        public BlobHelper(BlobServiceClient blobServiceClient, IConfiguration configuration, IValidator validator)
        {
            _blobServiceClient = blobServiceClient ?? throw new NullReferenceException(nameof(blobServiceClient));
            _configuration = configuration ?? throw new NullReferenceException(nameof(configuration));
            _validator = validator ?? throw new NullReferenceException(nameof(validator));
            _containerName = _configuration.GetSection(Config.BlobConfigurationSectionName).GetValue<String>(Config.ContainerValueName);
        }
        public MyResult SetContainersEmail(string email)
        {
            if (!_validator.IsEmailValid(email))
                return MyResult.Failed("Invalid email");

            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var properties = blobContainerClient.GetProperties().Value;

            properties.Metadata.TryGetValue(_emailKey, out var metadataEmail);
            if (metadataEmail == null || !metadataEmail.Equals(email))
            {
                var metadata = new Dictionary<string, string>() { { _emailKey, email } };
                blobContainerClient.SetMetadata(metadata);
            }
            return MyResult.Successful();
        }

        public MyResult UploadDocxFileToBlob(IFormFile file)
        {
            if (!_validator.IsDocxValid(file))
                return MyResult.Failed("Invalid file format");

            var container = _blobServiceClient.GetBlobContainerClient(_containerName);

            if(container.GetBlobs().Where( blob => blob.Name.Equals(file.FileName)).FirstOrDefault() != null)
                return MyResult.Failed("File with this name already exists");

            container.UploadBlob(file.FileName, file.OpenReadStream());
            return MyResult.Successful();
        }
    }
}
