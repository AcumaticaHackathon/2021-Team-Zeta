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
        //should be on setup page
        #region UsrTZAPIKey
        public abstract class usrTZAPIKey : BqlString.Field<usrTZAPIKey>
        {
        }
        [PXDBString(19)]
        [PXUIField(DisplayName = "API Key")]
        public string UsrTZAPIKey { get; set; }
        #endregion

        //should be on setup page
        #region UsrTZSecretKey
        public abstract class usrTZSecretKey : BqlString.Field<usrTZSecretKey>
        {
        }
        [PXDBString(16)]
        [PXUIField(DisplayName = "Secret Key")]
        public string UsrTZSecretKey { get; set; }
        #endregion

        //Address of wallet
        #region UsrTZWalletAddress
        public abstract class usrTZWalletAddress : BqlString.Field<usrTZWalletAddress>
        {
        }
        [PXDBString(35)]
        [PXUIField(DisplayName = "Wallet Address")]
        public string UsrTZWalletAddress { get; set; }
        #endregion
    }
}
