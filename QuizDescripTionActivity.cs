using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Project_2020
{
    [Activity(Label = "QuizDescripTionActivity", Theme = "@style/AppTheme")]
    public class QuizDescripTionActivity : AppCompatActivity
    {
        TextView quizTopicTextView;
        TextView descriptionTextView;
        Button startQuizButton;
        string quizTopic;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.credits_topic);
            quizTopicTextView = (TextView)FindViewById(Resource.Id.quizTopicText);
            descriptionTextView = (TextView)FindViewById(Resource.Id.quizDescriptionText);
            startQuizButton = (Button)FindViewById(Resource.Id.goBack);
           

            quizTopic = Intent.GetStringExtra("topic");
            quizTopicTextView.Text = quizTopic;
            // Retrieve Description

            descriptionTextView.Text = GetTopicDescription(quizTopic);
            startQuizButton.Click += StartQuizButton_Click;
        }

        private void StartQuizButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("topic", quizTopic);
            StartActivity(intent);
            Finish();
        }
        public string GetTopicDescription(string topic)
        {
            string topicDescription = "";
            if (topic == "credits")
            {
                topicDescription = "This game was made by Eyal chachmishvili from 12th grade in Shazar's highschool Bat-Yam. I'd like to thank Yoram, my computer science teacher and also thank my classmates that supported me a little by creating this app quiz.";
            }
            else if (topic == "HowToPlay")
            {
                topicDescription = "In this game there's going to be a word presented in front of you where you'll have to find 4 words inside of it. For example, the word Pineapple contains the words apple and pine. You have to type in 4 words and click check for every each one and then for the count of letters inside each word you'll be rewarded with points.For instance, the word apple has 5 letters so you'll get 5 points. after typing the 4 words you'll move to the next round untill the game will be over and you'll finish and your score will be saved in a table. the game is based on a timer so you better hurry! (:";
            }
            else if (topic == "aboutTheGame")
            {
                topicDescription = "The game is a dictionary quiz, it's target is to enrich your english language and also bring you joy by playing this game by discovering new words in the English language that you didn't know that have existed";
            }

            return topicDescription;
        }
    }
}