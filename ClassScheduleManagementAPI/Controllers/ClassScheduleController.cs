using Microsoft.AspNetCore.Http;
using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace ClassSchedManagementAPI.Controllers
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : ControllerBase
    {
        private readonly BusinessServices _services;

        public ScheduleController()
        {
            _services = new BusinessServices();
        }

        [HttpGet("classSchedules")]
        public async Task<IEnumerable<Models1.Schedule>> GetClassSchedules()
        {
            return _services.GetSchedules();
        }

        [HttpPost("addSchedule")]
        public async Task<IActionResult> AddSchedule([FromBody] Models1.Schedule schedule)
        {
            if (schedule == null)
            {
                return BadRequest("Schedule cannot be null.");
            }

            var result = _services.AddSchedule(schedule);
            if (result)
            {
                return CreatedAtAction(nameof(GetClassSchedules), new { className = schedule.Classes }, schedule);
            }
            return BadRequest("Failed to add schedule.");
        }

        [HttpDelete("deleteSchedule")]
        public async Task<IActionResult> DeleteSchedule([FromQuery] string className, [FromQuery] string subject, [FromQuery] string professor)
        {
            var result = _services.DeleteSchedule(className, subject, professor);
            if (result)
            {
                return Ok("Schedule deleted successfully!");
            }
            return NotFound("Failed to delete schedule. No matching schedule found.");
        }

        [HttpPatch("updateSchedule")]
        public async Task<IActionResult> UpdateSchedule([FromQuery] string className, [FromQuery] string subject, [FromQuery] string professor)
        {
            var result = _services.DeleteSchedule(className, subject, professor);
            if (result)
            {
                return Ok("Schedule updated successfully!");
            }
            return NotFound("Failed to update schedule.");
        }

        }
    } 

