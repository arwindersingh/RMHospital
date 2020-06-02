using System;
using System.Collections.Generic;
using System.Linq;
using HospitalAllocation.Data.Handover;
using HospitalAllocation.Model;
using HospitalAllocation.Providers.Handover.Inteface;
using Microsoft.EntityFrameworkCore;

namespace HospitalAllocation.Providers.Handover.Database
{
    /// <summary>
    /// Db handover provider implementation
    /// </summary>
    public class DbHandoverProvider : IHandoverProvider
    {
        // The database configuration options that we can construct a DB context (connection) from
        private readonly DbContextOptions<AllocationContext> _dbContextOptions;

        /// <summary>
        /// Construct a new staff provider around a database, as configured by a DbContextOptions object
        /// </summary>
        /// <param name="dbContextOptions"></param>
        public DbHandoverProvider(DbContextOptions<AllocationContext> dbContextOptions)
        {
            this._dbContextOptions = dbContextOptions;
        }

        /// <summary>
        /// Create the specified handoverDTO.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="handoverDTO">Handover dto.</param>
        public int Create(HandoverDTO handoverDTO)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                // Create the handover Model object
                Model.Handover handover = HandoverDTOToModel(handoverDTO);

                // Store the object in database
                dbContext.Handovers.Add(handover);
                dbContext.SaveChanges();

                // EF Core should magically populate this property after the commit
                return handover.HandoverID;
            }
        }

        /// <summary>
        /// Gets the handover.
        /// </summary>
        /// <returns>The handover.</returns>
        /// <param name="id">Identifier.</param>
        public HandoverDTO GetHandover(int id)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                Model.Handover handover = dbContext.Handovers
                    .Include(h => h.HandoverIssues)
                    .SingleOrDefault(h => h.HandoverID == id);
                return HandoverModelToDTO(handover);
            }
        }

        /// <summary>
        /// Update the specified id and handoverDTO.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="handoverDTO">Handover dto.</param>
        public bool Update(int id, HandoverDTO handoverDTO)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                Model.Handover handover = dbContext.Handovers
                    .Include(h => h.HandoverIssues)
                    .SingleOrDefault(h => h.HandoverID == id);
                if (handover == null)
                {
                    return false;
                }
                //Update all the attributes
                Model.Handover newHandover = HandoverDTOToModel(handoverDTO);
                handover.AdmissionUnit = newHandover.AdmissionUnit;
                handover.AdmissionDate = newHandover.AdmissionDate;
                handover.Alerts = newHandover.Alerts;                          
                handover.BedNumber = newHandover.BedNumber;
                handover.NurseName = newHandover.NurseName;
                handover.HandoverIssues = newHandover.HandoverIssues;
                handover.Isolation = newHandover.Isolation;
                handover.PatientName = newHandover.PatientName;
                handover.PatientId = newHandover.PatientId;
                handover.PastMedicalHistory = newHandover.PastMedicalHistory;
                handover.PresentingComplaint = newHandover.PresentingComplaint;
                handover.SignificantEvents = newHandover.SignificantEvents;
                handover.StudentName = newHandover.StudentName;
                handover.SwabSent = newHandover.SwabSent;
                handover.SwabSentDate = newHandover.SwabSentDate;

                // Update
                dbContext.Update(handover);
                dbContext.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Delete the specified id.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="id">Identifier.</param>
        public bool Delete(int id)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                Model.Handover handover = dbContext.Handovers.Find(id);
                if (handover == null)
                {
                    return false;
                }
                // Remove the object in database
                dbContext.Handovers.Remove(handover);
                dbContext.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Handovers the DTOT o model.
        /// </summary>
        /// <returns>The DTOT o model.</returns>
        /// <param name="handoverDTO">Handover dto.</param>
        private Model.Handover HandoverDTOToModel(HandoverDTO handoverDTO){
            // retrieve the handover issues DTO from handover form
            List<HandoverIssue> handoverIssues = new List<HandoverIssue>();
            // convert the DTO object into Model object for database storage
            foreach (HandoverIssueDTO handoverIssueDTO in handoverDTO.HandoverIssues)
            {
                HandoverIssue handoverIssue = new HandoverIssue()
                {
                    IssueNumber = handoverIssueDTO.IssueNumber,
                    CurrentIssue = handoverIssueDTO.CurrentIssue,
                    Management = handoverIssueDTO.Management,
                    FollowUp = handoverIssueDTO.FollowUp
                };
                handoverIssues.Add(handoverIssue);
            }
            // Create the handover Model object
            Model.Handover handover = new Model.Handover()
            {
                PatientId = handoverDTO.PatientId,
                PatientName = handoverDTO.PatientName,
                Alerts = handoverDTO.Alerts,
                BedNumber = handoverDTO.BedNumber,
                AdmissionDate = handoverDTO.AdmissionDate,
                AdmissionUnit = handoverDTO.AdmissionUnit,
                NurseName = handoverDTO.NurseName,
                StudentName = handoverDTO.StudentName,
                PresentingComplaint = handoverDTO.PresentingComplaint,
                PastMedicalHistory = handoverDTO.PastMedicalHistory,
                SignificantEvents = handoverDTO.SignificantEvents,
                HandoverIssues = handoverIssues,
                Isolation = handoverDTO.Isolation,
                SwabSent = handoverDTO.SwabSent,
                SwabSentDate = handoverDTO.SwabSentDate
            };
            return handover;
        }

        /// <summary>
        /// Handovers the model to dto.
        /// </summary>
        /// <returns>The model to dto.</returns>
        /// <param name="handover">Handover.</param>
        private HandoverDTO HandoverModelToDTO(Model.Handover handover)
        {
            int numberOfIssues = handover.HandoverIssues.Count;
            HandoverIssueDTO[] handoverIssueDTOs = new HandoverIssueDTO[numberOfIssues];
            int i = 0;
            foreach (HandoverIssue handoverIssue in handover.HandoverIssues)
            {
                handoverIssueDTOs[i] = new HandoverIssueDTO(
                    handoverIssue.IssueNumber,
                    handoverIssue.CurrentIssue,
                    handoverIssue.Management,
                    handoverIssue.FollowUp
                );
                i++;
            }
            HandoverDTO handoverDTO = new HandoverDTO(handover.BedNumber,
                                                      handover.AdmissionDate,
                                                      handover.AdmissionUnit,
                                                      handover.NurseName,
                                                      handover.StudentName,
                                                      handover.PatientId,
                                                      handover.PatientName,
                                                      handover.Alerts,
                                                      handover.PresentingComplaint,
                                                      handover.PastMedicalHistory,
                                                      handover.SignificantEvents,
                                                      handoverIssueDTOs,
                                                      handover.Isolation,
                                                      handover.SwabSent,
                                                      handover.SwabSentDate);
            return handoverDTO;
        }


    }
}
