
using PX.Data;
using PX.Objects.AP;
using PX.Objects.CA;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.CM;
using System.Collections.Generic;
using PX.Objects.IN;

namespace Hackathon
{
    public class APReleaseProcessExtension : PXGraphExtension<APReleaseProcess>
    {
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
