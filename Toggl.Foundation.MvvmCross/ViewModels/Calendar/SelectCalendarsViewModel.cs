using System;
using System.Collections.Generic;
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
    public sealed class SelectCalendarsViewModel : MvxViewModel
    {
        private readonly IInteractorFactory interactorFactory;

        public NestableObservableCollection<UserCalendarCollection, SelectableCalendarViewModel> Calendars { get; }
            = new NestableObservableCollection<UserCalendarCollection, SelectableCalendarViewModel>();

        public InputAction<SelectableCalendarViewModel> SelectCalendarAction { get; }

        public SelectCalendarsViewModel(IInteractorFactory interactorFactory)
        {
            Ensure.Argument.IsNotNull(interactorFactory, nameof(interactorFactory));

            this.interactorFactory = interactorFactory;

            SelectCalendarAction = new InputAction<SelectableCalendarViewModel>(selectCalendar);
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            var userCalendarCollections = await interactorFactory
                .GetUserCalendars()
                .Execute()
                .Select(calendars => calendars
                        .OrderBy(calendar => calendar.Name)
                        .GroupBy(calendar => calendar.SourceName)
                        .OrderBy(grouping => grouping.Key))
                .Select(createCalendarCollections);

            Calendars.AddRange(userCalendarCollections);
        }

        private IEnumerable<UserCalendarCollection> createCalendarCollections(
            IOrderedEnumerable<IGrouping<string, UserCalendar>> groups)
            => groups.Select(toUserCalendarCollection);

        private UserCalendarCollection toUserCalendarCollection(IGrouping<string, UserCalendar> groupedCalendars)
            => new UserCalendarCollection(
                groupedCalendars.Key,
                groupedCalendars.Select(toSelectable)
            );

        private SelectableCalendarViewModel toSelectable(UserCalendar calendar)
            => new SelectableCalendarViewModel(calendar, false);

        IObservable<Unit> selectCalendar(SelectableCalendarViewModel arg)
        {
            throw new NotImplementedException();
        }
    }
}
