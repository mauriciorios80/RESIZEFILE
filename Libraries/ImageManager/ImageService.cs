using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageManager
{
    public class ImageService
    {
        //Metodo para obtner bytes
        public static byte[] GetBytes(Stream stream)
        {
            byte[] data;
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                data = ms.ToArray();
                ms.Flush();
                ms.Close();
            }
            return data;
        }

        //Metodo para encasular la orientacion
        public static string Orientation(byte[] data, int maxSize)
        {
            //Variable de retorno
            string orientation;
            //Ejecucion del metodo de orientacion
            orientation = ImageHorientation(data, maxSize);
            //Retorno
            return orientation;
        }

        //Metodo para identificar la orientacion
        private static string ImageHorientation(byte[] data, int maxSize)

        {
            //Variable de retorno
            string orientation = "";

            //Variable de tipo imagen
            Image imagenOrientation;

            //Se crea espacion de memoria para trabajar
            using (MemoryStream memoria = new MemoryStream(data))
            {
                //Se asigna el mapa de bits a imagenOrientation
                imagenOrientation = Image.FromStream(memoria);

                //Variables para el alto y ancho
                int height = imagenOrientation.Height;
                int width = imagenOrientation.Width;
                //Se crean los limites de la imagen nueva
                int newHeight = imagenOrientation.Height;
                int newWidth = imagenOrientation.Width;

                //Si el alto es mayor al ancho entonces  es vertical
                if (height > width)
                {
                    //Si uno de los lados es mayor a maximo permitido se asignan los nuevos valores
                    if (height > maxSize)
                    {
                        //El nuevo alto se iguala con el maximo permitido
                        newHeight = maxSize;
                        //El ancho anterior se multiplica por el maximo permitido
                        //y se divide por el alto anterior para hacerlo proporcional
                        newWidth = (width * maxSize) / height;
                    }
                    //Se construye el mensaje
                    orientation = "vertical|" + width + "|" + height + "|" + newWidth + "|" + newHeight;
                }
                //si el ancho es mayor al alto entonces es horizontal
                else if (width > height)
                {
                    //Si uno de los lados es mayor a maximo permitido se asignan los nuevos valores
                    if (width > maxSize)
                    {
                        //Se asigna el nuevo ancho con el valor del maximo permitido
                        newWidth = maxSize;
                        //Se asigna el nuevo alto multiplicando el alto anterior por el maximo permitido
                        //y se divide por el anterior ancho para hacerlos proporcionales.
                        newHeight = (height * maxSize) / width;
                    }
                    //Se construye el mensaje
                    orientation = "horizontal|" + width + "|" + height + "|" + newWidth + "|" + newHeight;
                }
                //De lo contrario es un cuadrado
                else if (width == height)
                {
                    //Si uno de los lados es mayor a maximo permitido se asignan los nuevos valores
                    if (width > maxSize)
                    {
                        newWidth = maxSize;
                        newHeight = maxSize;
                    }

                    //Se construye el mensaje
                    orientation = "cuadrada|" + width + "|" + height + "|" + newWidth + "|" + newHeight;
                }
                //Se libera memoria cada vez que ser procesa una imagen
                memoria.Flush();
                memoria.Close();
            }

            //metodo para liberar memoria
            using (MemoryStream memoria = new MemoryStream())
            {
                memoria.Flush();
                memoria.Close();
            }
            return orientation;
        }

        //Metodo para  encasular el cambio de tamaño
        public static byte[] Resize(byte[] data, int maxSize)
        {
            //Se ejecuta el metodo de cambio de tamaño y la nueva imagen la resive el parametro data
            data = ResizeImage(data, maxSize);
            //Se retorna data
            return data;
        }

        //Metodo para cambiar el tamaño
        private static byte[] ResizeImage(byte[] data, int maxSize)
        {
            //Se crea la variable de retorno
            byte[] result;
            //Se crea una variable de tipo imagen
            Image img;

            //Se crea el espacio en memoria para trabajar
            using (MemoryStream mens = new MemoryStream(data))
            {
                //Se crea una imagen temporal con el mapa de bits resultante 
                img = Image.FromStream(mens);

                //Se crean los limites de la imagen
                int height = img.Height;
                int width = img.Width;
                //Se crean los limites de la imagen nueva
                int newHeight, newWidth;

                //Si alto mayor que ancho y el alto mayor que el maximo tamaño permitido
                //entonces la imagen es vertical y se asignan los nuevos límites
                if (height > width && height > maxSize)
                {
                    //El nuevo alto se iguala con el maximo permitido
                    newHeight = maxSize;
                    //El ancho anterior se multiplica por el maximo permitido
                    //y se divide por el alto anterior para hacerlo proporcional
                    newWidth = (width * maxSize) / height;

                }
                //Si el ancho es mayor que el alto y el ancho mayor que el maximo permitido
                //es horizontal y se redimensiona la imagen
                else if (width > height && width > maxSize)
                {
                    //Se asigna el nuevo ancho con el valor del maximo permitido
                    newWidth = maxSize;
                    //Se asigna el nuevo alto multiplicando el alto anterior por el maximo permitido
                    //y se divide por el anterior ancho para hacerlos proporcionales.
                    newHeight = (height * maxSize) / width;
                }
                //Si alto y ancho son iguales la imagen es cuadrada y si superan el maximo permitido entonces 
                //se asigna el valor de maxSize a ambos lados
                else if (width == height && width > maxSize)
                {
                    newWidth = maxSize;
                    newHeight = maxSize;
                }
                //De lo contrario la imagen no cambia
                else
                {
                    newHeight = height;
                    newWidth = width;
                }
                //Se inicializa nuevo mapa de bits con la nueva imagen
                Bitmap newImg = new Bitmap(img, newWidth, newHeight);

                //Se crea una variabla grafica con el nueva mapa de bits
                Graphics graph = Graphics.FromImage(newImg);

                //Se aplica compresion
                graph.InterpolationMode = InterpolationMode.HighQualityBilinear;

                //Se dibuja la imagen desde la coordenada 0 0
                graph.DrawImage(img, 0, 0, newImg.Width, newImg.Height);

                //A la variable img se la pasa la nueva imagen
                img = newImg;

                //Se limpia memoria
                mens.Flush();
                mens.Close();
            }

            //Se abre nuevo espacio de memoria para llenar la variable de retorno
            using (MemoryStream mems = new MemoryStream())
            {
                //Se guarda la imagen especificada
                img.Save(mems, ImageFormat.Png);
                //La variable de retorno se llena con el nuevo espacio de memoria
                result = mems.ToArray();
                //Se limpia memoria
                mems.Flush();
                mems.Close();
            }

            //Se retorna el resultado
            return result;
        }

        //Metodo que retorna la valides del formato de la imagen
        public static bool IsImage(string fileName)
        {
            //Se extrae la extension del archivo
            string extension = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();
            //Se busta en el arreglo si la extencio del archivo existe
            return extensiones.Contains(extension);
        }

        //Arreglo de extensiones
        private static readonly string[] extensiones = new string[]
        {
            "jpg"
            //"jpeg",
            //"bmp",
            //"png",
            //"gif"
        };
    }
}
