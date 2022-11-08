using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.Button;
using SharedDeliveryPersonProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TabLayoutExample;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace TabLayoutExample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class PickUpActivity : AppCompatActivity, IOnMapReadyCallback
    {
        private Toolbar _toolbar;
        private MapFragment _mapFragmentPickUp;
        private MaterialButton _materialButtonPickUP;
        private int userid;
        private int deliveryid;
        private double latitude, longitude;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.pickuplayout);
            UIReferences();
            UIClickEvents();
            SetSupportActionBar(_toolbar);
            SupportActionBar.Title = "Picked Up";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            ISharedPreferences sharedPreferences = this.GetSharedPreferences("LocalSharePrefrence", FileCreationMode.Private);
            if (sharedPreferences != null)
            {
                userid = sharedPreferences.GetInt("roles", 0);
            }
            deliveryid = Intent.GetIntExtra("deliveryid", 0);

            latitude = Intent.GetDoubleExtra("latitude", 0);
            longitude = Intent.GetDoubleExtra("longitude", 0);
            _mapFragmentPickUp.GetMapAsync(this);
        }

        private void UIClickEvents()
        {
            _toolbar.NavigationClick += _toolbar_NavigationClick;
            _materialButtonPickUP.Click += _materialButtonPickUP_Click;
        }

        private void _materialButtonPickUP_Click(object sender, EventArgs e)
        {
            var pickedup = DeliverDetailsDataBase.MarkAsPickedUp(deliveryid, userid);
            if (pickedup == true)
            {
                Toast.MakeText(this, "Picked up your parcel sucessfully", ToastLength.Short).Show();
                Finish();
            }
            else
            {
                Toast.MakeText(this,"Picking up your parcel has been failed",ToastLength.Short).Show();
            }
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
            _toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbarPickUp);
            _mapFragmentPickUp = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.fragmentMapPickUp);
            _materialButtonPickUP = FindViewById<MaterialButton>(Resource.Id.buttonPickedUp);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(new LatLng(latitude, longitude));
            markerOptions.SetTitle("Pick Up Here");
            googleMap.AddMarker(markerOptions);
            googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(latitude, longitude), 12));
        }
    }
}