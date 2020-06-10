using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WebForumsApp.Data.Interfaces;

namespace WebForumsApp.Service
{
    public class UploadService : IUpload
    {
        public IConfiguration Configuration;

        public UploadService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public CloudBlobContainer GetBlobContainer(string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}
