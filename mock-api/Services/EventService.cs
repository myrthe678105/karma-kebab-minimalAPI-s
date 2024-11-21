using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class EventService
{
    private readonly string _filePath;

    public EventService(string filePath)
    {
        _filePath = filePath;
    }

    public List<Event> GetAllEvents(DateTime? startDate = null, DateTime? endDate = null)
    {
        var jsonData = File.ReadAllText(_filePath);
        var events = JsonConvert.DeserializeObject<List<Event>>(jsonData);

        // If both startDate and endDate are null, return all events
        if (startDate == null && endDate == null)
        {
            return events;
        }

        // Filter events based on the date range
        return events.FindAll(e =>
        {
            bool isAfterStart = startDate == null || e.Date >= startDate;
            bool isBeforeEnd = endDate == null || e.Date <= endDate;
            return isAfterStart && isBeforeEnd;
        });
    }

    public Event GetEventById(String id)
    {
        var events = GetAllEvents();
        return events.Find(e => e.Id == id);
    }

    public void AddEvent(Event newEvent)
    {
        var events = GetAllEvents();
        events.Add(newEvent);
        File.WriteAllText(_filePath, JsonConvert.SerializeObject(events, Formatting.Indented));
    }

    public void UpdateEvent(Event updatedEvent)
    {
        var events = GetAllEvents();
        var index = events.FindIndex(e => e.Id == updatedEvent.Id);
        if (index != -1)
        {
            events[index] = updatedEvent;
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(events, Formatting.Indented));
        }
    }

    public void DeleteEvent(String id)
    {
        var events = GetAllEvents();
        var eventToRemove = events.Find(e => e.Id == id);
        if (eventToRemove != null)
        {
            events.Remove(eventToRemove);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(events, Formatting.Indented));
        }
    }
}