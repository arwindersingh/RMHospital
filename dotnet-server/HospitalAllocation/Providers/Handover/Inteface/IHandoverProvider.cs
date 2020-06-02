using System;
using HospitalAllocation.Data.Handover;

namespace HospitalAllocation.Providers.Handover.Inteface
{
    /// <summary>
    /// Handover provider Interface
    /// </summary>
    public interface IHandoverProvider
    {
        /// <summary>
        /// Create the specified handover.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="handover">Handover.</param>
        int Create(HandoverDTO handover);

        /// <summary>
        /// Gets the handover.
        /// </summary>
        /// <returns>The handover.</returns>
        /// <param name="id">Identifier.</param>
        HandoverDTO GetHandover(int id);

        /// <summary>
        /// Update the specified id and handover.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="handover">Handover.</param>
        bool Update(int id, HandoverDTO handover);

        /// <summary>
        /// Delete the specified id.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="id">Identifier.</param>
        bool Delete(int id);

    }
}
