// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Toggl.Daneel.ViewControllers.Settings
{
    [Register ("SendFeedbackViewController")]
    partial class SendFeedbackViewController
    {
        [Outlet]
        UIKit.UIButton CloseButton { get; set; }


        [Outlet]
        UIKit.UIView ErrorView { get; set; }


        [Outlet]
        UIKit.UITextView FeedbackPlaceholderTextView { get; set; }


        [Outlet]
        UIKit.UITextView FeedbackTextView { get; set; }


        [Outlet]
        Toggl.Daneel.Views.ActivityIndicatorView IndicatorView { get; set; }


        [Outlet]
        UIKit.UIButton SendButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CloseButton != null) {
                CloseButton.Dispose ();
                CloseButton = null;
            }

            if (ErrorView != null) {
                ErrorView.Dispose ();
                ErrorView = null;
            }

            if (FeedbackPlaceholderTextView != null) {
                FeedbackPlaceholderTextView.Dispose ();
                FeedbackPlaceholderTextView = null;
            }

            if (FeedbackTextView != null) {
                FeedbackTextView.Dispose ();
                FeedbackTextView = null;
            }

            if (IndicatorView != null) {
                IndicatorView.Dispose ();
                IndicatorView = null;
            }

            if (SendButton != null) {
                SendButton.Dispose ();
                SendButton = null;
            }
        }
    }
}