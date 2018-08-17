﻿using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Toggl.Foundation.MvvmCross.ViewModels.Calendar;
using Toggl.Foundation.MvvmCross.ViewModels.Selectable;
using Toggl.Foundation.Tests.Generators;
using Toggl.Multivac;
using Xunit;
using static Toggl.Multivac.Extensions.FunctionalExtensions;

namespace Toggl.Foundation.Tests.MvvmCross.ViewModels
{
    public sealed class SelectUserCalendarsViewModelTests
    {
        public abstract class SelectUserCalendarsViewModelTest : BaseViewModelTests<SelectUserCalendarsViewModel>
        {
            protected override SelectUserCalendarsViewModel CreateViewModel()
                => new SelectUserCalendarsViewModel(InteractorFactory, SchedulerProvider, NavigationService);
        }

        public sealed class TheConstructor : SelectUserCalendarsViewModelTest
        {
            [Theory, LogIfTooSlow]
            [ConstructorData]
            public void ThrowsIfAnyOfTheArgumentsIsNull(
                bool useInteractorFactory,
                bool useSchedulerProvider,
                bool useNavigationService)
            {
                Action tryingToConstructWithEmptyParameters =
                    () => new SelectUserCalendarsViewModel(
                        useInteractorFactory ? InteractorFactory : null,
                        useSchedulerProvider ? SchedulerProvider : null,
                        useNavigationService ? NavigationService : null
                    );

                tryingToConstructWithEmptyParameters
                    .Should().Throw<ArgumentNullException>();
            }
        }

        public sealed class TheInitializeMethod : SelectUserCalendarsViewModelTest
        {
            [Fact, LogIfTooSlow]
            public async Task FillsTheCalendarList()
            {
                var userCalendarsObservable = Enumerable
                    .Range(0, 9)
                    .Select(id => new UserCalendar(
                        id.ToString(),
                        $"Calendar #{id}",
                        $"Source #{id % 3}",
                        false))
                    .Apply(Observable.Return);
                InteractorFactory.GetUserCalendars().Execute().Returns(userCalendarsObservable);

                await ViewModel.Initialize();

                ViewModel.Calendars.Should().HaveCount(3);
                ViewModel.Calendars.ForEach(group => group.Should().HaveCount(3));
            }
        }

        public sealed class TheDoneAction : SelectUserCalendarsViewModelTest
        {
            [Fact, LogIfTooSlow]
            public async Task ClosesTheViewModelAndReturnsSelectedCalendarIds()
            {
                var userCalendars = Enumerable
                    .Range(0, 9)
                    .Select(id => new UserCalendar(
                        id.ToString(),
                        $"Calendar #{id}",
                        $"Source #{id % 3}",
                        false));
                InteractorFactory
                    .GetUserCalendars()
                    .Execute()
                    .Returns(Observable.Return(userCalendars));
                await ViewModel.Initialize();
                var selectedIds = new[] { "0", "2", "4", "7" };
                userCalendars
                    .Where(calendar => selectedIds.Contains(calendar.Id))
                    .Select(calendar => new SelectableUserCalendarViewModel(calendar, false))
                    .Do(calendar => ViewModel.SelectCalendarAction.Execute(calendar).Wait());
                
                await ViewModel.DoneAction.Execute(Unit.Default);

                await NavigationService.Received().Close(ViewModel, Arg.Is<string[]>(ids => ids.SequenceEqual(selectedIds)));
            }
        }

        public sealed class TheDoneActionEnabledProperty : SelectUserCalendarsViewModelTest
        {
            [Fact, LogIfTooSlow]
            public void StartsWithFalse()
            {
                var observer = Substitute.For<IObserver<bool>>();

                ViewModel.DoneAction.Enabled.Subscribe(observer);
                SchedulerProvider.TestScheduler.AdvanceBy(1);

                observer.Received().OnNext(false);
            }

            [Fact, LogIfTooSlow]
            public async Task EmitsTrueAfterOneCalendarHasBeenSelected()
            {
                var observer = Substitute.For<IObserver<bool>>();
                ViewModel.DoneAction.Enabled.Subscribe(observer);
                var selectableUserCalendar = new SelectableUserCalendarViewModel(
                    new UserCalendar(),
                    false
                );

                await ViewModel.SelectCalendarAction.Execute(selectableUserCalendar);
                SchedulerProvider.TestScheduler.AdvanceBy(2);

                Received.InOrder(() =>
                {
                    observer.OnNext(false);
                    observer.OnNext(true);
                });
            }

            [Fact, LogIfTooSlow]
            public void DoesNotEmitAnythingWhenSelectingAdditionalCalendars()
            {
                var observer = Substitute.For<IObserver<bool>>();
                ViewModel.DoneAction.Enabled.Subscribe(observer);
                var selectedableUserCalendars = Enumerable
                    .Range(0, 10)
                    .Select(id =>
                    {
                        var userCalendar = new UserCalendar(id.ToString(), id.ToString(), "Doenst matter");
                        return new SelectableUserCalendarViewModel(userCalendar, false);
                    });

                selectedableUserCalendars
                    .ForEach(calendar => ViewModel.SelectCalendarAction.Execute(calendar).Wait());
                SchedulerProvider.TestScheduler.AdvanceBy(2);

                Received.InOrder(() =>
                {
                    observer.OnNext(false);
                    observer.OnNext(true);
                });
            }

            [Fact, LogIfTooSlow]
            public void EmitsFalseAfterAllTheCalendarsHaveBeenDeselected()
            {
                var observer = Substitute.For<IObserver<bool>>();
                ViewModel.DoneAction.Enabled.Subscribe(observer);
                var selectedableUserCalendars = Enumerable
                    .Range(0, 10)
                    .Select(id =>
                    {
                        var userCalendar = new UserCalendar(id.ToString(), id.ToString(), "Doenst matter");
                        return new SelectableUserCalendarViewModel(userCalendar, false);
                    });

                //Select all the calendars
                selectedableUserCalendars
                    .ForEach(calendar => ViewModel.SelectCalendarAction.Execute(calendar).Wait());
                //Deselect all the calendars
                selectedableUserCalendars
                    .ForEach(calendar => ViewModel.SelectCalendarAction.Execute(calendar).Wait());
                SchedulerProvider.TestScheduler.AdvanceBy(3);

                Received.InOrder(() =>
                {
                    observer.OnNext(false);
                    observer.OnNext(true);
                    observer.OnNext(false);
                });
            }
        }
    }
}
