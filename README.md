# BJJ Training Tracker

BJJ Training Tracker is a C# Windows Forms application for recording training sessions, techniques, sparring rounds and weekly development.

## Current progress

- Windows Forms project structure created
- Training-session entry form created
- Session type, duration, techniques, rounds and notes can be entered
- Required technique input is validated before a session is added
- Sessions are saved to a local JSON file and restored when the application opens
- Saved sessions are displayed in the session list
- Existing sessions can be selected, edited and saved
- Sessions can be deleted after a confirmation prompt
- Search saved sessions by technique and filter by type and date range
- Select a week to see total sessions, training minutes and sparring rounds
- File access and invalid JSON errors are handled with clear messages
- `TrainingSession` class extends the abstract `TrainingEntry` base class
- Git and GitHub development workflow started

## Planned features

- Manage active focus areas

## Running the project

1. Open `BJJTrainingTracker.csproj` in Microsoft Visual Studio 2022.
2. Ensure the .NET 8 desktop development workload is installed.
3. Press **F5** to build and run the application.

## References and tools used

- Microsoft Learn documentation for C#, Windows Forms and `System.Text.Json`
