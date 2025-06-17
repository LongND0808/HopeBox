using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Core.R2Storage
{
    public interface IR2StorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string keyPrefix, string fileName);
    }
}
