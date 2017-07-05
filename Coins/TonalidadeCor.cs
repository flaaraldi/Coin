using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Coins
{
    public enum ECor
    {
        AMARELO,
        BEGE,
        LARANJA,
        ROSA,
        VERMELHO
    }

    public static class Cores
    {
        public static Color RetornaTonalidade(ECor cor, double quantidade, double minima, double maxima)
        {
            Color retorno;
            double tipoCor = (((quantidade - minima) * 100) / (maxima - minima));

            switch (cor)
            {
                case ECor.AMARELO:
                    if (tipoCor < 33.34)
                        retorno = Color.FromArgb(255, 243, 129);
                    else if (tipoCor > 66.67)
                        retorno = Color.FromArgb(255, 236, 0);
                    else
                        retorno = Color.FromArgb(255, 239, 67);

                    break;
                case ECor.BEGE:
                    if (tipoCor < 33.34)
                        retorno = Color.FromArgb(251, 201, 118);
                    else if (tipoCor > 66.67)
                        retorno = Color.FromArgb(246, 168, 0);
                    else
                        retorno = Color.FromArgb(250, 189, 67);

                    break;
                case ECor.LARANJA:
                    if (tipoCor < 33.34)
                        retorno = Color.FromArgb(246, 173, 110);
                    else if (tipoCor > 66.67)
                        retorno = Color.FromArgb(236, 116, 5);
                    else
                        retorno = Color.FromArgb(242, 150, 63);

                    break;
                case ECor.ROSA:
                    if (tipoCor < 33.34)
                        retorno = Color.FromArgb(240, 143, 116);
                    else if (tipoCor > 66.67)
                        retorno = Color.FromArgb(235, 107, 81);
                    else
                        retorno = Color.FromArgb(242, 150, 63);

                    break;
                case ECor.VERMELHO:
                    if (tipoCor < 33.34)
                        retorno = Color.FromArgb(236, 119, 107);
                    else if (tipoCor > 66.67)
                        retorno = Color.FromArgb(226, 0, 26);
                    else
                        retorno = Color.FromArgb(230, 68, 66);

                    break;
                default:
                    //BRANCO
                    retorno = Color.FromArgb(255, 236, 0);
                    break;
            }

            return retorno;

        }
    }

    public static class TonalidadeCor
    {
        public static Color RetornaCor(INivelCor nivel, string quantidade)
        {
            return nivel.RetornaCor(quantidade);
        }
    }

    public interface INivelCor
    {
        Color RetornaCor(string quantidade);
    }

    public class NivelBitcoin : INivelCor
    {
        public Color RetornaCor(string quantidade)
        {
            double qtde = double.Parse(quantidade);
            if (qtde > 0 && qtde <= 1)
                return Cores.RetornaTonalidade(ECor.AMARELO, qtde, 0, 1);

            else if (qtde > 1 && qtde <= 4)
                return Cores.RetornaTonalidade(ECor.BEGE, qtde, 1, 4);

            else if (qtde > 4 && qtde <= 7)
                return Cores.RetornaTonalidade(ECor.LARANJA, qtde, 4, 7);

            else if (qtde > 7 && qtde <= 13)
                return Cores.RetornaTonalidade(ECor.ROSA, qtde, 7, 13);
            
            else
                return Cores.RetornaTonalidade(ECor.VERMELHO, qtde, 16, 35);
        }
    }

    public class NivelLitecoin : INivelCor
    {
        public Color RetornaCor(string quantidade)
        {
            double qtde = double.Parse(quantidade);

            if (qtde > 0 && qtde <= 5)
                return Cores.RetornaTonalidade(ECor.AMARELO, qtde, 0, 5);

            else if (qtde > 5 && qtde <= 20)
                return Cores.RetornaTonalidade(ECor.BEGE, qtde, 5, 20);

            else if (qtde > 20 && qtde <= 50)
                return Cores.RetornaTonalidade(ECor.LARANJA, qtde, 20, 50);

            else if (qtde > 50 && qtde <= 90)
                return Cores.RetornaTonalidade(ECor.ROSA, qtde, 50, 90);
            else
                return Cores.RetornaTonalidade(ECor.VERMELHO, qtde, 90, 180);
        }
    }
}


        //public static Color AMARELO_3 = Color.FromArgb(255, 236, 0);
        //public static Color AMARELO_2 = Color.FromArgb(255, 239, 67);
        //public static Color AMARELO_1 = Color.FromArgb(255, 243, 129);
        //public static Color BEGE_3 = Color.FromArgb(246, 168, 0);
        //public static Color BEGE_2 = Color.FromArgb(250, 189, 67);
        //public static Color BEGE_1 = Color.FromArgb(251, 201, 118);
        //public static Color LARANJA_3 = Color.FromArgb(236, 116, 5);
        //public static Color LARANJA_2 = Color.FromArgb(242, 150, 63);
        //public static Color LARANJA_1 = Color.FromArgb(246, 173, 110);
        //public static Color ROSA_3 = Color.FromArgb(229, 53, 52);
        //public static Color ROSA_2 = Color.FromArgb(235, 107, 81);
        //public static Color ROSA_1 = Color.FromArgb(240, 143, 116);
        //public static Color VERMELHO_3 = Color.FromArgb(226, 0, 26);
        //public static Color VERMELHO_2 = Color.FromArgb(230, 68, 66);
        //public static Color VERMELHO_1 = Color.FromArgb(236, 119, 107);
