using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace Coins
{
    public class JSONHelper
    {
        public Ticker WebServiceTicker(string uri)
        {
            return ClasseTicker(RetornoWebService(uri));
        }

        private Ticker ClasseTicker(string json)
        {
            Ticker retorn = new Ticker();
            retorn.PropertyChanged += (sender, args) => VerificaValor(args.PropertyName);

            if (json.Equals(string.Empty)) return retorn;
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

            JObject jObject = JObject.Parse(json);
            JToken jUser = jObject["ticker"];
            retorn.High = double.Parse(jUser["high"].ToString()).ToString("n6");
            retorn.Low = double.Parse(jUser["low"].ToString()).ToString("n6");
            retorn.Vol = double.Parse(jUser["vol"].ToString()).ToString("n6");
            retorn.Last = double.Parse(jUser["last"].ToString()).ToString("n6");
            retorn.Buy = double.Parse(jUser["buy"].ToString()).ToString("n6");
            retorn.Sell = double.Parse(jUser["sell"].ToString()).ToString("n6");
            retorn.Date = epoch.AddMilliseconds(((double)long.Parse(jUser["date"].ToString())) * 1000d).AddHours(-3).ToString();

            return retorn;
        }

        private object VerificaValor(string valor)
        {
            List<Alerta> lItem = new List<Alerta>();
            StringBuilder msg = new StringBuilder();

            foreach (Alerta item in AlertaSalvo.lAlerta)
            {
                if (item.Negociacao == TipoNegociacao.Venda)
                {
                    if (item.TipoCoin == (double.Parse(valor) < 500 ? TipoCoin.Litecoin : TipoCoin.Bitcoin) && double.Parse(item.Valor) <= double.Parse(valor))
                    {
                        lItem.Add(item);
                        msg.AppendLine(item.Negociacao.ToString() + ": " + item.TipoCoin.ToString() + " - R$ " + item.Valor);
                    }
                }
                else
                {
                    if (item.TipoCoin == (double.Parse(valor) < 500 ? TipoCoin.Litecoin : TipoCoin.Bitcoin) && double.Parse(item.Valor) >= double.Parse(valor))
                    {
                        lItem.Add(item);
                        msg.AppendLine(item.Negociacao.ToString() + ": " + item.TipoCoin.ToString() + " - R$ " + item.Valor);
                    }
                }
            }

            foreach (Alerta item in lItem)
	        {
                AlertaSalvo.lAlerta.Remove(item);
	        }
            if (lItem.Count > 0)
            {
                AlertaSalvo.t = true;
                AlertaSalvo.Notifica(msg.ToString());

                AlertaSalvo.GravarAlerta();
            }

            return false;
        }

        public void WebServiceOrderbook(string uri, out List<Orderbook> asks, out List<Orderbook> bids)
        {
            asks = null;
            bids = null;
            ClasseOrderbook(RetornoWebService(uri), out asks, out bids);

            double totalVolume = 0;
            if (uri.Contains("_litecoin"))
                Coin.lOrderbookLite.Clear();
            else
                Coin.lOrderbookBit.Clear();
            foreach (Orderbook item in bids)
            {
                Orderbook teste = new Orderbook();
                teste.Preco = item.Preco;

                totalVolume += double.Parse(item.Volume);
                teste.Volume = totalVolume.ToString();

                if (uri.Contains("_litecoin"))
                    Coin.lOrderbookBit.Add(teste);
                else
                    Coin.lOrderbookLite.Add(teste);
            }
            Coin.lOrderbookLite.Reverse();
            Coin.lOrderbookBit.Reverse();

            foreach (Orderbook item in asks )
            {
                Orderbook teste = new Orderbook();
                teste.Preco = item.Preco;

                totalVolume += double.Parse(item.Volume);
                teste.Volume = totalVolume.ToString();

                if (uri.Contains("_litecoin"))
                    Coin.lOrderbookBit.Add(teste);
                else
                    Coin.lOrderbookLite.Add(teste);
            }
        }
        private void ClasseOrderbook(string json, out List<Orderbook> asks, out List<Orderbook> bids)
        {
            asks = new List<Orderbook>();
            bids = new List<Orderbook>();
            if (json.Equals(string.Empty)) return;

            JObject jObject = JObject.Parse(json);
            for (int i = 0; i < jObject.Count; i++)
            {
                JToken jUser;
                if (i == 0)
                    jUser = jObject["asks"];
                else
                    jUser = jObject["bids"];

                foreach (var item in jUser)
                {
                    Orderbook order = new Orderbook();
                    if (i == 0)
                    {
                        order.Preco = double.Parse(item[0].ToString()).ToString("n6");
                        order.Volume = double.Parse(item[1].ToString()).ToString("n6");
                        asks.Add(order);
                    }
                    else
                    {
                        order.Preco = double.Parse(item[0].ToString()).ToString("n6");
                        order.Volume = double.Parse(item[1].ToString()).ToString("n6");
                        bids.Add(order);
                    }

                }
			}
        }

        public List<Trades> WebServiceTrades(string uri)
        {
            return ClasseTrades(RetornoWebService(uri));
        }
        private List<Trades> ClasseTrades(string json)
        {
            List<Trades> retorn = new List<Trades>();
            if (json.Equals(string.Empty)) return retorn;

            JToken jUser = JToken.Parse(json);
            foreach (var item in jUser)
            {
		        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified );
                Trades trade = new Trades();

                trade.Time = epoch.AddMilliseconds(((double)long.Parse(item["date"].ToString())) * 1000d).AddHours(-3).ToString("HH:mm:ss");
                trade.Preco = double.Parse(item["price"].ToString()).ToString("n6");
                trade.Quantidade = double.Parse(item["amount"].ToString()).ToString("n6");
                trade.Codigo = item["tid"].ToString();
                trade.Tipo = item["type"].ToString().Equals("buy") ? "COMPRA" : "VENDA";

                retorn.Add(trade);
            }

            return retorn;
        }

        private static string RetornoWebService(string uri)
        {
            string jsonString = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(responseStream))
            {
                jsonString = reader.ReadToEnd();
                reader.Close();
            }

            return jsonString;
        }
    }
}
