using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncFrameworkApp.Controls.Data
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        //public string? LastName { get; set; }
        //public string? Email { get; set; }
        //public DateTime BirthDay { get; set; }
        //public DateTime RegisterDate { get; set; }

        public List<UserContact> Contacts { get; set; } = new List<UserContact>();
    }
}
