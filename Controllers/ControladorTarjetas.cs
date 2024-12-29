using System.Data;
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
            if (conexion != null)
            {
                conexion.Open();
            }
        }
        public void CerrarConexion()
        {
            if (conexion != null && conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }
        public async Task InsetarElementos()
        {
            try
            {
                InicializarConexion();
                string query = $"insert into {cadena?.ObtenerNombreDataBase()}.tarjetas(etiqueta,titulo,descripcion,perfil,id,urlImagen) " +
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
            catch (MySqlException)
            {
                return;
            }
            finally
            {
                CerrarConexion();
            }
        }
        private int ObtenerIdPerfil()
        {
            InicializarConexion();
            string query = $"select id from {cadena?.ObtenerNombreDataBase()}.usuarios where nombreUsuario = ?nombreUsuario;";
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
            catch (MySqlException)
            {
                return 0;
            }
            finally
            {
                CerrarConexion();
            }
            return 0;
        }
        public MySqlDataReader? AccederTablaTarjetas()
        {
            InicializarConexion();
            string query = $"SELECT * FROM {cadena?.ObtenerNombreDataBase()}.tarjetas;";
            try
            {
                MySqlCommand command = new MySqlCommand(query, conexion);

                return command.ExecuteReader();
            }
            catch (MySqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task ActualizarCampos(string NombreColumna, string opcion, string valor1, string valorOriginal)
        {
            InicializarConexion();
            string query = $"update {cadena?.ObtenerNombreDataBase()}.usuarios set {NombreColumna} = ?valorColumna where {opcion} = ?valorOriginal;";
            try
            {
                MySqlCommand command = new MySqlCommand(query, conexion);
                command.Parameters.Add("?ValorColumna", MySqlDbType.VarChar).Value = valor1;
                command.Parameters.Add("?valorOriginal", MySqlDbType.VarChar).Value = valorOriginal;

                await command.ExecuteNonQueryAsync();
            }
            catch (MySqlException)
            {
                return;
            }
            finally
            {
                CerrarConexion();
            }
        }
    }
}
