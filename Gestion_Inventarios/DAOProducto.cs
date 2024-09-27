using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Gestion_Inventarios
{
    public class DAOProducto
    {
        private readonly DBConexion db;

        public DAOProducto()
        {
            db = new DBConexion();
        }

        public void InsertarProducto(Producto producto)
        {
            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, Proveedor_Id) VALUES (@Nombre, @Descripcion, @Precio, @Stock, @ProveedorId)";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                    cmd.Parameters.AddWithValue("@ProveedorId", producto.ProveedorId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Producto> ObtenerProductos()
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT  productos.producto_id,productos.nombre,productos.descripcion,productos.precio,productos.stock,proveedores.nombre as proveedor FROM Productos  inner join  proveedores  on Proveedores.proveedor_id=productos.proveedor_id";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto producto = new Producto
                            {
                                ProductoId = Convert.ToInt32(reader["Producto_Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                Precio = Convert.ToDecimal(reader["Precio"]),
                                Stock = Convert.ToInt32(reader["Stock"]),
                                Nombres = reader["proveedor"].ToString()
                            };
                            productos.Add(producto);
                        }
                    }
                }
            }
            return productos;
        }

        public void ActualizarProducto(Producto producto)
        {
            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "UPDATE Productos SET Nombre = @Nombre, Descripcion = @Descripcion, Precio = @Precio, Stock = @Stock, Proveedor_Id = @ProveedorId WHERE Producto_Id = @ProductoId";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.AddWithValue("@ProductoId", producto.ProductoId);
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                    cmd.Parameters.AddWithValue("@ProveedorId", producto.ProveedorId);
                   
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
                string query = "SELECT Proveedor_Id, Nombre FROM Proveedores";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Proveedor proveedor = new Proveedor
                            {
                                ProveedorId = Convert.ToInt32(reader["Proveedor_Id"]),
                                Nombre = reader["Nombre"].ToString()
                            };
                            proveedores.Add(proveedor);
                        }
                    }
                }
            }
            return proveedores;
        }




        public void ActualizarStock(int productoId, int cantidad)
        {
            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "UPDATE Productos SET Stock = Stock + @Cantidad WHERE Producto_Id = @ProductoId";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductoId", productoId);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.ExecuteNonQuery();
                }
            }
        }



        public Producto ObtenerPrecioPorNombre(string nombreProducto)
        {
            var productoInfo = new Producto();

            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT Producto_Id, Precio FROM Productos WHERE Nombre = @Nombre";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombreProducto);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            productoInfo.ProductoId = Convert.ToInt32(reader["Producto_Id"]);
                            productoInfo.Precio = Convert.ToDecimal(reader["Precio"]);
                        }
                    }
                }
            }

            return productoInfo;
        }
        public List<Producto> ObtenerProductoss()
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT Producto_Id, Nombre FROM Productos";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto proveedor = new Producto
                            {
                                ProveedorId = Convert.ToInt32(reader["Producto_Id"]),
                                Nombre = reader["Nombre"].ToString()
                            };
                            productos.Add(proveedor);
                        }
                    }
                }
            }
            return productos;
        }


        public void EliminarProducto(int productoId)
        {
            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM Productos WHERE Producto_Id = @ProductoId";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductoId", productoId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

}