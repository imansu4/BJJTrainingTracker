using BJJTrainingTracker.Models;
using BJJTrainingTracker.Services;
using System.Text.Json;

namespace BJJTrainingTracker;

public class MainForm : Form
{
    private readonly DateTimePicker sessionDatePicker = new();
    private readonly ComboBox sessionTypeComboBox = new();
    private readonly NumericUpDown durationInput = new();
    private readonly NumericUpDown roundsInput = new();
    private readonly TextBox techniquesInput = new();
    private readonly TextBox notesInput = new();
    private readonly ListBox sessionList = new();
    private readonly JsonDataService dataService = new();
    private List<TrainingSession> sessions = [];

    public MainForm()
    {
        Text = "BJJ Training Tracker";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(920, 650);
        Font = new Font("Segoe UI", 10);

        var titleLabel = new Label
        {
            Text = "BJJ Training Tracker",
            AutoSize = true,
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            Dock = DockStyle.Top,
            Padding = new Padding(20, 18, 0, 12)
        };

        var content = new SplitContainer
        {
            Dock = DockStyle.Fill,
            SplitterDistance = 430,
            Padding = new Padding(20, 0, 20, 20)
        };

        content.Panel1.Controls.Add(CreateSessionForm());
        content.Panel2.Controls.Add(CreateSessionList());

        Controls.Add(content);
        Controls.Add(titleLabel);

        LoadSavedSessions();
    }

    private Control CreateSessionForm()
    {
        var form = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 8,
            Padding = new Padding(10),
            AutoScroll = true
        };

        form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        sessionDatePicker.Format = DateTimePickerFormat.Short;
        sessionDatePicker.Width = 220;

        sessionTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        sessionTypeComboBox.Items.AddRange(["Gi", "No-Gi", "Wrestling"]);
        sessionTypeComboBox.SelectedIndex = 0;
        sessionTypeComboBox.Width = 220;

        durationInput.Minimum = 1;
        durationInput.Maximum = 300;
        durationInput.Value = 60;
        durationInput.Width = 220;

        roundsInput.Minimum = 0;
        roundsInput.Maximum = 50;
        roundsInput.Width = 220;

        techniquesInput.Width = 250;
        techniquesInput.PlaceholderText = "Example: half guard, armbar";

        notesInput.Multiline = true;
        notesInput.Height = 100;
        notesInput.Width = 250;
        notesInput.ScrollBars = ScrollBars.Vertical;

        AddFormRow(form, 0, "Session date", sessionDatePicker);
        AddFormRow(form, 1, "Session type", sessionTypeComboBox);
        AddFormRow(form, 2, "Duration (min)", durationInput);
        AddFormRow(form, 3, "Sparring rounds", roundsInput);
        AddFormRow(form, 4, "Techniques", techniquesInput);
        AddFormRow(form, 5, "Notes", notesInput);

        var saveButton = new Button
        {
            Text = "Save Session",
            AutoSize = true,
            Padding = new Padding(12, 6, 12, 6),
            Margin = new Padding(3, 14, 3, 3)
        };
        saveButton.Click += SaveSession;
        form.Controls.Add(saveButton, 1, 6);

        return form;
    }

    private Control CreateSessionList()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(18, 10, 0, 10) };
        var heading = new Label
        {
            Text = "Saved sessions",
            AutoSize = true,
            Dock = DockStyle.Top,
            Font = new Font("Segoe UI", 13, FontStyle.Bold),
            Padding = new Padding(0, 0, 0, 10)
        };

        sessionList.Dock = DockStyle.Fill;
        sessionList.HorizontalScrollbar = true;

        panel.Controls.Add(sessionList);
        panel.Controls.Add(heading);
        return panel;
    }

    private static void AddFormRow(TableLayoutPanel form, int row, string labelText, Control input)
    {
        form.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var label = new Label
        {
            Text = labelText,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(3, 8, 3, 8)
        };
        input.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        input.Margin = new Padding(3, 6, 3, 6);
        form.Controls.Add(label, 0, row);
        form.Controls.Add(input, 1, row);
    }

    private void SaveSession(object? sender, EventArgs e)
    {
        var techniques = techniquesInput.Text.Trim();
        if (string.IsNullOrWhiteSpace(techniques))
        {
            MessageBox.Show(
                "Enter at least one technique practised.",
                "Missing information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            techniquesInput.Focus();
            return;
        }

        var session = new TrainingSession(
            sessionDatePicker.Value.Date,
            sessionTypeComboBox.SelectedItem?.ToString() ?? "Gi",
            (int)durationInput.Value,
            techniques,
            (int)roundsInput.Value,
            notesInput.Text.Trim());

        sessions.Add(session);

        try
        {
            dataService.SaveSessions(sessions);
            sessionList.Items.Add(session.GetSummary());
            ClearForm();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            sessions.Remove(session);
            MessageBox.Show(
                "The session could not be saved. " + ex.Message,
                "Save error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void LoadSavedSessions()
    {
        try
        {
            sessions = dataService.LoadSessions();
            foreach (var session in sessions.OrderByDescending(item => item.SessionDate))
            {
                sessionList.Items.Add(session.GetSummary());
            }
        }
        catch (JsonException)
        {
            sessions = [];
            MessageBox.Show(
                "The saved training data could not be read because the file is invalid.",
                "Data error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            sessions = [];
            MessageBox.Show(
                "The saved training data could not be loaded. " + ex.Message,
                "Load error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    private void ClearForm()
    {
        sessionDatePicker.Value = DateTime.Today;
        sessionTypeComboBox.SelectedIndex = 0;
        durationInput.Value = 60;
        roundsInput.Value = 0;
        techniquesInput.Clear();
        notesInput.Clear();
        techniquesInput.Focus();
    }
}
