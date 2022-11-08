using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Google.Android.Material.Button;
using Java.Lang;
using SharedDeliveryPersonProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace TabLayoutExample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class AddDeliveries : AppCompatActivity, ILocationListener
    {
      
        private EditText _editTextPackage;
        private MaterialButton _materialButtonSave;
        private DeliveryDetails _deliveryDetails;
        private MapFragment _mapFragment;
        private double _latitude,_longitude;
        private LocationManager _locationManager;
        private ISharedPreferences _sharedPrefrence;
        private ISharedPreferencesEditor _sharedPreferenceEditor;
        private AlertDialog.Builder builder;
        private const int REQUEST_STORAGE = 2;
        private Toolbar _toolbarAdd;
        private MapFragment _mapFragmentDestination;
        private MyMap _myMapOrigin;
        private MyMap _myMapDestination;

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.adddeliveries);
            DeliverDetailsDataBase.CreateDelivery<DeliveryDetails>();
           
            
            UIReference();
            UIClickEvents();
            SharedPrefrenceIntialization();
            SetSupportActionBar(_toolbarAdd);
            SupportActionBar.Title = "Add Delivery Package";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
           
            _toolbarAdd.NavigationClick += _toolbarAdd_NavigationClick;
            _locationManager = GetSystemService(Context.LocationService) as LocationManager;


        }

        private void _toolbarAdd_NavigationClick(object sender, Toolbar.NavigationClickEventArgs e)
        {
           Finish();
        }
        public override bool OnSupportNavigateUp()
        {
            OnBackPressed();
            return base.OnSupportNavigateUp();
        }
        [Obsolete]
        private void SharedPrefrenceIntialization()
        {
            _sharedPrefrence = PreferenceManager.GetDefaultSharedPreferences(this);
            _sharedPreferenceEditor = _sharedPrefrence.Edit();
        }

        protected override void OnResume()
        {
            base.OnResume();
            GetUSerPermissionForLocation();
           
        }

        private void GetUSerPermissionForLocation()
        {
            if (Build.VERSION.SdkInt > BuildVersionCodes.M)
            {
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Android.Content.PM.Permission.Granted)
                {
                    GetLocation();
                }
                else
                {
                    if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
                    {
                        ShowRationaleDialog(REQUEST_STORAGE);
                    }
                    else if (_sharedPrefrence.GetBoolean(Manifest.Permission.AccessFineLocation, false))
                    {
                        builder = new AlertDialog.Builder(this);
                        builder.SetTitle(Resource.String.storage_permission_title);
                        builder.SetMessage(Resource.String.storage_permission_message);
                        builder.SetPositiveButton(Resource.String.grant, (s, e) =>
                        {
                            Intent intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
                            Android.Net.Uri uri = Android.Net.Uri.FromParts(scheme: "package", PackageName, null);
                            intent.SetData(uri);
                            StartActivityForResult(intent, 12);
                        });
                        builder.SetNegativeButton(Resource.String.cancel, (s, e) => { builder.Dispose(); });
                        builder.Show();
                    }
                    else
                    {
                        RequestPermissions(REQUEST_STORAGE);
                    }
                    _sharedPreferenceEditor.PutBoolean(Manifest.Permission.AccessCoarseLocation, true);
                    _sharedPreferenceEditor.Commit();
                }

            }
            else
            {
                GetLocation();
            }

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if(requestCode == REQUEST_STORAGE)
            {
                if(grantResults.Length > 0 && (grantResults[0] == Permission.Granted))
                {
                    GetLocation();
                }
                else
                {

                    Toast.MakeText(this, "Permission Denied", ToastLength.Short).Show();

                }
            }
        }
        private void ShowRationaleDialog(int rEQUEST_STORAGE)
        {
            builder = new AlertDialog.Builder(this);
            builder.SetTitle(Resource.String.storage_permission_title);
            builder.SetMessage(Resource.String.storage_permission_message);
            builder.SetPositiveButton(Resource.String.ok, (s, e) => { RequestPermissions(rEQUEST_STORAGE); });
            builder.SetNegativeButton(Resource.String.cancel, (s, e) => { builder.Dispose(); });
            builder.Show();
        }

        private void RequestPermissions(int rEQUEST_STORAGE)
        {
            RequestPermissions(new string[] { Manifest.Permission.AccessFineLocation }, rEQUEST_STORAGE);
        }

        private void GetLocation()
        {
           
            string provider = LocationManager.GpsProvider;

            if (_locationManager.IsProviderEnabled(provider))
            {
                _locationManager.RequestLocationUpdates(provider, 5000, 1, this);

            }

            var location = _locationManager.GetLastKnownLocation(LocationManager.NetworkProvider);
            if (location != null)
            {
                _latitude = location.Latitude;
                _longitude = location.Longitude;
                _myMapOrigin = new MyMap(_latitude, _longitude);
                _myMapDestination = new MyMap(_latitude, _longitude);
                _mapFragment.GetMapAsync(_myMapOrigin);
                _mapFragmentDestination.GetMapAsync(_myMapDestination);
            }
            else
            {
                Toast.MakeText(this, "Location not Found", ToastLength.Short).Show();
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }
        private void UIClickEvents()
        {
            _materialButtonSave.Click += _materialButtonSave_Click;
        }

        private void _materialButtonSave_Click(object sender, EventArgs e)
        {

            var originLocation = _myMapOrigin.Map.CameraPosition.Target;
            var destinationLocation = _myMapDestination.Map.CameraPosition.Target;

            if (!string.IsNullOrWhiteSpace(_editTextPackage.Text))
            {   

                _deliveryDetails = new DeliveryDetails();
                _deliveryDetails.name = _editTextPackage.Text;
                _deliveryDetails.status = 0;
                _deliveryDetails.originLatitue = originLocation.Latitude;
                _deliveryDetails.originLongitutde = originLocation.Longitude;
                _deliveryDetails.destinationLatitude = destinationLocation.Latitude;
                _deliveryDetails.destinationLongitude = destinationLocation.Longitude;
                var datainserted = DeliverDetailsDataBase.InsertDelivery<DeliveryDetails>(_deliveryDetails);
                if (datainserted == true)
                {
                    Console.WriteLine("Data Inserted Successfully");
                    Toast.MakeText(this, "Data Inserted Successfully", ToastLength.Short).Show();
                    Finish();
    
                }
                else
                {
                    Toast.MakeText(this, "No action performed", ToastLength.Short).Show();
                }

            }
            else
            {
                Toast.MakeText(this, "Enter your package details", ToastLength.Short).Show();
            }
        }

        [Obsolete]
        private void UIReference()
        {
            _editTextPackage = FindViewById<EditText>(Resource.Id.editTextPackage);
            _materialButtonSave = FindViewById<MaterialButton>(Resource.Id.buttonSave);
            _mapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.mapFragment);
            _mapFragmentDestination = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.mapFragmentDestination);
            _toolbarAdd = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbarAddPackage);

        }

       

        public void OnLocationChanged(Location location)
        {   
            _latitude = location.Latitude;
            _longitude = location.Longitude;
            _myMapOrigin = new MyMap(_latitude, _longitude);
            _myMapDestination = new MyMap(_latitude, _longitude);
            _mapFragment.GetMapAsync(_myMapOrigin);
             _mapFragmentDestination.GetMapAsync(_myMapDestination);

        }

        public void OnProviderDisabled(string provider)
        {
         
        }

        public void OnProviderEnabled(string provider)
        {
           
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            
        }
    }
}