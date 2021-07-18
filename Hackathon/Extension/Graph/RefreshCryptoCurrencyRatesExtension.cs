using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using PX.Data;
using PX.Objects.CM;
using PX.Objects.CS;

namespace Hackathon
{
	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
	public class RefreshCryptoCurrencyRatesExtension : PXGraphExtension<RefreshCurrencyRates>
    {
		//private const string ApiKey = "1fb1-ce42-104a-dd17";

		//[PXOverride]
		//public virtual string GetApiKey(Func<string> baseMethod) => ApiKey;

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

                bool isCryptoCurrency = rateTypesCache.GetValueExt<CurrencyRateTypeCryptoExt.usrIsCryptoCurrency>(rateType) as bool? ?? false;
                Base.CurrencyRateList.Cache.SetValue<RefreshRateCryptoExt.usrIsCryptoCurrency>(rate, isCryptoCurrency);

				Base.CurrencyRateList.Cache.SetStatus(rate, PXEntryStatus.Held);
                yield return rate;
            }
        }
	}
}
