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
    public partial class FrmProveedo : Form
    {

        private DAOProveedor daoProveedor;


        public FrmProveedo()
        {
            InitializeComponent();

            daoProveedor = new DAOProveedor();
            CargarProveedores();

        }
        private void CargarProveedores()
        {
            dataGridViewProveedores.DataSource = daoProveedor.ObtenerProveedores();
        }

        private void LimpiarCampos()
        {
            txtProveedorId.Clear();
            txtNombre.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtTelefono.Text) || txtTelefono.Text.Length < 7)
            {
                MessageBox.Show("El teléfono es obligatorio y debe tener al menos 7 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTelefono.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(txtEmail.Text) && !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("El email debe contener un formato válido (ej: ejemplo@dominio.com).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail.Focus();
                return false;
            }

            return true;
        }

        private void FrmProveedo_Load(object sender, EventArgs e)
        {

        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                Proveedor proveedor = new Proveedor
                {
                    Nombre = txtNombre.Text,
                    Direccion = txtDireccion.Text,
                    Telefono = txtTelefono.Text,
                    Email = txtEmail.Text
                };
                daoProveedor.InsertarProveedor(proveedor);
                CargarProveedores();
                LimpiarCampos();
                MessageBox.Show("Proveedor agregado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btneditar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProveedorId.Text))
            {
                if (ValidarCampos())
                {
                    Proveedor proveedor = new Proveedor
                    {
                        ProveedorId = int.Parse(txtProveedorId.Text),
                        Nombre = txtNombre.Text,
                        Direccion = txtDireccion.Text,
                        Telefono = txtTelefono.Text,
                        Email = txtEmail.Text
                    };
                    daoProveedor.ActualizarProveedor(proveedor);
                    CargarProveedores();
                    LimpiarCampos();
                    MessageBox.Show("Proveedor actualizado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un proveedor para editar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProveedorId.Text))
            {
                int proveedorId = int.Parse(txtProveedorId.Text);
                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este proveedor?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    daoProveedor.EliminarProveedor(proveedorId);
                    CargarProveedores();
                    LimpiarCampos();
                    MessageBox.Show("Proveedor eliminado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un proveedor para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        
    }

        private void dataGridViewProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewProveedores.Rows[e.RowIndex];
                txtProveedorId.Text = row.Cells["ProveedorId"].Value.ToString();
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtDireccion.Text = row.Cells["Direccion"].Value.ToString();
                txtTelefono.Text = row.Cells["Telefono"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
            }

        }
    }
}
