using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CoreGraphics;
using Foundation;
using Toggl.Daneel.ViewSources;
using Toggl.Foundation.Calendar;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;
using UIKit;
using Math = System.Math;

namespace Toggl.Daneel.Views.Calendar
{
    public sealed class CalendarCollectionViewEditItemHelper : NSObject, IUIGestureRecognizerDelegate
    {
        private static readonly TimeSpan defaultDuration = TimeSpan.FromMinutes(15);

        private readonly UICollectionView collectionView;
        private readonly CalendarCollectionViewSource dataSource;
        private readonly CalendarCollectionViewLayout layout;

        private UILongPressGestureRecognizer longPressGestureRecognizer;
        private UIPanGestureRecognizer panGestureRecognizer;
        private UITapGestureRecognizer tapGestureRecognizer;

        private CalendarItem calendarItem;

        private NSIndexPath itemIndexPath;
        private nfloat verticalOffset;
        private CGPoint firstPoint;
        private CGPoint lastPoint;

        private bool isActive;

        private readonly ISubject<CalendarItem> editCalendarItemSuject = new Subject<CalendarItem>();
        public IObservable<CalendarItem> EditCalendarItem => editCalendarItemSuject.AsObservable();

        public CalendarCollectionViewEditItemHelper(
            UICollectionView collectionView,
            CalendarCollectionViewSource dataSource,
            CalendarCollectionViewLayout layout)
        {
            Ensure.Argument.IsNotNull(collectionView, nameof(collectionView));
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
            Ensure.Argument.IsNotNull(layout, nameof(layout));

            this.collectionView = collectionView;
            this.dataSource = dataSource;
            this.layout = layout;

            longPressGestureRecognizer = new UILongPressGestureRecognizer(onLongPress);
            longPressGestureRecognizer.Delegate = this;
            collectionView.AddGestureRecognizer(longPressGestureRecognizer);

            panGestureRecognizer = new UIPanGestureRecognizer(onPan);
            panGestureRecognizer.Delegate = this;

            tapGestureRecognizer = new UITapGestureRecognizer(onTap);
            tapGestureRecognizer.Delegate = this;
        }

        public CalendarCollectionViewEditItemHelper(IntPtr handle) : base(handle)
        {
        }

        [Export("gestureRecognizer:shouldRecognizeSimultaneouslyWithGestureRecognizer:")]
        public bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            if (gestureRecognizer == longPressGestureRecognizer)
                return otherGestureRecognizer is UILongPressGestureRecognizer;
            else
                return false;
        }

        private void onLongPress(UILongPressGestureRecognizer gesture)
        {
            var point = gesture.LocationInView(collectionView);

            switch (gesture.State)
            {
                case UIGestureRecognizerState.Began:
                    longPressBegan(point);
                    break;

                case UIGestureRecognizerState.Changed:
                    longPressChanged(point);
                    break;

                default:
                    break;
            }
        }

        private void onPan(UIPanGestureRecognizer gesture)
        {
            var point = gesture.LocationInView(collectionView);

            switch (gesture.State)
            {
                case UIGestureRecognizerState.Began:
                    panBegan(point);
                    break;

                case UIGestureRecognizerState.Changed:
                    if (point.Y - firstPoint.Y < 0)
                        panUp(point);
                    else
                        panDown(point);
                    break;

                default:
                    break;
            }
        }

        private void onTap(UITapGestureRecognizer gesture)
        {
            var point = gesture.LocationInView(collectionView);

            if (collectionView.IndexPathForItemAtPoint(point) == itemIndexPath)
                return;

            isActive = false;
            dataSource.IsEditing = false;
            collectionView.RemoveGestureRecognizer(panGestureRecognizer);
            collectionView.RemoveGestureRecognizer(tapGestureRecognizer);

            editCalendarItemSuject.OnNext(calendarItem);
        }

        private void longPressBegan(CGPoint point)
        {
            if (dataSource.IsEditing && !isActive)
                return;

            isActive = true;
            dataSource.IsEditing = true;
            itemIndexPath = collectionView.IndexPathForItemAtPoint(point);
            calendarItem = dataSource.CalendarItemAtPoint(point).Value;
            var startPoint = layout.PointAtDate(calendarItem.StartTime.ToLocalTime());
            firstPoint = point;
            lastPoint = point;
            verticalOffset = firstPoint.Y - startPoint.Y;
            collectionView.AddGestureRecognizer(panGestureRecognizer);
            collectionView.AddGestureRecognizer(tapGestureRecognizer);
        }

        private void longPressChanged(CGPoint point)
        {
            if (!isActive || itemIndexPath == null)
                return;

            if (Math.Abs(lastPoint.Y - point.Y) < layout.HourHeight / 4)
                return;

            lastPoint = point;
            var startPoint = new CGPoint(lastPoint.X, lastPoint.Y - verticalOffset);
            var startTime = layout.DateAtPoint(startPoint).ToLocalTime().RoundDownToClosestQuarter();

            calendarItem = calendarItem
                .WithStartTime(startTime);

            itemIndexPath = dataSource.UpdatePlaceholder(itemIndexPath, calendarItem.StartTime, calendarItem.Duration);
        }

        private void panBegan(CGPoint point)
        {
            if (!isActive)
                return;

            firstPoint = point;
            lastPoint = point;
        }

        private void panUp(CGPoint point)
        {
            if (!isActive || itemIndexPath == null)
                return;

            if (Math.Abs(lastPoint.Y - point.Y) < layout.HourHeight / 4)
                return;

            lastPoint = point;
            var startTime = layout.DateAtPoint(lastPoint).ToLocalTime().RoundDownToClosestQuarter();
            var duration = calendarItem.EndTime - startTime;

            calendarItem = calendarItem
                .WithStartTime(startTime)
                .WithDuration(duration);

            itemIndexPath = dataSource.UpdatePlaceholder(itemIndexPath, calendarItem.StartTime, calendarItem.Duration);
        }

        private void panDown(CGPoint point)
        {
            if (!isActive || itemIndexPath == null)
                return;

            if (Math.Abs(lastPoint.Y - point.Y) < layout.HourHeight / 4)
                return;

            lastPoint = point;
            var endTime = layout.DateAtPoint(lastPoint).ToLocalTime().RoundUpToClosestQuarter();
            var duration = endTime - calendarItem.StartTime;

            calendarItem = calendarItem
                .WithDuration(duration);

            itemIndexPath = dataSource.UpdatePlaceholder(itemIndexPath, calendarItem.StartTime, calendarItem.Duration);
        }
    }
}
