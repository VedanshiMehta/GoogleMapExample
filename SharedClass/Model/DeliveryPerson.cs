using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClass.Model
{
    [Table("DELIVERYPERSON")]
    public class DeliveryPerson
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int id { get; set; }

        [Column("Email")]
        public string deliveryemail { get; set; }

        [Column("Password")]
        public string deliverypassword { get; set; }

        [Column("MobileNumber")]
        public string deliverymobilenumber { get; set; }

        [OneToMany]
        public List<DeliveryDetails> deliveryDetails { get; set; }
    }
}
