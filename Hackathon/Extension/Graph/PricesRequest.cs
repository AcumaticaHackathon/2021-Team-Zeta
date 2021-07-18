using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using PX.Common;
using PX.Data;
using PX.Objects.CM;
using PX.Objects.CS;
using RestSharp;

namespace Hackathon
{
    public class PriceRequest
    { 
        public CryptoCurrencyRateInfo Currency { get; }
    }

    public class PricesRequest
    {
        public List<CryptoCurrencyRateInfo> Currencies { get; }
    }

    public class CryptoCurrencyRateInfo
	{
        public string CryproCurrencyName { get; }  

        public CurrencyRate Rate { get; }
    }

    public class CurrencyRate
	{
        public string ToCurrency { get; }

        public decimal Rate { get; }
    }
}
