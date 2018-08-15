using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Android.Support.V7.Widget;
using Android.Views;
using Toggl.Foundation.MvvmCross.Collections;

namespace Toggl.Giskard.Adapters
{
    public abstract class ReactiveSectionedRecyclerAdapter<TModel> : RecyclerView.Adapter
    {
        public const int SectionViewType = 0;
        public const int ItemViewType = 1;

        protected virtual int HeaderOffset { get; } = 0;

        protected readonly ObservableGroupedOrderedCollection<TModel> items;

        private ImmutableList<(int viewType, SectionedIndex sectionedIndex)> collectionToAdapterIndexesMap = ImmutableList<(int, SectionedIndex)>.Empty;
        private ImmutableList<int> sectionsIndexes = ImmutableList<int>.Empty;

        public ReactiveSectionedRecyclerAdapter(ObservableGroupedOrderedCollection<TModel> items)
        {
            this.items = items;
            updateSectionIndexes();
        }

        public override int ItemCount => collectionToAdapterIndexesMap.Count + HeaderOffset;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == ItemViewType) return CreateItemViewHolder(parent);
            return CreateHeaderViewHolder(parent);
        }

        public override int GetItemViewType(int position)
        {
            if (sectionsIndexes.Contains(position - HeaderOffset)) return SectionViewType;
            return ItemViewType;
        }

        public void UpdateChanges(IEnumerable<CollectionChange> changes)
        {
            var changesArray = changes.ToArray();

            if (changesArray.Length > 1)
            {
                updateSectionIndexes();
                NotifyDataSetChanged();
                return;
            }

            foreach (var change in changesArray)
            {
                switch (change.Type)
                {
                    case CollectionChangeType.Reload:
                        updateSectionIndexes();
                        NotifyDataSetChanged();
                        break;
                    case CollectionChangeType.AddRow:
                        updateSectionIndexes();
                        NotifyItemInserted(mapSectionIndexToAdapterIndex(change.Index));
                        NotifyItemChanged(sectionsIndexes[change.Index.Section] + HeaderOffset);
                        break;
                    case CollectionChangeType.AddSection:
                        updateSectionIndexes();
                        NotifyItemRangeInserted(sectionsIndexes[change.Index.Section] + HeaderOffset, 2);
                        break;
                    case CollectionChangeType.MoveRow:
                        if (change.OldIndex == null) break;
                        var oldIndex = mapSectionIndexToAdapterIndex(change.OldIndex.Value);
                        updateSectionIndexes();
                        var newIndex = mapSectionIndexToAdapterIndex(change.Index);
                        NotifyItemMoved(oldIndex, newIndex);
                        break;
                    case CollectionChangeType.RemoveRow:
                        var indexToBeRemoved = mapSectionIndexToAdapterIndex(change.Index);
                        updateSectionIndexes();
                        NotifyItemRemoved(indexToBeRemoved);
                        NotifyItemChanged(sectionsIndexes[change.Index.Section] + HeaderOffset);
                        break;
                    case CollectionChangeType.RemoveSection:
                        NotifyItemRangeRemoved(sectionsIndexes[change.Index.Section] + HeaderOffset, 2);
                        updateSectionIndexes();
                        break;
                    case CollectionChangeType.UpdateRow:
                        NotifyItemChanged(mapSectionIndexToAdapterIndex(change.Index));
                        NotifyItemChanged(sectionsIndexes[change.Index.Section] + HeaderOffset);
                        break;
                }
            }
        }

        public virtual object GetItem(int position)
        {
            var item = collectionToAdapterIndexesMap[position - HeaderOffset];
            if (item.viewType == ItemViewType)
            {
                return items[item.sectionedIndex.Section][item.sectionedIndex.Row];
            }

            return items[item.sectionedIndex.Section];
        }

        protected abstract RecyclerView.ViewHolder CreateHeaderViewHolder(ViewGroup parent);

        protected abstract RecyclerView.ViewHolder CreateItemViewHolder(ViewGroup parent);

        private int mapSectionIndexToAdapterIndex(SectionedIndex sectionedIndex)
        {
            return sectionsIndexes[sectionedIndex.Section] + sectionedIndex.Row + 1 + HeaderOffset;
        }

        private void updateSectionIndexes()
        {
            if (items.IsEmpty)
            {
                collectionToAdapterIndexesMap = ImmutableList<(int, SectionedIndex)>.Empty;
                sectionsIndexes = ImmutableList<int>.Empty;
            }
            else
            {
                var mappedIndexes = new List<(int, SectionedIndex)>();
                var newSectionsIndexes = new List<int>();
                var sectionIndex = 0;
                var sectionIndexOnCollection = 0;
                foreach (var section in items)
                {
                    newSectionsIndexes.Add(sectionIndex);
                    mappedIndexes.Add((SectionViewType, new SectionedIndex(sectionIndexOnCollection, 0)));
                    mappedIndexes.AddRange(Enumerable.Range(0, section.Count).Select(itemIndex => (ItemViewType, new SectionedIndex(sectionIndexOnCollection, itemIndex))));
                    sectionIndex += section.Count + 1;
                    sectionIndexOnCollection++;
                }

                sectionsIndexes = newSectionsIndexes.ToImmutableList();
                collectionToAdapterIndexesMap = mappedIndexes.ToImmutableList();
            }
        }
    }
}
