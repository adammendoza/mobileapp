// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Toggl.Daneel.ViewControllers
{
    [Register ("OnboardingView")]
    partial class OnboardingViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView BackgroundImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel FirstPageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Next { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPageControl PageControl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView PhoneContents { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView PhoneFrame { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Previous { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SecondPageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Skip { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ThirdPageLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackgroundImage != null) {
                BackgroundImage.Dispose ();
                BackgroundImage = null;
            }

            if (FirstPageLabel != null) {
                FirstPageLabel.Dispose ();
                FirstPageLabel = null;
            }

            if (Next != null) {
                Next.Dispose ();
                Next = null;
            }

            if (PageControl != null) {
                PageControl.Dispose ();
                PageControl = null;
            }

            if (PhoneContents != null) {
                PhoneContents.Dispose ();
                PhoneContents = null;
            }

            if (PhoneFrame != null) {
                PhoneFrame.Dispose ();
                PhoneFrame = null;
            }

            if (Previous != null) {
                Previous.Dispose ();
                Previous = null;
            }

            if (ScrollView != null) {
                ScrollView.Dispose ();
                ScrollView = null;
            }

            if (SecondPageLabel != null) {
                SecondPageLabel.Dispose ();
                SecondPageLabel = null;
            }

            if (Skip != null) {
                Skip.Dispose ();
                Skip = null;
            }

            if (ThirdPageLabel != null) {
                ThirdPageLabel.Dispose ();
                ThirdPageLabel = null;
            }
        }
    }
}