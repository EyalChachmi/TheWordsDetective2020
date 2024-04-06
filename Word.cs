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
using Plugin.CloudFirestore;

namespace Project_2020
{
    public class Word
    {
        public const string WORDS_COLLECTION = "words";
        public string WordId { get; set; }
        public string ZaWardo { get; set; }
        public Word()
        {

        }
        public Word(string word)
        {
            this.ZaWardo = word;
        }
        
        public async static Task<bool> AddWord(Word word)
        {
            try
            {
                word.WordId = AppData.ConnectionToFirestore.GetCollection(WORDS_COLLECTION).CreateDocument().Id;
                await AppData.ConnectionToFirestore.GetCollection(WORDS_COLLECTION).GetDocument(word.WordId).SetDataAsync(word);
                return true;
            }
            catch (Exception c)
            {
                Console.WriteLine(c.Message);
            }
            return false;
        }
        public async static Task<bool> Exist(string word)
        {
            try
            {
                IQuerySnapshot queryResult = await AppData.ConnectionToFirestore.GetCollection(WORDS_COLLECTION).WhereEqualsTo("ZaWardo", word).GetDocumentsAsync();
                List<Word> words = queryResult.ToObjects<Word>().ToList();
                return words.Count > 0;
            }
            catch (Exception c)
            {
                Console.WriteLine(c.Message);
            }
            return false;
        }
        public async static Task<Word> GetWord(string wordID)
        {
            try
            {
                IQuerySnapshot query = await AppData.ConnectionToFirestore.GetCollection(WORDS_COLLECTION).WhereEqualsTo("WordId", wordID).GetDocumentsAsync();
                Word myWord = query.ToObjects<Word>().ToList()[0];
                return myWord;
            }
            catch
            {
                return null;
            }
        }
        public async static Task<List<Word>> GetAllWords()
        {
            try
            {
                IQuerySnapshot query = await AppData.ConnectionToFirestore.GetCollection(WORDS_COLLECTION).GetDocumentsAsync();
                List<Word> wordsList = query.ToObjects<Word>().ToList();
                wordsList = wordsList.ToList();
                return wordsList;
            }
            catch
            {
                return null;
            }
        }
    }
}