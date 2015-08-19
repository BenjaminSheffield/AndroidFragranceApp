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

namespace AndroidFragrance
{
    class MyListViewAdapter : BaseAdapter<FragranceMenu>
    {
        private List<FragranceMenu> mItems;
        private Context mContext;

        public MyListViewAdapter(Context context, List<FragranceMenu> items)
        {
            mItems = items;
            mContext = context; 
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.listview_row, null, false);
            }

            TextView txtHouse = row.FindViewById<TextView>(Resource.Id.txtHouse);
            txtHouse.Text = mItems[position].Gender;

            TextView txtRating = row.FindViewById<TextView>(Resource.Id.txtRating);
            txtRating.Text = mItems[position].Rating;

            TextView txtGender = row.FindViewById<TextView>(Resource.Id.txtGender);
            txtGender.Text = mItems[position].Gender;

            TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
            txtPrice.Text = mItems[position].Price;

            return row;
        } 

        public override int Count
        {
            get {return mItems.Count; }
        }

        public override FragranceMenu this[int position]
        {
            get { return mItems[position]; }
        }
    }
}