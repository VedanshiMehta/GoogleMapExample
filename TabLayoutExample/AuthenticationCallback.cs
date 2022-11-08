using Android.App;
using Android.Content;
using Android.Hardware.Fingerprints;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Hardware.Fingerprint;
using SharedDeliveryPersonProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TabLayoutExample
{
    [Obsolete]
    public class AuthenticationCallback : FingerprintManagerCompat.AuthenticationCallback
    {
        Activity activity;
        int userId;
        bool isDeliveryPerson;
        ErrorModel errorModel;
        [Obsolete]
        public AuthenticationCallback(Activity activity, int userId,bool isDeliveryPerson, ErrorModel errorModel)
        {
            this.activity = activity;
            this.userId = userId;
            this.isDeliveryPerson = isDeliveryPerson;
            this.errorModel = errorModel;
        }


        public override void OnAuthenticationSucceeded(FingerprintManagerCompat.AuthenticationResult result)
        {
            base.OnAuthenticationSucceeded(result);
            if (isDeliveryPerson == false)
            {

                Intent i = new Intent(activity, typeof(MainActivity));
                i.PutExtra("deliveryPersonId", userId);
                activity.StartActivity(i);
                activity.Finish();
            }
            else if (isDeliveryPerson == true)
            {
                Intent j = new Intent(activity, typeof(DeliveryPersonDashboardActivity));
                j.PutExtra("deliveryPersonId", userId);
                activity.StartActivity(j);
                activity.Finish();
            }
        }
        public override void OnAuthenticationFailed()
        {
            base.OnAuthenticationFailed();
            Toast.MakeText(activity, "Faliure", ToastLength.Short).Show();
        }
    }

}