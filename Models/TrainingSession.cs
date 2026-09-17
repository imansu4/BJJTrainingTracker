using System.Text.Json.Serialization;

namespace BJJTrainingTracker.Models;

public class TrainingSession : TrainingEntry
{
    [JsonInclude]
    public DateTime SessionDate { get; private set; }

    [JsonInclude]
    public string SessionType { get; private set; }

    [JsonInclude]
    public int DurationMinutes { get; private set; }

    [JsonInclude]
    public string Techniques { get; private set; }

    [JsonInclude]
    public int SparringRounds { get; private set; }

    public TrainingSession()
    {
        SessionType = string.Empty;
        Techniques = string.Empty;
    }

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
