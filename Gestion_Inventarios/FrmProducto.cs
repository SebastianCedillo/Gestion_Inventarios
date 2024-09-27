using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Gestion_Inventarios
{
    public partial class FrmProducto : Form
    {

        private DAOProducto daoProducto;

        public FrmProducto()
        {
            InitializeComponent();

            daoProducto = new DAOProducto();
            CargarProductos();
            CargarProveedores();
        }


        private void CargarProductos()
        {
            dataGridViewProductos.DataSource = daoProducto.ObtenerProductos();
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
          
           txtID.Clear();


        }
        private void CargarProveedores()
        {
            List<Proveedor> proveedores = daoProducto.ObtenerProveedores(); 
            cboproveedor.DataSource = proveedores; 
            cboproveedor.DisplayMember = "Nombre"; 
            cboproveedor.ValueMember = "ProveedorId";
        }


        private bool ValidarCampos()
        {
            string mensajeError = "";

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                mensajeError = "Por favor, ingrese el nombre del producto.";
            }
            else if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                mensajeError = "Por favor, ingrese la descripción del producto.";
            }
            else if (txtPrecio.Text == "")
            {
                mensajeError = "Por favor, ingrese un precio válido.";
            }
            else if (txtStock.Text == "")
            {
                mensajeError = "Por favor, ingrese una cantidad de stock válida.";
            }
            else if (cboproveedor.SelectedValue == null)
            {
                mensajeError = "Por favor, seleccione un proveedor.";
            }

            if (!string.IsNullOrEmpty(mensajeError))
            {
                MessageBox.Show(mensajeError, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void FrmProducto_Load(object sender, EventArgs e)
        {

        }

        private void btnguardar_Click(object sender, EventArgs e)
        {

            if (ValidarCampos())
            {

                Producto producto = new Producto
                {
                    Nombre = txtNombre.Text,
                    Descripcion = txtDescripcion.Text,
                    Precio = decimal.Parse(txtPrecio.Text),
                    Stock = int.Parse(txtStock.Text),
                    ProveedorId = (int)cboproveedor.SelectedValue
                };

            daoProducto.InsertarProducto(producto);
            CargarProductos();
            LimpiarCampos();


                MessageBox.Show("Producto Registrado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }


        }

        private void btneditar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                Producto producto = new Producto
                {
                    ProductoId = int.Parse(txtID.Text), 
                    Nombre = txtNombre.Text,
                    Descripcion = txtDescripcion.Text,
                    Precio = decimal.Parse(txtPrecio.Text),
                    Stock = int.Parse(txtStock.Text),
                    ProveedorId = (int)cboproveedor.SelectedValue
                };

                daoProducto.ActualizarProducto(producto);
                CargarProductos();
                LimpiarCampos();
                MessageBox.Show("Producto actualizado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtID.Text))
            {
                int productoId = int.Parse(txtID.Text);
                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este producto?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    daoProducto.EliminarProducto(productoId);
                    CargarProductos();
                    LimpiarCampos();
                    MessageBox.Show("Producto eliminado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridViewProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridViewProductos.Rows[e.RowIndex];

                    
                    txtID.Text = row.Cells["ProductoId"].Value?.ToString();
                    txtNombre.Text = row.Cells["Nombre"].Value?.ToString();
                    txtDescripcion.Text = row.Cells["Descripcion"].Value?.ToString();
                    txtPrecio.Text = row.Cells["Precio"].Value?.ToString();
                    txtStock.Text = row.Cells["Stock"].Value?.ToString();
                    cboproveedor.Text = row.Cells["Proveedor"].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
              
            
        }
    }

        private void dataGridViewProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnreporte_Click(object sender, EventArgs e)
        {


            StringBuilder reporte = new StringBuilder();

           
            reporte.AppendLine("Reporte de Productos en Stock");
            reporte.AppendLine("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy"));
            reporte.AppendLine("============================================");

        
            reporte.AppendLine("Codigo\tProducto\tPrecio\tCantidad en Stock");

      
            foreach (DataGridViewRow row in dataGridViewProductos.Rows)
            {
                if (row.Cells[0].Value != null) 
                {
                    string idProducto = row.Cells[0].Value.ToString();
                    string nombreProducto = row.Cells[1].Value.ToString();
                    string descripcion = row.Cells[2].Value.ToString();
                    string precioProducto = row.Cells[3].Value.ToString(); 
                    string cantidadStock = row.Cells[4].Value.ToString(); 

               
                    reporte.AppendLine($"{idProducto}\t{nombreProducto}\t{precioProducto}\t{cantidadStock}");
                }
            }

            
            string rutaArchivo = "Reporte_Productos.txt";

        
            try
            {
                using (StreamWriter writer = new StreamWriter(rutaArchivo))
                {
                    writer.Write(reporte.ToString());
                }

               
                System.Diagnostics.Process.Start(rutaArchivo);

               
                MessageBox.Show("Reporte generado y guardado exitosamente.", "Reporte", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el reporte: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        


    }
    }
}
