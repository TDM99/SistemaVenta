using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaVentas.Datos;
using SistemaVentas.Entidades;

namespace SistemaVentas.Presentacion
{
    public partial class FrmDetalleVenta : Form
    {
        private static DataTable dt = new DataTable();
        private static FrmDetalleVenta _instancia=null;
        private DetalleVenta detVenta;

        public FrmDetalleVenta()
        {
            InitializeComponent();
        }

        public static FrmDetalleVenta GetInstance()
        {
            if (_instancia == null)
                _instancia = new FrmDetalleVenta();

            return _instancia;
            
        }
        private void BtnBuscarProducto_Click(object sender, EventArgs e)
        {
            FrmProducto frmProd =FrmProducto.GetInscance();
            frmProd.SetFlag("1");
            frmProd.ShowDialog();
        }

        internal void SetProducto(Producto producto)
        {
            txtProductoId.Text = producto.Id.ToString();
            txtProductoDescripcion.Text = producto.Nombre;
            txtStock.Text = producto.Stock.ToString();
            txtPrecioUnitario.Text = producto.PrecioVenta.ToString();
        }

        internal void SetVenta(Venta venta)
        {
          txtVentaId.Text = venta.Id.ToString();
          txtClienteId.Text = venta.Cliente.Id.ToString();
          txtClienteNombre.Text = venta.Cliente.Nombre;
          txtFecha.Text = venta.FechaVenta.ToShortDateString();
          cmbTipoDoc.Text = venta.TipoDocumento;
          txtNumeroDocumento.Text = venta.NumeroDocumento; 
        }

        private void FrmDetalleVenta_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = FDetalleVenta.GetAll(Convert.ToInt32(txtVentaId.Text));
                dt = ds.Tables[0];
                dgvVentas.DataSource = dt;
                //posible error
                dgvVentas.Columns["VentaId"].Visible = false;
                dgvVentas.Columns["Id"].Visible = false;
                dgvVentas.Columns["ProductoId"].Visible = false;
                dgvVentas.Columns["PrecioVenta"].Visible = false;

                if (dt.Rows.Count > 0)
                {
                    lblDatosNoEncontrados.Visible = false;
                    //DgvVentas_CellClick(null, null);
                }
                else
                {
                    lblDatosNoEncontrados.Visible = true;
                }

                //MostrasGuardarCancelar(false);
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
                   

                        DetalleVenta detVenta = new DetalleVenta();
                        detVenta.Venta.Id = Convert.ToInt32(txtVentaId.Text);
                        detVenta.Producto.Id = Convert.ToInt32(txtProductoId.Text);
                        detVenta.Cantidad = Convert.ToDouble(txtCantidad.Text);
                        detVenta.PrecioUnitario = Convert.ToDouble(txtPrecioUnitario.Text);


                        int iDetVentaId = FDetalleVenta.Insertar(detVenta);
                       
                        if(iDetVentaId > 0)
                        {
                            FDetalleVenta.DisminuirStock(detVenta);
                            FrmDetalleVenta_Load(null,null);
                            MessageBox.Show("El producto se agrego correctamente");
                            Limpiar();
                        }
                        else
                        
                            MessageBox.Show("El Producto no se pudo agregar intente nuevamente");
                        
                   

                }
                else
                {
                    MessageBox.Show(sResultado,"Error",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void Limpiar()
        {
            txtProductoId.Text = "";
            txtProductoDescripcion.Text = "";
            txtCantidad.Text = "1";
            txtStock.Text = "";
            txtPrecioUnitario.Text = "";
        }

        private string ValidarDatos()
        {
            //posible error
            string Resusltado = "";
            //posible error
            if (txtProductoId.Text == "")
            {
                Resusltado = Resusltado + "  Debe seleccionar un Producto \n";
            }
            if(Convert.ToInt32(txtCantidad.Text) > Convert.ToInt32(txtStock.Text))
            {
                Resusltado = Resusltado + "La cantidad que intenta vender supera al Stock \n";
            }

            return Resusltado;
        }

        private void DgvVentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvVentas.Columns["Eliminar"].Index)
            {
                DataGridViewCheckBoxCell chkEliminar =
                    (DataGridViewCheckBoxCell)dgvVentas.Rows[e.RowIndex].Cells["Eliminar"];
                chkEliminar.Value = !Convert.ToBoolean(chkEliminar.Value);
            }
        }

        private void BtnQuitar_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Quiere eliminar los productos selecionados?", "Eliminacion de Producto",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {

                    foreach (DataGridViewRow row in dgvVentas.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["Eliminar"].Value))
                        {
                            DetalleVenta deVenta = new DetalleVenta();
                            deVenta.Producto.Id = Convert.ToInt32(row.Cells["ProductoId"].Value);
                            deVenta.Cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value);
                            deVenta.Id = Convert.ToInt32(row.Cells["Id"].Value);

                            if (FDetalleVenta.Eliminar(deVenta) > 0)
                            {
                                if (FDetalleVenta.AumentarStock(deVenta) != 1)
                                {
                                    MessageBox.Show("No se pudo actualizar el Stock", 
                                        "Eliminacion de Producto",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("El producto no pudo ser quitado de la venta", "Eliminacion de Producto",
                                         MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                           

                        }

                    }

                    FrmDetalleVenta_Load(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
    }
}
