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
    public class User
    {
        public const string USERS_COLLECTION = "users";
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserId { get; set; }
        public int BestScore { get; set; }
        public User()
        {
        }

        public User(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
            this.BestScore =0;
        }
        public async static Task<User> GetUser(string userID)//פעולת החזרת משתמש
        {
            try
            {
                IQuerySnapshot query = await AppData.ConnectionToFirestore.GetCollection(USERS_COLLECTION).WhereEqualsTo("UserId", userID).GetDocumentsAsync();
                User myUser = query.ToObjects<User>().ToList()[0];
                return myUser;
            }
            catch
            {
                return null;
            }
        }
        public async static Task<List<User>> GetAllUsers()//פעולת החזרת רשימת משתמשים
        {
            try
            {
                IQuerySnapshot query = await AppData.ConnectionToFirestore.GetCollection(USERS_COLLECTION).GetDocumentsAsync();
                List<User> usersList = query.ToObjects<User>().ToList();
                usersList=usersList.OrderByDescending(user => user.BestScore).ToList();
                return usersList;
            }
            catch
            {
                return null;
            }
        }
        public async static Task<bool> AddUser(User user)//הוספת משתמש
        {
            try
            {
                user.UserId = AppData.ConnectionToFirestore.GetCollection(USERS_COLLECTION).CreateDocument().Id;
                await AppData.ConnectionToFirestore.GetCollection(USERS_COLLECTION).GetDocument(user.UserId).SetDataAsync(user);
                return true;
            }
            catch(Exception c)
            {
                Console.WriteLine(c.Message);
            }
            return false;
        }
        public async static Task<bool> UpdateUser(User user)
        {
            try
            {
                await AppData.ConnectionToFirestore.GetCollection(USERS_COLLECTION).GetDocument(user.UserId).SetDataAsync(user);
                return true;
            }
            catch (Exception c)
            {
                Console.WriteLine(c.Message);
            }
            return false;
        }
        public async static Task<bool> Exist(string userName)//פעולה הבודקת עם שם המשתמש קיים בענן
        {
            try
            {
                IQuerySnapshot queryResult = await AppData.ConnectionToFirestore.GetCollection(USERS_COLLECTION).WhereEqualsTo("UserName", userName).GetDocumentsAsync();
                List<User> users = queryResult.ToObjects<User>().ToList();
                return users.Count > 0;
            }
            catch(Exception c)
            {
                Console.WriteLine(c.Message);
            }
            return false;
        }
        public async static Task<string> GetId(string userName,string password)
        {
            try
            {
                IQuerySnapshot queryResult = await AppData.ConnectionToFirestore.GetCollection(USERS_COLLECTION).WhereEqualsTo("UserName", userName).WhereEqualsTo("Password", password).GetDocumentsAsync();
                List<User> users = queryResult.ToObjects<User>().ToList();//מכניס את שם המשתמש לרשימה
                return users[0].UserId;
            }
            catch (Exception c)
            {
                Console.WriteLine(c.Message);
            }
            return "-1";
        }
    }
}