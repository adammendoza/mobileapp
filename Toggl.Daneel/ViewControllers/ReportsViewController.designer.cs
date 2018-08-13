// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Toggl.Daneel.ViewControllers
{
    [Register ("ReportsViewController")]
    partial class ReportsViewController
    {
        [Outlet]
        UIKit.UIView CalendarContainer { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint CalendarHeightConstraint { get; set; }


        [Outlet]
        UIKit.UITableView ReportsTableView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint TopCalendarConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint TopConstraint { get; set; }


        [Outlet]
        UIKit.UIView WorkspaceButton { get; set; }


        [Outlet]
        Toggl.Daneel.Views.FadeView WorkspaceFadeView { get; set; }


        [Outlet]
        UIKit.UILabel WorkspaceLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CalendarContainer != null) {
                CalendarContainer.Dispose ();
                CalendarContainer = null;
            }

            if (ReportsTableView != null) {
                ReportsTableView.Dispose ();
                ReportsTableView = null;
            }

            if (TopCalendarConstraint != null) {
                TopCalendarConstraint.Dispose ();
                TopCalendarConstraint = null;
            }

            if (WorkspaceButton != null) {
                WorkspaceButton.Dispose ();
                WorkspaceButton = null;
            }

            if (WorkspaceFadeView != null) {
                WorkspaceFadeView.Dispose ();
                WorkspaceFadeView = null;
            }

            if (WorkspaceLabel != null) {
                WorkspaceLabel.Dispose ();
                WorkspaceLabel = null;
            }
        }
    }
}