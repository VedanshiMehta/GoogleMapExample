using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
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
using static Java.Util.Jar.Attributes;

namespace DeliveryPersonApp
{
    public class DeliveryPersonAdapter : RecyclerView.Adapter
    {
        private FragmentActivity activity;
        private List<DeliveryDetails> deliveryDetails;

        public event EventHandler<DeliveryPersonAdapterEventArgs> ItemClick;
        public DeliveryPersonAdapter(FragmentActivity activity, List<DeliveryDetails> deliveryDetails)
        {
            this.activity = activity;
            this.deliveryDetails = deliveryDetails;
        }

        public override int ItemCount => deliveryDetails.Count;

        public DeliveryDetails GetItem(int position)
        {
            return deliveryDetails[position];
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            DeliveryPersonViewHolder deliveryAdapterViewHolder = holder as DeliveryPersonViewHolder;
            deliveryAdapterViewHolder.BindData(deliveryDetails[position]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.deliveryrowitem, parent, false);
            return new DeliveryPersonViewHolder(v, OnClick);
        }
        void OnClick(DeliveryPersonAdapterEventArgs args) => ItemClick?.Invoke(this, args);
    }
    
    public class DeliveryPersonViewHolder : RecyclerView.ViewHolder
    {
        public TextView Name;
        public TextView Status;
        public DeliveryPersonViewHolder(View itemView, Action<DeliveryPersonAdapterEventArgs> clickListener) : base(itemView)
        {
            Name = itemView.FindViewById<TextView>(Resource.Id.textViewName);
            Status = itemView.FindViewById<TextView>(Resource.Id.textViewStatus);
            itemView.Click += (s, e) => clickListener(new DeliveryPersonAdapterEventArgs { View = itemView, Position = AbsoluteAdapterPosition });

        }

        internal void BindData(DeliveryDetails deliveryDetails)
        {
            Name.Text = deliveryDetails.name;
            switch (deliveryDetails.status)
            {
                case 0:
                    Status.Text = "waiting for delivery person";
                    break;
                case 1:
                    Status.Text = "out for drlivery";
                    break;
                case 2:
                    Status.Text = "deliver";
                    break;
            }
        }
    }
   
    public class DeliveryPersonAdapterEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}