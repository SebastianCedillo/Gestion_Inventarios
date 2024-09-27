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
    public partial class FrmMenu : Form
    {
        public FrmMenu()
        {
            InitializeComponent();
        }

        private void FrmMenu_Load(object sender, EventArgs e)
        {

        }

        private void registrarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmProducto abrir = new FrmProducto();

            abrir.Show();

        }

        private void registrarProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmProveedo abrir = new FrmProveedo();

            abrir.Show();
        }

        private void generarComprasToolStripMenuItem_Click(object sender, EventArgs e)
        {


            FrmCompra abrir = new FrmCompra();

            abrir.Show();


        }
    }
}
