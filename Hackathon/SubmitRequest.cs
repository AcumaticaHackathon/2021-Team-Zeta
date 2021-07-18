using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    public class Signature
    {
        public int input_index { get; set; }
        public string public_key { get; set; }
        public string signature { get; set; }
    }

    public class SubmitRequest
    {
        public string tx_type { get; set; }
        public string tx_hex { get; set; }
        public IList<Signature> signatures { get; set; }
    }

}
