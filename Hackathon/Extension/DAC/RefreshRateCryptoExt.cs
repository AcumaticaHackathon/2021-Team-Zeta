using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PX.Data;
using PX.Data.BQL;
using PX.Objects.CM;

namespace Hackathon
{
	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
	public sealed class RefreshRateCryptoExt : PXCacheExtension<RefreshRate>
	{
		#region UsrIsCryptoCurrency
		public abstract class usrIsCryptoCurrency : BqlBool.Field<usrIsCryptoCurrency>
		{
		}

		[PXBool]
		[PXUIField(DisplayName = "Is Crypto Currency")]
		public bool? UsrIsCryptoCurrency { get; set; }
		#endregion
	}
}
