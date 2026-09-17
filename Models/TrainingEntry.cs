using System.Text.Json.Serialization;

namespace BJJTrainingTracker.Models;

public abstract class TrainingEntry
{
    [JsonInclude]
    public Guid Id { get; private set; }

    [JsonInclude]
    public DateTime CreatedAt { get; private set; }

    public string Notes { get; set; }

    protected TrainingEntry()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        Notes = string.Empty;
    }

    protected TrainingEntry(string notes)
        : this()
    {
        Notes = notes;
    }

    public abstract string GetSummary();
}
