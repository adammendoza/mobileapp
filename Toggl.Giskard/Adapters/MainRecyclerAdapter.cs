using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.WeakSubscription;
using Toggl.Foundation.MvvmCross.Collections;
using Toggl.Foundation.MvvmCross.ViewModels;
using Toggl.Giskard.ViewHolders;
using Toggl.Giskard.Views;

namespace Toggl.Giskard.Adapters
{
    public class MainRecyclerAdapter : ReactiveSectionedRecyclerAdapter<TimeEntryViewModel>
    {
        public const int ViewTypeSuggestion = 2;

        public IObservable<TimeEntryViewModel> TimeEntryTaps
            => timeEntryTappedSubject.AsObservable();

        public IObservable<TimeEntryViewModel> ContinueTimeEntrySubject
            => continueTimeEntrySubject.AsObservable();

        public IObservable<TimeEntryViewModel> DeleteTimeEntrySubject
            => deleteTimeEntrySubject.AsObservable();

        public SuggestionsViewModel SuggestionsViewModel { get; set; }

        private Subject<TimeEntryViewModel> timeEntryTappedSubject = new Subject<TimeEntryViewModel>();
        private Subject<TimeEntryViewModel> continueTimeEntrySubject = new Subject<TimeEntryViewModel>();
        private Subject<TimeEntryViewModel> deleteTimeEntrySubject = new Subject<TimeEntryViewModel>();

        public MainRecyclerAdapter(ObservableGroupedOrderedCollection<TimeEntryViewModel> items) : base(items)
        {
        }

        public void ContinueTimeEntry(int position)
        {
            var continuedPosition = GetItem(position);
            if (continuedPosition != null && continuedPosition is TimeEntryViewModel timeEntry)
            {
                NotifyItemChanged(position);
                continueTimeEntrySubject.OnNext(timeEntry);
            }
        }

        public void DeleteTimeEntry(int position)
        {
            var deletedPosition = GetItem(position);
            if (deletedPosition != null && deletedPosition is TimeEntryViewModel timeEntry)
            {
                deleteTimeEntrySubject.OnNext(timeEntry);
            }
        }

        protected override int HeaderOffset => 1;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            switch (holder)
            {
                case MainLogCellViewHolder mainLogCellViewHolder:
                    mainLogCellViewHolder.Item = GetItem(position) as TimeEntryViewModel;
                    break;

                case MainLogSectionViewHolder mainLogHeaderViewHolder:
                    mainLogHeaderViewHolder.Item = GetItem(position) as IReadOnlyList<TimeEntryViewModel>;
                    break;

                case MainLogSuggestionsListViewHolder suggestionsViewHolder:
                    suggestionsViewHolder.UpdateView();
                    break;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == ViewTypeSuggestion)
            {
                var suggestionsView = LayoutInflater.FromContext(parent.Context).Inflate(Resource.Layout.MainSuggestions, parent, false);
                return new MainLogSuggestionsListViewHolder(suggestionsView, SuggestionsViewModel);
            }

            return base.OnCreateViewHolder(parent, viewType);
        }

        public override int GetItemViewType(int position)
        {
            if (position == 0)
            {
                return ViewTypeSuggestion;
            }

            return base.GetItemViewType(position);
        }

        public override object GetItem(int position)
        {
            if (position == 0)
            {
                return SuggestionsViewModel;
            }
            return base.GetItem(position);
        }

        protected override RecyclerView.ViewHolder CreateHeaderViewHolder(ViewGroup parent)
        {
            return new MainLogSectionViewHolder(LayoutInflater.FromContext(parent.Context).Inflate(Resource.Layout.MainLogHeader, parent, false));
        }

        protected override RecyclerView.ViewHolder CreateItemViewHolder(ViewGroup parent)
        {
            return new MainLogCellViewHolder(LayoutInflater.FromContext(parent.Context).Inflate(Resource.Layout.MainLogCell, parent, false))
            {
                TappedSubject = timeEntryTappedSubject,
                ContinueButtonTappedSubject = continueTimeEntrySubject
            };
        }
    }
}
