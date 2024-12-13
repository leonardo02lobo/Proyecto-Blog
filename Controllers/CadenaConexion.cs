using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Proyecto_Blog.Controllers
{
    public class CadenaConexion
    {
        public string ObtenerConexion()
        {
            string contenido = File.ReadAllText("appsettings.json");
            JObject json = JObject.Parse(contenido);
            return (string)json["ConnectionStrings"]["connectionBD"];
        }
    }
}
