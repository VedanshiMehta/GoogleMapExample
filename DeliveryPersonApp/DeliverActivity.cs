using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.Button;
using SharedDeliveryPersonProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace DeliveryPersonApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class DeliverActivity : AppCompatActivity,IOnMapReadyCallback
    {
        private Toolbar _toolbar;
        private MapFragment _mapFragmentDeliver;
        private MaterialButton _materialButtonDeliver;
        private int id;
        private double latitude, longitude;

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here\

            SetContentView(Resource.Layout.deliverlayout);
            UIReferences();
            UIClickEVents();
            SetSupportActionBar(_toolbar);
            SupportActionBar.Title = "Delivered";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            id = Intent.GetIntExtra("deliveryId", 0);
            latitude = Intent.GetDoubleExtra("latitude", 0);
            longitude = Intent.GetDoubleExtra("longitude", 0);
            _mapFragmentDeliver.GetMapAsync(this);


        }

        private void UIClickEVents()
        {
            _toolbar.NavigationClick += _toolbar_NavigationClick;
            _materialButtonDeliver.Click += _materialButtonDeliver_Click;
        }

        private void _materialButtonDeliver_Click(object sender, EventArgs e)
        {
            var deliveryDetails = DeliverDetailsDataBase.MarkAsDelivered(id);
        }

        private void _toolbar_NavigationClick(object sender, Toolbar.NavigationClickEventArgs e)
        {
            Finish();
        }
        public override bool OnSupportNavigateUp()
        {
            OnBackPressed();
            return base.OnSupportNavigateUp();
        }

        [Obsolete]
        private void UIReferences()
        {
            _toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbarDelivery);
            _mapFragmentDeliver = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.fragmentMapDelivery);
            _materialButtonDeliver = FindViewById<MaterialButton>(Resource.Id.buttonDelivered);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            MarkerOptions marker = new MarkerOptions();
            marker.SetPosition(new LatLng(latitude, longitude));
            marker.SetTitle("Deliver here");
            googleMap.AddMarker(marker);
            googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(latitude, longitude), 12));
        }
    }
}