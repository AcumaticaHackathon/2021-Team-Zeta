using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PX.Data;
using PX.Data.BQL;
using PX.Objects.CA;


namespace Hackathon
{
	/// <summary>
	/// A cash account crypto extentenstion. This class cannot be inherited.
	/// </summary>
	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
	public sealed class CashAccountCryptoExt : PXCacheExtension<CashAccount>
	{
		#region UsrTZAPIKey
		public abstract class usrTZAPIKey : BqlString.Field<usrTZAPIKey>
		{
		}
		[PXDBString(19)]
		[PXUIField(DisplayName = "API Key")]
		public string UsrTZAPIKey { get; set; }
		#endregion
	}
}
