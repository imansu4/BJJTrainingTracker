namespace BJJTrainingTracker.Models;

public abstract class TrainingEntry
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Notes { get; set; }

    protected TrainingEntry(string notes)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        Notes = notes;
    }

    public abstract string GetSummary();
}
