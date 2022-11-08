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

namespace TabLayoutExample
{
    public class DeliveryAdapter : RecyclerView.Adapter
    {
        private FragmentActivity activity;
        private List<DeliveryDetails> deliveryDetails;

        public DeliveryAdapter(FragmentActivity activity, List<DeliveryDetails> deliveryDetails)
        {
            this.activity = activity;
            this.deliveryDetails = deliveryDetails;
        }

        public event EventHandler<DeliveryAdapterEventArgs> ItemClick;




        public override int ItemCount => deliveryDetails == null ? 0 : deliveryDetails.Count;


        public DeliveryDetails GetItem(int position)
        {
            return deliveryDetails[position];
        }


      

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            DeliveryAdapterViewHolder deliveryAdapterViewHolder = holder as DeliveryAdapterViewHolder;
            deliveryAdapterViewHolder.BindData(deliveryDetails[position]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.deliveryrowitem, parent, false);
            return new DeliveryAdapterViewHolder(v,OnClick);
        }

        //Fill in cound here, currently 0
        void OnClick(DeliveryAdapterEventArgs args) => ItemClick?.Invoke(this, args);

    }

    public class DeliveryAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Name;
        public TextView Status;

        public DeliveryAdapterViewHolder(View itemView, Action<DeliveryAdapterEventArgs> clickListener) : base(itemView)
        {
            Name = itemView.FindViewById<TextView>(Resource.Id.textViewName);
            Status = itemView.FindViewById<TextView>(Resource.Id.textViewStatus);
            itemView.Click += (s,e) => clickListener(new DeliveryAdapterEventArgs { View = itemView, Position = AbsoluteAdapterPosition });

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



        //Your adapter views to re-use
    }
    public  class DeliveryAdapterEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
 