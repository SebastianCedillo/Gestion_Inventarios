using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Gestion_Inventarios
{
    public class DAOOrdenCompra
    {
        private readonly DBConexion db;

        public DAOOrdenCompra()
        {
            db = new DBConexion();
        }

        public void InsertarOrdenCompra(OrdenCompra ordenCompra)
        {
            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO OrdenesCompra (Orden_Id,Fecha_Orden, Proveedor_Id) VALUES (@Orden_Id,@FechaOrden, @ProveedorId);";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.AddWithValue("@Orden_Id", ordenCompra.OrdenId);
                    cmd.Parameters.AddWithValue("@FechaOrden", ordenCompra.FechaOrden);
                    cmd.Parameters.AddWithValue("@ProveedorId", ordenCompra.ProveedorId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        

        public void InsertarDetalleOrden(DetalleOrden detalleOrden)
        {
            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO DetalleOrden (Orden_Id, Producto_Id, Cantidad, Precio_Unitario) VALUES (@OrdenId, @ProductoId, @Cantidad, @PrecioUnitario);";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@OrdenId", detalleOrden.OrdenId);
                    cmd.Parameters.AddWithValue("@ProductoId", detalleOrden.ProductoId);
                    cmd.Parameters.AddWithValue("@Cantidad", detalleOrden.Cantidad);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", detalleOrden.PrecioUnitario);
                    cmd.ExecuteNonQuery();
                }
                
                DAOProducto daoProducto = new DAOProducto();
                daoProducto.ActualizarStock(detalleOrden.ProductoId, detalleOrden.Cantidad);
            }
        }

   



        public int ObtenerUltimoId()
        {
            int ultimoId = 0;
            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT MAX(Orden_Id) FROM OrdenesCompra"; 
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        ultimoId = Convert.ToInt32(result);
                    }
                }
            }
            return ultimoId;
        }


        public List<OrdenCompra> ObtenerOrdenesCompra()
        {
            List<OrdenCompra> ordenes = new List<OrdenCompra>();
            using (SqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM OrdenesCompra";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrdenCompra ordenCompra = new OrdenCompra
                            {
                                OrdenId = Convert.ToInt32(reader["Orden_Id"]),
                                FechaOrden = Convert.ToDateTime(reader["Fecha_Orden"]),
                                ProveedorId = Convert.ToInt32(reader["Proveedor_Id"])
                            };
                            ordenes.Add(ordenCompra);
                        }
                    }
                }
            }
            return ordenes;
        }
    }
}