using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Content;
using AndroidX.Core.Hardware.Fingerprint;
using BumpTech.GlideLib;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Java.Util.Jar;
using SharedDeliveryPersonProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Manifest = Android.Manifest;
using CancellationSignal = AndroidX.Core.OS.CancellationSignal;
using Android.Hardware.Fingerprints;
using System.Runtime.CompilerServices;
using Java.Lang;

namespace TabLayoutExample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class LoginUser : AppCompatActivity,ITextWatcher
    {
        private TextView _textViewWelocme;
        private TextInputLayout _textInputLayoutEmail;
        private TextInputEditText _textInputEditTextEmail;
        private TextInputLayout _textInputLayoutPassword;
        private TextInputEditText _textInputEditTextPassword;
        private MaterialButton _materialButtonLogin;
        private string emailid;
        private string password;
        private ISharedPreferences _sharedPreferences;
        private ISharedPreferencesEditor sharedPreferencesEditor;
        private int _deliveryPersonid;
        private string role;
        private ImageView _imageViewDelivery;
        bool isLogIn;
        bool _isDeliveryPerson;
        ErrorModel _errorModel = new ErrorModel();
        FingerprintManagerCompat fingerprintManager;
        CancellationSignal cancellation;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            fingerprintManager = FingerprintManagerCompat.From(this);
            cancellation = new CancellationSignal();
            // Create your application here
            SetContentView(Resource.Layout.loginuser);
            UIReferences();
            UIClickEvents();
            TextWatcherIntialization();
            role = Intent.GetStringExtra("roles");

        }

        private void TextWatcherIntialization()
        {
            _textInputEditTextEmail.AddTextChangedListener(this);
        }

        public void AfterTextChanged(IEditable s)
        {
           
           
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
           
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            emailid = s.ToString();
            var result = ValidationClass.isValidEmailId(emailid);
            if (!result.isErrorEmail)
            {
                _textInputLayoutEmail.Error = result.erroremail;
            }
            else if (result.isErrorEmail)
                _textInputLayoutEmail.Error = null;

        }
        private void UIClickEvents()
        {
            _materialButtonLogin.Click += _materialButtonLogin_Click;
            _textInputEditTextEmail.TextChanged += _textInputEditTextEmail_TextChanged;
            _textInputEditTextPassword.TextChanged += _textInputEditTextPassword_TextChanged;
        }

        private void _textInputEditTextPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            password = _textInputEditTextPassword.Text;
            var result = ValidationClass.isValidPasswords(password);
            if (!result.isErrorPassword)
            {
                _textInputLayoutPassword.Error = result.errorpassword;
            }
            else if (result.isErrorPassword)
                _textInputLayoutPassword.Error = null;

        }

        private void _textInputEditTextEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }






        private void _materialButtonLogin_Click(object sender, EventArgs e)
        {
            emailid = _textInputEditTextEmail.Text;
            password = _textInputEditTextPassword.Text;
            
            _errorModel = ValidationClass.Login(emailid, password);

            _deliveryPersonid = _errorModel.deliverPersonId;
            _isDeliveryPerson = _errorModel.isDeliveryPersons;

            _sharedPreferences = GetSharedPreferences("LocalSharePrefrence", FileCreationMode.Private);
            sharedPreferencesEditor = _sharedPreferences.Edit();
            sharedPreferencesEditor.PutInt("roles", _deliveryPersonid);
            sharedPreferencesEditor.PutBoolean("deliveryPerson", _isDeliveryPerson);
            sharedPreferencesEditor.Commit();

            var canUseFingerprint = CanUseFingerPrint();
            if (canUseFingerprint)
            { 
                

                LogUserIn();
            }
            else
            {

             
                if (_errorModel.isErrorEmail == false || _errorModel.isErrorPassword == false)
                {
                    _textInputLayoutEmail.Error = _errorModel.erroremail;
                    _textInputLayoutPassword.Error = _errorModel.errorpassword;
                }


                if (_errorModel.toastmessage == "LoginSuccessfull" && _errorModel.isDeliveryPersons == false)
                {
                    Toast.MakeText(this, "Login Sucessfull", ToastLength.Short).Show();
                    isLogIn = true;
                   
                    sharedPreferencesEditor.PutBoolean("Login", isLogIn);
                  
                    sharedPreferencesEditor.Commit();

                    StartActivity(new Intent(this, typeof(MainActivity)));
                    Finish();
                }
                else if (_errorModel.toastmessage == "LoginSuccessfull" && _errorModel.isDeliveryPersons == true)
                {
                    Toast.MakeText(this, "Login Sucessfull", ToastLength.Short).Show();
                    isLogIn = true;
                    sharedPreferencesEditor.PutBoolean("Login", isLogIn);
                    sharedPreferencesEditor.Commit();
                    StartActivity(new Intent(this, typeof(DeliveryPersonDashboardActivity)));
                    Finish();
                }
                else if (_errorModel.toastmessage == "PasswordIncorrect")
                {
                    isLogIn = false;
                    Toast.MakeText(this, "Password Incorrect", ToastLength.Short).Show();
                }
                else if (_errorModel.toastmessage == "Usernotfound")
                {
                    isLogIn = false;
                    Toast.MakeText(this, "User not found", ToastLength.Short).Show();
                }
                else
                {
                    isLogIn = false;
                    Toast.MakeText(this, "Enter your details", ToastLength.Short).Show();
                }
            }
        }

        [Obsolete]
        private void LogUserIn()
        {
            var cancellation = new CancellationSignal();
            FingerprintManagerCompat.AuthenticationCallback authenticationCallback = new AuthenticationCallback(this, _deliveryPersonid,_isDeliveryPerson,_errorModel );
            fingerprintManager.Authenticate(null, 0, cancellation, authenticationCallback, null);
        }

        [Obsolete]
        private bool CanUseFingerPrint()
        {
           
            if (_errorModel.deliverPersonId != 0)
            {
                if (fingerprintManager.IsHardwareDetected)
                {
                    if (fingerprintManager.HasEnrolledFingerprints)
                    {
                        var permissionResult = ContextCompat.CheckSelfPermission(this, Manifest.Permission.UseFingerprint);
                        if (permissionResult == Android.Content.PM.Permission.Granted)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //protected override void OnStart()
        //{
        //    base.OnStart();
        //    _sharedPreferences = this.GetSharedPreferences("LocalSharePrefrence", FileCreationMode.Private);
        //    if (_sharedPreferences != null)
        //    {
        //        int userid = 0;
        //        var isLogIn = _sharedPreferences.GetBoolean("Login", false);
        //        var id = _sharedPreferences.GetInt("roles", 0);
        //        if (isLogIn == true)
        //        {
        //            if (userid == id)
        //            {
        //                sharedPreferencesEditor.PutInt("roles", id);
        //                sharedPreferencesEditor.Commit();
        //                StartActivity(new Intent(this, typeof(DeliveryPersonDashboardActivity)));
        //                Finish();
        //            }
        //            else
        //            {
        //                StartActivity(new Intent(this, typeof(MainActivity)));
        //                Finish();
        //            }

        //        }

        //    }

        //}


        private void UIReferences()
        {
            _textViewWelocme = FindViewById<TextView>(Resource.Id.textViewLogin);
            _textInputLayoutEmail = FindViewById<TextInputLayout>(Resource.Id.textInputLayoutEmail);
            _textInputEditTextEmail = FindViewById<TextInputEditText>(Resource.Id.textInputEditTextEmail);
            _textInputLayoutPassword = FindViewById<TextInputLayout>(Resource.Id.textInputLayoutPassword);
            _textInputEditTextPassword = FindViewById<TextInputEditText>(Resource.Id.textInputEditTextPassword);
            _materialButtonLogin = FindViewById<MaterialButton>(Resource.Id.materialButtonLogin);
            _imageViewDelivery = FindViewById<ImageView>(Resource.Id.imageViewLoginGlide);

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
            Glide.With(this).Load(Resource.Drawable.scooterrunning).Into(_imageViewDelivery);

        }

        
    }

}