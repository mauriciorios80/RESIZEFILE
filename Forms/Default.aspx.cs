using System;
using System.Configuration;
using System.Linq;
using ImageManager;

namespace Forms
{
    public partial class Default : System.Web.UI.Page
    {


        protected void btnCargar_Click(object sender, EventArgs e)
        {
            //Se limpia las cajas de texto
            Message.Text = "";
            msOrientacion.Text = "";
            try
            {
                //Se limpia el mensaje
                ShowMessage("");

                //Se valida que el FileUpload tenga un archivo seleccionado
                if (!FileUploader.HasFile)
                {
                    throw new Exception("Seleccione un archivo.");
                }

                //Se pone en sesion el archivo cargado para reutilizarlo
                System.IO.Stream archivo = FileUploader.PostedFile.InputStream;

                //Se obtienen el mapa de bytes de la imagen
                byte[] bytesData = ImageService.GetBytes(archivo);

                //Variable con el nombre del archivo
                string fileName = FileUploader.PostedFile.FileName;


                //Session["File"] = FileUploader.PostedFile.InputStream;

                //Se valida la extension de la imagen 

                if (!ImageManager.ImageService.IsImage(fileName))
                {
                    throw new Exception("Formato de imagen inválido debe ser jpg.");
                }

                //Se obtiene la orientacion de la imagen
                string orientation = ImageService.Orientation(bytesData, GetConfigSettings<int>("MaxSize"));

                //Se redimensiona la imagen
                byte[] image = ImageService.Resize(bytesData, GetConfigSettings<int>("MaxSize"));


                //Se guarda el archivo en la rutaPArametizada en el WebConfig
                string folder = ConfigurationManager.AppSettings["ImagesFolder"];
                FileManager.FileService.SaveImage(image, fileName, Server.MapPath(folder));

                //Mensaje de salida para la imagen gudadada
                ShowMessage($"Imagen guardada como '{fileName}'");

                //Se separa la cadena por |
                string[] metadata = orientation.Split('|');

                if (metadata[1] != metadata[3])
                {
                    ShowMessageOrientation($"La imagen es '{ metadata[0]}' y el tamaño es de " + metadata[1] + " x " + metadata[2] + ". El nuevo tamaño es " + metadata[3] + " x " + metadata[4]);
        

                }
                else
                {
                    ShowMessageOrientation($"La imagen es '{ metadata[0]}' y el tamaño es de " + metadata[1] + " x " + metadata[2] + ". La imagen no cambió de tamaño");
                }


            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private T GetConfigSettings<T>(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(value)
                ? default(T)
                : (T)Convert.ChangeType(value, typeof(T));
        }


        private void ShowMessage(string message)
        {
            Message.Text = message;

        }

        private void ShowMessageOrientation(string message)
        {
            msOrientacion.Text = message;
        }

    }
}