using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TabLayoutExample
{
    public class MyMap : Java.Lang.Object,IOnMapReadyCallback
    {
        public GoogleMap Map;
        private double latitude;
        private double longitude;

        public MyMap(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public IntPtr Handle => Handled();


        public void OnMapReady(GoogleMap googleMap)
        {
            Map = googleMap;
            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(new LatLng(latitude, longitude));
            markerOptions.SetTitle("Your Location");
            googleMap.AddMarker(markerOptions);
            googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(latitude, longitude), 10));
        }


        private IntPtr Handled()
        {
            return IntPtr.Zero;
        }
    }
}