﻿namespace Toggl.Foundation.MvvmCross.Collections.Changes
{
    public struct AddRowCollectionChange<T> : ICollectionChange
    {
        public SectionedIndex Index { get; }

        public T Item { get; }

        public AddRowCollectionChange(SectionedIndex index, T item)
        {
            Index = index;
            Item = item;
        }

        public override string ToString() => $"Add row: {Index} ({Item})";
    }
}
