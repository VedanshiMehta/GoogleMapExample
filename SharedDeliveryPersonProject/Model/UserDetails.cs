using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedDeliveryPersonProject.Model
{
   
        [Table("USERDETAILS")]
        public class UserDetails
        {
            [PrimaryKey]
            [AutoIncrement]
            [Column("Id")]
            public int id { get; set; }

            [Column("Email")]
            public string email { get; set; }

            [Column("Password")]
            public string password { get; set; }


            [Column("MobileNumber")]
            public string mobilenumber { get; set; }

            [Column("IsDeliveryPerson")]
            public bool IsDeliveryPerson { get; set; }

        }
    
}
