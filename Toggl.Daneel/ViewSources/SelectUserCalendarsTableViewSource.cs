﻿using System;
using System.Linq;
using Foundation;
using Toggl.Daneel.Cells.Calendar;
using Toggl.Foundation.MvvmCross.Collections;
using Toggl.Foundation.MvvmCross.ViewModels.Selectable;
using UIKit;

namespace Toggl.Daneel.ViewSources
{
    public sealed class SelectUserCalendarsTableViewSource
        : ReactiveSectionedListTableViewSource<SelectableUserCalendarViewModel, SelectableUserCalendarViewCell>
    {
        private const int rowHeight = 48;
        private const int headerHeight = 48;

        private const string cellIdentifier = nameof(SelectableUserCalendarViewCell);
        private const string headerIdentifier = nameof(UserCalendarListHeaderViewCell);

        public SelectUserCalendarsTableViewSource(
            UITableView tableView,
            ObservableGroupedOrderedCollection<SelectableUserCalendarViewModel> items
        )
            : base(items, cellIdentifier)
        {
            tableView.RegisterNibForCellReuse(SelectableUserCalendarViewCell.Nib, cellIdentifier);
            tableView.RegisterNibForHeaderFooterViewReuse(UserCalendarListHeaderViewCell.Nib, headerIdentifier);
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
            => headerHeight;

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            => rowHeight;

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var header = (UserCalendarListHeaderViewCell)tableView.DequeueReusableHeaderFooterView(headerIdentifier);
            header.Item = collection[(int)section].First().SourceName;
            return header;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            base.RowSelected(tableView, indexPath);

            var cell = (SelectableUserCalendarViewCell)tableView.CellAt(indexPath);
            cell.Item = cell.Item.InvertSelected();

            tableView.DeselectRow(indexPath, true);
        }
    }
}
