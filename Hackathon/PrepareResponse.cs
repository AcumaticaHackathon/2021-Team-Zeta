using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    public class Input
    {
        public int input_index { get; set; }
        public string previous_txid { get; set; }
        public int previous_output_index { get; set; }
        public string input_value { get; set; }
        public string spending_address { get; set; }
    }

    public class Output
    {
        public int output_index { get; set; }
        public string output_category { get; set; }
        public string output_value { get; set; }
        public string receiving_address { get; set; }
    }

    public class InputAddressData
    {
        public int required_signatures { get; set; }
        public IList<string> public_keys { get; set; }
        public string address { get; set; }
        public string address_type { get; set; }
    }

    public class Algorithm
    {
        public string pbkdf2_salt { get; set; }
        public int pbkdf2_iterations { get; set; }
        public string pbkdf2_hash_function { get; set; }
        public int pbkdf2_phase1_key_length { get; set; }
        public int pbkdf2_phase2_key_length { get; set; }
        public string aes_iv { get; set; }
        public string aes_cipher { get; set; }
        public string aes_auth_tag { get; set; }
        public object aes_auth_data { get; set; }
    }

    public class UserKey
    {
        public string public_key { get; set; }
        public string encrypted_passphrase { get; set; }
        public Algorithm algorithm { get; set; }
    }

    public class Data
    {
        public string network { get; set; }
        public string tx_type { get; set; }
        public IList<Input> inputs { get; set; }
        public IList<Output> outputs { get; set; }
        public IList<InputAddressData> input_address_data { get; set; }
        public UserKey user_key { get; set; }
        public int estimated_tx_size { get; set; }
        public string expected_unsigned_txid { get; set; }
    }

    public class PrepareResponse
    {
        public string status { get; set; }
        public Data data { get; set; }
    }
}
