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
    [Register ("AboutViewController")]
    partial class AboutViewController
    {
        [Outlet]
        UIKit.UIView LicensesView { get; set; }

        [Outlet]
        UIKit.UIView PrivacyPolicyView { get; set; }

        [Outlet]
        UIKit.UIView TermsOfServiceView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint TopConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LicensesView != null) {
                LicensesView.Dispose ();
                LicensesView = null;
            }

            if (PrivacyPolicyView != null) {
                PrivacyPolicyView.Dispose ();
                PrivacyPolicyView = null;
            }

            if (TermsOfServiceView != null) {
                TermsOfServiceView.Dispose ();
                TermsOfServiceView = null;
            }

            if (TopConstraint != null) {
                TopConstraint.Dispose ();
                TopConstraint = null;
            }
        }
    }
}