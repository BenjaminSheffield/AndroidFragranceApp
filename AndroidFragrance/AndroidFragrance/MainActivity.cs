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
        private List<FragranceMenu> fragranceList;
        private ListView _mListView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            _mListView = FindViewById<ListView>(Resource.Id.mylistView);

            fragranceList = new List<FragranceMenu>();
            fragranceList.Add(new FragranceMenu(){House = "House", Rating = "Rating", Gender = "Gender", Price = "Price"});

            MyListViewAdapter adapter = new MyListViewAdapter(this, fragranceList);

            _mListView.Adapter = adapter;
            _mListView.ItemClick += _mListView_ItemClick;
            _mListView.ItemLongClick += _myListView_ItemLongClick;      

        }

        void _myListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Console.WriteLine(fragranceList[e.Position].Gender);
        }


        void _mListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Console.WriteLine(fragranceList[e.Position].House);
        }
    }
}

