using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace Project_2020
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "zaRadio" })]
    public class CheckReceiver : BroadcastReceiver
    {
        private Game game;
        private Context context;
        public CheckBox CheckBoxVaild { get; set; }
        public TextView ReceiverTextViewCurrPoints { get; set; }
        public TextView ReceiverTextViewTotalPoints { get; set; }

        public CheckReceiver()
        { }
        public CheckReceiver(Context context,Game game)
        {
            this.context = context;
            this.game = game;
        }
        //this.textViewCurrentPoints.Text = string.Format("current points: {0}", this.game.CurrPoints.ToString());
        public override void OnReceive(Context context, Intent intent)
        {
            bool correct = intent.GetBooleanExtra("exist",false);//משיג מהאינטנט אם המילה קיימת או לא
            string attempt = intent.GetStringExtra("attempt");//משיג את המילה מהאינטנט אשר באה מהענן

            if (correct == true)//(אם המילה קיימת יהיה צאק על הצאקבוקס והמשחק אתעדכן בהתאם לכך(תהיה העלאה בנקודות 
            {
                this.CheckBoxVaild.Checked = true;
                var duration = TimeSpan.FromSeconds(0.35);
                Vibration.Vibrate(duration);
                this.game.Update(attempt);
                this.ReceiverTextViewCurrPoints.Text = string.Format("current points: {0}", this.game.CurrPoints.ToString());
                this.ReceiverTextViewTotalPoints.Text = string.Format("Total points: {0}", this.game.TotalPoints.ToString());
            }
            this.game.UpdateNext(attempt);
        }

    }
}