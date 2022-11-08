using System;
using System.Collections.Generic;
using System.Text;

namespace SharedDeliveryPersonProject.Model
{
    public class ErrorModel
    {
        public string erroremail;
        public string errorpassword;
        public string errorconfirmpassword;
        public string errormobilenumber;

        public string toastmessage;

        public int deliverPersonId;

        public bool isDeliveryPersons;

        public bool isErrorPassword;
        public bool isErrorEmail;
        public bool isErrorConfirmPassword;
        public bool isErrorMobileNumber;
    }
}
