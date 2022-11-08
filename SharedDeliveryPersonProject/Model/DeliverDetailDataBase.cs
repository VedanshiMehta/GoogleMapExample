using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Environment = System.Environment;

namespace SharedDeliveryPersonProject.Model
{


    public static class DeliverDetailsDataBase 
    {
        public static string DBName = "SQLite.db3";
        public static string DBPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DBName);
        
        public static SQLiteConnection sQLiteConnection;


    
        public static void CreateDataBase()
        {
            try
            {
                Console.WriteLine(DBPath);

                sQLiteConnection = new SQLiteConnection(DBPath);
      
                Console.WriteLine("Database Created Sucessfully");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void CreateDelivery<T>()
        {
            try
            {
                CreateDataBase();
                var created = sQLiteConnection.CreateTable<T>();
                Console.WriteLine("Succefully Table Created!....");

            }

            catch (Exception ex)
            {
                Console.WriteLine("Database Exception:" + ex);

            }
        }


     

        public static bool InsertDelivery<T>(T t)
        {
            var insert = sQLiteConnection.Insert(t);

            if (insert == -1)
            {
                return false;
            }
            else
            {
                Console.WriteLine("Data inserted Sucessfully");
                return true;
            }
        }

        public static bool UpdateDelivery<T>(T t)
        {
            var insert = sQLiteConnection.Update(t);

            if (insert == -1)
            {
                return false;
            }
            else
            {
                Console.WriteLine("Data updated Sucessfully");
                return true;
            }
        }

        public static bool DeleteDelivery<T>(T t)
        {
            var insert = sQLiteConnection.Delete(t);
            if (insert == -1)
            {
                return false;
            }
            else
            {
                Console.WriteLine("Data updated Sucessfully");
                return true;
            }

        }


        public static List<UserDetails> ReadUsers()
        {
            try
            {
                var userDetails = sQLiteConnection.Table<UserDetails>().ToList();
                return userDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        public static List<DeliveryPerson> ReadDeliveryPerson()
        {
            try
            {
                var deliverypersonDetails = sQLiteConnection.Table<DeliveryPerson>().ToList();
                return deliverypersonDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public static List<DeliveryDetails> ReadDeliveryDetails()
        {
            try
            {
                var deliveriesDetails = sQLiteConnection.Table<DeliveryDetails>().ToList();
                return deliveriesDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        public static List<DeliveryDetails> ReadDeliveries()
        {
            try
            {
                var deliveriesDetails = sQLiteConnection.Table<DeliveryDetails>().Where(x => x.status != 2).ToList();
                return deliveriesDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        public static List<DeliveryDetails> ReadDelivered()
        {
            try
            {
                var deliveryDetails = sQLiteConnection.Table<DeliveryDetails>().Where(x => x.status == 2).ToList();
                return deliveryDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }


        public static DeliveryPerson GetDeliveryPerson(int personid)
        {
            return sQLiteConnection.Table<DeliveryPerson>().Where(x => x.id == personid).FirstOrDefault();
        }

        public static List<DeliveryDetails> GetDelivered(int userid)
        {
            try
            {
                var deliveryDetails = sQLiteConnection.Table<DeliveryDetails>().Where(x => x.status == 2 && x.deliveryPersonId == userid).ToList();
                return deliveryDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public static List<DeliveryDetails> GetBeingDelievred(int id)
        {
            return sQLiteConnection.Table<DeliveryDetails>().Where(x => x.status == 1 && x.deliveryPersonId == id).ToList();
        }

        public static List<DeliveryDetails> GetWaiting()
        {
            try
            {

                var deliveryDetails = sQLiteConnection.Table<DeliveryDetails>().Where(x => x.status == 0).ToList();
                return deliveryDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public static bool MarkAsPickedUp(int deliveryid, int deliveryPersonId)
        {
            try
            {
                var deliveryDetails = sQLiteConnection.Table<DeliveryDetails>().Where(x => x.id == deliveryid).ToList().FirstOrDefault();
                deliveryDetails.status = 1;
                deliveryDetails.deliveryPersonId = deliveryPersonId;
                var update = sQLiteConnection.Update(deliveryDetails);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public static bool MarkAsDelivered(int deliveryid)
        {
            try
            {
                var deliveryDetails = sQLiteConnection.Table<DeliveryDetails>().Where(x => x.id == deliveryid).ToList().FirstOrDefault();
                deliveryDetails.status = 2;
                var update = sQLiteConnection.Update(deliveryDetails);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

    }




}
