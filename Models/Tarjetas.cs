using MySql.Data.MySqlClient;
using System.Data;

namespace Proyecto_Blog.Models
{
    public class Tarjetas
    {
        private string etiqueta { get; set; } = "";
        private string titulo { get; set; } = "";
        private string descripcion { get; set; } = "";
        private string perfil { get; set; } = "";
        private string urlImagen { get; set; } = "";
        private int idPerfil { get; set; } = 0;

        public Tarjetas(){
        }
        public Tarjetas(string etiqueta,string titulo,string descripcion,string perfil, int idPerfil,string urlImagen)
        {
            this.etiqueta = etiqueta;
            this.titulo = titulo;
            this.descripcion = descripcion;
            this.perfil = perfil;
            this.idPerfil = idPerfil;
            this.urlImagen = urlImagen;
        }

        public string getEtiquetas() => etiqueta;
        public string getTitulo() => titulo;
        public string getDescripcion() => descripcion;
        public string getPerfil() => perfil;
        public int getIdPerfil() => idPerfil;
        public string getUrlImagen() => urlImagen;
    }
}
