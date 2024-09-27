using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Gestion_Inventarios
{
    public class DAOProveedor
    {
        private readonly DBConexion db;

        public DAOProveedor()
        {
            db = new DBConexion();
        }

        public void InsertarProveedor(Proveedor proveedor)
        {
            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Proveedores (Nombre, Direccion, Telefono, Email) VALUES (@Nombre, @Direccion, @Telefono, @Email)";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                    cmd.Parameters.AddWithValue("@Direccion", proveedor.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                    cmd.Parameters.AddWithValue("@Email", proveedor.Email);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Proveedor> ObtenerProveedores()
        {
            List<Proveedor> proveedores = new List<Proveedor>();
            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Proveedores";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Proveedor proveedor = new Proveedor
                            {
                                ProveedorId = Convert.ToInt32(reader["Proveedor_Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Direccion = reader["Direccion"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Email = reader["Email"].ToString()
                            };
                            proveedores.Add(proveedor);
                        }
                    }
                }
            }
            return proveedores;
        }

        public void ActualizarProveedor(Proveedor proveedor)
        {
            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "UPDATE Proveedores SET Nombre = @Nombre, Direccion = @Direccion, Telefono = @Telefono, Email = @Email WHERE Proveedor_Id = @ProveedorId";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                    cmd.Parameters.AddWithValue("@Direccion", proveedor.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                    cmd.Parameters.AddWithValue("@Email", proveedor.Email);
                    cmd.Parameters.AddWithValue("@ProveedorId", proveedor.ProveedorId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarProveedor(int proveedorId)
        {
            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM Proveedores WHERE Proveedor_Id = @ProveedorId";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProveedorId", proveedorId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}