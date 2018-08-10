using System;
using CoreGraphics;
using Toggl.Daneel.Presentation.Attributes;
using Toggl.Daneel.ViewSources;
using Toggl.Foundation.MvvmCross.ViewModels.Calendar;
using UIKit;

namespace Toggl.Daneel.ViewControllers.Calendar
{
    [ModalDialogPresentation]
    public sealed partial class SelectUserCalendarsViewController
        : ReactiveViewController<SelectUserCalendarsViewModel>
    {
        private const int heightAboveTableView = 98;
        private const int heightBelowTableView = 80;
        private const int maxHeight = 627;

        public SelectUserCalendarsViewController() : base(null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var source = new SelectUserCalendarsTableViewSource(TableView, ViewModel.Calendars);
            TableView.Source = source;

            PreferredContentSize = new CGSize(288, 458);
            var a = View.SizeThatFits(View.Frame.Size);
            var b = View.IntrinsicContentSize;
            var c = View.Frame;
            var d = TableView.ContentSize;
            Console.WriteLine();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            var targetHeight = heightAboveTableView + heightBelowTableView + TableView.ContentSize.Height;
            var actualHeight = targetHeight > maxHeight
                ? maxHeight
                : targetHeight;

            PreferredContentSize = new CGSize(
                288,
                actualHeight
            );

            if (actualHeight <= maxHeight)
                TableView.ScrollEnabled = false;
        }
    }
}
