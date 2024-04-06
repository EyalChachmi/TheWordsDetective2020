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
    public class RecordsAdapter : BaseAdapter<User>
    {
        private Context context;
        private List<User> usersList;

        public RecordsAdapter(Context context,List<User> usersList)
        {
            this.context = context;
            this.usersList = usersList;
        }
        public override User this[int position] => usersList[position];

        public override int Count => usersList.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            User current = this.usersList[position];
            View rowView = ((Activity)context).LayoutInflater.Inflate(Resource.Layout.RecordsLayoutRow, null);
            TextView textViewUsernameRow = rowView.FindViewById<TextView>(Resource.Id.textViewUsernameRow);
            TextView textViewRecordRow = rowView.FindViewById<TextView>(Resource.Id.textViewRecordRow);
            textViewUsernameRow.Text = current.UserName;
            textViewRecordRow.Text = current.BestScore.ToString();
            return rowView;
        }
    }
}