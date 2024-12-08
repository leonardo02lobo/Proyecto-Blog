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
        public MySqlConnection conexion;
        private string connectionString = "Server=localhost;Database=blog_prueba;Uid=root;Pwd=root1234;";

        public Tarjetas(string etiqueta,string titulo,string descripcion,string perfil, int idPerifil,string urlImagen)
        {
            conexion = new MySqlConnection(connectionString);
            conexion.Open();
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

        public async Task InsetarElementos(MySqlConnection connection)
        {
            try
            {
                string query = "insert into blog_prueba.tarjetas(etiqueta,titulo,descripcion,perfil,id,urlImagen) " +
                "values (?etiqueta,?titulo,?descripcion,?perfil,?id,?urlImagen);";
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.Add("?etiqueta", MySqlDbType.VarChar).Value = getEtiquetas();
                command.Parameters.Add("?titulo", MySqlDbType.VarChar).Value = getTitulo();
                command.Parameters.Add("?descripcion", MySqlDbType.VarChar).Value = getDescripcion();
                command.Parameters.Add("?perfil", MySqlDbType.VarChar).Value = getPerfil();
                command.Parameters.Add("?id", MySqlDbType.Int32).Value = ObtenerIdPerfil(connection);
                command.Parameters.Add("?urlImagen", MySqlDbType.VarChar).Value = getUrlImagen();

                await command.ExecuteNonQueryAsync();
            }finally
            {
                conexion.Close();
            }
        }

        private int ObtenerIdPerfil(MySqlConnection connection)
        {
            string query = "select id from blog_prueba.usuarios where nombreUsuario = ?nombreUsuario;";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.Add("?nombreUsuario", MySqlDbType.VarChar).Value = getPerfil();
            using(MySqlDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    return reader.GetInt32(0);
                }
            }
            return 0;
        }
    }
}
