using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using Android.Views;
using Xamarin.Essentials;

namespace Project_2020
{

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Admin admin;//משתמש מנהל לשמירת מילים בענן
        Button buttonLogin;
        Button buttonSign;
        Button buttonPlay;
        Button buttonAddWords;
        Button showRecords;
        string userId;//משתמש של ערך המשתמש
        User loggedUser;//שומר ערך על מנת לשמור משתמש מחובר
        LoginDialog loginDialog;
        AddWordsDialog addWordsDialog;
        public Android.Support.V4.Widget.DrawerLayout drawerLayout;
        public Android.Support.Design.Widget.NavigationView navigationView;
        Android.Support.V7.Widget.Toolbar toolbar;
        ISharedPreferences sp;// משתנה השומר על ערכים של המשתמש
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            this.Initialize();
        }
        public Button ButtonPlay //מחזירה הפנייה לכפתור המשחק
        {
            get
            {
                return this.buttonPlay;
            }
        }
        public Button ButtonAddWords
        {
            get
            {
                return this.buttonAddWords;
            }
        }
        public async void Initialize()
        {
            admin = new Admin();
            AppData.Initialize(this);//הפעלת הענן ברקע
            drawerLayout = (Android.Support.V4.Widget.DrawerLayout)FindViewById(Resource.Id.drawerLayout);
            toolbar = (Android.Support.V7.Widget.Toolbar)FindViewById(Resource.Id.toolbar);
            navigationView = (Android.Support.Design.Widget.NavigationView)FindViewById(Resource.Id.navview);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            SetSupportActionBar(toolbar);
            Android.Support.V7.App.ActionBar actionBar = SupportActionBar;
            this.SupportActionBar.Title = "The Words Detective";
            actionBar.SetHomeAsUpIndicator(Resource.Drawable.menuaction);
            actionBar.SetDisplayHomeAsUpEnabled(true);
            this.loginDialog = new LoginDialog(this);        
            this.loginDialog.DismissEvent += LoginDialog_DismissEvent;
            this.addWordsDialog = new AddWordsDialog(this);
            this.addWordsDialog.DismissEvent += AddWordsDialog_DismissEvent;
            this.sp = this.GetSharedPreferences("myProfile", FileCreationMode.Private);
            this.buttonPlay = this.FindViewById<Button>(Resource.Id.buttonPlay);
            this.buttonLogin = this.FindViewById<Button>(Resource.Id.buttonLogin);
            this.buttonSign = this.FindViewById<Button>(Resource.Id.buttonSign);
            this.showRecords = this.FindViewById<Button>(Resource.Id.buttonScores);
            this.buttonAddWords = this.FindViewById<Button>(Resource.Id.buttonAddWords);
            this.buttonAddWords.Click += ButtonAddWords_Click;
            this.buttonSign.Click += ButtonSign_Click;
            this.buttonLogin.Click += ButtonLogin_Click;
            this.buttonPlay.Click += ButtonPlay_Click;
            this.showRecords.Click += ShowRecords_Click;
            this.userId = sp.GetString("userId", "-1");//השג את הערך אשר נמצא במשתנה
            if (this.userId == "-1")//אם המשתמש אינו מחובר
            {
                this.buttonPlay.Visibility = Android.Views.ViewStates.Invisible;
                this.buttonAddWords.Visibility = Android.Views.ViewStates.Invisible;
            }
            else//אחרת אם הוא מחובר
            {
                this.buttonLogin.Text = "Logout";
                this.buttonSign.Visibility = Android.Views.ViewStates.Invisible;
                this.buttonPlay.Visibility = Android.Views.ViewStates.Visible;
                this.loggedUser = await User.GetUser(this.userId);
                if (admin.CheckAdministration(loggedUser.UserName, loggedUser.Password))
                {

                    this.ButtonAddWords.Visibility = ViewStates.Visible;//yes
                }
            }            
        }

        private void NavigationView_NavigationItemSelected(object sender, Android.Support.Design.Widget.NavigationView.NavigationItemSelectedEventArgs e)
        {
            if(e.MenuItem.ItemId==Resource.Id.aboutTheGame)//בעת לחיצה על כפתור המידע על המשחק בתפריט
            {
                Intent intent = new Intent(this, typeof(QuizDescripTionActivity));//מעבר אל דף המידע
                intent.PutExtra("topic", "aboutTheGame");
                StartActivity(intent);
                drawerLayout.CloseDrawers();//סוגר את מגירות התפריט כלומר drawermenu
            }
            else if(e.MenuItem.ItemId==Resource.Id.credits)
            {
                Intent intent = new Intent(this, typeof(QuizDescripTionActivity));
                intent.PutExtra("topic", "credits");
                StartActivity(intent);
                drawerLayout.CloseDrawers();
            }
            else if(e.MenuItem.ItemId==Resource.Id.HowToPlay)
            {
                Intent intent = new Intent(this, typeof(QuizDescripTionActivity));
                intent.PutExtra("topic", "HowToPlay");
                StartActivity(intent);
                drawerLayout.CloseDrawers();
            }
        }

        private void ShowRecords_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ActivityShowRecords));
            this.StartActivity(intent);
        }

        private void ButtonAddWords_Click(object sender, System.EventArgs e)
        {
            this.addWordsDialog.Show();
            var editor = sp.Edit();
            editor.Commit();
        }

        private void AddWordsDialog_DismissEvent(object sender, System.EventArgs e)
        {
            var editor = sp.Edit();
            if (sp.GetBoolean("bool", true))
            {
                Toast.MakeText(this, "Dear Admin, Your word has been submited", ToastLength.Long).Show();
                editor.PutBoolean("bool", false);
                editor.Commit();
                return;             
            }
            else 
            {
                Toast.MakeText(this, "The word you have entered exists or doesn't contain any letters", ToastLength.Long).Show();
                return;
            }    
        }

        private async void LoginDialog_DismissEvent(object sender, System.EventArgs e)
        {
            this.userId = sp.GetString("userId", "-1");//השג את הערך אשר נמצא במשתנה
            if (this.userId != "-1")//אם המשתמש מחובר
            {
                this.loggedUser = await User.GetUser(this.userId);//קבלת ערכי המשתמש המחובר ושמירתם בתוך משתנה
            }
        }
        protected async override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 100 && resultCode == Result.Ok)
            {
                int currentScore = data.GetIntExtra("score", -999);
                if (currentScore == -999)
                {
                    return;
                }
                if(currentScore>=50)
                {
                   Toast.MakeText(this, "Your score is: " + currentScore + ", Amazingly Played sir!", ToastLength.Long).Show();
                }
                if(currentScore<=15)
                {
                    Toast.MakeText(this, "Your score is: " + currentScore + ", You can do better than that (;", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, "Your score is: " + currentScore + ", Good job", ToastLength.Long).Show();
                }
                if (currentScore > this.loggedUser.BestScore)
                {
                    this.loggedUser.BestScore = currentScore;
                    await User.UpdateUser(this.loggedUser);
                }
            }

        }
        private void ButtonSign_Click(object sender, System.EventArgs e)//מעביר למסך ההרשמה
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Toast.MakeText(this, "Could not Sign up, Check your network", ToastLength.Long).Show();
                return;
            }
            Intent intent = new Intent(this, typeof(SignUpActivity));
            this.StartActivity(intent);
        }

        private  void ButtonLogin_Click(object sender, System.EventArgs e)
        {
            if(Connectivity.NetworkAccess!=NetworkAccess.Internet)//אם יש אינטרנט
            {
                Toast.MakeText(this, "Could not login into the game, Check your network", ToastLength.Long).Show();
                return;
            }
            if (this.userId == "-1")//מעביר אותנו למסך הכניסה אם אין משתמש מחובר ברגע הלחיצה
            {
                this.loginDialog.Show();
            }
            else //אם יש משתמש מחובר ברגע הלחיצה, המשתמש המחובר יוצא  
            {
                Toast.MakeText(this, "Logged Out", ToastLength.Short).Show();
                this.buttonLogin.Text = "Login";
                this.buttonSign.Visibility = Android.Views.ViewStates.Visible;
                this.userId = "-1";
                var editor = sp.Edit();
                editor.PutString("userId", "-1");
                editor.Commit();
                this.buttonPlay.Visibility = Android.Views.ViewStates.Invisible;
                this.buttonAddWords.Visibility = Android.Views.ViewStates.Invisible;
            }
        }
        protected override void OnResume()
        {
            base.OnResume();
        }
        private void ButtonPlay_Click(object sender, System.EventArgs e)//מעביר אותנו למסך המשחק
        {
            Intent intent = new Intent(this, typeof(GameActivity));
            this.StartActivityForResult(intent, 100);
        }
        public void LogoutChangedText()//id=-1 כאשר אין משתמש שמחובר
        {
            this.buttonLogin.Text = "Logout";
            this.userId = sp.GetString("userId", "-1");
        }
        public override bool OnOptionsItemSelected(IMenuItem item)//כאשר יש לחיצה על התפריט במסך
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home://במסך הבית הראשי
                    try
                    {
                        drawerLayout.OpenDrawer((int)GravityFlags.Right);// התפריט יופיע בצד ימין
                    }
                    catch
                    {
                        drawerLayout.OpenDrawer((int)GravityFlags.Left);//התפריט יופיע בצד שמאל
                    }
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.mymenu, menu);
        //    return base.OnCreateOptionsMenu(menu);
        //}
    }
}