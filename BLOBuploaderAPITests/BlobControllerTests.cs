using BLOBuploaderAPI.Controllers;
using BLOBuploaderAPI.Helpers;
using BLOBuploaderAPI.Helpers.Result.WebSpaceManager.Helpers.Result;
using Microsoft.AspNetCore.Http;
using Moq;

namespace BLOBuploaderAPITests
{
    public class BlobControllerTests
    {
        private Mock<IBlobHelper> _blobHelper;
        private Mock<IFormFile> _formFile;

        [SetUp]
        public void Setup()
        {
            _blobHelper = new Mock<IBlobHelper>();
            _formFile = new Mock<IFormFile>();
        }

        [Test]
        public void Constructor_throws_NullReferenceException()
        {
            //Act, Assert
            Assert.Throws<NullReferenceException>(() => new BlobController(null));
        }


        [Test]
        public void Constructor_passes()
        {
            //Act, Assert
            Assert.DoesNotThrow(() => new BlobController(_blobHelper.Object));
        }

        [Test]
        public void PostBlob_SetContainersEmail_Fails()
        {
            //Arrange
            var email = "email";
            var error = "Invalid email";
            _blobHelper.Setup(x => x.SetContainersEmail(email)).Returns(MyResult.Failed(error));
            var blobController = new BlobController(_blobHelper.Object); ;

            //Act
            var result = blobController.PostBlob(_formFile.Object, email);

            //Assert
            Assert.AreEqual(error, result);
        }

        [Test]
        public void PostBlob_UploadDocxFileToBlob_Fails()
        {
            //Arrange
            var email = "email";
            var error = "File already exists";
            _blobHelper.Setup(x => x.SetContainersEmail(email)).Returns(MyResult.Successful);
            _blobHelper.Setup(x => x.UploadDocxFileToBlob(_formFile.Object)).Returns(MyResult.Failed(error));
            var blobController = new BlobController(_blobHelper.Object); ;

            //Act
            var result = blobController.PostBlob(_formFile.Object, email);

            //Assert
            Assert.AreEqual(error, result);
        }

        [Test]
        public void PostBlob_UploadDocxFileToBlob_Successful()
        {
            //Arrange
            var email = "email";
            var expectedResult = "Ok";
            _blobHelper.Setup(x => x.SetContainersEmail(email)).Returns(MyResult.Successful);
            _blobHelper.Setup(x => x.UploadDocxFileToBlob(_formFile.Object)).Returns(MyResult.Successful);
            var blobController = new BlobController(_blobHelper.Object); ;

            //Act
            var result = blobController.PostBlob(_formFile.Object, email);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}