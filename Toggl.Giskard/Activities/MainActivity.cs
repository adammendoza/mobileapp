using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Toggl.Foundation.MvvmCross.ViewModels;
using Toggl.Giskard.Extensions;
using Toggl.Multivac.Extensions;
using static Toggl.Foundation.Sync.SyncProgress;
using static Toggl.Giskard.Extensions.CircularRevealAnimation.AnimationType;
using FoundationResources = Toggl.Foundation.Resources;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using System.Reactive.Linq;
using System.Threading;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Toggl.Foundation.Sync;
using Toggl.Giskard.Adapters;
using Toggl.Giskard.ViewHelpers;

namespace Toggl.Giskard.Activities
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme",
              ScreenOrientation = ScreenOrientation.Portrait,
              ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public sealed partial class MainActivity : MvxAppCompatActivity<MainViewModel>, IReactiveBindingHolder
    {
        private const int snackbarDuration = 5000;

        public CompositeDisposable DisposeBag { get; } = new CompositeDisposable();

        protected override void OnCreate(Bundle bundle)
        {
            this.ChangeStatusBarColor(Color.ParseColor("#2C2C2C"));

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainActivity);
            OverridePendingTransition(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);

            InitializeViews();

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.Toolbar));
            SupportActionBar.SetDisplayShowHomeEnabled(false);
            SupportActionBar.SetDisplayShowTitleEnabled(false);

            runningEntryCardFrame.Visibility = ViewStates.Invisible;

            ViewModel.IsTimeEntryRunning
                .Subscribe(onTimeEntryCardVisibilityChanged)
                .DisposedBy(DisposeBag);

            ViewModel.SyncProgressState
                .Subscribe(onSyncChanged)
                .DisposedBy(DisposeBag);

            var mainAdapter = new MainRecyclerAdapter(ViewModel.TimeEntries)
            {
                SuggestionsViewModel = ViewModel.SuggestionsViewModel
            };

            mainAdapter.TimeEntryTaps
                .Subscribe(ViewModel.SelectTimeEntry.Inputs)
                .DisposedBy(DisposeBag);

            mainAdapter.ContinueTimeEntrySubject
                .Subscribe(ViewModel.ContinueTimeEntry.Inputs)
                .DisposedBy(DisposeBag);

            mainAdapter.DeleteTimeEntrySubject
                .Subscribe(ViewModel.DeleteTimeEntry.Inputs)
                .DisposedBy(DisposeBag);

            var layoutManager = new LinearLayoutManager(this);
            layoutManager.ItemPrefetchEnabled = true;
            layoutManager.InitialPrefetchItemCount = 4;
            mainRecyclerView.SetLayoutManager(layoutManager);
            mainRecyclerView.SetAdapter(mainAdapter);
            ViewModel
                .TimeEntries
                .CollectionChanges
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(mainAdapter.UpdateChanges)
                .DisposedBy(DisposeBag);

            ViewModel.IsTimeEntryRunning
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(updateRecyclerViewPadding)
                .DisposedBy(DisposeBag);

            var callback = new MainRecyclerViewTouchCallback(mainAdapter);
            var itemTouchHelper = new ItemTouchHelper(callback);
            itemTouchHelper.AttachToRecyclerView(mainRecyclerView);

            setupStartTimeEntryOnboardingStep();
            setupStopTimeEntryOnboardingStep();
            setupTapToEditOnboardingStep();
        }

        private void updateRecyclerViewPadding(bool isRunning)
        {
            var newPadding = isRunning ? 104.DpToPixels(this) : 70.DpToPixels(this);
            mainRecyclerView.Post(() =>
            {
                mainRecyclerView.SetPadding(0, 0, 0, newPadding);
            });
        }

        private void onSyncChanged(SyncProgress syncProgress)
        {
            switch (syncProgress)
            {
                case Failed:
                case Unknown:
                case OfflineModeDetected:

                    var errorMessage = syncProgress == OfflineModeDetected
                                     ? FoundationResources.Offline
                                     : FoundationResources.SyncFailed;

                    var snackbar = Snackbar.Make(coordinatorLayout, errorMessage, Snackbar.LengthLong)
                        .SetAction(FoundationResources.TapToRetry, onRetryTapped);
                    snackbar.SetDuration(snackbarDuration);
                    snackbar.Show();
                    break;
            }

            void onRetryTapped(View view)
            {
                ViewModel.RefreshAction.Execute();
            }
        }

        private async void onTimeEntryCardVisibilityChanged(bool visible)
        {
            if (runningEntryCardFrame == null) return;

            var isCardVisible = runningEntryCardFrame.Visibility == ViewStates.Visible;
            if (isCardVisible == visible) return;

            var fabListener = new FabAsyncHideListener();
            var radialAnimation =
                runningEntryCardFrame
                    .AnimateWithCircularReveal()
                    .SetDuration(TimeSpan.FromSeconds(0.5))
                    .SetBehaviour((x, y, w, h) => (x, y + h, 0, w))
                    .SetType(() => visible ? Appear : Disappear);

            if (visible)
            {
                playButton.Hide(fabListener);
                await fabListener.HideAsync;

                radialAnimation
                    .OnAnimationEnd(_ => stopButton.Show())
                    .Start();
            }
            else
            {
                stopButton.Hide(fabListener);
                await fabListener.HideAsync;

                radialAnimation
                    .OnAnimationEnd(_ => playButton.Show())
                    .Start();
            }
        }

        private sealed class FabAsyncHideListener : FloatingActionButton.OnVisibilityChangedListener
        {
            private readonly TaskCompletionSource<object> hideTaskCompletionSource = new TaskCompletionSource<object>();

            public Task HideAsync => hideTaskCompletionSource.Task;

            public override void OnHidden(FloatingActionButton fab)
            {
                base.OnHidden(fab);
                hideTaskCompletionSource.SetResult(null);
            }
        }
    }
}
