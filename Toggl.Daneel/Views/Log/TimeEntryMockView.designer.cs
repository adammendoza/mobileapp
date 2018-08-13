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
    [Register ("TimeEntryMockView")]
    partial class TimeEntryMockView
    {
        [Outlet]
        UIKit.UIView ClientView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ClientWidthConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint DescriptionWidthConstraint { get; set; }


        [Outlet]
        UIKit.UIView ProjectView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ProjectWidthConstraint { get; set; }


        [Outlet]
        UIKit.UIView RootView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ClientView != null) {
                ClientView.Dispose ();
                ClientView = null;
            }

            if (ClientWidthConstraint != null) {
                ClientWidthConstraint.Dispose ();
                ClientWidthConstraint = null;
            }

            if (DescriptionWidthConstraint != null) {
                DescriptionWidthConstraint.Dispose ();
                DescriptionWidthConstraint = null;
            }

            if (ProjectView != null) {
                ProjectView.Dispose ();
                ProjectView = null;
            }

            if (ProjectWidthConstraint != null) {
                ProjectWidthConstraint.Dispose ();
                ProjectWidthConstraint = null;
            }

            if (RootView != null) {
                RootView.Dispose ();
                RootView = null;
            }
        }
    }
}