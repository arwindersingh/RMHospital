using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HospitalAllocation.Data.Allocation.StaffGroups;
using HospitalAllocation.Providers.Allocation;
using HospitalAllocation.Messages.Requests.Allocation;
using HospitalAllocation.Messages.Responses;
using HospitalAllocation.Messages.Responses.Allocation;

namespace HospitalAllocation.Controllers
{
    /// <summary>
    /// Controls the team allocation API ("/api/team"), to set and get the
    /// allocation of pods and the senior staff team
    /// </summary>
    [Produces("application/json")]
    [Route("[controller]")]
    public class TeamController : Controller
    {
        // Provides an interface for the current allocation
        private readonly AllocationProvider _allocation;

        /// <summary>
        /// A lock object on allocation updates.
        /// </summary>
        private static readonly object _updateAllocationLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HospitalAllocation.Controllers.TeamController"/> class.
        /// </summary>
        /// <param name="allocationProvider">Provides an interface to the allocation store</param>
        public TeamController(AllocationProvider allocationProvider)
        {
            _allocation = allocationProvider;
        }

        /// <summary>
        /// Return a response listing all teams in the allocation
        /// with their name and type
        /// </summary>
        /// <returns>An HTTP 200 result with a JSON message encoding a list of the teams</returns>
        [HttpGet]
        public IActionResult GetTeamList()
        {
            // Collect all the teams into a list of summaries
            List<TeamType> teams = new List<Team>
            {
                _allocation.PodA,
                _allocation.PodB,
                _allocation.PodC,
                _allocation.PodD,
                _allocation.SeniorTeam
            }.Select(t => t.TeamType).ToList();

            return Json(new TeamListResponse(teams));
        }

        /// <summary>
        /// Get a summary of a single team, denoted by name in the URL path
        /// (e.g. "api/team/a")
        /// </summary>
        /// <returns>A summary of the team, or an error response if no such team exists</returns>
        /// <param name="teamType">The name of the team, from the URL path</param>
        /// <param name="time">The time to query.</param>
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("{teamType}")]
        public IActionResult GetTeam(TeamType? teamType, [FromQuery(Name = "t")] long? time = null)
        {
            if (teamType == null)
            {
                return BadRequest(new ErrorResponse("No such team"));
            }

            // Get the current allocation if no time specified
            if (time == null)
            {
                // Look up the team name against all the teams in an allocation
                switch (teamType)
                {
                    case TeamType.A:
                        return Json(new TeamSummaryResponse(_allocation.PodA));

                    case TeamType.B:
                        return Json(new TeamSummaryResponse(_allocation.PodB));

                    case TeamType.C:
                        return Json(new TeamSummaryResponse(_allocation.PodC));

                    case TeamType.D:
                        return Json(new TeamSummaryResponse(_allocation.PodD));

                    case TeamType.Senior:
                        return Json(new TeamSummaryResponse(_allocation.SeniorTeam));

                    default:
                        return BadRequest(new ErrorResponse("Someone naughty has created a new TeamType instance"));
                }
            }
            // Otherwise try to find the allocation that was in place at the time specified
            else
            {
                Team allocation;
                switch (teamType)
                {
                    case TeamType.Senior:
                        allocation = _allocation.GetPastSeniorTeam(time.Value);
                        break;

                    case TeamType.A:
                    case TeamType.B:
                    case TeamType.C:
                    case TeamType.D:
                        allocation = _allocation.GetPastPod(time.Value, teamType.Value);
                        break;

                    default:
                        return BadRequest(new ErrorResponse("Someone naughty has created a new TeamType instance"));
                }

                // No allocation at the time or lookup unsupported
                if (allocation == null)
                {
                    if (teamType == TeamType.Senior)
                    {
                        return Json(new TeamSummaryResponse(SeniorTeam.Empty));
                    }
                    else
                    {
                        return Json(new TeamSummaryResponse(Pod.CreateEmpty(teamType.Value)));
                    }
                }
                else
                {
                    return Json(new TeamSummaryResponse(allocation));
                }
            }
        }

        [HttpPut("senior")]
        public IActionResult AddAllocationToSeniorTeam([FromBody] SeniorAllocationRequest seniorAllocation)
        {
            if (seniorAllocation == null)
            {
                return BadRequest(new ErrorResponse("Bad or invalid request body"));
            }

            if (!seniorAllocation.SeniorTeam.IsStateValid)
            {
                return BadRequest(new ErrorResponse("Invalid senior team allocation supplied. No changes were saved"));
            }
            _allocation.SetSeniorTeam(seniorAllocation.SeniorTeam);
            _allocation.Commit(TeamType.Senior);
            return Ok(new AllocationResponse(_allocation.SeniorTeam));
        }

        /// <summary>
        /// Take a JSON-encoded PUT request to encode new team allocations and
        /// overwrite them on the current allocation, returning a summary of the
        /// new resulting allocation
        /// </summary>
        /// <returns>
        /// A summary of the new allocation if successful, otherwise
        /// an error message
        /// </returns>
        /// <param name="teamType">The name of the team to change</param>
        /// <param name="pod">The changes to make to the team</param>
        [HttpPut("{teamType}")]
        public IActionResult AddAllocationToPod(TeamType? teamType, [FromBody] PodAllocationRequest pod)
        {
            if (teamType == null)
            {
                return BadRequest(new ErrorResponse("No pod by that name exists"));
            }

            // If the request body is null (likely because it was not correctly formatted)
            // respond with an appropriate error
            if (pod == null)
            {
                return BadRequest(new ErrorResponse("Bad or invalid request body"));
            }

            if (!pod.Pod.IsStateValid)
            {
                return BadRequest(new ErrorResponse("Invalid pod allocation supplied. No changes were saved"));
            }

            Pod newPodState;
            lock (_updateAllocationLock)
            {
                switch (teamType)
                {
                    case TeamType.A:
                        _allocation.SetPodAAllocation(pod.Pod);
                        newPodState = _allocation.PodA;
                        break;

                    case TeamType.B:
                        _allocation.SetPodBAllocation(pod.Pod);
                        newPodState = _allocation.PodB;
                        break;

                    case TeamType.C:
                        _allocation.SetPodCAllocation(pod.Pod);
                        newPodState = _allocation.PodC;
                        break;

                    case TeamType.D:
                        _allocation.SetPodDAllocation(pod.Pod);
                        newPodState = _allocation.PodD;
                        break;

                    default:
                        return NotFound(new ErrorResponse("No such pod"));
                }

                _allocation.Commit(teamType.Value);
            }

            return Json(newPodState);
        }
    }
}
