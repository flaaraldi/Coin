using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Coins
{
    public enum TipoCoin
    {
        Bitcoin = 0,
        Litecoin = 1
    }

    public enum TipoNegociacao
    {
        Compra = 0,
        Venda = 1
    }

    public static class AlertaSalvo
    {
        private static string strAppDir;
        private static string nomeArquivo;
        private static System.Windows.Forms.NotifyIcon ntPopup;
        private static System.ComponentModel.IContainer components;
        public static bool t;

        public static List<Alerta> lAlerta = new List<Alerta>();

        public static void LerAlerta()
        {
            lAlerta = new List<Alerta>();
            // Cria o nome do arquivo com ano, mês, dia, hora minuto e segundo
            strAppDir = Directory.GetCurrentDirectory();
            nomeArquivo = Path.Combine(strAppDir, @"user\alerta.txt");

            if (!Directory.Exists(Path.Combine(strAppDir,@"user")))
                Directory.CreateDirectory(Path.Combine(strAppDir, @"user"));

            if (!System.IO.File.Exists(nomeArquivo))
                File.Create(nomeArquivo);

            // Cria um novo arquivo e devolve um StreamWriter para ele
            try
            {
                using (StreamReader texto = new StreamReader(nomeArquivo))
                {
                    string linha;
                    while ((linha = texto.ReadLine()) != null)
                    {
                        Alerta alert = new Alerta();
                        List<String> lstring = linha.Split(';').ToList<String>();
                        alert.TipoCoin = (TipoCoin)Enum.Parse(typeof(TipoCoin), lstring[0]);
                        alert.Negociacao = (TipoNegociacao)Enum.Parse(typeof(TipoNegociacao), lstring[1]);
                        alert.Valor = lstring[2];

                        lAlerta.Add(alert);
                    }
                }
            }
            catch (Exception)
            { }
        }

        public static void GravarAlerta()
        {
            // Cria um novo arquivo e devolve um StreamWriter para ele
            using (StreamWriter outputFile = new StreamWriter(nomeArquivo))
            {
                if ((lAlerta.Count > 0))
                {
                    foreach (Alerta alert in lAlerta)
                        outputFile.WriteLine(string.Format("{0};{1};{2}", (int)alert.TipoCoin, (int)alert.Negociacao, alert.Valor));
                }
            }
        }

        public static void Notifica(string mensagem)
        {
            System.Windows.Forms.MessageBox.Show(mensagem);



            //ntPopup = new System.Windows.Forms.NotifyIcon();

            ////- Mostrar o ícone no tabuleiro
            //ntPopup.Icon = Me.Icon;
            //ntPopup.Visible = true;
            //ntPopup.Text = mensagem;

            ////- Mostrar um balão de dica
            //ntPopup.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            //ntPopup.BalloonTipTitle = "Mensagem de Alerta!";
            //ntPopup.BalloonTipText = mensagem;
            //ntPopup.ShowBalloonTip(1500);

        //    components = new System.ComponentModel.Container();

        //    ntPopup = new System.Windows.Forms.NotifyIcon(components);
        //    ntPopup.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
        //    ntPopup.BalloonTipTitle = "Mensagem de Alerta";
        //    ntPopup.BalloonTipText = mensagem;
        //    //ntPopup.Icon = ((System.Drawing.Icon)(resources.GetObject("ntPopup.Icon")));
        //    ntPopup.Text = "Coins";
        //    ntPopup.DoubleClick += new System.EventHandler(ntPopup_DoubleClick);
        //    ntPopup.Visible = true;
        //    ntPopup.ShowBalloonTip(30000);
        //    //}
        //}

        //private static void ntPopup_DoubleClick(object Sender, EventArgs e)
        //{
        //    ntPopup.Visible = false;
        //}
        }
    }

    public class Alerta
    {
        public TipoCoin TipoCoin { get; set; }
        public TipoNegociacao Negociacao { get; set; }
        public String Valor { get; set; }
    }
}
