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
    [Register ("ForgotPasswordViewController")]
    partial class ForgotPasswordViewController
    {
        [Outlet]
        Toggl.Daneel.Views.ActivityIndicatorView ActivityIndicator { get; set; }


        [Outlet]
        UIKit.UIView DoneCard { get; set; }


        [Outlet]
        Toggl.Daneel.Views.LoginTextField EmailTextField { get; set; }


        [Outlet]
        UIKit.UILabel ErrorLabel { get; set; }


        [Outlet]
        UIKit.UIButton ResetPasswordButton { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ResetPasswordButtonBottomConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint TopConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ActivityIndicator != null) {
                ActivityIndicator.Dispose ();
                ActivityIndicator = null;
            }

            if (DoneCard != null) {
                DoneCard.Dispose ();
                DoneCard = null;
            }

            if (EmailTextField != null) {
                EmailTextField.Dispose ();
                EmailTextField = null;
            }

            if (ErrorLabel != null) {
                ErrorLabel.Dispose ();
                ErrorLabel = null;
            }

            if (ResetPasswordButton != null) {
                ResetPasswordButton.Dispose ();
                ResetPasswordButton = null;
            }

            if (ResetPasswordButtonBottomConstraint != null) {
                ResetPasswordButtonBottomConstraint.Dispose ();
                ResetPasswordButtonBottomConstraint = null;
            }

            if (TopConstraint != null) {
                TopConstraint.Dispose ();
                TopConstraint = null;
            }
        }
    }
}