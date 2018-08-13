// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Toggl.Daneel.Views
{
    [Register ("ReportsCalendarViewCell")]
    partial class ReportsCalendarViewCell
    {
        [Outlet]
        Toggl.Daneel.Views.RoundedView BackgroundView { get; set; }


        [Outlet]
        UIKit.UILabel Text { get; set; }


        [Outlet]
        Toggl.Daneel.Views.RoundedView TodayBackgroundView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackgroundView != null) {
                BackgroundView.Dispose ();
                BackgroundView = null;
            }

            if (Text != null) {
                Text.Dispose ();
                Text = null;
            }

            if (TodayBackgroundView != null) {
                TodayBackgroundView.Dispose ();
                TodayBackgroundView = null;
            }
        }
    }
}