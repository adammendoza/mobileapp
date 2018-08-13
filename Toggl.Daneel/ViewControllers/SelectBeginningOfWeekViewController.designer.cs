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
    [Register ("SelectBeginningOfWeekViewController")]
    partial class SelectBeginningOfWeekViewController
    {
        [Outlet]
        UIKit.UIButton BackButton { get; set; }


        [Outlet]
        UIKit.UITableView DaysTableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (DaysTableView != null) {
                DaysTableView.Dispose ();
                DaysTableView = null;
            }
        }
    }
}