// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Toggl.Daneel
{
    [Register ("SuggestionView")]
    partial class SuggestionView
    {
        [Outlet]
        UIKit.UILabel ClientLabel { get; set; }


        [Outlet]
        UIKit.UILabel DescriptionLabel { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint DescriptionTopDistanceConstraint { get; set; }


        [Outlet]
        Toggl.Daneel.Views.FadeView FadeView { get; set; }


        [Outlet]
        UIKit.UIImageView ProjectDot { get; set; }


        [Outlet]
        UIKit.UILabel ProjectLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ClientLabel != null) {
                ClientLabel.Dispose ();
                ClientLabel = null;
            }

            if (DescriptionLabel != null) {
                DescriptionLabel.Dispose ();
                DescriptionLabel = null;
            }

            if (DescriptionTopDistanceConstraint != null) {
                DescriptionTopDistanceConstraint.Dispose ();
                DescriptionTopDistanceConstraint = null;
            }

            if (FadeView != null) {
                FadeView.Dispose ();
                FadeView = null;
            }

            if (ProjectDot != null) {
                ProjectDot.Dispose ();
                ProjectDot = null;
            }

            if (ProjectLabel != null) {
                ProjectLabel.Dispose ();
                ProjectLabel = null;
            }
        }
    }
}