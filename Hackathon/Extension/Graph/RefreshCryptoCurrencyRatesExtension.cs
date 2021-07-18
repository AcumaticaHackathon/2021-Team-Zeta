using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using PX.Common;
using PX.Data;
using PX.Objects.CM;
using PX.Objects.CS;
using BlockSharp;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hackathon
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class RefreshCryptoCurrencyRatesExtension : CryptoGraphExtensionBase<RefreshCurrencyRates>
    {
        private RestClient _client = new RestClient
        {
            BaseUrl = new Uri("https://api.coingecko.com/api/v3/")
        };

		public override void Initialize()
		{
			base.Initialize();
		}

		public delegate Dictionary<string, decimal> GetRatesFromServiceDelegate(RefreshFilter filter, List<RefreshRate> list, DateTime date);

		[PXOverride]
        public virtual IEnumerable currencyRateList(Func<IEnumerable> baseDelegate)
        {
            PXCache rateTypesCache = Base.Caches[typeof(CurrencyRateType)];

            var currenciesWithRatesCombinations =
                PXSelectJoin<CurrencyList,
                   CrossJoin<CurrencyRateType>, Where<CurrencyRateType.refreshOnline, Equal<boolTrue>,
                       And<CurrencyList.isActive, Equal<boolTrue>,
                       And<CurrencyList.curyID, NotEqual<Current<RefreshFilter.curyID>>,
                       And<
                           Where<CurrencyRateType.curyRateTypeID, Equal<Current<RefreshFilter.curyRateTypeID>>,
                                Or<Current<RefreshFilter.curyRateTypeID>, IsNull>>>>>>>
                    .Select(Base);

            foreach (PXResult<CurrencyList, CurrencyRateType> res in currenciesWithRatesCombinations)
            {
                CurrencyList curr = res;
                CurrencyRateType rateType = res;

                RefreshRate rate = new RefreshRate
                {
                    FromCuryID = curr.CuryID,
                    CuryRateType = rateType.CuryRateTypeID,
                    OnlineRateAdjustment = rateType.OnlineRateAdjustment
                };

                bool isCryptoCurrency = rateType.GetIsCryptoCurrency(rateTypesCache);
                rate.SetIsCryptoCurrency(Base.CurrencyRateList.Cache, isCryptoCurrency);

                Base.CurrencyRateList.Cache.SetStatus(rate, PXEntryStatus.Held);
                yield return rate;
            }
        }

        /// <summary>
        /// Receive Currency Rates from external service
        /// </summary>
        /// <param name="filter">RefreshCurrency Rates Parameters (to get ToCurrency)</param>
        /// <param name="list">Rates to update (For overrides only: to switch services for different currencies etc.)</param>
        /// <param name="date">Date to pass to external service</param>
        /// <returns>Rate value for each currency returned by service</returns>
        [PXOverride]
        public virtual Dictionary<string, decimal> GetRatesFromService(RefreshFilter filter, List<RefreshRate> ratesToRefresh, DateTime date,
                                                                       GetRatesFromServiceDelegate getRatesFromServiceBaseMethod)
        {
            var ratesByIsCryptoCurrencyFlag = ratesToRefresh.ToLookup(refreshRate => refreshRate.GetIsCryptoCurrency(Base.CurrencyRateList.Cache));
            var cryptoCurrencyRates = ratesByIsCryptoCurrencyFlag[true].ToList();
            List<RefreshRate> nonCryptoCurrencyRates = ratesByIsCryptoCurrencyFlag[false].ToList();

            Dictionary<string, decimal> nonCryptoCurrencyRatesFromExternalApi = getRatesFromServiceBaseMethod(filter, nonCryptoCurrencyRates, date);
            Dictionary<string, decimal> cryptoCurrencyRatesFromExternalApi = RatesFromExternalApiForCryptoCurrencies(filter, cryptoCurrencyRates, date);

            var ratesFromExternalApi = nonCryptoCurrencyRatesFromExternalApi;
            ratesFromExternalApi.AddRange(cryptoCurrencyRatesFromExternalApi);

            return ratesFromExternalApi;
        }

        protected virtual Dictionary<string, decimal> RatesFromExternalApiForCryptoCurrencies(RefreshFilter filter, List<RefreshRate> cryptoCurrencyRates, DateTime date)
        {
            var cryptoCurrencyRatesFromExternalApi = new Dictionary<string, decimal>();

            IRestRequest restRequest = new RestRequest()
            {
                Resource = "simple/price"
            };

            var idsString = "bitcoin";//cryptoCurrencyRates.Select(rate => rate.FromCuryID).Join(",");
            restRequest = restRequest.AddParameter("vs_currencies", filter.CuryID)
                                     .AddParameter("ids", idsString);

            var ratesResponse = _client.Execute(restRequest);

            if (ratesResponse == null)
                return cryptoCurrencyRatesFromExternalApi;

            if (ratesResponse.ErrorException != null)
            {
                throw ratesResponse.ErrorException;
            }

            JObject json = (JObject)JsonConvert.DeserializeObject(ratesResponse.Content);

            if (json == null)
                return cryptoCurrencyRatesFromExternalApi;

            foreach (var curencyObj in json.Children().OfType<JProperty>())
			{
                string name = curencyObj.Name;

                if (name == "bitcoin")
                    name = "BTC";

                var rateObj = curencyObj.First.First as JProperty;
                var rateValue = rateObj.Value.ToString();

                if (rateObj != null && Decimal.TryParse(rateValue, out var rate))
                {
                    cryptoCurrencyRatesFromExternalApi.Add(name, rate);
                }          
            }

			return cryptoCurrencyRatesFromExternalApi;
        }
    }
}
