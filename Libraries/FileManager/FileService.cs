using System.IO;

namespace FileManager
{
    public class FileService
    {
        private static string SetSavePath(string directory, string fileName)
        {
            const string format = "{0}{1}.{2}";
            const string copyFormat = "{0}{1} ({2}).{3}";
            if (!directory.EndsWith(@"\"))
            {
                directory += @"\";
            }

            string name = fileName.Substring(0, fileName.LastIndexOf("."));
            string extension = fileName.Substring(fileName.LastIndexOf(".") + 1);
            string path = string.Format(format, directory, name, extension);

            if (File.Exists(path))
            {
                int count = 1;
                path = string.Format(copyFormat, directory, name, count, extension);
                while (File.Exists(path))
                {
                    count += 1;
                    path = string.Format(copyFormat, directory, name, count, extension);
                }
            }

            return path;
        }

        public static void SaveImage(byte[] content, string fileName, string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string path = SetSavePath(directory, fileName);

            using (FileStream fs = new FileStream(path, FileMode.CreateNew))
            {
                fs.Write(content, 0, content.Length);
                fs.Flush();
                fs.Close();
            }
        }
    }
}
