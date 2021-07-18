using PX.Data;
using PX.Objects.AP;
using PX.Objects.CA;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.CM;
using System.Collections.Generic;
using PX.Objects.IN;
using System.Reflection;
using System.IO;
using System;

namespace Hackathon
{
    public class APReleaseProcessExtension : PXGraphExtension<APReleaseProcess>
    {
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


		public delegate List<APRegister> ReleaseInvoiceDelegate(JournalEntry je, ref APRegister doc, PXResult<APInvoice, CurrencyInfo, Terms, Vendor> res, bool isPrebooking, out List<INRegister> inDocs);

        [PXOverride]
        public virtual List<APRegister> ReleaseInvoice(JournalEntry je, ref APRegister doc, PXResult<APInvoice, CurrencyInfo, Terms, Vendor> res, bool isPrebooking, out List<INRegister> inDocs, ReleaseInvoiceDelegate del)
        {
            APPayment payment = doc as APPayment;

            CashAccount cash = PXSelectReadonly<CashAccount, Where<CashAccount.accountID, Equal<Required<CashAccount.accountID>>>>.Select(this.Base, payment.CashAccountID);
            if (cash != null)
            {
                CashAccountCryptoExt cashExt = PXCache<CashAccount>.GetExtension<CashAccountCryptoExt>(cash);

                BlockSharp.BlockSharp client = new BlockSharp.BlockSharp(cashExt.UsrTZAPIKey);
            }
            return del(je, ref doc, res, isPrebooking, out inDocs);
        }
    }
}
