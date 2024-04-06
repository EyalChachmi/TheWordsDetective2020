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

namespace Project_2020
{
    class AddWordsDialog:Dialog
    {
        private ISharedPreferences sp;
        private TextInputEditText editTextAddWords;
        private Button buttonSumbit;
        private Context context;
        public AddWordsDialog(Context context) : base(context)
        {
            //this.dialog = new Dialog(context);
            this.SetContentView(Resource.Layout.AddWordsDialog);
            this.SetCancelable(true);
            this.Initialize(context);
        }
        public void Initialize(Context context)
        {
            this.CancelEvent += AddWordsDialog_CancelEvent;
            this.editTextAddWords = this.FindViewById<TextInputEditText>(Resource.Id.textInputEditTextAddWords);
            this.buttonSumbit = this.FindViewById<Button>(Resource.Id.buttonSubmit);
            this.buttonSumbit.Click += ButtonSumbit_Click;
            this.sp = this.Context.GetSharedPreferences("myProfile", FileCreationMode.Private);
            this.context = context;
        }
        private void AddWordsDialog_CancelEvent(object sender, EventArgs e)
        {
            editTextAddWords.Text = "";
        }

        private async void ButtonSumbit_Click(object sender, EventArgs e)
        {
            var editor = sp.Edit();
            string word = editTextAddWords.Text;
            // נמחק את הנתונים מהאדיט טקסט בשביל הפעם הבאה
            editTextAddWords.Text = "";
            if (word.Length == 0)
            {
                Toast.MakeText(this.Context, "You haven't entered your word", ToastLength.Long).Show();
                return;
            }
            if (await Word.Exist(word))//מודיע במידה ומשתמש קיים כבר בענן בפעולה אסיכרונית
            {
                this.editTextAddWords.Text = "";
                editor.PutBoolean("bool", false);
                editor.Commit();
                this.Dismiss();
            }
            else
            {
                Word wordToCollection = new Word(word);
                if (await Word.AddWord(wordToCollection))//אם המשתמש נרשם בהצלחה,מקבל אותה למשחק
                {
                    editor.PutBoolean("bool", true);
                    editor.Commit();
                    this.Dismiss();
                }
                else
                {
                    editor.PutBoolean("bool", false);
                    editor.Commit();
                    this.Dismiss();
                }
            }
            this.Dismiss();
        }
    }
}