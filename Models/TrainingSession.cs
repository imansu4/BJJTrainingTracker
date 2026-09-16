namespace BJJTrainingTracker.Models;

public class TrainingSession : TrainingEntry
{
    public DateTime SessionDate { get; }
    public string SessionType { get; }
    public int DurationMinutes { get; }
    public string Techniques { get; }
    public int SparringRounds { get; }

    public TrainingSession(
        DateTime sessionDate,
        string sessionType,
        int durationMinutes,
        string techniques,
        int sparringRounds,
        string notes) : base(notes)
    {
        SessionDate = sessionDate;
        SessionType = sessionType;
        DurationMinutes = durationMinutes;
        Techniques = techniques;
        SparringRounds = sparringRounds;
    }

    public override string GetSummary()
    {
        return $"{SessionDate:dd MMM yyyy} | {SessionType} | " +
               $"{DurationMinutes} min | {SparringRounds} rounds | {Techniques}";
    }
}
