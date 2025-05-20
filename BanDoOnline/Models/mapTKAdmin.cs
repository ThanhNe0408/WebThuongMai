using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDoOnline.Models
{
    public class mapTKAdmin
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public TKADMIN TimKiem(string username, string password)
        {
            var user = db.TKADMINs.Where(m => m.Username == username & m.Passwords == password).ToList();
            if (user.Count() > 0)
            {
                return user[0];
            }
            else
            {
                return null;
            }
        }
    }
}