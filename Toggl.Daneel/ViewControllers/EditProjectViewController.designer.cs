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
    [Register ("EditProjectViewController")]
    partial class EditProjectViewController
    {
        [Outlet]
        UIKit.UILabel ClientLabel { get; set; }


        [Outlet]
        UIKit.UIButton CloseButton { get; set; }


        [Outlet]
        UIKit.UIView ColorCircleView { get; set; }


        [Outlet]
        UIKit.UIButton DoneButton { get; set; }


        [Outlet]
        UIKit.UITextField NameTextField { get; set; }


        [Outlet]
        UIKit.UISwitch PrivateProjectSwitch { get; set; }


        [Outlet]
        UIKit.UIView PrivateProjectSwitchContainer { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint ProjectNameUsedErrorTextHeight { get; set; }


        [Outlet]
        UIKit.UIView ProjectNameUsedErrorView { get; set; }


        [Outlet]
        UIKit.UILabel TemplateLabel { get; set; }


        [Outlet]
        UIKit.UILabel TitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel WorkspaceLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ClientLabel != null) {
                ClientLabel.Dispose ();
                ClientLabel = null;
            }

            if (CloseButton != null) {
                CloseButton.Dispose ();
                CloseButton = null;
            }

            if (ColorCircleView != null) {
                ColorCircleView.Dispose ();
                ColorCircleView = null;
            }

            if (DoneButton != null) {
                DoneButton.Dispose ();
                DoneButton = null;
            }

            if (NameTextField != null) {
                NameTextField.Dispose ();
                NameTextField = null;
            }

            if (PrivateProjectSwitch != null) {
                PrivateProjectSwitch.Dispose ();
                PrivateProjectSwitch = null;
            }

            if (PrivateProjectSwitchContainer != null) {
                PrivateProjectSwitchContainer.Dispose ();
                PrivateProjectSwitchContainer = null;
            }

            if (ProjectNameUsedErrorTextHeight != null) {
                ProjectNameUsedErrorTextHeight.Dispose ();
                ProjectNameUsedErrorTextHeight = null;
            }

            if (ProjectNameUsedErrorView != null) {
                ProjectNameUsedErrorView.Dispose ();
                ProjectNameUsedErrorView = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }

            if (WorkspaceLabel != null) {
                WorkspaceLabel.Dispose ();
                WorkspaceLabel = null;
            }
        }
    }
}