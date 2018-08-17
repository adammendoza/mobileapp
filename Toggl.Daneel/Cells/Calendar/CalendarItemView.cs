using System;
using System.Collections.Generic;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using MvvmCross.Plugin.Color.Platforms.Ios;
using MvvmCross.UI;
using Toggl.Daneel.Views;
using Toggl.Foundation.Calendar;
using UIKit;

namespace Toggl.Daneel.Cells.Calendar
{
    public sealed partial class CalendarItemView : ReactiveCollectionViewCell<CalendarItem>
    {
        private static readonly Dictionary<CalendarIconKind, UIImage> images;

        private CAShapeLayer topDragIndicatorBorderLayer;
        private CAShapeLayer bottomDragIndicatorBorderLayer;

        public static readonly NSString Key = new NSString(nameof(CalendarItemView));
        public static readonly UINib Nib;

        static CalendarItemView()
        {
            Nib = UINib.FromName(nameof(CalendarItemView), NSBundle.MainBundle);

            images = new Dictionary<CalendarIconKind, UIImage>
            {
                { CalendarIconKind.Unsynced, templateImage("icUnsynced") },
                { CalendarIconKind.Event, templateImage("icCalendarSmall") },
                { CalendarIconKind.Unsyncable, templateImage("icErrorSmall") }
            };

            UIImage templateImage(string iconName)
                => UIImage.FromBundle(iconName)
                      .ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
        }

        public CalendarItemView(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            var topRect = TopDragIndicator.Bounds.Inset(1, 1);
            topDragIndicatorBorderLayer = new CAShapeLayer();
            topDragIndicatorBorderLayer.Path = UIBezierPath.FromOval(topRect).CGPath;
            topDragIndicatorBorderLayer.BorderWidth = 2;
            topDragIndicatorBorderLayer.FillColor = UIColor.Clear.CGColor;
            TopDragIndicator.Layer.AddSublayer(topDragIndicatorBorderLayer);

            var bottomRect = TopDragIndicator.Bounds.Inset(1, 1);
            bottomDragIndicatorBorderLayer = new CAShapeLayer();
            bottomDragIndicatorBorderLayer.Path = UIBezierPath.FromOval(bottomRect).CGPath;
            bottomDragIndicatorBorderLayer.BorderWidth = 2;
            bottomDragIndicatorBorderLayer.FillColor = UIColor.Clear.CGColor;
            BottomDragIndicator.Layer.AddSublayer(bottomDragIndicatorBorderLayer);
        }

        protected override void UpdateView()
        {
            var color = MvxColor.ParseHexString(Item.Color).ToNativeColor();
            DescriptionLabel.Text = Item.Description;
            DescriptionLabel.TextColor = textColor(color);
            ColorView.BackgroundColor = backgroundColor(Item.Source, color);
            updateIcon(color);
            updateDragIndicators(false, color);
        }

        private UIColor backgroundColor(CalendarItemSource source, UIColor color)
        {
            switch (source)
            {
                case CalendarItemSource.Calendar:
                    return color.ColorWithAlpha((nfloat)0.24);
                case CalendarItemSource.TimeEntry:
                    return color;
                default:
                    throw new ArgumentException("Unexpected calendar item source");
            }
        }

        private UIColor textColor(UIColor color)
        {
            switch (Item.Source)
            {
                case CalendarItemSource.Calendar:
                    return color;
                case CalendarItemSource.TimeEntry:
                    return UIColor.White;
                default:
                    throw new ArgumentException("Unexpected calendar item source");
            }
        }

        private void updateIcon(UIColor color)
        {
            if (Item.IconKind == CalendarIconKind.None)
            {
                CalendarIconImageView.Hidden = true;
                CalendarIconLeadingConstraint.Active = false;
                CalendarIconTrailingConstraint.Active = false;
                return;
            }

            CalendarIconImageView.Hidden = false;
            CalendarIconLeadingConstraint.Active = true;
            CalendarIconTrailingConstraint.Active = true;
            CalendarIconImageView.TintColor = textColor(color);
            CalendarIconImageView.Image = images[Item.IconKind];
        }

        private void updateDragIndicators(bool isDragging, UIColor color)
        {
            TopDragIndicator.Hidden = !isDragging;
            BottomDragIndicator.Hidden = !isDragging;
            topDragIndicatorBorderLayer.StrokeColor = color.CGColor;
            bottomDragIndicatorBorderLayer.StrokeColor = color.CGColor;
        }
    }
}
