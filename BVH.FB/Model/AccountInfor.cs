using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BVH.FB.Model
{
    public class AccountInfor
    {
        public string UID { get; set; }
        public string Password { get; set; }
        public string TwoFactor { get; set; }
        public string Others { get; set; }
        public string PageID { get; set; }
        public string Cookie { get; set; }
        public string State { get; set; }
    }
}
