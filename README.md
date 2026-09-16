# BJJ Training Tracker

BJJ Training Tracker is a C# Windows Forms application for recording training sessions, techniques, sparring rounds and weekly development.

## Current progress

- Windows Forms project structure created
- Training-session entry form created
- Session type, duration, techniques, rounds and notes can be entered
- Required technique input is validated before a session is added
- Added sessions are displayed in the current session list
- `TrainingSession` class extends the abstract `TrainingEntry` base class
- Git and GitHub development workflow started

## Planned features

- Edit and delete training sessions
- Record techniques and sparring rounds
- Save and load records from JSON
- Search and filter training history
- Generate a weekly summary
- Manage active focus areas

## Running the project

1. Open `BJJTrainingTracker.csproj` in Microsoft Visual Studio 2022.
2. Ensure the .NET 8 desktop development workload is installed.
3. Press **F5** to build and run the application.

## References and tools used

- Microsoft Learn documentation for C#, Windows Forms and `System.Text.Json`
