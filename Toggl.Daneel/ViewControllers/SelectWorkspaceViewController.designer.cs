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
    [Register ("SelectWorkspaceViewController")]
    partial class SelectWorkspaceViewController
    {
        [Outlet]
        UIKit.NSLayoutConstraint BottomConstraint { get; set; }


        [Outlet]
        UIKit.UIButton CloseButton { get; set; }


        [Outlet]
        UIKit.UITextField SearchTextField { get; set; }


        [Outlet]
        UIKit.UITableView SuggestionsTableView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint SuggestionsTableViewConstraint { get; set; }


        [Outlet]
        UIKit.UILabel TitleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BottomConstraint != null) {
                BottomConstraint.Dispose ();
                BottomConstraint = null;
            }

            if (CloseButton != null) {
                CloseButton.Dispose ();
                CloseButton = null;
            }

            if (SearchTextField != null) {
                SearchTextField.Dispose ();
                SearchTextField = null;
            }

            if (SuggestionsTableView != null) {
                SuggestionsTableView.Dispose ();
                SuggestionsTableView = null;
            }

            if (SuggestionsTableViewConstraint != null) {
                SuggestionsTableViewConstraint.Dispose ();
                SuggestionsTableViewConstraint = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }
        }
    }
}