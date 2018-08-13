// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Toggl.Daneel.Views
{
    [Register ("ColorSelectionViewCell")]
    partial class ColorSelectionViewCell
    {
        [Outlet]
        UIKit.UIView ColorCircleView { get; set; }


        [Outlet]
        UIKit.UIImageView SelectedImageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ColorCircleView != null) {
                ColorCircleView.Dispose ();
                ColorCircleView = null;
            }

            if (SelectedImageView != null) {
                SelectedImageView.Dispose ();
                SelectedImageView = null;
            }
        }
    }
}