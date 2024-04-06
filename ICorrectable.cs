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
    public interface ICorrectable
    {
        bool Correct(string attempt);//מחייב שלכל מילה תהיה התנהגות נכונה, התנהגות הבודקת האם המילה קיימת או לא בעלת משמעות
    }
}