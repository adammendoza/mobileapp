using System.Collections.Generic;
using MvvmCross.ViewModels;
using Toggl.Foundation.MvvmCross.ViewModels.Selectable;
using Toggl.Multivac;

namespace Toggl.Foundation.MvvmCross.Collections
{
    public sealed class UserCalendarCollection
        : MvxObservableCollection<SelectableCalendarViewModel>
    {
        public string SourceTitle { get; }

        public UserCalendarCollection(string sourceTitle, IEnumerable<SelectableCalendarViewModel> items)
            : base(items)
        {
            Ensure.Argument.IsNotNull(items, nameof(items));
            Ensure.Argument.IsNotNull(sourceTitle, nameof(sourceTitle));

            SourceTitle = sourceTitle;
        }
    }
}
