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

namespace Project_2020
{
    class Admin:User
    {
        public Admin()
        {

        } 
        public Admin(string userName,string password):base(userName,password)
        {
        }
        public async static Task<bool> AddWord(Word word)
        {
            try
            {
                word.WordId = AppData.ConnectionToFirestore.GetCollection(Word.WORDS_COLLECTION).CreateDocument().Id;
                await AppData.ConnectionToFirestore.GetCollection(Word.WORDS_COLLECTION).GetDocument(word.WordId).SetDataAsync(word);
                return true;
            }
            catch (Exception c)
            {
                Console.WriteLine(c.Message);
            }
            return false;
        }
        public bool CheckAdministration(string username,string password)
        {
            return username == "eyal" && password == "theadmin123";
        }
    }
}