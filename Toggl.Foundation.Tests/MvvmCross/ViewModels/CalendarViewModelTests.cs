﻿using System;
using System.Reactive;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Toggl.Foundation.MvvmCross.ViewModels.Calendar;
using Toggl.Foundation.Calendar;
using Toggl.Foundation.Interactors;
using Toggl.Foundation.MvvmCross.ViewModels;
using Toggl.Foundation.Tests.Generators;
using Toggl.Foundation.Tests.Mocks;
using Xunit;
using ITimeEntryPrototype = Toggl.Foundation.Models.ITimeEntryPrototype;
using Toggl.Foundation.Analytics;

namespace Toggl.Foundation.Tests.MvvmCross.ViewModels
{
    public sealed class CalendarViewModelTests
    {
        public abstract class CalendarViewModelTest : BaseViewModelTests<CalendarViewModel>
        {
            protected const long TimeEntryId = 10;
            protected const long DefaultWorkspaceId = 1;

            protected static DateTimeOffset Now { get; } = new DateTimeOffset(2018, 8, 10, 12, 0, 0, TimeSpan.Zero);

            protected IInteractor<IObservable<IEnumerable<CalendarItem>>> CalendarInteractor { get; }

            protected CalendarViewModelTest()
            {
                CalendarInteractor = Substitute.For<IInteractor<IObservable<IEnumerable<CalendarItem>>>>();

                var workspace = new MockWorkspace { Id = DefaultWorkspaceId };
                var timeEntry = new MockTimeEntry { Id = TimeEntryId };

                TimeService.CurrentDateTime.Returns(Now);

                InteractorFactory
                    .GetCalendarItemsForDate(Arg.Any<DateTime>())
                    .Returns(CalendarInteractor);

                InteractorFactory
                    .GetDefaultWorkspace()
                    .Execute()
                    .Returns(Observable.Return(workspace));

                InteractorFactory
                    .CreateTimeEntry(Arg.Any<ITimeEntryPrototype>())
                    .Execute()
                    .Returns(Observable.Return(timeEntry));
            }

            protected override CalendarViewModel CreateViewModel()
                => new CalendarViewModel(
                    DataSource,
                    TimeService,
                    AnalyticsService,
                    InteractorFactory,
                    OnboardingStorage,
                    PermissionsService,
                    NavigationService
                );
        }

        public sealed class TheConstructor : CalendarViewModelTest
        {
            [Theory, LogIfTooSlow]
            [ConstructorData]
            public void ThrowsIfAnyOfTheArgumentsIsNull(
                bool useDataSource,
                bool useTimeService,
                bool useAnalyticsService,
                bool useInteractorFactory,
                bool useOnboardingStorage,
                bool useNavigationService,
                bool usePermissionsService)
            {
                var dataSource = useDataSource ? DataSource : null;
                var timeService = useTimeService ? TimeService : null;
                var analyticsService = useAnalyticsService ? AnalyticsService : null;
                var interactorFactory = useInteractorFactory ? InteractorFactory : null;
                var onboardingStorage = useOnboardingStorage ? OnboardingStorage : null;
                var navigationService = useNavigationService ? NavigationService : null;
                var permissionsService = usePermissionsService ? PermissionsService : null;

                Action tryingToConstructWithEmptyParameters =
                    () => new CalendarViewModel(
                        dataSource,
                        timeService,
                        analyticsService,
                        interactorFactory,
                        onboardingStorage,
                        permissionsService,
                        navigationService);

                tryingToConstructWithEmptyParameters.Should().Throw<ArgumentNullException>();
            }
        }

        public sealed class TheShouldShowOnboardingProperty : CalendarViewModelTest
        {
            [Fact, LogIfTooSlow]
            public async Task ReturnsTrueIfCalendarOnboardingHasntBeenCompleted()
            {
                (await ViewModel.ShouldShowOnboarding).Should().BeTrue();
            }

            [Fact, LogIfTooSlow]
            public async Task ReturnsFalseIfCalendarOnboardingHasBeenCompleted()
            {
                OnboardingStorage.CompletedCalendarOnboarding().Returns(true);
                var viewModel = CreateViewModel();

                (await viewModel.ShouldShowOnboarding).Should().BeFalse();
            }
        }

        public sealed class TheGetStartedAction : CalendarViewModelTest
        {
            [Fact, LogIfTooSlow]
            public async Task RequestsCalendarPermission()
            {
                await ViewModel.GetStartedAction.Execute(Unit.Default);

                await PermissionsService.Received().RequestCalendarAuthorization();
            }

            [Fact, LogIfTooSlow]
            public async Task NavigatesToTheCalendarPermissionDeniedViewModelWhenPermissionIsDenied()
            {
                PermissionsService.RequestCalendarAuthorization().Returns(Observable.Return(false));

                await ViewModel.GetStartedAction.Execute(Unit.Default);

                await NavigationService.Received().Navigate<CalendarPermissionDeniedViewModel>();
            }

            [Fact, LogIfTooSlow]
            public async Task TracksTheCalendarOnbardingStartedEvent()
            {
                PermissionsService.RequestCalendarAuthorization().Returns(Observable.Return(false));

                await ViewModel.GetStartedAction.Execute(Unit.Default);

                AnalyticsService.CalendarOnboardingStarted.Received().Track();
            }
        }

        public sealed class TheCalendarItemsProperty : CalendarViewModelTest
        {
            [Fact, LogIfTooSlow]
            public async Task ReturnsTheCalendarItemsForToday()
            {
                var now = new DateTimeOffset(2018, 8, 9, 12, 0, 0, TimeSpan.Zero);
                TimeService.CurrentDateTime.Returns(now);

                var items = new List<CalendarItem>
                {
                    new CalendarItem(CalendarItemSource.Calendar, now.AddMinutes(30), TimeSpan.FromMinutes(15), "Weekly meeting", CalendarIconKind.Event, "#ff0000"),
                    new CalendarItem(CalendarItemSource.TimeEntry, now.AddHours(-3), TimeSpan.FromMinutes(30), "Bug fixes", CalendarIconKind.None, "#00ff00"),
                    new CalendarItem(CalendarItemSource.Calendar, now.AddHours(2), TimeSpan.FromMinutes(30), "F**** timesheets", CalendarIconKind.Event, "#ff0000")
                };
                var interactor = Substitute.For<IInteractor<IObservable<IEnumerable<CalendarItem>>>>();
                interactor.Execute().Returns(Observable.Return(items));
                InteractorFactory.GetCalendarItemsForDate(Arg.Any<DateTime>()).Returns(interactor);

                await ViewModel.Initialize();

                ViewModel.CalendarItems[0].Should().BeEquivalentTo(items);
            }
        }

        public abstract class TheOnItemTappedAction : CalendarViewModelTest
        {
            protected abstract IAnalyticsEvent Event { get; }

            protected abstract CalendarItem CalendarItem { get; }

            [Fact, LogIfTooSlow]
            public async Task NavigatesToTheEditTimeEntryViewModelUsingTheTimeEntryId()
            {
                await ViewModel.OnItemTapped.Execute(CalendarItem);

                await NavigationService.Received().Navigate<EditTimeEntryViewModel, long, Unit>(Arg.Is(TimeEntryId));
            }

            [Fact, LogIfTooSlow]
            public async Task RefetchesTheTimeEntryItemsUsingTheInteractor()
            {
                await ViewModel.OnItemTapped.Execute(CalendarItem);

                await CalendarInteractor.Received().Execute();
            }

            [Fact, LogIfTooSlow]
            public async Task TracksTheAppropriateEventToTheAnalyticsService()
            {
                await ViewModel.OnItemTapped.Execute(CalendarItem);

                Event.Received().Track();
            }

            public sealed class WhenHandlingTimeEntryItems : TheOnItemTappedAction
            {
                protected override IAnalyticsEvent Event => AnalyticsService.EditViewOpenedFromCalendar;

                protected override CalendarItem CalendarItem { get; } = new CalendarItem(
                    CalendarItemSource.TimeEntry,
                    new DateTimeOffset(2018, 08, 10, 0, 0, 0, TimeSpan.Zero),
                    TimeSpan.FromMinutes(10),
                    "Working on something",
                    CalendarIconKind.None,
                    "#00FF00",
                    TimeEntryId
                );
            }

            public sealed class WhenHandlingCalendarItems : TheOnItemTappedAction
            {
                protected override IAnalyticsEvent Event => AnalyticsService.TimeEntryCreateFromCalendarEvent;

                protected override CalendarItem CalendarItem { get; } = new CalendarItem(
                    CalendarItemSource.Calendar,
                    new DateTimeOffset(2018, 08, 10, 0, 15, 0, TimeSpan.Zero),
                    TimeSpan.FromMinutes(10),
                    "Meeting with someone",
                    CalendarIconKind.Event
                );

                [Fact, LogIfTooSlow]
                public async Task CreatesATimeEntryUsingTheCalendarItemInfo()
                {
                    await ViewModel.OnItemTapped.Execute(CalendarItem);

                    await InteractorFactory
                        .CreateTimeEntry(Arg.Is<ITimeEntryPrototype>(p => p.Description == CalendarItem.Description))
                        .Received()
                        .Execute();
                }

                [Fact, LogIfTooSlow]
                public async Task CreatesATimeEntryInTheDefaultWorkspace()
                {
                    await ViewModel.OnItemTapped.Execute(CalendarItem);

                    await InteractorFactory
                        .CreateTimeEntry(Arg.Is<ITimeEntryPrototype>(p => p.WorkspaceId == DefaultWorkspaceId))
                        .Received()
                        .Execute();
                }
            }
        }

        public sealed class TheOnDurationSelectedAction : CalendarViewModelTest
        {
            [Fact, LogIfTooSlow]
            public async Task CreatesATimeEntryWithTheSelectedStartDate()
            {
                var now = DateTimeOffset.UtcNow;
                var duration = TimeSpan.FromMinutes(30);
                var tuple = (now, duration);

                await ViewModel.OnDurationSelected.Execute(tuple);

                await InteractorFactory
                    .CreateTimeEntry(Arg.Is<ITimeEntryPrototype>(p => p.StartTime == now))
                    .Received()
                    .Execute();
            }

            [Fact, LogIfTooSlow]
            public async Task CreatesATimeEntryWithTheSelectedDuration()
            {
                var now = DateTimeOffset.UtcNow;
                var duration = TimeSpan.FromMinutes(30);
                var tuple = (now, duration);

                await ViewModel.OnDurationSelected.Execute(tuple);

                await InteractorFactory
                    .CreateTimeEntry(Arg.Is<ITimeEntryPrototype>(p => p.Duration == duration))
                    .Received()
                    .Execute();
            }

            [Fact, LogIfTooSlow]
            public async Task CreatesATimeEntryInTheDefaultWorkspace()
            {
                var now = DateTimeOffset.UtcNow;
                var duration = TimeSpan.FromMinutes(30);
                var tuple = (now, duration);

                await ViewModel.OnDurationSelected.Execute(tuple);

                await InteractorFactory
                    .CreateTimeEntry(Arg.Is<ITimeEntryPrototype>(p => p.WorkspaceId == DefaultWorkspaceId))
                    .Received()
                    .Execute();
            }

            [Fact, LogIfTooSlow]
            public async Task RefetchesTheTimeEntryItemsUsingTheInteractor()
            {
                var now = DateTimeOffset.UtcNow;
                var duration = TimeSpan.FromMinutes(30);
                var tuple = (now, duration);

                await ViewModel.OnDurationSelected.Execute(tuple);

                await CalendarInteractor.Received().Execute();
            }

            [Fact, LogIfTooSlow]
            public async Task TracksTheTimeEntryCreatedFromCalendarTappingEventToTheAnalyticsService()
            {
                var now = DateTimeOffset.UtcNow;
                var duration = TimeSpan.FromMinutes(30);
                var tuple = (now, duration);

                await ViewModel.OnDurationSelected.Execute(tuple);

                AnalyticsService.TimeEntryCreatedFromCalendarTapping.Received().Track();
            }
        }
    }
}
