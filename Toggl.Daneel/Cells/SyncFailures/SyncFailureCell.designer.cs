// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Toggl.Daneel.Cells
{
    [Register ("SyncFailureCell")]
    partial class SyncFailureCell
    {
        [Outlet]
        UIKit.UILabel errorMessageLabel { get; set; }


        [Outlet]
        UIKit.UILabel nameLabel { get; set; }


        [Outlet]
        UIKit.UILabel syncStatusLabel { get; set; }


        [Outlet]
        UIKit.UILabel typeLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (errorMessageLabel != null) {
                errorMessageLabel.Dispose ();
                errorMessageLabel = null;
            }

            if (nameLabel != null) {
                nameLabel.Dispose ();
                nameLabel = null;
            }

            if (syncStatusLabel != null) {
                syncStatusLabel.Dispose ();
                syncStatusLabel = null;
            }

            if (typeLabel != null) {
                typeLabel.Dispose ();
                typeLabel = null;
            }
        }
    }
}