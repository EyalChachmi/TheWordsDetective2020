using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace Project_2020
{
    public class LoginDialog:Dialog
    {
        //private Dialog dialog;
        private Admin admin;
        private EditText editTextUsername;
        private EditText editTextPassword;
        private Button buttonLogin;
        private Dialog dialogLoginFail;
        private ISharedPreferences sp;
        private Context context;
        string userId;
        public LoginDialog(Context context):base(context)
        {
            //this.dialog = new Dialog(context);
            this.SetContentView(Resource.Layout.DialogLayoutLogin);
            this.SetCancelable(true);
            //Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
            //Accelerometer.Start(SensorSpeed.Game);
            this.Initialize(context);
        }

        //private void Accelerometer_ShakeDetected(object sender, EventArgs e)
        //{
        //    MainThread.BeginInvokeOnMainThread(() =>
        //    {
        //        this.editTextPassword.Text = "";
        //        this.editTextUsername.Text = "";
                
        //    });
        //}

        private void Initialize(Context context)
        {
            admin = new Admin();
            this.CancelEvent += LoginDialog_CancelEvent;
            this.editTextUsername = this.FindViewById<EditText>(Resource.Id.EditTextUsername);
            this.editTextPassword = (EditText)this.FindViewById(Resource.Id.EditTextPassword);
            
            this.buttonLogin = (Button)this.FindViewById(Resource.Id.buttonDialogLogin);
            this.buttonLogin.Click += ButtonLogin_Click;
            this.sp = this.Context.GetSharedPreferences("myProfile", FileCreationMode.Private);
            this.context = context;
        }

        private void LoginDialog_CancelEvent(object sender, EventArgs e)
        {
            editTextPassword.Text = "";
            editTextUsername.Text = "";
            //Accelerometer.Stop();
        }

        private async void ButtonLogin_Click(object sender, EventArgs e)
        {
            string userName = editTextUsername.Text;
            string password = editTextPassword.Text;
            if (userName.Length==0 || password.Length==0)
            {
                Toast.MakeText(this.Context, "You haven't entered your password", ToastLength.Long).Show();
                return;
            }
            this.userId= await User.GetId(userName, password);
            var editor = sp.Edit();
            editor.PutString("userId", this.userId);
            editor.Commit();

            if (this.userId=="-1")
            {
                this.CreateLoginFailedDialog();
            }
            else
            {
                if (admin.CheckAdministration(userName, password))
                {

                ((MainActivity)this.context).ButtonAddWords.Visibility = ViewStates.Visible;//yes
                }
                ((MainActivity)this.context).ButtonPlay.Visibility = ViewStates.Visible;
                Toast.MakeText(this.Context, "Logged in!", ToastLength.Long).Show();
                ((MainActivity)this.context).LogoutChangedText();
                //Accelerometer.Stop();
                this.Dismiss();
            }          
        }

        public void CreateLoginFailedDialog()
        {
            this.dialogLoginFail = new Dialog(this.Context);
            this.dialogLoginFail.SetContentView(Resource.Layout.LoginFailedDialog);
            this.dialogLoginFail.SetTitle("Incorrect information");
            this.dialogLoginFail.SetCancelable(true);
            Button buttonOk = (Button)this.dialogLoginFail.FindViewById(Resource.Id.buttonOk);
            buttonOk.Click += ButtonOk_Click;
            this.dialogLoginFail.Show();

        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            this.dialogLoginFail.Dismiss();
        }

    }
}