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

namespace Project_2020
{
    [Activity(Label = "ActivityShowRecords")]
    public class ActivityShowRecords : Activity
    {
        ListView listViewRecords;
        RecordsAdapter recordsAdapter;
        List<User> usersList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.ShowRecordsLayout);
            this.Initialize();
        }
        public async void Initialize()
        {
            this.listViewRecords = this.FindViewById<ListView>(Resource.Id.listViewRecords);
            View headerRowView = this.LayoutInflater.Inflate(Resource.Layout.RecordsLayoutRow, null);
            this.listViewRecords.AddHeaderView(headerRowView);
            this.usersList = await User.GetAllUsers();
            this.recordsAdapter = new RecordsAdapter(this, this.usersList);
            this.listViewRecords.Adapter = this.recordsAdapter;
        }
    }
}