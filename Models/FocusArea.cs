using System.Text.Json.Serialization;

namespace BJJTrainingTracker.Models;

public class FocusArea
{
    [JsonInclude]
    public Guid Id { get; private set; }

    [JsonInclude]
    public string Name { get; private set; }

    [JsonInclude]
    public DateTime CreatedAt { get; private set; }

    [JsonInclude]
    public bool IsCompleted { get; private set; }

    [JsonInclude]
    public DateTime? CompletedAt { get; private set; }

    public FocusArea()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
        CreatedAt = DateTime.Now;
    }

    public FocusArea(string name)
        : this()
    {
        Name = name;
    }

    public void MarkCompleted()
    {
        MarkCompleted(DateTime.Now);
    }

    public void MarkCompleted(DateTime completedAt)
    {
        IsCompleted = true;
        CompletedAt = completedAt;
    }

    public void Reopen()
    {
        IsCompleted = false;
        CompletedAt = null;
    }

    public override string ToString()
    {
        return IsCompleted && CompletedAt.HasValue
            ? $"{Name} (completed {CompletedAt.Value:dd MMM yyyy})"
            : Name;
    }
}
