using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace BlobFunctionApp
{
    public class BlobAddedFunction
    {
        private readonly IConfiguration _configuration;

        public BlobAddedFunction(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [FunctionName("BlobAddedFunction")]
        public void Run([BlobTrigger("docxcontainer/{name}", Connection = "StorageConnectionString")]Stream myBlob, string name, ILogger log)
        {
            var connection        = _configuration.GetConnectionStringOrSetting("StorageConnectionString");
            var container         = _configuration.GetValue<string>("ContainerName");
            var blobServiceClient = new BlobServiceClient(connection);
            var properties        = blobServiceClient.GetBlobContainerClient(container).GetProperties().Value;

            properties.Metadata.TryGetValue("Email", out var toEmail);
            if(String.IsNullOrEmpty(toEmail))
            {
                log.LogWarning($"C# Blob trigger function Processed blob\n Name:{name} \n EmailTo is not defined");
                return;
            }

            var fromEmail = _configuration.GetValue<string>("Sender");
            var password  = _configuration.GetValue<string>("Password");

            try
            {
                using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587))
                {
                    var subject = "BLOB Info";
                    var text    = $"New BLOB - {name} was added to container \"{container}\"!";

                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    SmtpServer.Timeout        = 5000;
                    SmtpServer.EnableSsl      = true;
                    SmtpServer.Credentials    = new NetworkCredential(fromEmail, password);

                    using (MailMessage message = new MailMessage() { 
                            From    = new MailAddress(fromEmail),
                            Subject = subject,
                            Body    = text,
                        })
                    {
                        message.To.Add(toEmail);
                        message.CC.Add(fromEmail);

                        SmtpServer.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError($"C# Blob trigger function Processed blob\n Name:{name} \n Exception occured: {ex.Message}");
            }
        }
    }
}
