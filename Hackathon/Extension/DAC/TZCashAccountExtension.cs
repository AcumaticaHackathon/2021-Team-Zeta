using PX.Data;
using PX.Data.BQL;
using PX.Objects.CA;

namespace Hackathon
{
    public class TZCashAccountExtension : PXCacheExtension<CashAccount>
    {
		#region UsrTZ
		public abstract class usrTZAPIKey : BqlString.Field<usrTZAPIKey>
		{
		}
		[PXDBString(InputMask = "AAAA-AAAA-AAAA-AAAA")]
		[PXUIField(DisplayName = "API Key")]
		public string UsrTZAPIKey { get; set; }
		#endregion
	}
}
