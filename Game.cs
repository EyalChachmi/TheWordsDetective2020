using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;

namespace Project_2020
{
    public class Game
    {
        //private string hint;
        private List<Word> wordsList;
        private string cloudWord = "";
        private Random random;
        private List<string> words;
        private List<string> attemptList;
        private List<string> randomWords;
        private List<string> nextList;
        private int wordsCount;
        public int TotalPoints { get; set; }
        public int CurrPoints { get; set; }
        public static int RoundsCounter { get; set; }
        public Game()
        {
            wordsCount = 0;
            RoundsCounter = 0;
            this.random = new Random(DateTime.Now.Millisecond);
            this.wordsList = new List<Word>();//רשימת העצמים של המילים מתוך הענן
            this.words = new List<string>();//רשימת מילים
            this.attemptList = new List<string>();//רשימת הנסיונות הנכונות של המשתמש של המילים בתוך המילון
            this.randomWords = new List<string>();//רשימת המילים האקראיות
            this.nextList = new List<string>();//רשימת שלבי המשחק
        }

        public void GetARandomizedList()//עושה רשימה עם מילים רנדומליות
        {
            for (int i = 0; i < words.Count; i++)
            {
                //int x = words.Count + 1;
                int y = random.Next(words.Count);
                string str = words[y];
                randomWords.Add(str);
                words.RemoveAt(y);
            }    
        }
        public bool InsertedBefore(string attempt)
        {
            foreach (var word in this.attemptList)
            {
                if (word.Length == attempt.Length)
                {
                    return false;
                }
            }
            return true;
        }
        public string GetTheWord()//משיג את המילה מהרשימה
        {
            if (wordsCount < randomWords.Count)
            {
                this.cloudWord = this.randomWords[wordsCount++];
            }
            return this.cloudWord;
        }
        public async Task<bool> GenerateNewWord()//משיגה את הרשימה מהענן, וממירה אותה לרשימה של מחרוזת
        {
            this.CurrPoints = 0;
            this.words.Clear();
            List<Word> wordsList = await Word.GetAllWords();
            for (int i = 0; i < wordsList.Count; i++)
            {
                this.words.Add(wordsList[i].ZaWardo);
            }
            return true;
        }
        public bool SameWord(string word)
        {
            return this.cloudWord == word;
        }
        public bool ExistInBank(string attempt)//בודק עבור כל אות במילה האם היא קיימת בתוך המילה בענן
        {
            foreach (char letter in attempt)
            {
                if (!this.cloudWord.Contains(letter))
                {
                    return false;
                }
            }
            return true;
        }
        public bool ExistInAttempts(string attempt)//בודק האם הוא קיים במילים הנכונות הקודמות בהם השתמש המשתמש
        {
            for (int i = 0; i < attemptList.Count; i++)
            {
                if (attemptList[i]==attempt)
                {
                    return true;
                }
            }
            return false;
        }
        public void Update(string attempt)//מוסיפה לרשימה את המילים הנכונות בה השתמשו
        {
            this.attemptList.Add(attempt);
            this.CurrPoints += attempt.Length;// הנוכחי, כל אות שווה נקודה אחת points עדכון של 
            this.TotalPoints += attempt.Length;//הטוטאלי, כל אות שווה נקודה אחת points עדכון של 
        }
        public void UpdateNext(string attempt)//ברשימה יגיע ל4 המשחק יסתיים לפי הפעולה הבאה count-מוסיפה לרשימה את המילים בה השתמשו(כולל הלא נכונות),כאשר ה 
        {
            this.nextList.Add(attempt);
        }
        public bool RoundFinished()//ברשימה יגיע ל4 המשחק יסתיים לפי הפעולה הזו countכאשר ה
        {
            if(nextList.Count == 4)
            {
                this.CurrPoints = 0;
                this.attemptList.Clear();
                this.nextList.Clear();
                RoundsCounter++;
                return true;
            }
            return false;
        }

    }
}