
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using Google.Android.Material.Tabs;
using SharedDeliveryPersonProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TabLayoutExample;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace DeliveryPersonApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class DeliveryPersonDashboardActivity : AppCompatActivity
    {
        private Toolbar _toolbar;
        private TabLayout _tabLayout;
        private int tabid;
        public int deliveryPersonId;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.dashboardlayout);
            UIReferences();
            UIClickEvents();
            SetSupportActionBar(_toolbar);
            SupportActionBar.Title = "Welcome";
            ISharedPreferences sharedPreferences = this.GetSharedPreferences("LocalSharePrefrence", FileCreationMode.Private);
            if (sharedPreferences != null)
            {
                deliveryPersonId = sharedPreferences.GetInt("deliveryPerson", 0);
            }
            FragmentNavigate(new DeliveringFragment(), "FragmentDelivering");
        }

        private void UIClickEvents()
        {
            _tabLayout.TabSelected += _tabLayout_TabSelected;
        }

        private void _tabLayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            tabid = e.Tab.Position;
            switch (tabid)
            {
                case 0:
                    FragmentNavigate(new DeliveringFragment(), "FragmentDelivering");
                    break;

                case 1:
                    FragmentNavigate(new WaitingFragment(), "FragmentWaiting");
                    break;

                case 2:
                    FragmentNavigate(new DeliveredFragment(), "FragmentDelivered");
                    break;
            }
        }

        private void FragmentNavigate(Fragment fragment, string v)
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

            FragmentNavigate(new DeliveringFragment(), "FragmentDelivering");
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
            _toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbarDashboard);
        }
    }
}