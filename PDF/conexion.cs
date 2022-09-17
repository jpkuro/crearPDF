using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF
{
    internal class conexion
    {
        public static MySqlConnection Conexion()
        {
            string servidor = "localhost";
            string bd = "tienda";
            string usuario = "root";
            string password = "Kuro921*";

            string cadenaConexion = "Database=" + bd + "; Data Source=" + servidor + "; User ID=" + usuario + "; Password=" + password + "";

            try
            {
                MySqlConnection conexionDB= new MySqlConnection(cadenaConexion);
                return conexionDB;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al conectarse " + ex.Message);
                return null;
            }
        }
    }
}
