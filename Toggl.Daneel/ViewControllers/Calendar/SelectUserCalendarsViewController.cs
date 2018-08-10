﻿using CoreGraphics;
using Toggl.Daneel.Presentation.Attributes;
using Toggl.Daneel.ViewSources;
using Toggl.Foundation.MvvmCross.ViewModels.Calendar;

namespace Toggl.Daneel.ViewControllers.Calendar
{
    [ModalDialogPresentation]
    public sealed partial class SelectUserCalendarsViewController
        : ReactiveViewController<SelectUserCalendarsViewModel>
    {
        private const int heightAboveTableView = 98;
        private const int heightBelowTableView = 80;
        private const int maxHeight = 627;
        private const int width = 288;

        public SelectUserCalendarsViewController() : base(null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var source = new SelectUserCalendarsTableViewSource(TableView, ViewModel.Calendars);
            TableView.Source = source;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            setDialogSize();
        }

        private void setDialogSize()
        {
            var targetHeight = calculateTargetHeight();
            PreferredContentSize = new CGSize(
                width,
                targetHeight > maxHeight ? maxHeight : targetHeight
            );

            //Implementation in ModalPresentationController
            View.Frame = PresentationController.FrameOfPresentedViewInContainerView;

            TableView.ScrollEnabled = targetHeight > maxHeight;
        }

        private int calculateTargetHeight()
            => heightAboveTableView + heightBelowTableView + (int)TableView.ContentSize.Height;
    }
}
