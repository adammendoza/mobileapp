using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using Toggl.Daneel.Extensions;
using Toggl.Foundation.MvvmCross.ViewModels.Selectable;
using UIKit;

namespace Toggl.Daneel.Views.Calendar
{
    public sealed partial class SelectableUserCalendarViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString(nameof(SelectableUserCalendarViewCell));
        public static readonly UINib Nib;

        static SelectableUserCalendarViewCell()
        {
            Nib = UINib.FromName(nameof(SelectableUserCalendarViewCell), NSBundle.MainBundle);
        }

        protected SelectableUserCalendarViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            prepareViews();

            this.DelayBind(() =>
            {
                var bindingSet = this.CreateBindingSet<SelectableUserCalendarViewCell, SelectableCalendarViewModel>();

                bindingSet.Bind(CalendarNameLabel).To(vm => vm.Name);
                bindingSet.Bind(IsSelectedSwitch).To(vm => vm.Selected);

                bindingSet.Apply();
            });
        }

        private void prepareViews()
        {
            IsSelectedSwitch.Resize();
        }
    }
}
