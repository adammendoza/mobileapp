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
using FsCheck.Xunit;
using FsCheck;
using System.Linq;

namespace Toggl.Foundation.Tests.MvvmCross.ViewModels
{
    public sealed class CalendarViewModelTests
    {
        public abstract class CalendarViewModelTest : BaseViewModelTests<CalendarViewModel>
        {
            protected override CalendarViewModel CreateViewModel()
                => new CalendarViewModel(
                    DataSource,
                    TimeService,
                    InteractorFactory,
                    OnboardingStorage,
                    PermissionsService,
                    NavigationService);
        }

        public sealed class TheConstructor : CalendarViewModelTest
        {
            [Theory, LogIfTooSlow]
            [ConstructorData]
            public void ThrowsIfAnyOfTheArgumentsIsNull(
                bool useDataSource,
                bool useTimeService,
                bool useInteractorFactory,
                bool useOnboardingStorage,
                bool useNavigationService,
                bool usePermissionsService)
            {
                var dataSource = useDataSource ? DataSource : null;
                var timeService = useTimeService ? TimeService : null;
                var interactorFactory = useInteractorFactory ? InteractorFactory : null;
                var onboardingStorage = useOnboardingStorage ? OnboardingStorage : null;
                var navigationService = useNavigationService ? NavigationService : null;
                var permissionsService = usePermissionsService ? PermissionsService : null;

                Action tryingToConstructWithEmptyParameters =
                    () => new CalendarViewModel(
                        dataSource,
                        timeService,
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
                OnboardingStorage.CompletedCalendarOnboarding().Returns(false);
                var viewModel = CreateViewModel();

                (await viewModel.ShouldShowOnboarding.FirstAsync()).Should().BeTrue();
            }

            [Fact, LogIfTooSlow]
            public async Task ReturnsFalseIfCalendarOnboardingHasBeenCompleted()
            {
                OnboardingStorage.CompletedCalendarOnboarding().Returns(true);
                var viewModel = CreateViewModel();

                (await viewModel.ShouldShowOnboarding.FirstAsync()).Should().BeFalse();
            }

            [Fact, LogIfTooSlow]
            public async Task EnmitsFalseWhenUserGrantsCalendarAccess()
            {
                OnboardingStorage.CompletedCalendarOnboarding().Returns(false);
                var observer = Substitute.For<IObserver<bool>>();
                ViewModel.ShouldShowOnboarding.Subscribe(observer);
                PermissionsService.RequestCalendarAuthorization().Returns(Observable.Return(true));
                NavigationService.Navigate<SelectUserCalendarsViewModel, string[]>().Returns(new string[0]);

                await ViewModel.GetStartedAction.Execute(Unit.Default);

                Received.InOrder(() =>
                {
                    observer.OnNext(true);
                    observer.OnNext(false);
                });
            }

            [Fact, LogIfTooSlow]
            public async Task EnmitsFalseWhenUserWantsToContinueWithoutCalendarAccess()
            {
                OnboardingStorage.CompletedCalendarOnboarding().Returns(false);
                var observer = Substitute.For<IObserver<bool>>();
                ViewModel.ShouldShowOnboarding.Subscribe(observer);
                PermissionsService.RequestCalendarAuthorization().Returns(Observable.Return(false));
                NavigationService.Navigate<CalendarPermissionDeniedViewModel, bool>().Returns(true);

                await ViewModel.GetStartedAction.Execute(Unit.Default);

                Received.InOrder(() =>
                {
                    observer.OnNext(true);
                    observer.OnNext(false);
                });
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

                await NavigationService.Received().Navigate<CalendarPermissionDeniedViewModel, bool>();
            }

            [Fact, LogIfTooSlow]
            public async Task NavigatesToTheSelectUserCalendarsViewModel()
            {
                PermissionsService.RequestCalendarAuthorization().Returns(Observable.Return(true));

                await ViewModel.GetStartedAction.Execute(Unit.Default);

                await NavigationService.Received().Navigate<SelectUserCalendarsViewModel, string[]>();
            }

            [Property]
            public void SetsTheEnabledCalendars(NonEmptyString[] nonEmptyStrings)
            {
                if (nonEmptyStrings == null) return;
                var calendarIds = nonEmptyStrings.Select(str => str.Get).ToArray();
                NavigationService.Navigate<SelectUserCalendarsViewModel, string[]>().Returns(calendarIds);
                PermissionsService.RequestCalendarAuthorization().Returns(Observable.Return(true));

                ViewModel.GetStartedAction.Execute(Unit.Default).Wait();

                InteractorFactory.Received().SetEnabledCalendars(calendarIds).Execute();
            }

            [Fact, LogIfTooSlow]
            public async Task SetsCalendarOnboardingAsCompletedIfUserGrantsAccess()
            {
                PermissionsService.RequestCalendarAuthorization().Returns(Observable.Return(true));
                NavigationService.Navigate<SelectUserCalendarsViewModel, string[]>().Returns(new string[0]);

                await ViewModel.GetStartedAction.Execute(Unit.Default);

                OnboardingStorage.Received().SetCompletedCalendarOnboarding();
            }

            [Fact, LogIfTooSlow]
            public async Task SetsCalendarOnboardingAsCompletedIfUserWantsToContinueWithoutGivingPermission()
            {
                PermissionsService.RequestCalendarAuthorization().Returns(Observable.Return(false));
                NavigationService.Navigate<CalendarPermissionDeniedViewModel, bool>().Returns(true);

                await ViewModel.GetStartedAction.Execute(Unit.Default);

                OnboardingStorage.Received().SetCompletedCalendarOnboarding();
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
                    new CalendarItem(CalendarItemSource.Calendar, now.AddMinutes(30), TimeSpan.FromMinutes(15), "Weekly meeting", "#ff0000"),
                    new CalendarItem(CalendarItemSource.TimeEntry, now.AddHours(-3), TimeSpan.FromMinutes(30), "Bug fixes", "#00ff00"),
                    new CalendarItem(CalendarItemSource.Calendar, now.AddHours(2), TimeSpan.FromMinutes(30), "F**** timesheets", "#ff0000")
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
            protected const long TimeEntryId = 10;

            private static readonly DateTimeOffset now = new DateTimeOffset(2018, 8, 10, 12, 0, 0, TimeSpan.Zero);
            private static readonly IInteractor<IObservable<IEnumerable<CalendarItem>>> interactor = Substitute.For<IInteractor<IObservable<IEnumerable<CalendarItem>>>>();

            protected abstract CalendarItem CalendarItem { get; }

            protected TheOnItemTappedAction()
            {
                TimeService.CurrentDateTime.Returns(now);

                InteractorFactory
                    .GetCalendarItemsForDate(Arg.Any<DateTime>())
                    .Returns(interactor);

            }

            [Fact]
            public async Task NavigatesToTheEditTimeEntryViewModelUsingTheTimeEntryId()
            {
                await ViewModel.OnItemTapped.Execute(CalendarItem);

                await NavigationService.Received().Navigate<EditTimeEntryViewModel, long>(Arg.Is(TimeEntryId));
            }

            [Fact]
            public async Task RefetchesTheTimeEntryItemsUsingTheInteractor()
            {
                await ViewModel.OnItemTapped.Execute(CalendarItem);

                await interactor.Received().Execute();
            }

            public sealed class WhenHandlingTimeEntryItems : TheOnItemTappedAction
            {
                protected override CalendarItem CalendarItem { get; } = new CalendarItem(
                    CalendarItemSource.TimeEntry,
                    new DateTimeOffset(2018, 08, 10, 0, 0, 0, TimeSpan.Zero),
                    TimeSpan.FromMinutes(10),
                    "Working on something",
                    "#00FF00",
                    TimeEntryId
                );
            }

            public sealed class WhenHandlingCalendarItems : TheOnItemTappedAction
            {
                private const long defaultWorkspaceId = 1;

                protected override CalendarItem CalendarItem { get; } = new CalendarItem(
                    CalendarItemSource.Calendar,
                    new DateTimeOffset(2018, 08, 10, 0, 15, 0, TimeSpan.Zero),
                    TimeSpan.FromMinutes(10),
                    "Meeting with someone"
                );

                public WhenHandlingCalendarItems()
                {
                    var workspace = new MockWorkspace { Id = defaultWorkspaceId };
                    var timeEntry = new MockTimeEntry { Id = TimeEntryId };

                    InteractorFactory
                        .GetDefaultWorkspace()
                        .Execute()
                        .Returns(Observable.Return(workspace));

                    InteractorFactory
                        .CreateTimeEntry(Arg.Any<ITimeEntryPrototype>())
                        .Execute()
                        .Returns(Observable.Return(timeEntry));
                }

                [Fact]
                public async Task CreatesATimeEntryUsingTheCalendarItemInfo()
                {
                    await ViewModel.OnItemTapped.Execute(CalendarItem);

                    await InteractorFactory
                        .CreateTimeEntry(Arg.Is<ITimeEntryPrototype>(p => p.Description == CalendarItem.Description))
                        .Received()
                        .Execute();
                }

                [Fact]
                public async Task CreatesATimeEntryInTheDefaultWorkspace()
                {
                    await ViewModel.OnItemTapped.Execute(CalendarItem);

                    await InteractorFactory
                        .CreateTimeEntry(Arg.Is<ITimeEntryPrototype>(p => p.WorkspaceId == defaultWorkspaceId))
                        .Received()
                        .Execute();
                }
            }
        }
    }
}
