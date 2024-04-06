using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;

namespace Project_2020
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity
    {
        private Game game;
        private Timer timer;
        private const int DURATION = 45;
        private int counter;
        private ISharedPreferences sp;
        private EditText editTextRow1;//פקד תשובה 1
        private EditText editTextRow2;
        private EditText editTextRow3;
        private EditText editTextRow4;
        private Button buttonCheckRow1;
        private Button buttonCheckRow2;
        private Button buttonCheckRow3;
        private Button buttonCheckRow4;
        private CheckBox checkBoxRow1;
        private CheckBox checkBoxRow2;
        private CheckBox checkBoxRow3;
        private CheckBox checkBoxRow4;
        private Button buttonNext;
        private TextView zaWordo;//פקד הופעת המילה
        private TextView textViewCurrentPoints;
        private TextView textViewSumPoints;
        private CheckReceiver checkReceiver;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.GameLayout);
            base.OnCreate(savedInstanceState);
            this.game = new Game();
            this.checkReceiver = new CheckReceiver(this, game);
            await game.GenerateNewWord();        
            this.Initialize();
        }
        public void Initialize()
        {
            //this.game = new Game();
            this.editTextRow1 = (EditText)this.FindViewById(Resource.Id.editTextRow1);
            this.editTextRow2 = (EditText)this.FindViewById(Resource.Id.editTextRow2);
            this.editTextRow3 = (EditText)this.FindViewById(Resource.Id.editTextRow3);
            this.editTextRow4 = (EditText)this.FindViewById(Resource.Id.editTextRow4);
            this.buttonCheckRow1 = (Button)FindViewById(Resource.Id.buttonCheckRow1);
            this.buttonCheckRow2 = (Button)FindViewById(Resource.Id.buttonCheckRow2);
            this.buttonCheckRow3 = (Button)FindViewById(Resource.Id.buttonCheckRow3);
            this.buttonCheckRow4 = (Button)FindViewById(Resource.Id.buttonCheckRow4);
            this.checkBoxRow1 = (CheckBox)FindViewById(Resource.Id.checkBoxRow1);
            this.checkBoxRow2 = (CheckBox)FindViewById(Resource.Id.checkBoxRow2);
            this.checkBoxRow3 = (CheckBox)FindViewById(Resource.Id.checkBoxRow3);
            this.checkBoxRow4 = (CheckBox)FindViewById(Resource.Id.checkBoxRow4);
            this.textViewCurrentPoints = this.FindViewById<TextView>(Resource.Id.textViewPagePoints);
            this.textViewSumPoints = this.FindViewById<TextView>(Resource.Id.textViewTotalPoints);
            this.buttonNext = (Button)FindViewById(Resource.Id.buttonNext);
            this.textViewCurrentPoints.Text= string.Format("current points: {0}", this.game.CurrPoints.ToString());
            this.textViewSumPoints.Text = string.Format("Total points: {0}", this.game.TotalPoints.ToString());
            this.buttonCheckRow1.Click += ButtonCheckRow1_Click;
            this.buttonCheckRow2.Click += ButtonCheckRow2_Click;
            this.buttonCheckRow3.Click += ButtonCheckRow3_Click;
            this.buttonCheckRow4.Click += ButtonCheckRow4_Click;
            this.buttonNext.Click += ButtonNext_Click;
            this.zaWordo = (TextView)FindViewById(Resource.Id.textViewLettersBank);
            game.GetARandomizedList();
            this.zaWordo.Text = game.GetTheWord();
            this.timer = new Timer(1000);
            this.timer.Elapsed += Timer_Elapsed;
            this.counter = GameActivity.DURATION;
            this.buttonNext.Enabled = false;
            this.buttonNext.Text = counter.ToString();
            //this.checkReceiver = new CheckReceiver(this, game);
            timer.Start();
            this.sp = this.GetSharedPreferences("myProfile", FileCreationMode.Private);
        }


        public void MainLoop()
        {
            this.counter--;
            this.buttonNext.Text = this.counter.ToString();
            if (this.game.RoundFinished())//אם השלב הסתיים
            {
                buttonNext.Enabled = true;
                buttonNext.Text = "Next";
                this.counter = GameActivity.DURATION;
                timer.Stop();
            }
            if (counter == 0||Game.RoundsCounter==5)//אם המשחק הסתיים
            {
                Game.RoundsCounter = 0;
                buttonNext.Enabled = true;
                buttonNext.Text = "The game is Finished";
                this.buttonCheckRow1.Enabled = false;
                this.buttonCheckRow2.Enabled = false;
                this.buttonCheckRow3.Enabled = false;
                this.buttonCheckRow4.Enabled = false;
                timer.Stop();
                int userId = this.sp.GetInt("id", -1);
                int scoreValue = this.game.TotalPoints;
                if (scoreValue > 0)
                {

                    Intent intent = new Intent();
                    intent.PutExtra("score", scoreValue);
                    this.SetResult(Result.Ok, intent);
                    this.Finish();
                }
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.RunOnUiThread(this.MainLoop);
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            if (this.counter <= 0)
            {
                this.Finish();
                return;
            }
            this.editTextRow1.Text = "";
            this.editTextRow2.Text = "";
            this.editTextRow3.Text = "";
            this.editTextRow4.Text = "";
            this.buttonCheckRow1.Enabled = true;
            this.buttonCheckRow2.Enabled = true;
            this.buttonCheckRow3.Enabled = true;
            this.buttonCheckRow4.Enabled = true;
            this.checkBoxRow1.Checked = false;
            this.checkBoxRow2.Checked = false;
            this.checkBoxRow3.Checked = false;
            this.checkBoxRow4.Checked = false;
            this.editTextRow1.Enabled = true;
            this.editTextRow2.Enabled = true;
            this.editTextRow3.Enabled = true;
            this.editTextRow4.Enabled = true;
            this.textViewCurrentPoints.Text = string.Format("current points: {0}", this.game.CurrPoints.ToString());
            this.zaWordo.Text = game.GetTheWord();
            this.timer.Start();
            this.buttonNext.Enabled = false;
        }

        private void ButtonCheckRow4_Click(object sender, EventArgs e)
        {
            string attempt = this.editTextRow4.Text;
            this.ButtonClick(this.buttonCheckRow4, this.checkBoxRow4, this.editTextRow4,this.textViewCurrentPoints, this.textViewSumPoints, attempt);
        }

        private void ButtonCheckRow3_Click(object sender, EventArgs e)
        {
            string attempt = this.editTextRow3.Text;
            this.ButtonClick(this.buttonCheckRow3, this.checkBoxRow3, this.editTextRow3, this.textViewCurrentPoints, this.textViewSumPoints, attempt);
        }

        private void ButtonCheckRow2_Click(object sender, EventArgs e)
        {
            string attempt = this.editTextRow2.Text;
            this.ButtonClick(this.buttonCheckRow2, this.checkBoxRow2, this.editTextRow2, this.textViewCurrentPoints, this.textViewSumPoints, attempt);
        }

        private void ButtonCheckRow1_Click(object sender, EventArgs e)
        {
            string attempt = this.editTextRow1.Text;
            this.ButtonClick(this.buttonCheckRow1, this.checkBoxRow1, this.editTextRow1, this.textViewCurrentPoints, this.textViewSumPoints, attempt);
        }
        private void ButtonClick(Button button, CheckBox checkBox, EditText editText,TextView textViewCurrPoints,TextView textViewTotalPoints, string attempt)
        {
            string myAttempt = editText.Text.Trim();
            if (myAttempt == "")//אם לא הוכנס שום מילה
            {
                Toast.MakeText(this, "Please type a word", ToastLength.Long).Show();
                return;
            }
            if (myAttempt.Length <2)//אם המילה מכילה רק אות אחת
            {
                Toast.MakeText(this, "Type a word that consists more than a 1 letter", ToastLength.Long).Show();
                return;
            }
            if (!game.ExistInBank(myAttempt))//אם המילה לא מכילה את האותיות מהמילה בבנק
            {
                Toast.MakeText(this, "the word you entered not consists the letters from the word given, Try again", ToastLength.Long).Show();
                return;
            }
            if(game.SameWord(myAttempt))
            {
                Toast.MakeText(this, "You can only use words that are included inside the word, Try again", ToastLength.Long).Show();
                return;
            }
            if(game.ExistInAttempts(attempt))
            {
                Toast.MakeText(this, "You have already used this word, Try another", ToastLength.Long).Show();
                return;
            }
            Intent intent = new Intent(this, typeof(CheckService));
            
            checkReceiver.CheckBoxVaild = checkBox;
            checkReceiver.ReceiverTextViewCurrPoints = textViewCurrentPoints;
            checkReceiver.ReceiverTextViewTotalPoints = textViewSumPoints;
            intent.PutExtra("attempt", attempt);
            this.StartService(intent);
            button.Enabled = false;
            editText.Enabled = false;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.UnregisterReceiver(this.checkReceiver);
        }
        protected override void OnResume()
        {
            base.OnResume();
            this.RegisterReceiver(this.checkReceiver, new IntentFilter("zaRadio"));
        }
    }

}