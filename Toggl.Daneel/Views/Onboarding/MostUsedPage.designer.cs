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
    [Register ("LogPage")]
    partial class MostUsedPage
    {
        [Outlet]
        UIKit.UIView FirstCell { get; set; }


        [Outlet]
        UIKit.UIView SecondCell { get; set; }


        [Outlet]
        UIKit.UIView ThirdCell { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (FirstCell != null) {
                FirstCell.Dispose ();
                FirstCell = null;
            }

            if (SecondCell != null) {
                SecondCell.Dispose ();
                SecondCell = null;
            }

            if (ThirdCell != null) {
                ThirdCell.Dispose ();
                ThirdCell = null;
            }
        }
    }
}