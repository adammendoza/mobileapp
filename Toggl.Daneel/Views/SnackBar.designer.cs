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
    [Register ("SnackBar")]
    partial class SnackBar
    {
        [Outlet]
        UIKit.UIStackView buttonsStackView { get; set; }


        [Outlet]
        UIKit.UILabel label { get; set; }


        [Outlet]
        UIKit.UIStackView stackView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (buttonsStackView != null) {
                buttonsStackView.Dispose ();
                buttonsStackView = null;
            }

            if (label != null) {
                label.Dispose ();
                label = null;
            }

            if (stackView != null) {
                stackView.Dispose ();
                stackView = null;
            }
        }
    }
}