using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace Project_2020
{
    [Activity(Label = "SignUpActivity")]
    public class SignUpActivity : Activity
    {
        TextInputEditText TextUserNameSignUp;
        TextInputEditText TextPasswordSignUp;
        TextInputEditText TextPasswordConfirmSignUp;
        Button buttonSignUp;
        protected override void OnCreate(Bundle savedInstanceState)
        {          
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.SignUpLayout);
            Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
            Accelerometer.Start(SensorSpeed.UI);
            this.Initialize();
        }

        private void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            this.TextUserNameSignUp.Text = "";
            this.TextPasswordSignUp.Text = "";
            this.TextPasswordConfirmSignUp.Text= "";
            var duration = TimeSpan.FromSeconds(0.25);
            Vibration.Vibrate(duration);
        }

        public void Initialize()
        {
            this.TextUserNameSignUp = this.FindViewById<TextInputEditText>(Resource.Id.editTextSignUpName);
            this.TextPasswordSignUp=  this.FindViewById<TextInputEditText>(Resource.Id.editTextSignUpPassword);
            this.TextPasswordConfirmSignUp = this.FindViewById<TextInputEditText>(Resource.Id.editTextSignUpRePassword);
            this.buttonSignUp = this.FindViewById<Button>(Resource.Id.buttonSignUp);
            this.buttonSignUp.Click += ButtonSignUp_Click;
        }

        private async void ButtonSignUp_Click(object sender, EventArgs e)
        {
            string userName = this.TextUserNameSignUp.Text.Trim();
            string password = this.TextPasswordSignUp.Text.Trim();
            string rePassword = this.TextPasswordConfirmSignUp.Text.Trim();
            if (password != rePassword) // בודק אם הסיסמא שווה לווידוי הסיסמא
            {
                Toast.MakeText(this, "The passwords don't match, Please confirm your password", ToastLength.Short).Show();
                return;
            }
            if(userName.Length<2||password.Length<2)
            {
                Toast.MakeText(this, "Please Type a nickname or a password that contains more than 3 letters", ToastLength.Short).Show();
                return;
            }
            if (await User.Exist(userName))//מודיע במידה ומשתמש קיים כבר בענן בפעולה אסיכרונית
            {
                this.TextUserNameSignUp.Text = "";
                Toast.MakeText(this, "UserName Exists Already", ToastLength.Short).Show();
                return;
            }
            User userToSignUp = new User(userName, password);
            
            if (await User.AddUser(userToSignUp))//אם המשתמש נרשם בהצלחה,מקבל אותה למשחק
            {
                Toast.MakeText(this, "Welcome to the game", ToastLength.Short).Show();
                Finish();
            }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Accelerometer.Stop();
        }
    }
}