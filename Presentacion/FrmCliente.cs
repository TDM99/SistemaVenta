using System;
using System.Data;
using System.Windows.Forms;
using SistemaVentas.Datos;

namespace SistemaVentas.Presentacion
{
    public partial class FrmCliente : Form
    {
        private static DataTable dt= new DataTable();
        public FrmCliente()
        {
            InitializeComponent();
        }

        private void FrmCliente_Load(object sender, EventArgs e)
        {
            try
            {
               DataSet ds= FCliente.GetAll();
                dt = ds.Tables[0];
                dgvClientes.DataSource = dt;

                if(dt.Rows.Count > 0)
                {
                    lblDatosNoEncontrados.Visible = false;
                }
                else
                {
                    lblDatosNoEncontrados.Visible = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
