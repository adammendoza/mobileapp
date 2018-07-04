﻿using System;
using System.Reactive.Linq;
using Toggl.Foundation.Analytics;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Extensions;
using Toggl.Foundation.Helper;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;
using Toggl.Multivac.Models;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.DTOs;
using Toggl.Ultrawave.Exceptions;
using static Toggl.Multivac.Extensions.CommonFunctions;

namespace Toggl.Foundation.Sync.States.Pull
{
    public sealed class CreateGhostProjectsState : IPersistState
    {
        private readonly IProjectsSource dataSource;

        private readonly IAnalyticsService analyticsService;

        public StateResult<IFetchObservables> FinishedPersisting { get; } = new StateResult<IFetchObservables>();

        public StateResult<ApiException> ErrorOccured { get; } = new StateResult<ApiException>();

        public CreateGhostProjectsState(
            IProjectsSource dataSource,
            IAnalyticsService analyticsService)
        {
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
            Ensure.Argument.IsNotNull(analyticsService, nameof(analyticsService));

            this.dataSource = dataSource;
            this.analyticsService = analyticsService;
        }

        public IObservable<ITransition> Start(IFetchObservables fetch)
            => fetch.GetList<ITimeEntry>()
                .SingleAsync()
                .SelectMany(Identity)
                .Distinct(timeEntry => timeEntry.ProjectId)
                .WhereAsync(needsGhostProject)
                .SelectMany(createGhostProject)
                .Count()
                .Track(analyticsService.ProjectGhostsCreated)
                .Select(FinishedPersisting.Transition(fetch))
                .OnErrorReturnResult(ErrorOccured);

        private IObservable<bool> needsGhostProject(ITimeEntry timeEntry)
            => timeEntry.ProjectId.HasValue
                ? dataSource.GetAll(project => project.Id == timeEntry.ProjectId.Value)
                    .SingleAsync()
                    .Select(projects => projects.None())
                : Observable.Return(false);

        private IObservable<IThreadSafeProject> createGhostProject(ITimeEntry timeEntry)
        {
            var ghost = new ProjectDto(
                id: timeEntry.ProjectId.Value,
                workspaceId: timeEntry.WorkspaceId,
                name: Resources.InaccessibleProject,
                active: false,
                color: Color.NoProject,
                at: default(DateTimeOffset),
                syncStatus: SyncStatus.RefetchingNeeded,
                clientId: null,
                isPrivate: true,
                billable: false);

            return dataSource.Create(ghost);
        }
    }
}
