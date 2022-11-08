using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using TabLayoutExample;
using System.Text.RegularExpressions;
using Android.Graphics;
using Android.Text;
using System.Linq;
using Android.Content;
using SharedDeliveryPersonProject.Model;

namespace DeliveryPersonApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private TextView _textViewWelocme;
        private TextInputLayout _textInputLayoutEmail;
        private TextInputEditText _textInputEditTextEmail;
        private TextInputLayout _textInputLayoutPassword;
        private TextInputEditText _textInputEditTextPassword;
        private MaterialButton _materialButtonLogin;
        private TextView _textViewRegister;
        private string emailid;
        private string password;
       ISharedPreferences sharedPreferences;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            UIReferences();
            UIClickEvents();
        }
        private void UIClickEvents()
        {
            _materialButtonLogin.Click += _materialButtonLogin_Click;

            _textViewRegister.Click += _textViewRegister_Click;
            _textInputEditTextEmail.TextChanged += _textInputEditTextEmail_TextChanged;
            _textInputEditTextPassword.TextChanged += _textInputEditTextPassword_TextChanged;
        }

        private void _textViewRegister_Click(object sender, System.EventArgs e)
        {
            StartActivity(new Intent(this, typeof(UserRegister)));
        }

        private void _materialButtonLogin_Click(object sender, System.EventArgs e)
        {
            emailid = _textInputEditTextEmail.Text;
            password = _textInputEditTextPassword.Text;
            var result = ValidationClass.DeliveryPersonLogin(emailid, password);
            sharedPreferences = GetSharedPreferences("LocalSharePrefrence",FileCreationMode.Private);
            int _deliveryPersonId = result.deliverPersonId;
            ISharedPreferencesEditor sharedPreferencesEditor = sharedPreferences.Edit();
            sharedPreferencesEditor.PutInt("deliveryPerson", _deliveryPersonId);
            sharedPreferencesEditor.Commit();
            if (result.isErrorEmail == false || result.isErrorPassword == false)
            {
                _textInputLayoutEmail.Error = result.erroremail;
                _textInputLayoutPassword.Error = result.errorpassword;
            }

  
            if (result.toastmessage == "LoginSuccessfull")
            {
                Toast.MakeText(this, "Login Sucessfull", ToastLength.Short).Show();
                Intent i = new Intent(this, typeof(DeliveryPersonDashboardActivity));
                StartActivity(i);
            }
            else if (result.toastmessage == "PasswordIncorrect")
            {
                Toast.MakeText(this, "Password Incorrect", ToastLength.Short).Show();
            }
            else if (result.toastmessage == "Usernotfound")
            {
                Toast.MakeText(this, "User not found", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Enter your details", ToastLength.Short).Show();
            }
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
            emailid = _textInputEditTextEmail.Text;
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
            _textViewWelocme = FindViewById<TextView>(Resource.Id.textViewWelcome);
            _textInputLayoutEmail = FindViewById<TextInputLayout>(Resource.Id.textInputLayoutEmail);
            _textInputEditTextEmail = FindViewById<TextInputEditText>(Resource.Id.textInputEditTextEmail);
            _textInputLayoutPassword = FindViewById<TextInputLayout>(Resource.Id.textInputLayoutPassword);
            _textInputEditTextPassword = FindViewById<TextInputEditText>(Resource.Id.textInputEditTextPassword);
            _materialButtonLogin = FindViewById<MaterialButton>(Resource.Id.materialButtonLogin);
            _textViewRegister = FindViewById<TextView>(Resource.Id.textViewRegister);

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


        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}