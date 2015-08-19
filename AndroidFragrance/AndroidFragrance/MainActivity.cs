using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AndroidFragrance
{
    [Activity(Label = "AndroidFragrance", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private List<string> fragranceList;
        private ListView _mListView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            _mListView = FindViewById<ListView>(Resource.Id.mylistView);

            fragranceList = new List<string>();
            fragranceList.Add("House");
            fragranceList.Add("Rating");
            fragranceList.Add("Gender");
            fragranceList.Add("Price");

            MyListViewAdapter adapter = new MyListViewAdapter(this, fragranceList);

            _mListView.Adapter = adapter; 

        }
    }
}

