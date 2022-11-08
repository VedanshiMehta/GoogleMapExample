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
    public class Deliveries : Fragment
    {
        private RecyclerView _recyclerView;
      
        private RecyclerView.LayoutManager _linearLayout;
        private DeliveryAdapter _adapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
           
            
        }
        public override void OnResume()
        {
            base.OnResume();
            GetDeliveriesData();
            
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            View view = inflater.Inflate(Resource.Layout.deliveries, container, false);
            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewDeliveries);
            _linearLayout = new LinearLayoutManager(Activity);
            _recyclerView.SetLayoutManager(_linearLayout);
            GetDeliveriesData();
            
            return view;
        }

        private void GetDeliveriesData()
        {
            var deliveries = DeliverDetailsDataBase.ReadDeliveries();
            
                _adapter = new DeliveryAdapter(Activity, deliveries);
                _recyclerView.SetAdapter(_adapter);
           
        }
    }
}