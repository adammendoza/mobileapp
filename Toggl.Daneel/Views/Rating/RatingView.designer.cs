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
    [Register ("RatingView")]
    partial class RatingView
    {
        [Outlet]
        UIKit.UIButton CtaButton { get; set; }


        [Outlet]
        UIKit.UILabel CtaDescription { get; set; }


        [Outlet]
        UIKit.UILabel CtaTitle { get; set; }


        [Outlet]
        UIKit.UIView CtaView { get; set; }


        [Outlet]
        UIKit.UIButton DismissButton { get; set; }


        [Outlet]
        UIKit.UILabel NotReallyLabel { get; set; }


        [Outlet]
        UIKit.UIView NotReallyView { get; set; }


        [Outlet]
        UIKit.UIView QuestionView { get; set; }


        [Outlet]
        UIKit.UILabel TitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel YesLabel { get; set; }


        [Outlet]
        UIKit.UIView YesView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CtaButton != null) {
                CtaButton.Dispose ();
                CtaButton = null;
            }

            if (CtaDescription != null) {
                CtaDescription.Dispose ();
                CtaDescription = null;
            }

            if (CtaTitle != null) {
                CtaTitle.Dispose ();
                CtaTitle = null;
            }

            if (CtaView != null) {
                CtaView.Dispose ();
                CtaView = null;
            }

            if (DismissButton != null) {
                DismissButton.Dispose ();
                DismissButton = null;
            }

            if (NotReallyLabel != null) {
                NotReallyLabel.Dispose ();
                NotReallyLabel = null;
            }

            if (NotReallyView != null) {
                NotReallyView.Dispose ();
                NotReallyView = null;
            }

            if (QuestionView != null) {
                QuestionView.Dispose ();
                QuestionView = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }

            if (YesLabel != null) {
                YesLabel.Dispose ();
                YesLabel = null;
            }

            if (YesView != null) {
                YesView.Dispose ();
                YesView = null;
            }
        }
    }
}