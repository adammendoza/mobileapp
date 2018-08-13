// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Toggl.Daneel.Cells.Settings
{
    [Register ("LicensesViewCell")]
    partial class LicensesViewCell
    {
        [Outlet]
        UIKit.UIView GrayBackground { get; set; }


        [Outlet]
        UIKit.UILabel LicenseLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (GrayBackground != null) {
                GrayBackground.Dispose ();
                GrayBackground = null;
            }

            if (LicenseLabel != null) {
                LicenseLabel.Dispose ();
                LicenseLabel = null;
            }
        }
    }
}