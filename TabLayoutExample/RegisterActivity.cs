﻿using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using BumpTech.GlideLib;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using SharedDeliveryPersonProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TabLayoutExample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class RegisterActivity : AppCompatActivity
    {
        private TextView _textViewWelocme;
        private TextInputLayout _textInputLayoutEmail;
        private TextInputEditText _textInputEditTextEmail;
        private TextInputLayout _textInputLayoutPassword;
        private TextInputEditText _textInputEditTextPassword;
        private TextInputLayout _textInputLayoutConfirmPassword;
        private TextInputEditText _textInputEditTextConfirmPassword;
        private TextInputLayout _textInputLayoutMobileNumber;
        private TextInputEditText _textInputEditTextMobileNumber;
        private MaterialButton _materialButtonRegister;
        private string role;
        private ImageView _imageViewRegister;
      
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.registeruser);
            UIReferences();
            UIClickEvents();
            role = Intent.GetStringExtra("roles");

        }
        private void UIClickEvents()
        {
            _materialButtonRegister.Click += _materialButtonRegister_Click;
            _textInputEditTextEmail.TextChanged += _textInputEditTextEmail_TextChanged;
            _textInputEditTextPassword.TextChanged += _textInputEditTextPassword_TextChanged;
            _textInputEditTextConfirmPassword.TextChanged += _textInputEditTextConfirmPassword_TextChanged;
            _textInputEditTextMobileNumber.TextChanged += _textInputEditTextMobileNumber_TextChanged;
        }

        private void _textInputEditTextMobileNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            string mobileNumber = _textInputEditTextMobileNumber.Text;
            var result = ValidationClass.isValidMobileNumber(mobileNumber);
            if(!result.isErrorMobileNumber)
            {
                _textInputLayoutMobileNumber.Error = result.errormobilenumber;
            }
            else if(result.isErrorMobileNumber)
            {
                _textInputLayoutMobileNumber.Error = null;
            }

        }

        private void _textInputEditTextConfirmPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            string confirmPassword = _textInputEditTextConfirmPassword.Text;
            string password = _textInputEditTextPassword.Text;
            var result = ValidationClass.isValidConfirmPassword(confirmPassword, password);
            if (!result.isErrorConfirmPassword)
            {
                _textInputLayoutConfirmPassword.Error = result.errorconfirmpassword;
            }
            else if (result.isErrorConfirmPassword)
            {
                _textInputLayoutConfirmPassword.Error = null;
            }
        }

        private void _materialButtonRegister_Click(object sender, EventArgs e)
        {
            string emailid = _textInputEditTextEmail.Text;
            string password = _textInputEditTextPassword.Text;
            string confirmPassword = _textInputEditTextConfirmPassword.Text;
            string mobileNumber = _textInputEditTextMobileNumber.Text;
            var register = ValidationClass.Register( emailid,password,confirmPassword,mobileNumber,role); 
            if(register.isErrorEmail == false || register.isErrorPassword == false || register.isErrorConfirmPassword == false || register.isErrorMobileNumber == false)
            {
                _textInputLayoutEmail.Error = register.erroremail;
                _textInputLayoutPassword.Error = register.errorpassword;
                _textInputLayoutConfirmPassword.Error = register.errorconfirmpassword;
                _textInputLayoutMobileNumber.Error = register.errormobilenumber;
            }
            if(register.toastmessage == "RegisterSuccessfully")
            {
                Toast.MakeText(this,"Register Sucessfully",ToastLength.Short).Show();
                Intent intent = new Intent(this,typeof(LoginUser));
                StartActivity(intent);
                Finish();
            }
            else if(register.toastmessage == "Useralreadyfound")
            {
                Toast.MakeText(this, "User already found", ToastLength.Short).Show();
               
            }
            else if(register.toastmessage == "Enteryourdetails")
            {
                Toast.MakeText(this, "Enter your details", ToastLength.Short).Show();
            }
        }

       

      

        private void _textInputEditTextPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            string password = _textInputEditTextPassword.Text;
            var result = ValidationClass.isValidPasswords(password);
            if (!result.isErrorPassword)
            {
                _textInputLayoutPassword.Error = result.errorpassword;
            }
            else if (result.isErrorPassword)
            {
                _textInputLayoutPassword.Error = null;
            }
        }

        private void _textInputEditTextEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

            string emailid = _textInputEditTextEmail.Text;

            var result = ValidationClass.isValidEmailId(emailid);
            if (!result.isErrorEmail)
            {
                _textInputLayoutEmail.Error = result.erroremail;
            }
            else if (result.isErrorEmail)
           
                _textInputLayoutEmail.Error = null;
            
        }

  
        private void UIReferences()
        {
            _textViewWelocme = FindViewById<TextView>(Resource.Id.textViewWelcomeRegister);
            _textInputLayoutEmail = FindViewById<TextInputLayout>(Resource.Id.textInputLayoutEmailRegister);
            _textInputEditTextEmail = FindViewById<TextInputEditText>(Resource.Id.textInputEditTextEmailRegister);
            _textInputLayoutPassword = FindViewById<TextInputLayout>(Resource.Id.textInputLayoutPasswordRegister);
            _textInputEditTextPassword = FindViewById<TextInputEditText>(Resource.Id.textInputEditTextPasswordRegister);
            _textInputLayoutConfirmPassword = FindViewById<TextInputLayout>(Resource.Id.textInputLayoutPasswordConfirmRegister);
            _textInputEditTextConfirmPassword = FindViewById<TextInputEditText>(Resource.Id.textInputEditTextConfirmPasswordRegister);
            _textInputLayoutMobileNumber = FindViewById<TextInputLayout>(Resource.Id.textInputLayoutMobileNumberRegister);
            _textInputEditTextMobileNumber = FindViewById<TextInputEditText>(Resource.Id.textInputEditTextMobileNumberRegister);
            _materialButtonRegister = FindViewById<MaterialButton>(Resource.Id.materialButtonRegister);
            _imageViewRegister = FindViewById<ImageView>(Resource.Id.imageViewRegisterGlide);

            TextPaint textPaint = _textViewWelocme.Paint;

            float width = textPaint.MeasureText(_textViewWelocme.Text);

            int[] vs = new int[]
            {
                Color.ParseColor("#006a4e"),
                Color.ParseColor("#2f847c"),
                Color.ParseColor("#00cc99"),
                Color.ParseColor("#98ff98")
            };
            Shader shader = new LinearGradient(0, 150, width, _textViewWelocme.TextSize, vs, null, Shader.TileMode.Clamp);
            _textViewWelocme.Paint.SetShader(shader);


            Glide.With(this).Load(Resource.Drawable.scooterrunning).Into(_imageViewRegister);
        }
    
}
}