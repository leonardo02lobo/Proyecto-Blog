using MySql.Data.MySqlClient;
using Proyecto_Blog.Models;

namespace Proyecto_Blog.Controllers
{
    public class ControladorTarjetas
    {
        private MySqlConnection? conexion = null;
        private CadenaConexion? cadena = new CadenaConexion();
        Tarjetas tarjetas { get; set; } = null;

        public ControladorTarjetas(Tarjetas tarjetas)
        {
            this.tarjetas = tarjetas;
            conexion = new MySqlConnection(cadena.ObtenerConexion());
        }
        public void InicializarConexion()
        {
            conexion = new MySqlConnection(cadena.ObtenerConexion());
        }
        public async Task InsetarElementos()
        {
            try
            {
                conexion.Open();
                string query = "insert into blog_prueba.tarjetas(etiqueta,titulo,descripcion,perfil,id,urlImagen) " +
                "values (?etiqueta,?titulo,?descripcion,?perfil,?id,?urlImagen);";
                
                MySqlCommand command = new MySqlCommand(query, conexion);

                command.Parameters.Add("?etiqueta", MySqlDbType.VarChar).Value = tarjetas.getEtiquetas();
                command.Parameters.Add("?titulo", MySqlDbType.VarChar).Value = tarjetas.getTitulo();
                command.Parameters.Add("?descripcion", MySqlDbType.VarChar).Value = tarjetas.getDescripcion();
                command.Parameters.Add("?perfil", MySqlDbType.VarChar).Value = tarjetas.getPerfil();
                command.Parameters.Add("?id", MySqlDbType.Int32).Value = ObtenerIdPerfil();
                command.Parameters.Add("?urlImagen", MySqlDbType.VarChar).Value = tarjetas.getUrlImagen();

                await command.ExecuteNonQueryAsync();
            }
            catch (MySqlException e)
            {
                return;
            }
            finally
            {
                conexion.Close();
            }
        }
        private int ObtenerIdPerfil()
        {
            conexion.Open();
            string query = "select id from blog_prueba.usuarios where nombreUsuario = ?nombreUsuario;";
            MySqlCommand command = new MySqlCommand(query, conexion);
            command.Parameters.Add("?nombreUsuario", MySqlDbType.VarChar).Value = tarjetas.getPerfil();
            try
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            catch (MySqlException e)
            {
                return 0;
            }
            return 0;
        }
    }
}
