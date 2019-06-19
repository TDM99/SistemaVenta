using SistemaVentas.Datos;
using SistemaVentas.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaVentas.Presentacion
{
    public partial class FrmCategoria : Form
    {
        private static DataTable dt = new DataTable();
        public FrmCategoria()
        {
            InitializeComponent();
        }

        public void SetFlag(string valor)
        {
            txtFlag.Text = valor;
        }
        private void FrmCategoria_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = FCategoria.GetAll();
                dt = ds.Tables[0];
                dgvCategoria.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    lblDatosNoEncontrados.Visible = false;
                    DgvCategoria_CellClick(null, null);
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

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string sResultado = ValidarDatos();

                if (sResultado == "")
                {
                    if (txtId.Text == "")
                    {

                        Categoria categoria = new Categoria();
                        categoria.Descripcion = txtNombre.Text;
                        

                        if (FCategoria.Insertar(categoria) >= 0)
                        {
                            MessageBox.Show("Datos insertados correctamente");
                            FrmCategoria_Load(null, null);
                        }
                    }
                    else
                    {
                        Categoria categoria = new Categoria();
                        categoria.Descripcion = txtNombre.Text;
                        categoria.Id = Convert.ToInt32(txtId.Text);
                        

                        if (FCategoria.Actualizar(categoria) == 1)
                        {
                            MessageBox.Show("Datos Modificados correctamente");
                            FrmCategoria_Load(null, null);
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

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            MostrasGuardarCancelar(false);
            DgvCategoria_CellClick(null, null);
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            MostrasGuardarCancelar(true);
            txtId.Text = "";
            txtNombre.Text = "";
            
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            MostrasGuardarCancelar(true);
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Quiere eliminar las categorias selecionadas?", "Eliminacion de Categoria",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {

                    foreach (DataGridViewRow row in dgvCategoria.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["Eliminar"].Value))
                        {
                            Categoria categoria = new Categoria();
                            categoria.Id = Convert.ToInt32(row.Cells["Id"].Value);
                            if (FCategoria.Eliminar(categoria) != 1)
                            {
                                MessageBox.Show("La categoria no pudo ser eliminada", "Eliminacion de Categoria",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }

                    }

                    FrmCategoria_Load(null, null);
                }
            }
            catch (Exception ex)
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

                dgvCategoria.DataSource = dv;

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

        private void DgvCategoria_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvCategoria.Columns["Eliminar"].Index)
            {
                DataGridViewCheckBoxCell chkEliminar =
                    (DataGridViewCheckBoxCell)dgvCategoria.Rows[e.RowIndex].Cells["Eliminar"];
                chkEliminar.Value = !Convert.ToBoolean(chkEliminar.Value);
            }
        }

        private void DgvCategoria_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCategoria.CurrentRow != null)
            {
                txtId.Text = dgvCategoria.CurrentRow.Cells[1].Value.ToString();
                txtNombre.Text = dgvCategoria.CurrentRow.Cells[2].Value.ToString();
                
            }
        }

        public string ValidarDatos()
        {
            string Resusltado = "";
            if (txtNombre.Text == "")
            {
                Resusltado = Resusltado + "Nombre \n";
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

            dgvCategoria.Enabled = !b;

            txtNombre.Enabled = b;
            
        }

        private void DgvCategoria_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (txtFlag.Text == "1")
            {

                FrmProducto frmProd = FrmProducto.GetInscance();
                if (dgvCategoria.CurrentRow != null)
                {
                    frmProd.SetCategoria(dgvCategoria.CurrentRow.Cells[1].Value.ToString(), dgvCategoria.CurrentRow.Cells[2].Value.ToString());
                    frmProd.Show();
                    Close();
                }
            }
        }
    }
}
