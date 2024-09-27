using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_Inventarios
{
    public partial class FrmCompra : Form
    {
        private List<DetalleOrden> detallesOrden = new List<DetalleOrden>();

        private DAOProducto daoProducto;
        private DAOOrdenCompra daoOrdenCompra;


        public FrmCompra()
        {
            InitializeComponent();

            daoProducto = new DAOProducto();
   
            CargarProveedores();
            CargarProductos();


            daoOrdenCompra = new DAOOrdenCompra();
            CargarNuevoId();


        }
        private void CargarProveedores()
        {
            List<Proveedor> proveedores = daoProducto.ObtenerProveedores();
            cboproveedor.DataSource = proveedores;
            cboproveedor.DisplayMember = "Nombre";
            cboproveedor.ValueMember = "ProveedorId";
        }
        private void CargarProductos()
        {
            List<Producto> productoss = daoProducto.ObtenerProductoss();
            cboproducto.DataSource = productoss;
            cboproducto.DisplayMember = "Nombre";
            cboproducto.ValueMember = "ProductoId";
        }
        private void btneditar_Click(object sender, EventArgs e)
        {


            int productoId = (int)cboproducto.SelectedValue; 
            bool productoRepetido = false;

        
            foreach (DataGridViewRow row in dgDatos.Rows)
            {
               
                if (row.Cells[2].Value != null && row.Cells[2].Value is int)
                {
 
                    int idProductoEnFila = (int)row.Cells[2].Value;

                  
                    if (idProductoEnFila == productoId)
                    {
                        productoRepetido = true;
                        break;
                    }
                }
            }

           
            if (productoRepetido)
            {
                MessageBox.Show("El producto ya ha sido agregado al detalle. No se puede agregar repetido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }



            int rowEscribir = dgDatos.Rows.Count - 1;

            dgDatos.Rows.Add(1);
            dgDatos.Rows[rowEscribir].Cells[0].Value = this.txtID.Text;
            dgDatos.Rows[rowEscribir].Cells[1].Value = this.txtCod.Text;

            dgDatos.Rows[rowEscribir].Cells[2].Value = this.cboproducto.Text;


            dgDatos.Rows[rowEscribir].Cells[3].Value = this.txtPrecio.Text;
            dgDatos.Rows[rowEscribir].Cells[4].Value = this.txtCantidad.Text;






        }


        private void Limpiar()
        {
            cboproducto.SelectedIndex = -1;
            txtPrecio.Clear();
            txtCantidad.Clear();
            dgDatos.Rows.Clear();
            cboproveedor.Text="";
            txtCod.Clear();


        }
        private void btneliminar_Click(object sender, EventArgs e)
        {



        

            try
            {


                int Todo = dgDatos.RowCount;
                if (Todo >= 1)
                {
                    int Fil = dgDatos.CurrentRow.Index;


                    dgDatos.Rows.RemoveAt(dgDatos.CurrentRow.Index);
                }
                else
                {
                    MessageBox.Show("No Existe Ninguna Detalle!",
                    "Aviso", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                }


               

               


            }
            catch (Exception ex)
            {
            }


        }

        private void btnguardar_Click(object sender, EventArgs e)
        {

            try
            {
               
                OrdenCompra ordenCompra = new OrdenCompra
                {
                    OrdenId = int.Parse(txtID.Text),
                    FechaOrden = DateTime.Now, 
                    ProveedorId = int.Parse(cboproveedor.SelectedValue.ToString()) 
                };

             
                daoOrdenCompra.InsertarOrdenCompra(ordenCompra);

            
                foreach (DataGridViewRow row in dgDatos.Rows)
                {
                    if (row.Cells[0].Value != null) 
                    {
                        DetalleOrden detalleOrden = new DetalleOrden
                        {
                            OrdenId = ordenCompra.OrdenId, 
                            ProductoId = int.Parse(row.Cells[1].Value.ToString()), 
                            Cantidad = int.Parse(row.Cells[4].Value.ToString()), 
                            PrecioUnitario = decimal.Parse(row.Cells[3].Value.ToString()) 
                        };

                    
                        daoOrdenCompra.InsertarDetalleOrden(detalleOrden);
                    }
                }

                MessageBox.Show("Orden de compra guardados con éxito", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);



                Limpiar(); 
                CargarNuevoId(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Tiene Que Ingresar los Datos de la  Orden: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

    
        private void CargarNuevoId()
        {
           
            int nuevoId = daoOrdenCompra.ObtenerUltimoId() + 1;
            txtID.Text = nuevoId.ToString(); 
        }



        private void FrmCompra_Load(object sender, EventArgs e)
        {
           

        }

        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboproducto_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboproducto.SelectedItem != null)
            {
                Producto selectedProducto = cboproducto.SelectedItem as Producto;

                if (selectedProducto != null)
                {
                    
                    Producto productoInfo = daoProducto.ObtenerPrecioPorNombre(selectedProducto.Nombre);

               
                    txtCod.Text = productoInfo.ProductoId.ToString(); 
                    txtPrecio.Text = productoInfo.Precio.ToString();
                    txtCantidad.Clear(); 
                }
                else
                {
                    MessageBox.Show("Error al seleccionar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }

        private void cboproveedor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
