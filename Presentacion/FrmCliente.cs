﻿using System;
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

        }

        private void DgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           txtId.Text= dgvClientes.CurrentRow.Cells[1].Value.ToString();
            txtNombre.Text = dgvClientes.CurrentRow.Cells[2].Value.ToString();
            txtApellido.Text = dgvClientes.CurrentRow.Cells[3].Value.ToString();
            txtTelefono.Text = dgvClientes.CurrentRow.Cells[4].Value.ToString();
            txtDni.Text = dgvClientes.CurrentRow.Cells[5].Value.ToString();
            txtDomicilio.Text = dgvClientes.CurrentRow.Cells[6].Value.ToString();

        }
    }
}
