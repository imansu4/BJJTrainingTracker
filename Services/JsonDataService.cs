using System.Text.Json;
using BJJTrainingTracker.Models;

namespace BJJTrainingTracker.Services;

public class JsonDataService
{
    private readonly string filePath;
    private readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true
    };

    public JsonDataService()
    {
        var dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");
        filePath = Path.Combine(dataFolder, "trainingData.json");
    }

    public List<TrainingSession> LoadSessions()
    {
        if (!File.Exists(filePath))
        {
            return [];
        }

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<TrainingSession>>(json, options) ?? [];
    }

    public void SaveSessions(List<TrainingSession> sessions)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(sessions, options);
        File.WriteAllText(filePath, json);
    }
}
