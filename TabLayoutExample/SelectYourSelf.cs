using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.Core.Content;
using BumpTech.GlideLib;
using Google.Android.Material.Button;
using Google.Android.Material.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TabLayoutExample
{
    [Activity(Label = "@string/app_name", NoHistory =true,Theme = "@style/AppTheme", MainLauncher = true)]
    public class SelectYourSelf :   AppCompatActivity
    {
        private TextView _textViewWelcomeText;
        private ImageView _imageViewDelivery;
        private MaterialCardView _materialCardViewInducers;
        private MaterialCardView _materialCardViewDeliveryPerson;
        private ConstraintLayout _constraintLayoutInducers;
        private ConstraintLayout _constraintLayoutDeliveryPerson;
        private TextView _textViewInducers;
        string role;
        private TextView _textViewDeliveryPerson;
        private MaterialButton _materialButtonNext;
        private TextView _textViewLogIn;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.selectusers);
            UIReferences();
            UIClickEvents();
        }

        private void UIClickEvents()
        {
            _materialCardViewInducers.Click += _materialCardViewInducers_Click;
            _materialCardViewDeliveryPerson.Click += _materialCardViewDeliveryPerson_Click;
            _materialButtonNext.Click += _materialButtonNext_Click;
            _textViewLogIn.Click += _textViewLogIn_Click;
        }

        private void _textViewLogIn_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoginUser));
            intent.PutExtra("roles", role);
            StartActivity(intent);
        }

        private void _materialButtonNext_Click(object sender, EventArgs e)
        {

          
            if (string.IsNullOrWhiteSpace(role))
            {
                Toast.MakeText(this,"Select users",ToastLength.Short).Show();
            }
            else
            {
                Intent intent = new Intent(this,typeof(RegisterActivity));
                intent.PutExtra("roles",role);
                StartActivity(intent);
            }
        }

        private void _materialCardViewDeliveryPerson_Click(object sender, EventArgs e)
        {
          
            _constraintLayoutDeliveryPerson.SetBackgroundColor(Color.ParseColor("#2e8b57"));
            _textViewDeliveryPerson.SetTextColor(Color.White);

            _materialCardViewInducers.SetStrokeColor(ColorStateList.ValueOf(Color.ParseColor("#2e8b57")));
            _materialCardViewInducers.StrokeWidth = 2;
            _constraintLayoutInducers.SetBackgroundColor(Color.GhostWhite);
            _textViewInducers.SetTextColor(Color.ParseColor("#2e8b57"));
            _materialButtonNext.Enabled = true;
            role = _textViewDeliveryPerson.Text;


        }

        private void _materialCardViewInducers_Click(object sender, EventArgs e)
        {

            
            _constraintLayoutInducers.SetBackgroundColor(Color.ParseColor("#2e8b57"));
            _textViewInducers.SetTextColor(Color.White);


            _materialCardViewDeliveryPerson.SetStrokeColor(ColorStateList.ValueOf(Color.ParseColor("#2e8b57")));
            _materialCardViewDeliveryPerson.StrokeWidth = 2;
            _constraintLayoutDeliveryPerson.SetBackgroundColor(Color.GhostWhite);
            _textViewDeliveryPerson.SetTextColor(Color.ParseColor("#2e8b57"));
            _materialButtonNext.Enabled = true;
            role = _textViewInducers.Text;
        }

        private void UIReferences()
        {
            _textViewWelcomeText = FindViewById<TextView>(Resource.Id.textViewWelcomeHere);
            _imageViewDelivery = FindViewById<ImageView>(Resource.Id.imageViewGlide);
            _materialCardViewInducers = FindViewById<MaterialCardView>(Resource.Id.materialCardViewInducers);
            _materialCardViewDeliveryPerson = FindViewById<MaterialCardView>(Resource.Id.materialCardViewDeliveryPerson);
            _constraintLayoutInducers = FindViewById<ConstraintLayout>(Resource.Id.constraintLayoutInducers);
            _constraintLayoutDeliveryPerson = FindViewById<ConstraintLayout>(Resource.Id.constraintLayoutDeliveryPerson);
            _textViewInducers = FindViewById<TextView>(Resource.Id.textViewInducers);
            _textViewDeliveryPerson = FindViewById<TextView>(Resource.Id.textViewDeliveryPerson);
            _textViewLogIn = FindViewById<TextView>(Resource.Id.textViewLogIn);
            _materialButtonNext = FindViewById<MaterialButton>(Resource.Id.materialButtonNext);

            TextPaint textPaint = _textViewWelcomeText.Paint;

            float width = textPaint.MeasureText(_textViewWelcomeText.Text);

            int[] vs = new int[]
            {
                Color.ParseColor("#006a4e"),
                Color.ParseColor("#2f847c"),
                Color.ParseColor("#00cc99"),
                Color.ParseColor("#98ff98")
            };
            Shader shader = new LinearGradient(0, 150, width, _textViewWelcomeText.TextSize, vs, null, Shader.TileMode.Clamp);
            _textViewWelcomeText.Paint.SetShader(shader);

            Glide.With(this).Load(Resource.Drawable.scooterrunning).Into(_imageViewDelivery);
        }
    }
}