using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Runtime.InteropServices.Marshalling;
namespace VirusProofSelenium.Helper
{
    public class PathCreator
    {
        public async Task<string> getPathOfFile(IFormFile file)
        {
            var filePath = Path.GetTempFileName();

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }
    }
}
