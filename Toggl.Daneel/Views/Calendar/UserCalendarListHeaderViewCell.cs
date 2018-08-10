using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Toggl.Daneel.Views.Calendar
{
    public sealed partial class UserCalendarListHeaderViewCell : MvxTableViewHeaderFooterView
    {
        public static readonly NSString Key = new NSString(nameof(UserCalendarListHeaderViewCell));
        public static readonly UINib Nib;

        static UserCalendarListHeaderViewCell()
        {
            Nib = UINib.FromName(nameof(UserCalendarListHeaderViewCell), NSBundle.MainBundle);
        }

        protected UserCalendarListHeaderViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            this.DelayBind(() =>
            {
                var bindingSet = this.CreateBindingSet<UserCalendarListHeaderViewCell, string>();

                bindingSet.Bind(TitleLabel).To(vm => vm);

                bindingSet.Apply();
            });
        }
    }
}
