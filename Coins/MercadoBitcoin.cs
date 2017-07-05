using System;
using System.Collections.Generic;
using System.Text;

namespace Coins
{
    public static class MercadoBitcoin
    {
        //As requisições são no formato GET.
        //As requisições devem ser feitas para a URL
        private static string requisicaoDados = @"https://www.mercadobitcoin.net/api";
        public static string litecoin = @"_litecoin";

        public static string ticker = requisicaoDados + @"/v2/ticker";
        public static string orderbook = requisicaoDados + @"/orderbook";
        public static string trades = requisicaoDados + @"/trades";

        //https://www.mercadobitcoin.net/tapi/v3/.
        private static string requisicaoNegociacoes = @"https://www.mercadobitcoin.net/tapi/v3";
    }
}
