using SyncFrameworkApp.Controls.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncFrameworkApp.Pages
{
    public partial class Index
    {
        public List<User> User { get; set; } = new();

        protected override void OnInitialized()
        {


        }
    }
}
