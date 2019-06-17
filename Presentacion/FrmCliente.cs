using System;
using System.Data;
using System.Windows.Forms;
using SistemaVentas.Datos;
using SistemaVentas.Entidades;

namespace SistemaVentas.Presentacion
{
    public partial class FrmCliente : Form
    {
        private static DataTable dt = new DataTable();
        public FrmCliente()
        {
            InitializeComponent();
        }

        private void FrmCliente_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = FCliente.GetAll();
                dt = ds.Tables[0];
                dgvClientes.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    lblDatosNoEncontrados.Visible = false;
                    DgvClientes_CellClick(null, null);
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

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
               string sResultado = ValidarDatos();

                if(sResultado=="")
                {
                    if (txtId.Text == "")
                    {

                        Cliente cliente = new Cliente();
                        cliente.Nombre = txtNombre.Text;
                        cliente.Apellido = txtApellido.Text;
                        cliente.Domicilio = txtDomicilio.Text;
                        cliente.Dni = Convert.ToInt32(txtDni.Text);
                        cliente.Telefono = txtTelefono.Text;

                        if (FCliente.Insertar(cliente) >= 0)
                        {
                            MessageBox.Show("Datos insertados correctamente");
                            FrmCliente_Load(null, null);
                        }
                    }
                    else
                    {
                        Cliente cliente = new Cliente();
                        cliente.Id = Convert.ToInt32(txtId.Text);
                        cliente.Nombre = txtNombre.Text;
                        cliente.Apellido = txtApellido.Text;
                        cliente.Domicilio = txtDomicilio.Text;
                        cliente.Dni = Convert.ToInt32(txtDni.Text);
                        cliente.Telefono = txtTelefono.Text;

                        if (FCliente.Actualizar(cliente) == 1)
                        {
                            MessageBox.Show("Datos Modificados correctamente");
                            FrmCliente_Load(null, null);
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

        public string ValidarDatos()
        {
            string Resusltado = "";
            if(txtNombre.Text =="")
            {
                Resusltado = Resusltado + "Nombre \n";
            }
            if (txtApellido.Text == "")
            {
                Resusltado = Resusltado + "Apellido";
            }

            return Resusltado;
        }

        public void MostrasGuardarCancelar(bool b)
        {
            btnGuardar.Visible = b;
            btnCancelar.Visible = b;
            btnNuevo.Visible = !b;
            btnEditar.Visible = !b;
            btnEliminar.Visible = !b;

            dgvClientes.Enabled = !b;

            txtNombre.Enabled = b;
            txtApellido.Enabled = b;
            txtDni.Enabled = b;
            txtDomicilio.Enabled = b;
            txtTelefono.Enabled = b;
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            MostrasGuardarCancelar(true);
            txtId.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtDni.Text = "";
            txtTelefono.Text = "";
            txtDomicilio.Text = "";
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            MostrasGuardarCancelar(true);
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            MostrasGuardarCancelar(false);
            DgvClientes_CellClick(null, null);
        }

        private void DgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvClientes.CurrentRow != null)
            {
                txtId.Text = dgvClientes.CurrentRow.Cells[1].Value.ToString();
                txtNombre.Text = dgvClientes.CurrentRow.Cells[2].Value.ToString();
                txtApellido.Text = dgvClientes.CurrentRow.Cells[3].Value.ToString();
                txtTelefono.Text = dgvClientes.CurrentRow.Cells[4].Value.ToString();
                txtDni.Text = dgvClientes.CurrentRow.Cells[5].Value.ToString();
                txtDomicilio.Text = dgvClientes.CurrentRow.Cells[6].Value.ToString();
            }

        }

        private void DgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==dgvClientes.Columns["Eliminar"].Index)
            {
                DataGridViewCheckBoxCell chkEliminar =
                    (DataGridViewCheckBoxCell) dgvClientes.Rows[e.RowIndex].Cells["Eliminar"];
                chkEliminar.Value = !Convert.ToBoolean(chkEliminar.Value);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Quiere eliminar los clientes selecionados?", "Eliminacion de Cliente",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {

                    foreach (DataGridViewRow row in dgvClientes.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["Eliminar"].Value))
                        {
                            Cliente cliente = new Cliente();
                            cliente.Id = Convert.ToInt32(row.Cells["Id"].Value);
                            if (FCliente.Eliminar(cliente) != 1)
                            {
                                MessageBox.Show("El cliente no pudo ser eliminado", "Eliminacion de Cliente",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }

                    }

                    FrmCliente_Load(null, null);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = new DataView(dt.Copy());
                dv.RowFilter = cmbBuscar.Text + " Like '" + txtBuscar.Text + "%'";

                dgvClientes.DataSource = dv;

                if(dv.Count == 0)
                {
                    lblDatosNoEncontrados.Visible = true;
                }
                else
                {
                    lblDatosNoEncontrados.Visible = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
    }
}
