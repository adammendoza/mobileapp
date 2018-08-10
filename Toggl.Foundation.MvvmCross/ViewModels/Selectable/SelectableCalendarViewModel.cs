using Toggl.Multivac;

namespace Toggl.Foundation.MvvmCross.ViewModels.Selectable
{
    public sealed class SelectableCalendarViewModel
    {
        public string Id { get; }

        public string Name { get; }

        public bool Selected { get; set; }

        public SelectableCalendarViewModel(UserCalendar calendar, bool selected)
        {
            Ensure.Argument.IsNotNull(calendar, nameof(calendar));

            Id = calendar.Id;
            Name = calendar.Name;
            Selected = selected;
        }
    }
}
