using System;

namespace AuditEventService.Models
{
    public interface IEventStorage
    {
        /// <summary>
        /// Stores an audit event record
        /// </summary>
        /// <param name="Event"></param>
        /// <returns> true if stored successfully otherwise returns false</returns>
        Task<bool> AddAsync(AuditEvent Event);
        Task ReplayAsync(Guid[] IdArray);
        Task<IEnumerable<AuditEvent>> GetEventsAsync();
        Task<IEnumerable<AuditEvent>> GetEventsByServiceNameAsync(string serviceName);
        Task<IEnumerable<AuditEvent>> GetEventsByEventTypeAsync( string eventType);
        Task<IEnumerable<AuditEvent>> GetEventsByTimeRangeAsync(DateTimeOffset start, DateTimeOffset end);
    }
    public class DictionaryEventStorage : IEventStorage
    {
        private Dictionary<Guid, AuditEvent> _eventDictionary;
        public DictionaryEventStorage()
        {
            _eventDictionary = new Dictionary<Guid, AuditEvent>();
        }

        public Task<bool> AddAsync(AuditEvent Event)
        {
            return Task.Run(() =>
            {
                var result = false;
                try
                {
                    lock (_eventDictionary)
                    {
                        if (!_eventDictionary.ContainsKey(Event.Id))
                        {
                            _eventDictionary.Add(Event.Id, Event);
                            result = true;
                        }
                    }
                }
                catch (Exception)
                {
                    result = false;
                }

                return result;
            });
        }
        public Task ReplayAsync(Guid[] IdArray)
        {
            return Task.Run(() =>
            {
                var events = GetEventsByPredicate(E => IdArray.Contains(E.Id));
                foreach (var auditEvent in events.OrderBy(E => E.Timestamp))
                    Console.WriteLine($"Id: {auditEvent.Id} Timestamp: {auditEvent.Timestamp} ServiceName:{auditEvent.ServiceName} EventType:{auditEvent.EventType} Payload:{auditEvent.Payload}");
            });
        }
        public Task<IEnumerable<AuditEvent>> GetEventsAsync() 
        {
            return Task.Run(() =>
            {
                lock (_eventDictionary)
                {
                    return _eventDictionary.Values.AsEnumerable();
                }
            });
        }
        public Task<IEnumerable<AuditEvent>> GetEventsByServiceNameAsync(string serviceName) => Task.Run(() => GetEventsByPredicate(E => E.ServiceName == serviceName));
        public Task<IEnumerable<AuditEvent>> GetEventsByEventTypeAsync(string eventType) => Task.Run(() => GetEventsByPredicate(E => E.EventType == eventType));
        public Task<IEnumerable<AuditEvent>> GetEventsByTimeRangeAsync(DateTimeOffset start, DateTimeOffset end) => Task.Run(() => GetEventsByPredicate(E => E.Timestamp >= start && E.Timestamp <= end));


        private IEnumerable<AuditEvent> GetEventsByPredicate(Func<AuditEvent, bool> predicate)
        {
            lock (_eventDictionary)
            {
                return _eventDictionary.Values.Where(predicate);
            }
        }
    }
}
