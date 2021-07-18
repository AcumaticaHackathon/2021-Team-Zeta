using PX.Data;
using PX.Objects.CA;
using System.Collections;

namespace Hackathon
{
    public class CashAccountMaintExtension : PXGraphExtension<CashAccountMaint>
    {
		#region CheckBalance

		public PXAction<CashAccount> CheckBalance;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="adapter"></param>
		/// <returns></returns>
		[PXButton]
		[PXUIField(DisplayName = "Check Balance", MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update)]
		protected virtual IEnumerable checkBalance(PXAdapter adapter)
		{
			CashAccount account = Base.CashAccount.Current;
			if(account != null)
            {
				CashAccountCryptoExt accountExt = PXCache<CashAccount>.GetExtension<CashAccountCryptoExt>(account);

				BlockSharp.BlockSharp client = new BlockSharp.BlockSharp(accountExt.UsrTZAPIKey);

				var result = client.GetBalance();
            }

			return adapter.Get();
		}

		#endregion
	}
}
