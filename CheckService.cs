using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using DictionaryLib;
using Android.Views;
using Android.Widget;

namespace Project_2020
{
    [Service]
    class CheckService : Service,ICorrectable
    {
        private string attempt = "";
        DictionaryType dictionaryType = new DictionaryType();//יוצר הפנייה למילון בו מאוכסנות ככמעט 180,000 מילים

        public bool Correct(string attempt)
        {
            DictionaryLib.DictionaryLib dictionaryLib = new DictionaryLib.DictionaryLib(dictionaryType, null);//יוצר את המילון בפועל 
            bool exist = dictionaryLib.IsWord(attempt);//בודק האם המילה קיימת במילון או לא אם כן יחזיר אמת אחרת שקר
            return exist;
        }
        //private com.aonaware.services.DictService dictService = new com.aonaware.services.DictService();
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            this.attempt = intent.GetStringExtra("attempt");
            ThreadStart threadStart = new ThreadStart(Check);
            Thread thread = new Thread(threadStart);
            thread.Start();
            return base.OnStartCommand(intent, flags, startId);
        }
        private void Check()
        {
            Intent intent = new Intent("zaRadio");
            intent.PutExtra("exist", Correct(attempt));// נשים באינטנט אם המילה קיימת או לא אם כן יחזיר אמת אחרת שקר
            intent.PutExtra("attempt", attempt);//checkReceiverנשים באינטנט את המילה ונעביר ל
            this.StopSelf();
            this.SendBroadcast(intent);
        }
    }
}