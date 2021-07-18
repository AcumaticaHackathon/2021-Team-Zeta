using System;
using System.IO;
using System.Reflection;
using PX.Data;
using PX.Objects.CA;
using System.Collections;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace Hackathon
{
	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
	public class CashAccountMaintExtension : CryptoGraphExtensionBase<CashAccountMaint>
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

                throw new Exception(result.AvailableBalance + " Doge coin available");
            }

            return adapter.Get();
        }

        public PXAction<CashAccount> MakeTransfer;

        [PXButton]
        [PXUIField(DisplayName = "Make Transfer", MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update)]
        protected virtual IEnumerable makeTransfer(PXAdapter adapter)
        {
            CashAccount account = Base.CashAccount.Current;
            if (account != null)
            {
                WebRequest webRequest = WebRequest.Create(@"https://block.io/api/v2/prepare_transaction/?api_key=1fb1-ce42-104a-dd17&from_addresses=2Myzgr5pCrBLQ8zr6S7SRbsoAUs6zv5ew89&to_address=2MzPUSrjkauaWtuzGV9RYm6pi4SfygEhHBL&amounts=10");
                WebResponse webResp = webRequest.GetResponse();

                String responseString = string.Empty;
                string responseHex = string.Empty;
                using (Stream stream = webResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    responseString = reader.ReadToEnd();
                    responseHex = BlockSharp.Core.Crypto.ASCIIToHex(responseString);
                }

                PrepareResponse response = JsonConvert.DeserializeObject<PrepareResponse>(responseString);


                SubmitRequest request = new SubmitRequest();

                request.tx_type = response.data.tx_type;
                request.tx_hex = responseHex;

                Signature sig = new Signature();
                foreach (InputAddressData addressData in response.data.input_address_data)
                {
                    int index = 0;
                    foreach (string publicKey in addressData.public_keys)
                    {
                        sig.input_index = index;
                        sig.public_key = publicKey;
                        request.signatures.Add(sig);
                        index++;
                    }
                }

                string result = Newtonsoft.Json.JsonConvert.SerializeObject(request);

                webRequest = WebRequest.Create(@"https://block.io/api/v2/submit_transaction/?api_key=1fb1-ce42-104a-dd17 -d " + result + " -H 'Content-Type: application/json'");
                webResp = webRequest.GetResponse();

                responseString = string.Empty;
                using (Stream stream = webResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    responseString = reader.ReadToEnd();
                }
            }
            return adapter.Get();
        }

        #endregion
    }
}
