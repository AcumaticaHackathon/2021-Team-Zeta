using PX.Data;
using PX.Data.BQL;
using PX.Objects.CM;

namespace Hackathon
{	
	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
	public sealed class CurrencyRateTypeCryptoExt : PXCacheExtension<CurrencyRateType>
	{
		#region UsrIsCryptoCurrency
		public abstract class usrIsCryptoCurrency : BqlBool.Field<usrIsCryptoCurrency>
		{
		}

		[PXDBBool()]
		[PXDefault(false)]
		[PXUIField(DisplayName = "Is Crypto Currency", Enabled = false)]
		public bool? UsrIsCryptoCurrency { get; set; }
		#endregion
	}
}
