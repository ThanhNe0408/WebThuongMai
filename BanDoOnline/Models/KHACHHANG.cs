using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoOnline.Models
{
    public partial class KHACHHANG
    {

        public int FailedLoginAttempts { get; set; }
        public DateTime? LockoutEndTime { get; set; }
    }

}
