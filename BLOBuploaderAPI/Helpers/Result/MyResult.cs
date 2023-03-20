namespace BLOBuploaderAPI.Helpers.Result
{
    namespace WebSpaceManager.Helpers.Result
    {
        public class MyResult
        {
            public bool IsSuccessful { get; private set; }

            public string? ErrorMessage { get; private set; }

            public static MyResult Successful()
            {
                return new MyResult(true, null);
            }

            public static MyResult Failed(string errorMessage)
            {
                return new MyResult(false, errorMessage);
            }

            private MyResult(bool isSuccessful, string? errorMessage = null)
            {
                IsSuccessful = isSuccessful;
                ErrorMessage = errorMessage;
            }
        }
    }
}
