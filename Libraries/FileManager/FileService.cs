using System.IO;

namespace FileManager
{
    public class FileService
    {
        //Metodo para subir el archivo al servidor
        private static string SetSavePath(string directory, string fileName)
        {
            const string format = "{0}{1}.{2}";
            const string copyFormat = "{0}{1} ({2}).{3}";
            
            //Derterminar si el final de esta instancia de cadena oincide con la instancia especificada
            if (!directory.EndsWith(@"\"))
            {
                directory += @"\";
            }
            //Se extrae el nombre del archivo
            string name = fileName.Substring(0, fileName.LastIndexOf("."));
            //Se obtienen la extension
            string extension = fileName.Substring(fileName.LastIndexOf(".") + 1);
            //Se construye la ruta
            string path = string.Format(format, directory, name, extension);

            //Se valida la existencia de la ruta
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
            //Se retorna la ruta
            return path;
        }

        //Metodo para salvar el archivo
        public static void SaveImage(byte[] content, string fileName, string directory)
        {
            //Se valida la existencia de la ruta
            if (!Directory.Exists(directory))
            {
                //Si no existe se crea la ruta
                Directory.CreateDirectory(directory);
            }

            string path = SetSavePath(directory, fileName);

            //Manejo de espacio en memoria
            using (FileStream fs = new FileStream(path, FileMode.CreateNew))
            {
                fs.Write(content, 0, content.Length);
                fs.Flush();
                fs.Close();
            }
        }
    }
}
