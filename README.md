# BJJ Training Tracker

BJJ Training Tracker is a C# Windows Forms application for recording Brazilian Jiu-Jitsu training sessions, reviewing training history and managing current development goals.

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
- Add training focus areas and separate them into active and completed lists
- Reopen or delete saved focus areas
- File access and invalid JSON errors are handled with clear messages
- Data is written through a temporary file before replacing the saved JSON file
- `TrainingSession` class extends the abstract `TrainingEntry` base class

## Project structure

- `MainForm.cs` builds the interface and handles user interaction.
- `Models/TrainingEntry.cs` is the abstract base class for training records.
- `Models/TrainingSession.cs` stores session data and overrides the summary method.
- `Models/FocusArea.cs` stores active and completed development goals.
- `Services/JsonDataService.cs` saves and loads sessions and focus areas as JSON.

## Running the project

1. Open `BJJTrainingTracker.sln` in Microsoft Visual Studio 2022.
2. Ensure the .NET 8 desktop development workload is installed.
3. Press **F5** to build and run the application.

The program creates its data files automatically inside the application output folder. No database or account setup is required.

## OOP design

The application uses classes and objects to separate session data, focus areas, file storage and interface behaviour. Properties with private setters protect key model values. `TrainingSession` inherits from the abstract `TrainingEntry` class and overrides `GetSummary()`, demonstrating inheritance, abstraction and polymorphism. File operations are wrapped in exception handling so invalid JSON, inaccessible files and failed saves can be reported without closing the program.

## Project documents

- `Docs/ITS203_Milestone_1_BJJ_Training_Tracker.docx` contains the approved project proposal.

## References and tools used

- Microsoft Learn documentation for C#, Windows Forms and `System.Text.Json`
- Git and GitHub for version control and repository hosting
