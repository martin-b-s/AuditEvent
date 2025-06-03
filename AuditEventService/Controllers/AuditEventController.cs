using AuditEventService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuditEventService.Controllers
{
    [Route("events")]
    [ApiController]
    public class AuditEventController : ControllerBase
    {
        private readonly ILogger<AuditEventController> _logger;
        private readonly IEventStorage _eventStorage;

        public AuditEventController(IEventStorage eventStorage, ILogger<AuditEventController> logger)
        {
            _logger = logger;
            _eventStorage = eventStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditEvent>>> GetEvents()
        {
            try
            {
                _logger.Log(LogLevel.Information, nameof(GetEvents));
                var result = await _eventStorage.GetEventsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }
        [HttpGet("serviceName/{serviceName}")]
        public async Task<ActionResult<IEnumerable<AuditEvent>>> GetEventsByServiceName(string serviceName)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"{nameof(GetEventsByServiceName)} {serviceName}");
                var result =  await _eventStorage.GetEventsByServiceNameAsync(serviceName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }
        [HttpGet("eventType/{eventType}")]
        public async Task<ActionResult<IEnumerable<AuditEvent>>> GetEventsByEventType(string eventType)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"{nameof(GetEventsByEventType)} {eventType}");
                var result = await _eventStorage.GetEventsByEventTypeAsync(eventType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }
        [HttpGet("timeRange/{start}/{end}")]
        public async Task<ActionResult<IEnumerable<AuditEvent>>> GetEventsByTimeRange(DateTimeOffset start, DateTimeOffset end)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"{nameof(GetEventsByTimeRange)} {start} {end}");
                var result = await _eventStorage.GetEventsByTimeRangeAsync(start, end);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AuditEvent auditEvent)
        {
            try
            {
                _logger.Log(LogLevel.Information, nameof(Post));
                var result = await _eventStorage.AddAsync(auditEvent);
                if (!result)
                    return Forbid();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }
        [HttpPost("replay")]
        public async Task<IActionResult> PostReplay([FromBody] Guid[] IdArray)
        {
            try
            {
                _logger.Log(LogLevel.Information, nameof(PostReplay));
                await _eventStorage.ReplayAsync(IdArray);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }
    }
}
