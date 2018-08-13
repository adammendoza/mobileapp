// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Toggl.Daneel.Views.Settings
{
    [Register ("DateFormatViewCell")]
    partial class DateFormatViewCell
    {
        [Outlet]
        UIKit.UILabel DateFormatLabel { get; set; }


        [Outlet]
        UIKit.UIImageView SelectedImageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DateFormatLabel != null) {
                DateFormatLabel.Dispose ();
                DateFormatLabel = null;
            }

            if (SelectedImageView != null) {
                SelectedImageView.Dispose ();
                SelectedImageView = null;
            }
        }
    }
}