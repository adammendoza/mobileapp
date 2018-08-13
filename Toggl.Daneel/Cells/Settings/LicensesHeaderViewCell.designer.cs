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
    [Register ("LicensesHeaderViewCell")]
    partial class LicensesHeaderViewCell
    {
        [Outlet]
        UIKit.UILabel HeaderLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (HeaderLabel != null) {
                HeaderLabel.Dispose ();
                HeaderLabel = null;
            }
        }
    }
}