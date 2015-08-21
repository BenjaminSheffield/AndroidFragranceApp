using System;
using Android;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SlidingTabLayoutTutorial
{
    public class SlidingTabScrollView : HorizontalScrollView
    {
        private const int TitleOffsetDips = 24;
        private const int TabViewPaddingDips = 16;
        private const int TabViewTextSizeSp = 12;
        private static SlidingTabStrip _mTabStrip;
        private readonly int _mTitleOffset;
        private int _mScrollState;
        private int _mTabViewLayoutId;
        private int _mTabViewTextViewId;
        private ViewPager _mViewPager;
        private ViewPager.IOnPageChangeListener _mViewPagerPageChangeListener;

        public SlidingTabScrollView(Context context) : this(context, null)
        {
        }

        public SlidingTabScrollView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public SlidingTabScrollView(Context context, IAttributeSet attrs, int defaultStyle)
            : base(context, attrs, defaultStyle)
        {
            //Disable the scroll bar
            HorizontalScrollBarEnabled = false;

            //Make sure the tab strips fill the view
            FillViewport = true;
            SetBackgroundColor(Color.Rgb(0xE5, 0xE5, 0xE5)); //Gray color

            _mTitleOffset = (int) (TitleOffsetDips*Resources.DisplayMetrics.Density);

            _mTabStrip = new SlidingTabStrip(context);
            AddView(_mTabStrip, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
        }

        public ITabColorizer CustomTabColorizer
        {
            set { _mTabStrip.CustomTabColorizer = value; }
        }

        public int[] SelectedIndicatorColor
        {
            set { _mTabStrip.SelectedIndicatorColors = value; }
        }

        public int[] DividerColors
        {
            set { _mTabStrip.DividerColors = value; }
        }

        public ViewPager.IOnPageChangeListener OnPageListener
        {
            set { _mViewPagerPageChangeListener = value; }
        }

        public ViewPager ViewPager
        {
            set
            {
                _mTabStrip.RemoveAllViews();

                _mViewPager = value;
                if (value != null)
                {
                    value.PageSelected += value_PageSelected;
                    value.PageScrollStateChanged += value_PageScrollStateChanged;
                    value.PageScrolled += value_PageScrolled;
                    PopulateTabStrip();
                }
            }
        }

        private void value_PageScrolled(object sender, ViewPager.PageScrolledEventArgs e)
        {
            var tabCount = _mTabStrip.ChildCount;

            if ((tabCount == 0) || (e.Position < 0) || (e.Position >= tabCount))
            {
                //if any of these conditions apply, return, no need to scroll
                return;
            }

            _mTabStrip.OnViewPagerPageChanged(e.Position, e.PositionOffset);

            var selectedTitle = _mTabStrip.GetChildAt(e.Position);

            var extraOffset = (selectedTitle != null ? e.Position*selectedTitle.Width : 0);

            ScrollToTab(e.Position, extraOffset);

            if (_mViewPagerPageChangeListener != null)
            {
                _mViewPagerPageChangeListener.OnPageScrolled(e.Position, e.PositionOffset, e.PositionOffsetPixels);
            }
        }

        private void value_PageScrollStateChanged(object sender, ViewPager.PageScrollStateChangedEventArgs e)
        {
            _mScrollState = e.State;

            if (_mViewPagerPageChangeListener != null)
            {
                _mViewPagerPageChangeListener.OnPageScrollStateChanged(e.State);
            }
        }

        private void value_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if (_mScrollState == ViewPager.ScrollStateIdle)
            {
                _mTabStrip.OnViewPagerPageChanged(e.Position, 0f);
                ScrollToTab(e.Position, 0);
            }

            if (_mViewPagerPageChangeListener != null)
            {
                _mViewPagerPageChangeListener.OnPageSelected(e.Position);
            }
        }

        private void PopulateTabStrip()
        {
            var adapter = _mViewPager.Adapter;

            for (var i = 0; i < adapter.Count; i++)
            {
                var tabView = CreateDefaultTabView(Context);
                tabView.Text = ((SlidingTabsFragment.SamplePagerAdapter) adapter).GetHeaderTitle(i);
                tabView.SetTextColor(Color.Black);
                tabView.Tag = i;
                tabView.Click += tabView_Click;
                _mTabStrip.AddView(tabView);
            }
        }

        private void tabView_Click(object sender, EventArgs e)
        {
            var clickTab = (TextView) sender;
            var pageToScrollTo = (int) clickTab.Tag;
            _mViewPager.CurrentItem = pageToScrollTo;
        }

        private TextView CreateDefaultTabView(Context context)
        {
            var textView = new TextView(context);
            textView.Gravity = GravityFlags.Center;
            textView.SetTextSize(ComplexUnitType.Sp, TabViewTextSizeSp);
            textView.Typeface = Typeface.DefaultBold;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
            {
                var outValue = new TypedValue();
                Context.Theme.ResolveAttribute(Resource.Attribute.SelectableItemBackground, outValue, false);
                textView.SetBackgroundResource(outValue.ResourceId);
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.IceCreamSandwich)
            {
                textView.SetAllCaps(true);
            }

            var padding = (int) (TabViewPaddingDips*Resources.DisplayMetrics.Density);
            textView.SetPadding(padding, padding, padding, padding);

            return textView;
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            if (_mViewPager != null)
            {
                ScrollToTab(_mViewPager.CurrentItem, 0);
            }
        }

        private void ScrollToTab(int tabIndex, int extraOffset)
        {
            var tabCount = _mTabStrip.ChildCount;

            if (tabCount == 0 || tabIndex < 0 || tabIndex >= tabCount)
            {
                //No need to go further, dont scroll
                return;
            }

            var selectedChild = _mTabStrip.GetChildAt(tabIndex);
            if (selectedChild != null)
            {
                var scrollAmountX = selectedChild.Left + extraOffset;

                if (tabIndex > 0 || extraOffset > 0)
                {
                    scrollAmountX -= _mTitleOffset;
                }

                ScrollTo(scrollAmountX, 0);
            }
        }

        public interface ITabColorizer
        {
            int GetIndicatorColor(int position);
            int GetDividerColor(int position);
        }
    }
}