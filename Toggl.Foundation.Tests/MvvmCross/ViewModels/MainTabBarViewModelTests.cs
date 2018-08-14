﻿using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Toggl.Foundation.MvvmCross.ViewModels;
using Toggl.Foundation.Tests.Generators;
using Xunit;

namespace Toggl.Foundation.Tests.MvvmCross.ViewModels
{
    public class MainTabViewModelTests
    {
        public abstract class MainTabViewModelTest : BaseViewModelTests<MainTabBarViewModel>
        {
            protected override MainTabBarViewModel CreateViewModel()
                => new MainTabBarViewModel(
                    TimeService,
                    DataSource,
                    DialogService,
                    RatingService,
                    UserPreferences,
                    AnalyticsService,
                    InteractorFactory,
                    OnboardingStorage,
                    SchedulerProvider,
                    PermissionsService,
                    NavigationService,
                    RemoteConfigService,
                    SuggestionProviderContainer
                );
        }

        public sealed class TheConstructor : MainTabViewModelTest
        {
            [Theory, LogIfTooSlow]
            [ConstructorData]
            public void ThrowsIfAnyOfTheArgumentsIsNull(
                    bool useTimeService,
                    bool useDataSource,
                    bool useDialogService,
                    bool useRatingService,
                    bool useUserPreferences,
                    bool useAnalyticsService,
                    bool useInteractorFactory,
                    bool useOnboardingStorage,
                    bool useSchedulerProvider,
                    bool usePermissionsService,
                    bool useNavigationService,
                    bool useRemoteConfigService,
                    bool useSuggestionProviderContainer)
            {
                var timeService = useTimeService ? TimeService : null;
                var dataSource = useDataSource ? DataSource : null;
                var dialogService = useDialogService ? DialogService : null;
                var ratingService = useRatingService ? RatingService : null;
                var userPreferences = useUserPreferences ? UserPreferences : null;
                var analyticsService = useAnalyticsService ? AnalyticsService : null;
                var interactorFactory = useInteractorFactory ? InteractorFactory : null;
                var onboardingStorage = useOnboardingStorage ? OnboardingStorage : null;
                var schedulerProvider = useSchedulerProvider ? SchedulerProvider : null;
                var permissionsService = usePermissionsService ? PermissionsService : null;
                var navigationService = useNavigationService ? NavigationService : null;
                var remoteConfigService = useRemoteConfigService ? RemoteConfigService : null;
                var suggestionProviderContainer = useSuggestionProviderContainer ? SuggestionProviderContainer : null;

                Action tryingToConstructWithEmptyParameters =
                    () => new MainTabBarViewModel(
                        timeService,
                        dataSource,
                        dialogService,
                        ratingService,
                        userPreferences,
                        analyticsService,
                        interactorFactory,
                        onboardingStorage,
                        schedulerProvider,
                        permissionsService,
                        navigationService,
                        remoteConfigService,
                        suggestionProviderContainer
                    );

                tryingToConstructWithEmptyParameters
                    .Should().Throw<ArgumentNullException>();
            }
        }

        public sealed class TheTabsProperty : MainTabViewModelTest
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public async Task ShouldConditionallyIncludeTheCalendarViewModelBasedOntheRemoteConfigService(bool enabled)
            {
                var expectedTabCount = 2 + (enabled ? 1 : 0);
                RemoteConfigService
                    .IsCalendarFeatureEnabled
                    .Returns(Observable.Return(enabled));

                await ViewModel.Initialize();

                ViewModel.Tabs.Count().Should().Be(expectedTabCount);
            }
        }
    }
}
