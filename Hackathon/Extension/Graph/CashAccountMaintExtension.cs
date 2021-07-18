using System;
using System.IO;
using System.Reflection;

using PX.Data;
using PX.Objects.CA;
using System.Collections;

namespace Hackathon
{
	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
	public class CashAccountMaintExtension : PXGraphExtension<CashAccountMaint>
    {
		#region CheckBalance

		public PXAction<CashAccount> CheckBalance;

		public override void Initialize()
		{
			AppDomain.CurrentDomain.AssemblyResolve += RestSharpAssemblyResolve;
		}

		private Assembly RestSharpAssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (args.Name != null && args.Name.Contains("RestSharp"))
			{
				string pxDataLocation = typeof(PXGraph).Assembly.CodeBase;
				
				if (Uri.IsWellFormedUriString(pxDataLocation, UriKind.Absolute))
				{
					Uri pxDataUri = new Uri(pxDataLocation);
					pxDataLocation = pxDataUri.LocalPath;
				}

				string binDir = Path.GetDirectoryName(pxDataLocation);
				string restSharp = Path.Combine(binDir, "RestSharp.dll");

				if (!File.Exists(restSharp))
					return null;

				try
				{
					var restSharpAssembly = Assembly.LoadFrom(restSharp);
					return restSharpAssembly;
				}
				catch (Exception)
				{
					return null;
				}
			}

			return null;
		}

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
