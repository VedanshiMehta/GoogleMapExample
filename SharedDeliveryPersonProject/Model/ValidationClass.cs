using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TabLayoutExample;

namespace SharedDeliveryPersonProject.Model
{
    public enum UserRegister
    {
        Useralreadyfound,
        RegisterSuccessfully,
        Enteryourdetails,
        LoginSuccessfull,
        PasswordIncorrect,
        Usernotfound,

    }

    public static class ValidationClass
    {
        private static ErrorModel errorModel;
        private static UserDetails _userDetails = new UserDetails();
        private static Regex email = new Regex(@"^(?=.{12,150}$)^[a-zA-Z0-9]+([\.]?[a-zA-Z0-9])*@[a-zA-Z0-9]+([\.])([a-zA-Z]{2,3}|[a-zA-Z]{2}\.[a-zA-Z]{2,})$");
        private static Regex password = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\/!""#$%&'\\()*+,-.:;<=>?@[\]^_`{|}~])[A-Za-z\d\/!""#$%&'\\()*+,-.:;<=>?@[\]^_`{|}~]{8,12}$");
        public static ErrorModel Login(string emailid, string password)
        {
            errorModel = new ErrorModel();
            DeliverDetailsDataBase.CreateDelivery<UserDetails>();
            var emailids = isValidEmailId(emailid);
            var passwords = isValidPasswords(password);

            if (emailids.isErrorEmail && passwords.isErrorPassword)
            {
                
                _userDetails = DeliverDetailsDataBase.ReadUsers().Where(x => x.email == emailid).ToList().FirstOrDefault();
                if (_userDetails != null)
                {
                    if (_userDetails.password == password)
                    {
                        errorModel.toastmessage = UserRegister.LoginSuccessfull.ToString();
                        if (_userDetails.IsDeliveryPerson == true)
                        {
                            errorModel.deliverPersonId = _userDetails.id;
                            errorModel.isDeliveryPersons = true;
                        }
                        else
                        {
                            errorModel.deliverPersonId = _userDetails.id;
                            errorModel.isDeliveryPersons = false;
                        }

                    }
                    else
                    {
                        errorModel.toastmessage = UserRegister.PasswordIncorrect.ToString(); ;
                    }

                }
                else
                {
                    errorModel.toastmessage = UserRegister.Usernotfound.ToString();
                }

            }

            else
            {
                errorModel.toastmessage = UserRegister.Enteryourdetails.ToString();
            }

            return errorModel;
        }
        public static ErrorModel Register(string emailid, string password, string confirmPassword, string mobileNumber, string role)
        {
            DeliverDetailsDataBase.CreateDelivery<UserDetails>();
            errorModel = new ErrorModel();
            if (_userDetails == null)
            {
                _userDetails = new UserDetails();
            }
            var emailids = isValidEmailId(emailid);
            var passwords = isValidPasswords(password);
            var confirmpasswords = isValidConfirmPassword(password, confirmPassword);
            var mobilenumber = isValidMobileNumber(mobileNumber);
            if (emailids.isErrorEmail && passwords.isErrorPassword && confirmpasswords.isErrorConfirmPassword && mobilenumber.isErrorMobileNumber)
            {
                _userDetails.email = emailid;
                _userDetails.password = password;
                _userDetails.mobilenumber = mobileNumber;
                if(role == "Delivery Person")
                {
                    _userDetails.IsDeliveryPerson = true;
                }
                else if(role == "Inducers")
                {
                     _userDetails.IsDeliveryPerson = false;
                }
                var results = DeliverDetailsDataBase.ReadUsers().Where(x => x.email == emailid || x.mobilenumber == mobileNumber).ToList().FirstOrDefault();
                if (results != null)
                {
                    if (emailid == results.email || mobileNumber == results.mobilenumber)
                    {
                        errorModel.toastmessage = UserRegister.Useralreadyfound.ToString();

                    }
                }
                if (errorModel.toastmessage != UserRegister.Useralreadyfound.ToString())

                {
                    var insertdata = DeliverDetailsDataBase.InsertDelivery<UserDetails>(_userDetails);
                    if (insertdata == true)
                    {
                       

                        errorModel.toastmessage = UserRegister.RegisterSuccessfully.ToString();


                    }


                }
                else
                {
                    errorModel.toastmessage = UserRegister.Useralreadyfound.ToString();

                }


            }
            else
            {
                errorModel.toastmessage = UserRegister.Enteryourdetails.ToString();
            }
            return errorModel;
        }

        
        public static ErrorModel isValidPasswords(string password)
        {
            GenerateErrorModel();
            if (password.Length == 0)
            {
                errorModel.errorpassword = "This field is required.";
                errorModel.isErrorPassword = false;

            }
            else if (!isValidatePassword(password))
            {
                errorModel.errorpassword = "Password must be 8 -12 characters long, 1 numeric character, 1 uppercase letter, 1 lowercase letter, & 1 special character.";
                errorModel.isErrorPassword = false;
            }
            else
            {
                errorModel.errorpassword = null;
                errorModel.isErrorPassword = true;
            }

            return errorModel;
        }

        public static ErrorModel isValidEmailId(string emailid)
        {
            GenerateErrorModel();

            if (emailid.Length == 0)
            {

                errorModel.erroremail = "This field is required.";
                errorModel.isErrorEmail = false;
            }
            else if (!isValidateEmail(emailid))
            {
                errorModel.erroremail = "Invalid Email ID.";
                errorModel.isErrorEmail = false;
            }
            else
            {
                errorModel.erroremail = null;
                errorModel.isErrorEmail = true;
            }


            return errorModel;
        }

        private static void GenerateErrorModel()
        {
            if (errorModel == null)
            {
                errorModel = new ErrorModel();
            }
        }



        public static ErrorModel isValidMobileNumber(string mobiletext)
        {
            GenerateErrorModel();
            if (mobiletext.Length == 0)
            {
                errorModel.errormobilenumber = "This field is required.";
                errorModel.isErrorMobileNumber = false;
            }
            else if (mobiletext.Length < 10)
            {
                errorModel.errormobilenumber = "Invalid Mobile Number.";
                errorModel.isErrorMobileNumber = false;

            }
            else
            {
                errorModel.errormobilenumber = null;
                errorModel.isErrorMobileNumber = true;
            }
            return errorModel;
        }

        public static ErrorModel isValidConfirmPassword(string confirmpasswordtext, string password)
        {
            GenerateErrorModel();
            if (confirmpasswordtext.Length == 0)
            {
                errorModel.errorconfirmpassword = "This field is required.";
                errorModel.isErrorConfirmPassword = false;
            }
            else if (confirmpasswordtext != password)
            {
                errorModel.errorconfirmpassword = "Password must be 8 -12 characters long, 1 numeric character, 1 uppercase letter, 1 lowercase letter, & 1 special character.";
                errorModel.isErrorConfirmPassword = false;
            }
            else
            {
                errorModel.errorconfirmpassword = null;
                errorModel.isErrorConfirmPassword = true;
            }
            return errorModel;
        }




        private static bool isValidateEmail(string text)
        {
            if (text == null)
                return false;
            return email.IsMatch(text);
        }


        private static bool isValidatePassword(string text)
        {
            if (text == null)
                return false;
            return password.IsMatch(text);
        }

        internal static object Login(string emailid, string password, string role)
        {
            throw new NotImplementedException();
        }
    }
}
