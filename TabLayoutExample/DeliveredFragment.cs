
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using SharedDeliveryPersonProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TabLayoutExample
{
    public class DeliveredFragment : Fragment
    {

        private RecyclerView _recyclerView;

        private RecyclerView.LayoutManager _linearLayout;
        private DeliveryAdapter _adapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.deliveredfragment, container, false);
            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewDelivered);
            _linearLayout = new LinearLayoutManager(Activity);
            _recyclerView.SetLayoutManager(_linearLayout);
            GetDeliveredData();

            return view;
        }

        private void GetDeliveredData()
        {
            var userid = (Activity as DeliveryPersonDashboardActivity).deliveryPersonId;
            var _deliveryDetails = DeliverDetailsDataBase.GetDelivered(userid);
            _adapter = new DeliveryAdapter(Activity, _deliveryDetails);
            _recyclerView.SetAdapter(_adapter);
        }
        public override void OnResume()
        {
            base.OnResume();
            GetDeliveredData();
        }
    }
}