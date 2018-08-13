// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Toggl.Daneel.ViewControllers
{
    [Register ("OutdatedAppViewController")]
    partial class OutdatedAppViewController
    {
        [Outlet]
        UIKit.UIButton UpdateButton { get; set; }


        [Outlet]
        UIKit.UIButton WebsiteButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (UpdateButton != null) {
                UpdateButton.Dispose ();
                UpdateButton = null;
            }

            if (WebsiteButton != null) {
                WebsiteButton.Dispose ();
                WebsiteButton = null;
            }
        }
    }
}