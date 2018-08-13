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
    [Register ("NoWorkspaceViewController")]
    partial class NoWorkspaceViewController
    {
        [Outlet]
        Toggl.Daneel.Views.ActivityIndicatorView ActivityIndicatorView { get; set; }


        [Outlet]
        UIKit.UIButton CreateWorkspaceButton { get; set; }


        [Outlet]
        UIKit.UIButton TryAgainButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ActivityIndicatorView != null) {
                ActivityIndicatorView.Dispose ();
                ActivityIndicatorView = null;
            }

            if (CreateWorkspaceButton != null) {
                CreateWorkspaceButton.Dispose ();
                CreateWorkspaceButton = null;
            }

            if (TryAgainButton != null) {
                TryAgainButton.Dispose ();
                TryAgainButton = null;
            }
        }
    }
}