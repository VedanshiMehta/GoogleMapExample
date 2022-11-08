
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
using TabLayoutExample;

namespace TabLayoutExample
{
    public class DeliveringFragment : Fragment
    {
        private List<DeliveryDetails> _deliveryDetails = new List<DeliveryDetails>();
        private RecyclerView _recyclerView;

        private RecyclerView.LayoutManager _linearLayout;
        private DeliveryAdapter _adapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.deliveringfragment, container, false);
            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewDelivering);
            _linearLayout = new LinearLayoutManager(Activity);
            _recyclerView.SetLayoutManager(_linearLayout);
            GetDeliveringData();

            return view;

        }

        private void GetDeliveringData()
        {
             var userid = (Activity as DeliveryPersonDashboardActivity).deliveryPersonId;
            DeliverDetailsDataBase.CreateDelivery<DeliveryDetails>();
            _deliveryDetails = DeliverDetailsDataBase.GetBeingDelievred(userid);
            _adapter = new DeliveryAdapter(Activity, _deliveryDetails);
            _recyclerView.SetAdapter(_adapter);
            _adapter.ItemClick += _adapter_ItemClick1;
        }

        private void _adapter_ItemClick1(object sender, DeliveryAdapterEventArgs e)
        {
            var selecteddelivery = _adapter.GetItem(e.Position);
            Intent i = new Intent(Activity, typeof(DeliverActivity));
            i.PutExtra("latitude", selecteddelivery.destinationLatitude);
            i.PutExtra("longitude", selecteddelivery.destinationLongitude);
            i.PutExtra("deliveryId", selecteddelivery.id);
            StartActivity(i);
        }

        public override void OnResume()
        {
            base.OnResume();
            GetDeliveringData();
        }


    }
}