﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Toggl.Foundation.Sync.States.Results;
using Toggl.Multivac;
using Toggl.Multivac.Models;
using Toggl.PrimeRadiant;

namespace Toggl.Foundation.Sync.States.Pull
{
    public sealed class PullSinceState<TInterface, TDatabaseInterface> : IState, ISpeculativePreloadable
        where TInterface : IIdentifiable, ILastChangedDatable
        where TDatabaseInterface : TInterface, IDatabaseSyncable
    {
        private readonly Func<DateTimeOffset?, IObservable<IEnumerable<TInterface>>> fetchSince;
        private readonly ISinceParameterRepository sinceParameterRepository;
        private readonly PullState<TInterface, TDatabaseInterface> pullState;

        private DateTimeOffset? lastUpdate;

        public IEnumerable<IResult> AllPossibleOutcomes => pullState.AllPossibleOutcomes;

        public PullSinceState(
            Func<DateTimeOffset?, IObservable<IEnumerable<TInterface>>> fetchSince,
            IRepository<TDatabaseInterface> repository,
            Func<TInterface, TDatabaseInterface> convertToDatabaseEntity,
            ISinceParameterRepository sinceParameterRepository,
            Func<TDatabaseInterface, TDatabaseInterface, ConflictResolutionMode> conflictResolution,
            IRivalsResolver<TDatabaseInterface> rivalsResolver,
            IState nextState = null)
        {
            Ensure.Argument.IsNotNull(fetchSince, nameof(fetchSince));
            Ensure.Argument.IsNotNull(repository, nameof(repository));
            Ensure.Argument.IsNotNull(convertToDatabaseEntity, nameof(convertToDatabaseEntity));
            Ensure.Argument.IsNotNull(sinceParameterRepository, nameof(sinceParameterRepository));

            this.fetchSince = fetchSince;
            this.sinceParameterRepository = sinceParameterRepository;

            pullState = new PullState<TInterface, TDatabaseInterface>(
                fetch, repository, convertToDatabaseEntity, conflictResolution, rivalsResolver, nextState);
        }

        public void Preload()
        {
            pullState.Preload();
        }

        public IObservable<IResult> Run()
            => pullState.Run().Do(updateSince);

        private IObservable<IEnumerable<TInterface>> fetch()
        {
            var since = sinceParameterRepository.Get<TDatabaseInterface>();
            return fetchSince(since)
                .Select(fetchedEntities => fetchedEntities.ToList())
                .Do(storeLastUpdate);
        }

        private void storeLastUpdate(IList<TInterface> fetchedEntities)
        {
            if (fetchedEntities.Count == 0) return;

            lastUpdate = fetchedEntities.Select(entity => entity.At).Max();
        }

        private void updateSince(IResult result)
        {
            if (result is Proceed == false || lastUpdate.HasValue == false)
                return;

            sinceParameterRepository.Set<TDatabaseInterface>(lastUpdate);
        }
    }
}
