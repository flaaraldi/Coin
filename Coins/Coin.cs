using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Coins
{
    public static class Coin 
    {
        public static Ticker ticket = new Ticker();
        public static List<Orderbook> lOrderbook_Lances = new List<Orderbook>();
        public static List<Orderbook> lOrderbook_Pedidos = new List<Orderbook>();
        public static List<Trades> lTrades = new List<Trades>();

        public static List<Orderbook> lOrderbookBit = new List<Orderbook>();
        public static List<Orderbook> lOrderbookLite = new List<Orderbook>();

        public static bool WebServiceTicker(String uri)
        {
            ticket = new JSONHelper().WebServiceTicker(uri);

            return (!ticket.Buy.Equals(string.Empty));
        }

        public static bool WebServiceOrderbook(String uri)
        {
            List<Orderbook> lances;
            List<Orderbook> pedidos;

            new JSONHelper().WebServiceOrderbook(uri, out pedidos, out lances);
            if (pedidos.Count > 0)
            {
                lOrderbook_Pedidos = pedidos;
                lOrderbook_Lances = lances;
            }
            return (pedidos.Count > 0);
        }

        public static bool WebServiceTrades(String uri)
        {
            lTrades = new JSONHelper().WebServiceTrades(uri);
            return (lTrades.Count > 0);
        }
    }

    public class Ticker : INotifyPropertyChanged
    {
        public string High { get; set; }
        public string Low { get; set; }
        public string Vol { get; set; }
        public string Buy { get; set; }
        public string Sell { get; set; }
        public string Date { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private string _last;
        public string Last 
        { 
            get { return this._last; } 
            set 
            { 
                this._last = value; 
                PropertyChanged(this, new PropertyChangedEventArgs(value)); 
            } 
        }


    }

    public class Orderbook
    {
        public string Preco { get; set; }
        public string Volume { get; set; }
    }

    public class Trades
    {
        public string Time { get; set; }
        public string Preco { get; set; }
        public string Quantidade { get; set; }
        public string Codigo { get; set; }
        public string Tipo { get; set; }
    }
}
