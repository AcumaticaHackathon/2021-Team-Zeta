using PX.Data;
using PX.Data.BQL;
using PX.Objects.CM;

namespace Hackathon
{
	
	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
	public sealed class CurrencyRateTypeCryptoExt : PXCacheExtension<CurrencyRateType>
	{
		#region IsCryptoCurrency
		public abstract class isCryptoCurrency : BqlBool.Field<isCryptoCurrency>
		{
		}
		[PXDBBool()]
		[PXDefault(false)]
		[PXUIField(DisplayName = "Is Crypto Currency")]
		public bool? IsCryptoCurrency { get; set; }
		#endregion
	}
}
