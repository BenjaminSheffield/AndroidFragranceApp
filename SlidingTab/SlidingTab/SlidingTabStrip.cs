using System;
using Android;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;

namespace SlidingTabLayoutTutorial
{
    public class SlidingTabStrip : LinearLayout
    {
        private const int DefaultBottomBorderThicknessDips = 2;
        private const byte DefaultBottomBorderColorAlpha = 0X26;
        private const int SelectedIndicatorThicknessDips = 8;
        private const int DefaultDividerThicknessDips = 1;
        private const float DefaultDividerHeight = 0.5f;
        private readonly int[] _dividerColors = {0xC5C5C5};
        private readonly int[] _indicatorColors = { 0x19A319, 0x0000FC, 0xff0000 };
        private readonly Paint _mBottomBorderPaint;
        //Bottom border
        private readonly int _mBottomBorderThickness;
        private readonly float _mDividerHeight;
        //Divider
        private readonly Paint _mDividerPaint;
        private readonly Paint _mSelectedIndicatorPaint;
        //Indicator
        private readonly int _mSelectedIndicatorThickness;
        //Tab colorizer
        private SlidingTabScrollView.ITabColorizer _mCustomTabColorizer;
        private int _mDefaultBottomBorderColor;
        private SimpleTabColorizer _mDefaultTabColorizer;
        //Selected position and offset
        private int _mSelectedPosition;
        private float _mSelectionOffset;
        
        //Constructors
        public SlidingTabStrip(Context context)
            : this(context, null)
        {
        }

        public SlidingTabStrip(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            SetWillNotDraw(false);

            var density = Resources.DisplayMetrics.Density;

            var outValue = new TypedValue();
            context.Theme.ResolveAttribute(Resource.Attribute.ColorForeground, outValue, true);
            var themeForeGround = outValue.Data;
            _mDefaultBottomBorderColor = SetColorAlpha(themeForeGround, DefaultBottomBorderColorAlpha);

            _mDefaultTabColorizer = new SimpleTabColorizer();
            _mDefaultTabColorizer.IndicatorColors = _indicatorColors;
            _mDefaultTabColorizer.DividerColors = _dividerColors;

            _mBottomBorderThickness = (int) (DefaultBottomBorderThicknessDips*density);
            _mBottomBorderPaint = new Paint();
            _mBottomBorderPaint.Color = GetColorFromInteger(0xC5C5C5); //Gray

            _mSelectedIndicatorThickness = (int) (SelectedIndicatorThicknessDips*density);
            _mSelectedIndicatorPaint = new Paint();

            _mDividerHeight = DefaultDividerHeight;
            _mDividerPaint = new Paint();
            _mDividerPaint.StrokeWidth = (int) (DefaultDividerThicknessDips*density);
        }

        public SlidingTabScrollView.ITabColorizer CustomTabColorizer
        {
            set
            {
                _mCustomTabColorizer = value;
                Invalidate();
            }
        }

        public int[] SelectedIndicatorColors
        {
            set
            {
                _mCustomTabColorizer = null;
                _mDefaultTabColorizer.IndicatorColors = value;
                Invalidate();
            }
        }

        public int[] DividerColors
        {
            set
            {
                _mDefaultTabColorizer = null;
                _mDefaultTabColorizer.DividerColors = value;
                Invalidate();
            }
        }

        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }

        private int SetColorAlpha(int color, byte alpha)
        {
            return Color.Argb(alpha, Color.GetRedComponent(color), Color.GetGreenComponent(color),
                Color.GetBlueComponent(color));
        }

        public void OnViewPagerPageChanged(int position, float positionOffset)
        {
            _mSelectedPosition = position;
            _mSelectionOffset = positionOffset;
            Invalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            var height = Height;
            var tabCount = ChildCount;
            var dividerHeightPx = (int) (Math.Min(Math.Max(0f, _mDividerHeight), 1f)*height);
            var tabColorizer = _mCustomTabColorizer != null ? _mCustomTabColorizer : _mDefaultTabColorizer;

            //Thick colored underline below the current selection
            if (tabCount > 0)
            {
                var selectedTitle = GetChildAt(_mSelectedPosition);
                var left = selectedTitle.Left;
                var right = selectedTitle.Right;
                var color = tabColorizer.GetIndicatorColor(_mSelectedPosition);

                if (_mSelectionOffset > 0f && _mSelectedPosition < (tabCount - 1))
                {
                    var nextColor = tabColorizer.GetIndicatorColor(_mSelectedPosition + 1);
                    if (color != nextColor)
                    {
                        color = BlendColor(nextColor, color, _mSelectionOffset);
                    }

                    var nextTitle = GetChildAt(_mSelectedPosition + 1);
                    left = (int) (_mSelectionOffset*nextTitle.Left + (1.0f - _mSelectionOffset)*left);
                    right = (int) (_mSelectionOffset*nextTitle.Right + (1.0f - _mSelectionOffset)*right);
                }

                _mSelectedIndicatorPaint.Color = GetColorFromInteger(color);

                canvas.DrawRect(left, height - _mSelectedIndicatorThickness, right, height, _mSelectedIndicatorPaint);

                //Creat vertical dividers between tabs
                var separatorTop = (height - dividerHeightPx)/2;
                for (var i = 0; i < ChildCount; i++)
                {
                    var child = GetChildAt(i);
                    _mDividerPaint.Color = GetColorFromInteger(tabColorizer.GetDividerColor(i));
                    canvas.DrawLine(child.Right, separatorTop, child.Right, separatorTop + dividerHeightPx,
                        _mDividerPaint);
                }

                canvas.DrawRect(0, height - _mBottomBorderThickness, Width, height, _mBottomBorderPaint);
            }
        }

        private int BlendColor(int color1, int color2, float ratio)
        {
            var inverseRatio = 1f - ratio;
            var r = (Color.GetRedComponent(color1)*ratio) + (Color.GetRedComponent(color2)*inverseRatio);
            var g = (Color.GetGreenComponent(color1)*ratio) + (Color.GetGreenComponent(color2)*inverseRatio);
            var b = (Color.GetBlueComponent(color1)*ratio) + (Color.GetBlueComponent(color2)*inverseRatio);

            return Color.Rgb((int) r, (int) g, (int) b);
        }

        private class SimpleTabColorizer : SlidingTabScrollView.ITabColorizer
        {
            private int[] _mDividerColors;
            private int[] _mIndicatorColors;

            public int[] IndicatorColors
            {
                set { _mIndicatorColors = value; }
            }

            public int[] DividerColors
            {
                set { _mDividerColors = value; }
            }

            public int GetIndicatorColor(int position)
            {
                return _mIndicatorColors[position%_mIndicatorColors.Length];
            }

            public int GetDividerColor(int position)
            {
                return _mDividerColors[position%_mDividerColors.Length];
            }
        }
    }
}