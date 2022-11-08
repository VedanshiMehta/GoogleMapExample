using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Google.Android.Material.Tabs;
using System;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace TabLayoutExample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity: AppCompatActivity
    {
        private Toolbar _toolbar;
        private TabLayout _tabLayout;
        private  int tabid;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            UIReferences();
            UIClickEvents();
            _toolbar.InflateMenu(Resource.Menu.addmenu);
            _toolbar.MenuItemClick += _toolbar_MenuItemClick;
           // SupportActionBar.Title = "Welcome";
           
            FragmentNavigate(new Deliveries(), "FragmentDeliveries");
        }

        private void _toolbar_MenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {
            if (e.Item.ItemId == Resource.Id.actionadd)
            {
                StartActivity(new Intent(this, typeof(AddDeliveries)));

            }
        }

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    new MenuInflater(this).Inflate(Resource.Menu.addmenu, menu);
        //    return base.OnCreateOptionsMenu(menu);
        //}
        private void UIClickEvents()
        {
            _tabLayout.TabSelected += _tabLayout_TabSelected;
           
        }

        private void _tabLayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            tabid = e.Tab.Position;
            switch(tabid)
            {
                case 0: FragmentNavigate(new Deliveries(), "FragmentDeliveries");
                    break;

                case 1: FragmentNavigate(new Delivered(), "FragmentDelivered");
                    break;

                case 2: FragmentNavigate(new Profile(), "FragmentProfile");
                    break;
            }
           

        }
        private void FragmentNavigate(Fragment fragment,string name)
        {
            var fragmentManager = SupportFragmentManager;
            var transaction = fragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.frameLayout, fragment);
            
            transaction.AddToBackStack(null);
            transaction.Commit();

            
            
        }

        public override void OnBackPressed()
        {
            int backstackentrycount = SupportFragmentManager.BackStackEntryCount;
            if (backstackentrycount == 0)
            {
                Finish();
            }
            else
            {
                TraverseHome();
            }

        }

        private void TraverseHome()
        {
            _tabLayout.GetTabAt(0).Select();

           FragmentNavigate(new Deliveries(), "FragmentDeliveries");
            SupportFragmentManager.PopBackStack();
            int count = SupportFragmentManager.BackStackEntryCount;
           
            
                for (int i = 0; i < count; i++)
                {
                    SupportFragmentManager.PopBackStack();
                }
            
        }

       

        private void UIReferences()
        {
           _tabLayout = FindViewById<TabLayout>(Resource.Id.tabLayout);
           _toolbar = FindViewById<Toolbar>(Resource.Id.toolbar); 
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}