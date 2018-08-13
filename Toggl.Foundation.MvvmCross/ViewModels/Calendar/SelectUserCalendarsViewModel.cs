using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using Toggl.Foundation.Interactors;
using Toggl.Foundation.MvvmCross.Collections;
using Toggl.Foundation.MvvmCross.ViewModels.Selectable;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;

namespace Toggl.Foundation.MvvmCross.ViewModels.Calendar
{
    [Preserve(AllMembers = true)]
    public sealed class SelectUserCalendarsViewModel : MvxViewModel
    {
        private readonly IInteractorFactory interactorFactory;

        public ObservableGroupedOrderedCollection<SelectableUserCalendarViewModel> Calendars { get; }
            = new ObservableGroupedOrderedCollection<SelectableUserCalendarViewModel>(
                indexKey: c => c.Id,
                orderingKey: c => c.Name,
                groupingKey: c => c.SourceName
            );

        public InputAction<SelectableUserCalendarViewModel> SelectCalendarAction { get; }

        public SelectUserCalendarsViewModel(IInteractorFactory interactorFactory)
        {
            Ensure.Argument.IsNotNull(interactorFactory, nameof(interactorFactory));

            this.interactorFactory = interactorFactory;

            SelectCalendarAction = new InputAction<SelectableUserCalendarViewModel>(selectCalendar);
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            await interactorFactory
                .GetUserCalendars()
                .Execute()
                .Select(calendars => calendars.Select(toSelectable))
                .Do(calendars => calendars
                    .ForEach(calendar => Calendars.InsertItem(calendar)));
        }

        private SelectableUserCalendarViewModel toSelectable(UserCalendar calendar)
            => new SelectableUserCalendarViewModel(calendar, false);

        IObservable<Unit> selectCalendar(SelectableUserCalendarViewModel calendar)
        {
            Console.WriteLine($"Selected {calendar.Name}");
            return Observable.Return(Unit.Default);
        }
    }
}
