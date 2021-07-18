using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PX.Data;
using PX.Objects.CA;


namespace Hackathon.Extension.DAC
{
	/// <summary>
	/// A cash account crypto extentenstion. This class cannot be inherited.
	/// </summary>
	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
	public sealed class CashAccountCryptoExt : PXCacheExtension<CashAccount>
	{

	}
}
