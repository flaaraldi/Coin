using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Coins
{
    public partial class frmNotificacao : Form
    {
        public frmNotificacao()
        {
            InitializeComponent();
            this.TopLevel = true;
            this.TopMost = true;
            this.BringToFront();
            this.ShowInTaskbar = false;

            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width,  workingArea.Bottom - Size.Height);

            try
            {
                if (Coin.WebServiceTicker(MercadoBitcoin.ticker + MercadoBitcoin.litecoin))
                    PreencherLitecoin(Coin.ticket);
                if (Coin.WebServiceTicker(MercadoBitcoin.ticker))
                    PreencherBitcoin(Coin.ticket);
            }
            catch (Exception)
            { }

            tmAtualizar.Start();
        }

        private void PreencherBitcoin(Ticker coin)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    lbl_bit_ultPreco.ForeColor = ((double.Parse(lbl_bit_ultPreco.Text) < double.Parse(coin.Last)) ? Color.FromArgb(255, 150, 150) : Color.FromArgb(174, 234, 164));
                    lbl_bit_ultPreco.Text = coin.Last;
                    lbl_bit_data.Text = coin.Date;
                });
            }
            catch (Exception)
            { }
        }

        private void PreencherLitecoin(Ticker coin)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    lbl_lite_ultPreco.ForeColor = ((double.Parse(lbl_lite_ultPreco.Text) < double.Parse(coin.Last)) ? Color.FromArgb(255, 150, 150) : Color.FromArgb(174, 234, 164));
                    lbl_lite_ultPreco.Text = coin.Last;
                    lbl_lite_data.Text = coin.Date;
                });
            }
            catch (Exception)
            { }
        }

        private void tmAtualizar_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Coin.WebServiceTicker(MercadoBitcoin.ticker + MercadoBitcoin.litecoin))
                    PreencherLitecoin(Coin.ticket);
                if (Coin.WebServiceTicker(MercadoBitcoin.ticker))
                    PreencherBitcoin(Coin.ticket);
            }
            catch(Exception)
            { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAlerta_Click(object sender, EventArgs e)
        {
            new frmAlerta(lbl_bit_ultPreco.Text, lbl_lite_ultPreco.Text).Show();
        }
    }
}
