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
    [Register ("TaskSuggestionViewCell")]
    partial class TaskSuggestionViewCell
    {
        [Outlet]
        Toggl.Daneel.Views.FadeView FadeView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TaskNameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (FadeView != null) {
                FadeView.Dispose ();
                FadeView = null;
            }

            if (TaskNameLabel != null) {
                TaskNameLabel.Dispose ();
                TaskNameLabel = null;
            }
        }
    }
}