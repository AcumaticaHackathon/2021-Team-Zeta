using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PX.Common;
using PX.Data;
using PX.Objects.CM;

namespace Hackathon
{
    public static class Utils
    {
        public static bool GetIsCryptoCurrency(this CurrencyRateType currencyRateType, PXCache rateTypesCache) =>
            rateTypesCache.CheckIfNull(nameof(rateTypesCache))
                          .GetValueExt<CurrencyRateTypeCryptoExt.usrIsCryptoCurrency>(currencyRateType) as bool? ?? false;

        public static void SetIsCryptoCurrency(this CurrencyRateType currencyRateType, PXCache rateTypesCache, bool isCryptoCurrency) =>
              rateTypesCache.CheckIfNull(nameof(rateTypesCache))
                            .SetValue<CurrencyRateTypeCryptoExt.usrIsCryptoCurrency>(currencyRateType, isCryptoCurrency);

        public static bool GetIsCryptoCurrency(this RefreshRate refreshRate, PXCache cache) =>
           cache.CheckIfNull(nameof(cache))
                .GetValueExt<RefreshRateCryptoExt.usrIsCryptoCurrency>(refreshRate) as bool? ?? false;

        public static void SetIsCryptoCurrency(this RefreshRate refreshRate, PXCache cache, bool isCryptoCurrency) =>
           cache.CheckIfNull(nameof(cache))
                .SetValue<RefreshRateCryptoExt.usrIsCryptoCurrency>(refreshRate, isCryptoCurrency);
    }
}
