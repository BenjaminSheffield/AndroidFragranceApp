using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Java.Lang;
using SlidingTab;

namespace SlidingTabLayoutTutorial
{
    public class SlidingTabsFragment : Fragment
    {
        private SlidingTabScrollView _mSlidingTabScrollView;
        private ViewPager _mViewPager;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_sample, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            _mSlidingTabScrollView = view.FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
            _mViewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            _mViewPager.Adapter = new SamplePagerAdapter();

            _mSlidingTabScrollView.ViewPager = _mViewPager;
        }

        public class SamplePagerAdapter : PagerAdapter
        {
            private readonly List<string> _items = new List<string>();

            public SamplePagerAdapter()
            {
                _items.Add("Xamarin");
                _items.Add("Android");
                _items.Add("Tutorial");
                _items.Add("Part");
                _items.Add("12");
                _items.Add("Hooray");
            }

            public override int Count
            {
                get { return _items.Count; }
            }

            public override bool IsViewFromObject(View view, Object obj)
            {
                return view == obj;
            }

            public override Object InstantiateItem(ViewGroup container, int position)
            {
                var view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.pager_item, container, false);
                container.AddView(view);

                var txtTitle = view.FindViewById<TextView>(Resource.Id.item_title);
                var pos = position + 1;
                txtTitle.Text = pos.ToString();

                return view;
            }

            public string GetHeaderTitle(int position)
            {
                return _items[position];
            }

            public override void DestroyItem(ViewGroup container, int position, Object obj)
            {
                container.RemoveView((View) obj);
            }
        }
    }
}