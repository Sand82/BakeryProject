using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

namespace Bakery.Tests.Mock
{
    public static class FormFileMock
    {
        public static FormFile ImageInstance
        {
            get
            {
                return new FormFile(
                new MemoryStream(Encoding.UTF8.GetBytes("dummy image")), 1000, 2000, "Data", "image.png");                
            }
        }

        public static FormFile FileInstance
        {
            get
            {
                return new FormFile(
                new MemoryStream(Encoding.UTF8.GetBytes("dummy file")), 1000, 2000, "Data", "file.doc");                
            }
        }
    }
}
