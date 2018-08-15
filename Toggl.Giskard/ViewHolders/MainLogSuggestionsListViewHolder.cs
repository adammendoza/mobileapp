using System;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Toggl.Foundation.MvvmCross.ViewModels;
using Toggl.Foundation.Suggestions;
using Toggl.Giskard.Adapters;
using Toggl.Giskard.Extensions;
using Toggl.Giskard.ViewHelpers;
using Toggl.Multivac.Extensions;
using TogglResources = Toggl.Foundation.Resources;

namespace Toggl.Giskard.ViewHolders
{
    public class MainLogSuggestionsListViewHolder : RecyclerView.ViewHolder
    {
        private SuggestionsViewModel suggestionsViewModel;

        private TextView hintTextView;
        private TextView indicatorTextView;
        private RecyclerView suggestionsRecyclerView;
        private MainSuggestionsRecyclerAdapter mainSuggestionsRecyclerAdapter;

        private CompositeDisposable disposeBag;

        private int currentSuggestionCard = 1;

        public MainLogSuggestionsListViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public MainLogSuggestionsListViewHolder(View itemView, SuggestionsViewModel suggestionsViewModel) : base(itemView)
        {
            this.suggestionsViewModel = suggestionsViewModel;
            disposeBag = new CompositeDisposable();

            hintTextView = ItemView.FindViewById<TextView>(Resource.Id.SuggestionsHintTextView);
            indicatorTextView = ItemView.FindViewById<TextView>(Resource.Id.SuggestionsIndicatorTextView);
            suggestionsRecyclerView = ItemView.FindViewById<RecyclerView>(Resource.Id.SuggestionsRecyclerView);

            suggestionsRecyclerView.SetLayoutManager(new LinearLayoutManager(ItemView.Context, LinearLayoutManager.Horizontal, false));
            var snapMargin = 16.DpToPixels(ItemView.Context);
            var snapHelper = new SuggestionsRecyclerViewSnapHelper(snapMargin);
            snapHelper.AttachToRecyclerView(suggestionsRecyclerView);

            snapHelper
                .CurrentIndexObservable
                .Subscribe(onCurrentSuggestionIndexChanged)
                .DisposedBy(disposeBag);

            mainSuggestionsRecyclerAdapter = new MainSuggestionsRecyclerAdapter();
            mainSuggestionsRecyclerAdapter
                .SuggestionTaps
                .Subscribe(onSuggestionTapped)
                .DisposedBy(disposeBag);
            suggestionsRecyclerView.SetAdapter(mainSuggestionsRecyclerAdapter);

            var collectionChangesObservable = Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                e => this.suggestionsViewModel.Suggestions.CollectionChanged += e,
                e => this.suggestionsViewModel.Suggestions.CollectionChanged -= e);

            collectionChangesObservable
                .Select(args => this.suggestionsViewModel.Suggestions.ToImmutableList())
                .StartWith(this.suggestionsViewModel.Suggestions.ToImmutableList())
                .Subscribe(onSuggestionsCollectionChanged)
                .DisposedBy(disposeBag);

            collectionChangesObservable
                .Select(args => this.suggestionsViewModel.Suggestions.Count)
                .StartWith(this.suggestionsViewModel.Suggestions.Count)
                .DistinctUntilChanged()
                .Subscribe(onCollectionCountChanged)
                .DisposedBy(disposeBag);
        }

        public void UpdateView()
        {
            if (suggestionsViewModel.Suggestions.None())
            {
                hintTextView.Visibility = ViewStates.Gone;
                indicatorTextView.Visibility = ViewStates.Gone;
                suggestionsRecyclerView.Visibility = ViewStates.Gone;
            }
            else
            {
                hintTextView.Visibility = ViewStates.Visible;
                suggestionsRecyclerView.Visibility = ViewStates.Visible;
                updateHintText();
            }
        }

        private void onSuggestionTapped(Suggestion suggestion)
        {
            if (suggestionsViewModel == null) return;
            if (suggestionsViewModel.StartTimeEntryCommand.CanExecute())
            {
                suggestionsViewModel.StartTimeEntryCommand.Execute(suggestion);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;

            disposeBag.Dispose();
        }

        private void onSuggestionsCollectionChanged(ImmutableList<Suggestion> suggestions)
        {
            mainSuggestionsRecyclerAdapter.UpdateDataset(suggestions);
            UpdateView();
        }

        private void onCurrentSuggestionIndexChanged(int currentIndex)
        {
            currentSuggestionCard = currentIndex;
            updateHintText();
        }

        private void onCollectionCountChanged(int itemCount)
        {
            updateHintText();
        }

        private void updateHintText()
        {
            var numberOfSuggestions = suggestionsViewModel.Suggestions.Count;

            switch (numberOfSuggestions)
            {
                case 0:
                    return;

                case 1:
                    hintTextView.Text = TogglResources.WorkingOnThis;
                    indicatorTextView.Visibility = ViewStates.Gone;
                    break;

                default:
                    var indicatorText = $"{currentSuggestionCard} {TogglResources.Of.ToUpper()} {numberOfSuggestions}";
                    hintTextView.Text = TogglResources.WorkingOnThese;
                    indicatorTextView.Visibility = ViewStates.Visible;
                    indicatorTextView.Text = indicatorText;
                    break;
            }

        }
    }
}
