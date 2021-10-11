﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.Data.Sync
{
    public interface IDeltaStore
    {
        string Identity { get; }
        void SetIdentity(string Identity);
        /// <summary>
        /// Saves the IEnumerable<IDelta> of deltas in the current store
        /// </summary>
        /// <param name="deltas">The IEnumerable<IDelta> to be saved</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An empty task</returns>
        Task SaveDeltasAsync(IEnumerable<IDelta> deltas, CancellationToken cancellationToken);
        /// <summary>
        /// Gets an IEnumerable<IDelta> of deltas generated by other nodes with indeces greater than the start index 
        /// </summary>
        /// <param name="startindex">The start index</param>
        /// <param name="myIdentity">The identity of the current node </param>
        /// <param name="cancellationToken">a Cancellation token</param>
        /// <returns>An IEnumerable with deltas generated by other nodes</returns>
        Task<IEnumerable<IDelta>> GetDeltasFromOtherNodes(Guid startindex, string myIdentity, CancellationToken cancellationToken);
        /// <summary>
        /// Get all deltas in the store with an index greater than the start index
        /// </summary>
        /// <param name="startindex">The start index</param>
        /// <param name="cancellationToken">a cancellation token</param>
        /// <returns>An IEnumerable of deltas</returns>
        Task<IEnumerable<IDelta>> GetDeltasAsync(Guid startindex, CancellationToken cancellationToken);
        /// <summary>
        /// Gets the count of deltas with indeces greater that the start index
        /// </summary>
        /// <param name="startindex">The start index</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The count</returns>
        Task<int> GetDeltaCountAsync(Guid startindex, CancellationToken cancellationToken);
        /// <summary>
        /// Gets the index of the last delta process by this data object
        /// </summary>
        /// <param name="cancellationToken"> cancellation token</param>
        /// <returns>The index of the last delta process by this data object</returns>
        Task<Guid> GetLastProcessedDeltaAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Sets the index of the last delta process by this data object
        /// </summary>
        /// <param name="Index">The index to be saved</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An empty task</returns>
        Task SetLastProcessedDeltaAsync(Guid Index, CancellationToken cancellationToken);
        /// <summary>
        ///  Gets the index of the last delta pushed to the server node
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>the index of the last delta pushed to the server node</returns>
        Task<Guid> GetLastPushedDeltaAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Sets the index of the last delta pushed to the server node
        /// </summary>
        /// <param name="Index">The index to be saved</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An empty task</returns>
        Task SetLastPushedDeltaAsync(Guid Index, CancellationToken cancellationToken);
        /// <summary>
        /// Delete all deltas in the store
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An empty task</returns>
        Task PurgeDeltasAsync(CancellationToken cancellationToken);

    }
}