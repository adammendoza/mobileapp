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
    [Register ("SelectDateFormatViewController")]
    partial class SelectDateFormatViewController
    {
        [Outlet]
        UIKit.UIButton BackButton { get; set; }


        [Outlet]
        UIKit.UITableView DateFormatsTableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (DateFormatsTableView != null) {
                DateFormatsTableView.Dispose ();
                DateFormatsTableView = null;
            }
        }
    }
}