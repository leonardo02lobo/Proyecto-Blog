using MySql.Data.MySqlClient;
using Proyecto_Blog.Models;

namespace Proyecto_Blog.Controllers
{
    public class ControladorTarjetas
    {
        private MySqlConnection? conexion = null;
        private CadenaConexion? cadena = new CadenaConexion();
        private Tarjetas? tarjetas { get; set; } = null;

        public ControladorTarjetas(Tarjetas tarjetas)
        {
            this.tarjetas = tarjetas;
            conexion = new MySqlConnection(cadena.ObtenerConexion());
        }
        public void InicializarConexion()
        {
            conexion = new MySqlConnection(cadena?.ObtenerConexion());
        }
        public void CerrarConexion()
        {
            conexion?.Close();
        }
        public async Task InsetarElementos()
        {
            try
            {
                conexion?.Open();
                string query = "insert into blog_prueba.tarjetas(etiqueta,titulo,descripcion,perfil,id,urlImagen) " +
                "values (?etiqueta,?titulo,?descripcion,?perfil,?id,?urlImagen);";

                MySqlCommand command = new MySqlCommand(query, conexion);

                command.Parameters.Add("?etiqueta", MySqlDbType.VarChar).Value = tarjetas?.getEtiquetas();
                command.Parameters.Add("?titulo", MySqlDbType.VarChar).Value = tarjetas?.getTitulo();
                command.Parameters.Add("?descripcion", MySqlDbType.VarChar).Value = tarjetas?.getDescripcion();
                command.Parameters.Add("?perfil", MySqlDbType.VarChar).Value = tarjetas?.getPerfil();
                command.Parameters.Add("?id", MySqlDbType.Int32).Value = ObtenerIdPerfil();
                command.Parameters.Add("?urlImagen", MySqlDbType.VarChar).Value = tarjetas?.getUrlImagen();

                await command.ExecuteNonQueryAsync();
            }
            catch (MySqlException e)
            {
                return;
            }
            finally
            {
                conexion?.Close();
            }
        }
        private int ObtenerIdPerfil()
        {
            conexion?.Open();
            string query = "select id from blog_prueba.usuarios where nombreUsuario = ?nombreUsuario;";
            MySqlCommand command = new MySqlCommand(query, conexion);
            command.Parameters.Add("?nombreUsuario", MySqlDbType.VarChar).Value = tarjetas?.getPerfil();
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
        public MySqlDataReader AccederTablaTarjetas(string categoria)
        {
            string query = "SELECT * FROM blog_prueba.tarjetas;";
            if (categoria != "")
            {
                query = $"SELECT * FROM blog_prueba.tarjetas WHERE etiqueta = ?categoria;";
            }
            try
            {
                conexion?.Open();
                MySqlCommand command = new MySqlCommand(query, conexion);
                if (categoria != "")
                {
                    command.Parameters.Add("?categoria", MySqlDbType.VarChar).Value = categoria;
                }

                return command.ExecuteReader();
            }
            catch (MySqlException e)
            {
                return null;
            }
        }

        public async Task ActualizarCampos(string NombreColumna, string opcion, string valor1, string valorOriginal)
        {
            string query = $"update blog_prueba.usuarios set {NombreColumna} = ?valorColumna where {opcion} = ?valorOriginal;";
            conexion?.Open();
            MySqlCommand command = new MySqlCommand(query, conexion);
            command.Parameters.Add("?ValorColumna", MySqlDbType.VarChar).Value = valor1;
            command.Parameters.Add("?valorOriginal", MySqlDbType.VarChar).Value = valorOriginal;

            await command.ExecuteNonQueryAsync();
        }
    }
}
