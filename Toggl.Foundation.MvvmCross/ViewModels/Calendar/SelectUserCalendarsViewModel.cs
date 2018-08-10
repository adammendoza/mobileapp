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
    public sealed class SelectUserCalendarsViewModel : MvxViewModel
    {
        private readonly IInteractorFactory interactorFactory;

        public NestableObservableCollection<UserCalendarCollection, SelectableUserCalendarViewModel> Calendars { get; }
            = new NestableObservableCollection<UserCalendarCollection, SelectableUserCalendarViewModel>();

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

            var userCalendarCollections = await interactorFactory
                .GetUserCalendars()
                .Execute()
                .Do(test)
                .Select(calendars => calendars
                        .OrderBy(calendar => calendar.Name)
                        .GroupBy(calendar => calendar.SourceName)
                        .OrderBy(grouping => grouping.Key))
                .Select(createCalendarCollections);

            Calendars.AddRange(userCalendarCollections);
        }

        private void test(IEnumerable<UserCalendar> c)
        {
            Console.WriteLine(c);
        }

        private IEnumerable<UserCalendarCollection> createCalendarCollections(
            IOrderedEnumerable<IGrouping<string, UserCalendar>> groups)
            => groups.Select(toUserCalendarCollection);

        private UserCalendarCollection toUserCalendarCollection(IGrouping<string, UserCalendar> groupedCalendars)
            => new UserCalendarCollection(
                groupedCalendars.Key,
                groupedCalendars.Select(toSelectable)
            );

        private SelectableUserCalendarViewModel toSelectable(UserCalendar calendar)
            => new SelectableUserCalendarViewModel(calendar, false);

        IObservable<Unit> selectCalendar(SelectableUserCalendarViewModel arg)
        {
            throw new NotImplementedException();
        }
    }
}
