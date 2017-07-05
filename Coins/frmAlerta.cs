using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Coins
{
    public partial class frmAlerta : Form
    {
        private double Bitcoin;
        private double Litecoin;
        public frmAlerta(string valor_bitcoin, string valor_litecoin)
        {
            InitializeComponent();
            AlertaSalvo.LerAlerta();
            foreach (Alerta item in AlertaSalvo.lAlerta)
            {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewComboBoxCell codeCell = new DataGridViewComboBoxCell();
                row.CreateCells(dtAlerta);
                ((DataGridViewComboBoxCell)row.Cells[0]).Value = item.TipoCoin.Equals(TipoCoin.Bitcoin) ?
                    ((DataGridViewComboBoxCell)row.Cells[0]).Items[0] :
                    ((DataGridViewComboBoxCell)row.Cells[0]).Items[1];
                     //== item.TipoCoin;
                row.Cells[1].Value = item.Valor;

                dtAlerta.Rows.Add(row);
            }

            Bitcoin = double.Parse(valor_bitcoin);
            Litecoin = double.Parse(valor_litecoin);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            AlertaSalvo.lAlerta = new List<Alerta>();
            foreach (DataGridViewRow item in dtAlerta.Rows)
            {
                if (item.Index != dtAlerta.RowCount -1)
                {
                    try
                    {
                        Alerta alerta = new Alerta();
                        alerta.TipoCoin = (TipoCoin)Enum.Parse(typeof(TipoCoin), item.Cells[0].Value.ToString());
                        alerta.Valor = double.Parse(item.Cells[1].Value.ToString()).ToString("n6");
                        alerta.Negociacao = (((alerta.TipoCoin == TipoCoin.Bitcoin ? Bitcoin : Litecoin) > double.Parse(alerta.Valor)) ? TipoNegociacao.Compra : TipoNegociacao.Venda);

                        AlertaSalvo.lAlerta.Add(alerta);
                    }
                    catch (Exception)
                    { }
                }
            }

            AlertaSalvo.GravarAlerta();
            this.Close();
        }
    }
}
