using System;

public class Event
{
    public String Id { get; set; }
    public DateTime Date { get; set; }
    public string Address { get; set; }
    public string Venue { get; set; }
    public string Description { get; set; }
    public double Money { get; set; }
    public Status Status { get; set; }
    public Person Person { get; set; }
    public string Note { get; set; }
}

public enum Status
{
    Scheduled,
    Completed,
    Cancelled
}