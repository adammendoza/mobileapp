﻿using System;
using System.Collections.Generic;

namespace Toggl.Foundation.MvvmCross.Collections
{
    public interface IGroupOrderedCollection<TItem> : IReadOnlyList<IReadOnlyList<TItem>>
    {
        bool IsEmpty { get; }

        SectionedIndex? IndexOf(TItem item);
        SectionedIndex? IndexOf(IComparable itemId);

        (SectionedIndex index, bool needsNewSection) InsertItem(TItem item);
        SectionedIndex? UpdateItem(IComparable key, TItem item);
        void ReplaceWith(IEnumerable<TItem> items);
        TItem RemoveItemAt(int section, int row);
    }
}
