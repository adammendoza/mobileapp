// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Toggl.Daneel.Cells.Calendar
{
    [Register ("HourSupplementaryView")]
    partial class HourSupplementaryView
    {
        [Outlet]
        UIKit.UILabel HourLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (HourLabel != null) {
                HourLabel.Dispose ();
                HourLabel = null;
            }
        }
    }
}