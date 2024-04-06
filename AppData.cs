using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Plugin.CloudFirestore;

namespace Project_2020
{
    public class AppData
    {
        private static AppData AppDataInstance { get; set; }
        public static IFirestore ConnectionToFirestore { get; private set; }
        private AppData(Context context)
        {
            FirebaseOptions options = new FirebaseOptions.Builder()
               .SetProjectId("appwordsinword")
               .SetApplicationId("appwordsinword")
               .SetApiKey("AIzaSyCQVrJnXlVM6UyxMLBsYj8zOKoZI-1fw0E")
               .SetDatabaseUrl("https://appwordsinword.firebaseio.com")
               .SetStorageBucket("appwordsinword.appspot.com")
               .Build();
            //מאתחל את הפיירבייס באפליקציה שלנו
            FirebaseApp.InitializeApp(context, options);

            // נותן לי גישה לפייר סטור
            ConnectionToFirestore = CrossCloudFirestore.Current.Instance;
        }
        public static AppData Initialize(Context context)
        {
            if (AppDataInstance == null)
            {
                AppDataInstance = new AppData(context);
            }
            return AppDataInstance;
        }
    }
}