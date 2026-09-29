using System.Text.Json;
using BJJTrainingTracker.Models;

namespace BJJTrainingTracker.Services;

public class JsonDataService
{
    private readonly string sessionsFilePath;
    private readonly string focusAreasFilePath;
    private readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true
    };

    public JsonDataService()
    {
        var dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");
        sessionsFilePath = Path.Combine(dataFolder, "trainingData.json");
        focusAreasFilePath = Path.Combine(dataFolder, "focusAreas.json");
    }

    public List<TrainingSession> LoadSessions()
    {
        return LoadList<TrainingSession>(sessionsFilePath);
    }

    public void SaveSessions(List<TrainingSession> sessions)
    {
        SaveList(sessionsFilePath, sessions);
    }

    public List<FocusArea> LoadFocusAreas()
    {
        return LoadList<FocusArea>(focusAreasFilePath);
    }

    public void SaveFocusAreas(List<FocusArea> focusAreas)
    {
        SaveList(focusAreasFilePath, focusAreas);
    }

    private List<T> LoadList<T>(string path)
    {
        if (!File.Exists(path))
        {
            return [];
        }

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(json, options) ?? [];
    }

    private void SaveList<T>(string path, List<T> items)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(items, options);
        File.WriteAllText(path, json);
    }
}
