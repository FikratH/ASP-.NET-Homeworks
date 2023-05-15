using Microsoft.AspNetCore.Hosting;
using Pronia.Models;

namespace Pronia.Utils
{
    public static class FileService
    {
        public static void DeleteFile(params string[] path)
        {
            string resultPath = DefinePath(path);

            if (System.IO.File.Exists(resultPath))
            {
                System.IO.File.Delete(resultPath);
            }
        }
        public static string DefinePath(params string[] path) => Path.Combine(path);
    }
}
