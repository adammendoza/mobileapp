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
    [Register ("SelectColorViewController")]
    partial class SelectColorViewController
    {
        [Outlet]
        UIKit.UIButton CloseButton { get; set; }


        [Outlet]
        UIKit.UICollectionView ColorCollectionView { get; set; }


        [Outlet]
        Toggl.Daneel.Views.HueSaturationPickerView PickerView { get; set; }


        [Outlet]
        UIKit.UIButton SaveButton { get; set; }


        [Outlet]
        Toggl.Daneel.Views.ValueSliderView SliderBackgroundView { get; set; }


        [Outlet]
        UIKit.UISlider SliderView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CloseButton != null) {
                CloseButton.Dispose ();
                CloseButton = null;
            }

            if (ColorCollectionView != null) {
                ColorCollectionView.Dispose ();
                ColorCollectionView = null;
            }

            if (PickerView != null) {
                PickerView.Dispose ();
                PickerView = null;
            }

            if (SaveButton != null) {
                SaveButton.Dispose ();
                SaveButton = null;
            }

            if (SliderBackgroundView != null) {
                SliderBackgroundView.Dispose ();
                SliderBackgroundView = null;
            }

            if (SliderView != null) {
                SliderView.Dispose ();
                SliderView = null;
            }
        }
    }
}