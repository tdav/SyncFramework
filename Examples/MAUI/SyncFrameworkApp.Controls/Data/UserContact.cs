using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncFrameworkApp.Controls.Data
{
    public class UserContact
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Phones { get; set; }
        public string Address { get; set; }
    }
}
