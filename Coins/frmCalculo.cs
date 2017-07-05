using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Coins
{
    public partial class frmCalculo : Form
    {
        #region .: Variaveis :.
        private bool threadAtiva;
        #endregion

        #region .: Eventos :.
        public frmCalculo()
        {
            InitializeComponent();
            ComponenteFormat_grid();

            AlertaSalvo.LerAlerta();
            new Thread(AtualizaValores).Start();
            new Thread(AtualizaGridsOrder).Start();

            timerValores.Start();
            timerGrid.Start();
            threadAtiva = true;
        }
        private void frmCalculo_FormClosing(object sender, FormClosingEventArgs e)
        {
            threadAtiva = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string vl_invest;
            Validacao();
            txtInvest.Text = CalculaValor(txtCotacao.Text, txtQtd.Text, out vl_invest);
            txtRetorno.Text = vl_invest;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string qtd_invest;
            txtQtd.Text = CalculaQuantidade(txtCotacao.Text, txtInvest.Text, out qtd_invest);
            txtRetorno.Text = qtd_invest;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string qtd_invest;
            txtCotacao.Text = CalculaCotacao(txtInvest.Text, txtQtd.Text, out qtd_invest);
            txtRetorno.Text = qtd_invest;
        }

        private void btnFormMenor_Click(object sender, EventArgs e)
        {
            timerValores.Stop();
            timerGrid.Stop();
            this.Visible = false;

            new frmNotificacao().ShowDialog();
            new Thread(AtualizaValores).Start();
            new Thread(AtualizaGridsOrder).Start();

            this.Visible = true;
            timerValores.Start();
            timerGrid.Start();
        }
        private void btnMsgAlerta_Click(object sender, EventArgs e)
        {
            new frmAlerta(lbl_bit_ultPreco.Text, lbl_lit_ultPreco.Text).Show();
        }
        private void btnGrafico_Click(object sender, EventArgs e)
        {
            new frmGrafico().Show();
        }

        private void dtGridTrades_bit_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 4)
                PintarCelula(dtGridTrades_bit, e);
        }
        private void dtGridTrades_lite_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 4)
                PintarCelula(dtGridTrades_lite, e);
        }

        private void dtGridOrderVenda_bit_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 1)
                PintarCelula_BidsAsks(dtGridOrderVenda_bit, e, TipoCoin.Bitcoin);
        }
        private void dtGridOrderCompra_bit_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 1)
                PintarCelula_BidsAsks(dtGridOrderCompra_bit, e, TipoCoin.Bitcoin);
        }
        private void dtGridOrderVenda_lite_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 1)
                PintarCelula_BidsAsks(dtGridOrderVenda_lite, e, TipoCoin.Litecoin);
        }
        private void dtGridOrderCompra_lite_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 1)
                PintarCelula_BidsAsks(dtGridOrderCompra_lite, e, TipoCoin.Litecoin);
        }

        private void timerGrid_Tick(object sender, EventArgs e)
        {
            try
            {
                if (WindowState.ToString() != "Minimized" && threadAtiva)
                    new Thread(AtualizaGridsOrder).Start();
            }
            catch (Exception)
            { }
        }
        private void timerValores_Tick(object sender, EventArgs e)
        {
            try
            {
                if (WindowState.ToString() != "Minimized" && threadAtiva)
                    new Thread(AtualizaValores).Start();
            }
            catch (Exception) { }
        }
        #endregion

        #region .: Metodos :.
        private void AtualizaValores()
        {
            try
            {
                if (Coin.WebServiceTicker(MercadoBitcoin.ticker))
                    PreencherBitcoin(Coin.ticket);
                if (Coin.WebServiceTicker(MercadoBitcoin.ticker + MercadoBitcoin.litecoin))
                    PreencherLitecoin(Coin.ticket);
            }
            catch (Exception) { }
        }
        private void AtualizaGridsOrder()
        {
            try
            {
                if (Coin.WebServiceOrderbook(MercadoBitcoin.orderbook))
                    PreencherOrderbook(dtGridOrderVenda_bit, dtGridOrderCompra_bit);
                if (Coin.WebServiceTrades(MercadoBitcoin.trades))
                    PreencherTrades(dtGridTrades_bit);

                if (Coin.WebServiceOrderbook(MercadoBitcoin.orderbook + MercadoBitcoin.litecoin))
                    PreencherOrderbook(dtGridOrderVenda_lite, dtGridOrderCompra_lite);
                if (Coin.WebServiceTrades(MercadoBitcoin.trades + MercadoBitcoin.litecoin))
                    PreencherTrades(dtGridTrades_lite);
            }
            catch (Exception) { }
        }

        private void PreencherTrades(DataGridView dtGrid)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    int posicao = dtGrid.FirstDisplayedScrollingRowIndex;
                    int linhaSelecionada = dtGrid.FirstDisplayedScrollingColumnIndex;
                    int indexRow = 0;
                    int currentCellIndex = 0;
                    if (posicao > -1)
                    {
                        indexRow = getDGindex(dtGrid, dtGrid[0, dtGrid.CurrentRow.Index].Value.ToString(), dtGrid[1, dtGrid.CurrentRow.Index].Value.ToString());
                        currentCellIndex = dtGrid.CurrentCell.ColumnIndex;
                    }

                    dtGrid.DataSource = null; //Limpa o grid;
                    dtGrid.DataSource = Coin.lTrades;
                    dtGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dtGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dtGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dtGrid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dtGrid.Columns[3].Visible = false;
                    if (posicao > -1)
                    {
                        dtGrid.CurrentCell = dtGrid.Rows[indexRow].Cells[currentCellIndex];
                        dtGrid.FirstDisplayedScrollingRowIndex = posicao;
                        dtGrid.FirstDisplayedScrollingColumnIndex = linhaSelecionada;
                    }

                });
            }
            catch (Exception)
            { }
        }
        private void PreencherOrderbook(DataGridView dtGridVenda, DataGridView dtGridCompra)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    //-----------------------------
                    //  dtGridVenda
                    //-----------------------------
                    int posicao = dtGridVenda.FirstDisplayedScrollingRowIndex;
                    int linhaSelecionada = dtGridVenda.FirstDisplayedScrollingColumnIndex;
                    int indexRow = 0;
                    int currentCellIndex = 0;
                    if (posicao > -1)
                    {
                        indexRow = getDGindex(dtGridVenda, dtGridVenda[0, dtGridVenda.CurrentRow.Index].Value.ToString(), dtGridVenda[1, dtGridVenda.CurrentRow.Index].Value.ToString());
                        currentCellIndex = dtGridVenda.CurrentCell.ColumnIndex;
                    }

                    dtGridVenda.DataSource = null; //Limpa o grid;
                    dtGridVenda.DataSource = Coin.lOrderbook_Lances;
                    dtGridVenda.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dtGridVenda.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    if (posicao > -1)
                    {
                        dtGridVenda.CurrentCell = dtGridVenda.Rows[indexRow].Cells[currentCellIndex];
                        dtGridVenda.FirstDisplayedScrollingRowIndex = posicao;
                        dtGridVenda.FirstDisplayedScrollingColumnIndex = linhaSelecionada;
                    }

                    //-----------------------------
                    //  dtGridCompra
                    //-----------------------------
                    posicao = dtGridCompra.FirstDisplayedScrollingRowIndex;
                    linhaSelecionada = dtGridCompra.FirstDisplayedScrollingColumnIndex;
                    if (posicao > -1)
                    {
                        indexRow = getDGindex(dtGridCompra, dtGridCompra[0, dtGridCompra.CurrentRow.Index].Value.ToString(), dtGridCompra[1, dtGridCompra.CurrentRow.Index].Value.ToString());
                        currentCellIndex = dtGridCompra.CurrentCell.ColumnIndex;
                    }

                    dtGridCompra.DataSource = null; //Limpa o grid;
                    dtGridCompra.DataSource = Coin.lOrderbook_Pedidos;
                    dtGridCompra.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dtGridCompra.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    if (posicao > -1)
                    {
                        dtGridCompra.CurrentCell = dtGridCompra.Rows[indexRow].Cells[currentCellIndex];
                        dtGridCompra.FirstDisplayedScrollingRowIndex = posicao;
                        dtGridCompra.FirstDisplayedScrollingColumnIndex = linhaSelecionada;
                    }
                });
            }
            catch (Exception)
            { }
        }

        private void PreencherBitcoin(Ticker coin)
        {
            if (coin.Buy.Equals(string.Empty)) return;
            try
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    lbl_bit_ultPreco.ForeColor = ((double.Parse(lbl_bit_ultPreco.Text) < double.Parse(coin.Last)) ? Color.FromArgb(255, 150, 150) : Color.FromArgb(174, 234, 164));
                    //Color.FromArgb(255, 57, 57) Color.FromArgb(94, 213, 72) 174; 234; 164
                    lbl_bit_ultPreco.Text = coin.Last;
                    lbl_bit_maior.Text = coin.High;
                    lbl_bit_menor.Text = coin.Low;
                    lbl_bit_vol.Text = coin.Vol;
                    lbl_bit_compra.Text = coin.Buy;
                    lbl_bit_venda.Text = coin.Sell;
                    lbl_bit_data.Text = coin.Date;
                });
            }
            catch(Exception)
            {}
        }
        private void PreencherLitecoin(Ticker coin)
        {
            if (coin.Buy.Equals(string.Empty)) return;
            try
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    lbl_bit_ultPreco.ForeColor = ((double.Parse(lbl_lit_ultPreco.Text) < double.Parse(coin.Last)) ? Color.FromArgb(255, 150, 150) : Color.FromArgb(174, 234, 164));

                    lbl_lit_ultPreco.Text = coin.Last;
                    lbl_lit_maior.Text = coin.High;
                    lbl_lit_menor.Text = coin.Low;
                    lbl_lit_vol.Text = coin.Vol;
                    lbl_lit_compra.Text = coin.Buy;
                    lbl_lit_venda.Text = coin.Sell;
                    lbl_lite_data.Text = coin.Date;
                });
            }
            catch (Exception)
            { }
        }
        private int getDGindex(DataGridView dtGrid, string campo0, string campo1)
        {
            foreach (DataGridViewRow item in dtGrid.Rows)
            {
                if (item.Cells[0].Value.ToString() == campo0 && item.Cells[1].Value.ToString() == campo1)
                    return item.Index;
            }
            return 0;
        }

        private void Validacao()
        {
            double valor;
            double.TryParse(txtCotacao.Text, out valor);
            txtCotacao.Text = valor.ToString("n5");

            double.TryParse(txtInvest.Text, out valor);
            txtInvest.Text = valor.ToString("n5");

            double.TryParse(txtQtd.Text, out valor);
            txtQtd.Text = valor.ToString("n5");
        }
        private void PintarCelula(DataGridView dtGrid, DataGridViewCellFormattingEventArgs e)
        {
            dtGrid.Rows[e.RowIndex].Cells[1].Style.BackColor = e.Value.Equals("COMPRA") ? Color.FromArgb(255, 150, 150) : Color.FromArgb(174, 234, 164);
        }

        private void PintarCelula_BidsAsks(DataGridView dtGrid, DataGridViewCellFormattingEventArgs e, TipoCoin tipoCoin)
        {
            INivelCor nivel;
            if (tipoCoin == TipoCoin.Bitcoin)
                nivel = new NivelBitcoin();
            else
                nivel = new NivelLitecoin();

            dtGrid.Rows[e.RowIndex].Cells[1].Style.BackColor = TonalidadeCor.RetornaCor(nivel, dtGrid.Rows[e.RowIndex].Cells[1].Value.ToString());
        }
        private void ComponenteFormat_grid()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();

            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtGridOrderCompra_bit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtGridOrderVenda_bit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtGridOrderCompra_lite.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtGridOrderVenda_lite.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtGridTrades_bit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtGridTrades_lite.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;

            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtGridOrderCompra_bit.DefaultCellStyle = dataGridViewCellStyle2;
            this.dtGridOrderVenda_bit.DefaultCellStyle = dataGridViewCellStyle2;
            this.dtGridOrderCompra_lite.DefaultCellStyle = dataGridViewCellStyle2;
            this.dtGridOrderVenda_lite.DefaultCellStyle = dataGridViewCellStyle2;
            this.dtGridTrades_bit.DefaultCellStyle = dataGridViewCellStyle2;
            this.dtGridTrades_lite.DefaultCellStyle = dataGridViewCellStyle2;

            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtGridOrderVenda_bit.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dtGridOrderCompra_lite.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dtGridOrderVenda_lite.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dtGridOrderCompra_bit.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dtGridTrades_bit.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dtGridTrades_lite.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
        }

        private string CalculaQuantidade(string sVl_moeda, string sVl_invest, out string sQtd_invest)
        {
            Validacao();
            string retorno = "";
            sQtd_invest = "";
            sVl_moeda = sVl_moeda.Equals(string.Empty) ? "0" : sVl_moeda;
            sVl_invest = sVl_invest.Equals(string.Empty) ? "0" : sVl_invest;
            double vl_moeda = double.Parse(sVl_moeda);
            double vl_invest = Math.Round(double.Parse(sVl_invest), 6);
            if (vl_invest > 0 && vl_moeda > 0)
            {
                //Qtantidade a receber
                double qtd = (vl_invest / vl_moeda);
                retorno = qtd.ToString("n5");

                //Qtantidade a receber sem a taxa
                double taxa = chkExecutora.Checked ? 0.7 : 0.3;
                qtd = (qtd - ((qtd * taxa) / 100));
                sQtd_invest = qtd.ToString("n5");

                CalculaMinLucro(vl_invest, qtd);
            }

            return retorno;
        }
        private string CalculaValor(string sVl_moeda, string sQtd, out string sVl_invest)
        {
            Validacao();
            string retorno = "";
            sVl_invest = "";
            sVl_moeda = sVl_moeda.Equals(string.Empty) ? "0" : sVl_moeda;
            sQtd = sQtd.Equals(string.Empty) ? "0" : sQtd;
            double vl_moeda = double.Parse(sVl_moeda);
            double qtd = Math.Round(double.Parse(sQtd), 6);
            if (qtd > 0 && vl_moeda > 0)
            {
                double vl_invest = qtd * vl_moeda;
                retorno = vl_invest.ToString("n5");

                double taxa = chkExecutora.Checked ? 0.7 : 0.3;
                CalculaMinLucro(vl_invest, (qtd -((qtd * taxa) / 100)));
                vl_invest = (vl_invest - ((vl_invest * taxa) / 100));
                sVl_invest = vl_invest.ToString("n5");

            }

            return retorno;
        }
        private string CalculaCotacao(string sValor, string sQtd, out string sQtd_invest)
        {
            Validacao();
            string retorno = "";
            sQtd_invest = "";
            sValor = sValor.Equals(string.Empty) ? "0" : sValor;
            sQtd = sQtd.Equals(string.Empty) ? "0" : sQtd;
            double vl_moeda = double.Parse(sValor);
            double qtd = double.Parse(sQtd);
            if (qtd > 0 && vl_moeda > 0)
            {
                double sVl_cotacao = vl_moeda / qtd;
                retorno = sVl_cotacao.ToString("n5");

                double taxa = chkExecutora.Checked ? 0.7 : 0.3;
                sVl_cotacao = (qtd - ((qtd * taxa) / 100));
                sQtd_invest = sVl_cotacao.ToString("n5");
            }

            return retorno;
        }
        private void CalculaMinLucro(double valor1, double valor2)
        {
            string retorno = "0,00000";
            if (valor1 > 0 && valor2 > 0)
            {
                double taxa = chkExecutora.Checked ? 0.7 : 0.3;
                //
                // cotacao = 100
                // valor = 500
                // qtd = 5 (valor / cotacao)
                // taxa = 0,3% (0,003)
                // qtd_adquirida = 4,985 (qtd - (qtd * taxa)
                //
                // Minimo Valor = ((((valor) + (valor * taxa)) / qtd_adquirida)) 
                //
                double sVl_cotacao = (((valor1 + ((valor1 * taxa) / 100)) / valor2)) + 0.1;
                retorno = sVl_cotacao.ToString("n5");
            }

            txt_minLucro.Text = retorno;
        }
        #endregion
    }
}
