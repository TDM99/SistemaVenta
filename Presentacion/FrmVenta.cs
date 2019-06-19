using SistemaVentas.Datos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaVentas.Entidades;

namespace SistemaVentas.Presentacion
{
    public partial class FrmVenta : Form
    {
        private static DataTable dt = new DataTable();
        private static FrmVenta _instancia=null;
        public FrmVenta()
        {
            InitializeComponent();
        }

        public static FrmVenta GetInscance()
        {
            if (_instancia == null)
                _instancia = new FrmVenta();
            return _instancia;
        }

        private void FrmVenta_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = FVenta.GetAll();
                dt = ds.Tables[0];
                dgvVentas.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    lblDatosNoEncontrados.Visible = false;
                    DgvVentas_CellClick(null, null);
                }
                else
                {
                    lblDatosNoEncontrados.Visible = true;
                }

                MostrasGuardarCancelar(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        public void MostrasGuardarCancelar(bool b)
        {
            btnGuardar.Visible = b;
            btnCancelar.Visible = b;
            btnNuevo.Visible = !b;
            btnEditar.Visible = !b;
           
            dgvVentas.Enabled = !b;

            txtFecha.Enabled = b;
            cmbTipoDoc.Enabled = b;
            txtNumeroDocumento.Enabled = b;


        }

        private void BtnBuscarCliente_Click(object sender, EventArgs e)
        {
            FrmCliente frmcclic = new FrmCliente();
            frmcclic.SetFlag("1");
            frmcclic.ShowDialog();
        }

        public string ValidarDatos()
        {
            string Resusltado = "";
            if (txtClienteId.Text == "")
            {
                Resusltado = Resusltado + "Nombre \n";
            }
            if (txtNumeroDocumento.Text == "")
            {
                Resusltado = Resusltado + "Apellido";
            }

            return Resusltado;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string sResultado = ValidarDatos();

                if (sResultado == "")
                {
                    if (txtId.Text == "")
                    {

                        Venta venta = new Venta();
                        venta.Cliente.Id = Convert.ToInt32(txtClienteId.Text);
                        venta.FechaVenta = txtFecha.Value;
                        venta.TipoDocumento = cmbTipoDoc.Text;
                        venta.NumeroDocumento = txtNumeroDocumento.Text;

                        int iVentaId = FVenta.Insertar(venta);
                        if (iVentaId > 0)
                        {

                            FrmVenta_Load(null,null);
                            //CargarDetalle(iVentaId);
                        }
                    }
                    else
                    {
                        Venta venta = new Venta();
                        venta.Id = Convert.ToInt32(txtId.Text);
                        venta.Cliente.Id = Convert.ToInt32(txtClienteId.Text);
                        venta.FechaVenta = txtFecha.Value;
                        venta.TipoDocumento = cmbTipoDoc.Text;
                        venta.NumeroDocumento = txtNumeroDocumento.Text;

                        if (FVenta.Actualizar(venta) == 1)
                        {
                            MessageBox.Show("Datos Modificados correctamente");
                            FrmVenta_Load(null, null);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Faltan Campos por llenar: \n" + sResultado);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void CargarDetalle(int iVentaId)
        {
            throw new NotImplementedException();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            MostrasGuardarCancelar(false);
            DgvVentas_CellClick(null, null);
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            MostrasGuardarCancelar(true);
            txtId.Text = "";
            txtClienteId.Text = "";
            txtClienteNombre.Text = "";
            txtNumeroDocumento.Text = "";
            
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            MostrasGuardarCancelar(true);
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = new DataView(dt.Copy());
                dv.RowFilter = cmbBuscar.Text + " Like '" + txtBuscar.Text + "%'";

                dgvVentas.DataSource = dv;

                if (dv.Count == 0)
                {
                    lblDatosNoEncontrados.Visible = true;
                }
                else
                {
                    lblDatosNoEncontrados.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void DgvVentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == dgvVentas.Columns["Eliminar"].Index)
            //{
            //    DataGridViewCheckBoxCell chkEliminar =
            //        (DataGridViewCheckBoxCell)dgvVentas.Rows[e.RowIndex].Cells["Eliminar"];
            //    chkEliminar.Value = !Convert.ToBoolean(chkEliminar.Value);
            //}
        }

        private void DgvVentas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvVentas.CurrentRow != null)
            {
                txtId.Text = dgvVentas.CurrentRow.Cells["Id"].Value.ToString();
                txtClienteId.Text = dgvVentas.CurrentRow.Cells["ClienteId"].Value.ToString();
                txtClienteNombre.Text = dgvVentas.CurrentRow.Cells["Nombre"].Value.ToString() + " " + dgvVentas.CurrentRow.Cells["Apellido"].Value.ToString(); 
                txtFecha.Text = dgvVentas.CurrentRow.Cells["FechaVenta"].Value.ToString();
                cmbTipoDoc.Text = dgvVentas.CurrentRow.Cells["TipoDocumento"].Value.ToString();
                txtNumeroDocumento.Text = dgvVentas.CurrentRow.Cells["NumeroDocumento"].Value.ToString();
            }
        }

        internal void SetCliente(string sIdCliente, string sNombreCliente)
        {
            txtClienteId.Text = sIdCliente;
            txtClienteNombre.Text = sNombreCliente;
        }
    }
}
