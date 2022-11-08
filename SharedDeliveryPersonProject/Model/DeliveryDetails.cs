using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedDeliveryPersonProject.Model
{
    [Table("DELIVERYDETAILS")]
    public class DeliveryDetails
    {
        [PrimaryKey]
        [Column("Id")]
        [AutoIncrement]
        public int id { get; set; }

        [Column("Name")]
        public string name { get; set; }

        [Column("OLatitude")]
        public double originLatitue { get; set; }

        [Column("OLongitude")]
        public double originLongitutde { get; set; }

        [Column("DLatitude")]
        public double destinationLatitude { get; set; }

        [Column("DLongitude")]
        public double destinationLongitude { get; set; }
        /// <summary>
        /// 0 - waiting delievry person
        /// 1 - being deliver
        /// 2 - delivered
        /// </summary>
        [Column("Status")]
        public int status { get; set; }

        [Column("DeliveryPersonId"),ForeignKey(typeof(DeliveryPerson))]
        public int deliveryPersonId { get; set; }




        public override string ToString()
        {
            return $"{name} - {status}";
        }

    }
}
