using System;
using System.Collections.Generic;
using System.Linq;

using PX.Data;
using PX.Objects.CA;
using PX.Objects.CM;

using System.Collections;

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
			var 
		}
	}
}
