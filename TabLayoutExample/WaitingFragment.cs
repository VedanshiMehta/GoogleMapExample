
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
    public class WaitingFragment : Fragment
    {
        private List<DeliveryDetails> _deliveryDetails = new List<DeliveryDetails>();
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

            View view = inflater.Inflate(Resource.Layout.waitingfragment, container, false);
            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewWaiting);
            _linearLayout = new LinearLayoutManager(Activity);
            _recyclerView.SetLayoutManager(_linearLayout);
            GetWaitingData();

            return view;
        }

        private void GetWaitingData()
        {
            var _deliveryDetails = DeliverDetailsDataBase.GetWaiting();
            _adapter = new DeliveryAdapter(Activity, _deliveryDetails);
            _recyclerView.SetAdapter(_adapter);
            _adapter.ItemClick += _adapter_ItemClick1;
        }

        private void _adapter_ItemClick1(object sender, DeliveryAdapterEventArgs e)
        {
            var selecteddelivery = _adapter.GetItem(e.Position);
            Intent i = new Intent(Activity, typeof(PickUpActivity));
            i.PutExtra("latitude", selecteddelivery.originLatitue);
            i.PutExtra("longitude", selecteddelivery.originLongitutde);
            i.PutExtra("deliveryid", selecteddelivery.id);
            StartActivity(i);
        }

        public override void OnResume()
        {
            base.OnResume();
            GetWaitingData();
        }
       




    }
}