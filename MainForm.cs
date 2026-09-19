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
    private readonly Button updateButton = new();
    private readonly Button deleteButton = new();
    private readonly TextBox techniqueSearch = new();
    private readonly ComboBox typeFilter = new();
    private readonly DateTimePicker fromDateFilter = new();
    private readonly DateTimePicker toDateFilter = new();
    private readonly Label filterStatus = new();
    private readonly JsonDataService dataService = new();
    private List<TrainingSession> sessions = [];
    private Guid? selectedSessionId;

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
            Margin = new Padding(3)
        };
        saveButton.Click += SaveSession;

        updateButton.Text = "Update Selected";
        updateButton.AutoSize = true;
        updateButton.Padding = new Padding(12, 6, 12, 6);
        updateButton.Margin = new Padding(3);
        updateButton.Enabled = false;
        updateButton.Click += UpdateSelectedSession;

        deleteButton.Text = "Delete Selected";
        deleteButton.AutoSize = true;
        deleteButton.Padding = new Padding(12, 6, 12, 6);
        deleteButton.Margin = new Padding(3);
        deleteButton.Enabled = false;
        deleteButton.Click += DeleteSelectedSession;

        var buttonPanel = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = new Padding(0, 14, 0, 0)
        };
        buttonPanel.Controls.Add(saveButton);
        buttonPanel.Controls.Add(updateButton);
        buttonPanel.Controls.Add(deleteButton);
        form.Controls.Add(buttonPanel, 1, 6);

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
        sessionList.SelectedIndexChanged += LoadSelectedSession;

        panel.Controls.Add(sessionList);
        panel.Controls.Add(CreateFilters());
        panel.Controls.Add(heading);
        return panel;
    }

    private Control CreateFilters()
    {
        var filters = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 205,
            ColumnCount = 2,
            RowCount = 5,
            Padding = new Padding(0, 8, 8, 6)
        };
        filters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95));
        filters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        techniqueSearch.PlaceholderText = "Search techniques";
        techniqueSearch.Dock = DockStyle.Fill;
        typeFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        typeFilter.Items.AddRange(["All types", "Gi", "No-Gi", "Wrestling"]);
        typeFilter.SelectedIndex = 0;
        typeFilter.Dock = DockStyle.Fill;

        fromDateFilter.Format = DateTimePickerFormat.Short;
        fromDateFilter.ShowCheckBox = true;
        fromDateFilter.Checked = false;
        fromDateFilter.Dock = DockStyle.Fill;
        toDateFilter.Format = DateTimePickerFormat.Short;
        toDateFilter.ShowCheckBox = true;
        toDateFilter.Checked = false;
        toDateFilter.Dock = DockStyle.Fill;

        AddFormRow(filters, 0, "Technique", techniqueSearch);
        AddFormRow(filters, 1, "Type", typeFilter);
        AddFormRow(filters, 2, "From date", fromDateFilter);
        AddFormRow(filters, 3, "To date", toDateFilter);

        var clearButton = new Button { Text = "Clear filters", AutoSize = true };
        clearButton.Click += (_, _) =>
        {
            techniqueSearch.Clear();
            typeFilter.SelectedIndex = 0;
            fromDateFilter.Checked = false;
            toDateFilter.Checked = false;
            RefreshSessionList(selectedSessionId);
        };
        filterStatus.AutoSize = true;
        filterStatus.Anchor = AnchorStyles.Left;
        var footer = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        footer.Controls.Add(clearButton);
        footer.Controls.Add(filterStatus);
        filters.Controls.Add(footer, 1, 4);

        techniqueSearch.TextChanged += FilterChanged;
        typeFilter.SelectedIndexChanged += FilterChanged;
        fromDateFilter.ValueChanged += FilterChanged;
        toDateFilter.ValueChanged += FilterChanged;
        return filters;
    }

    private void FilterChanged(object? sender, EventArgs e)
    {
        RefreshSessionList(selectedSessionId);
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
        if (!TryGetTechniques(out var techniques))
        {
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
            RefreshSessionList();
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

    private void UpdateSelectedSession(object? sender, EventArgs e)
    {
        if (selectedSessionId is null || !TryGetTechniques(out var techniques))
        {
            return;
        }

        var session = sessions.FirstOrDefault(item => item.Id == selectedSessionId.Value);
        if (session is null)
        {
            return;
        }

        var previousValues = new
        {
            session.SessionDate,
            session.SessionType,
            session.DurationMinutes,
            session.Techniques,
            session.SparringRounds,
            session.Notes
        };

        session.Update(
            sessionDatePicker.Value.Date,
            sessionTypeComboBox.SelectedItem?.ToString() ?? "Gi",
            (int)durationInput.Value,
            techniques,
            (int)roundsInput.Value,
            notesInput.Text.Trim());

        try
        {
            dataService.SaveSessions(sessions);
            RefreshSessionList(session.Id);
            ClearForm();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            session.Update(
                previousValues.SessionDate,
                previousValues.SessionType,
                previousValues.DurationMinutes,
                previousValues.Techniques,
                previousValues.SparringRounds,
                previousValues.Notes);
            ShowSaveError(ex);
        }
    }

    private void DeleteSelectedSession(object? sender, EventArgs e)
    {
        if (selectedSessionId is null)
        {
            return;
        }

        var session = sessions.FirstOrDefault(item => item.Id == selectedSessionId.Value);
        if (session is null)
        {
            return;
        }

        var confirmation = MessageBox.Show(
            "Delete the selected training session?",
            "Confirm deletion",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmation != DialogResult.Yes)
        {
            return;
        }

        sessions.Remove(session);
        try
        {
            dataService.SaveSessions(sessions);
            RefreshSessionList();
            ClearForm();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            sessions.Add(session);
            ShowSaveError(ex);
        }
    }

    private void LoadSelectedSession(object? sender, EventArgs e)
    {
        if (sessionList.SelectedItem is not TrainingSession session)
        {
            selectedSessionId = null;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
            return;
        }

        selectedSessionId = session.Id;
        sessionDatePicker.Value = session.SessionDate;
        sessionTypeComboBox.SelectedItem = session.SessionType;
        if (sessionTypeComboBox.SelectedIndex < 0)
        {
            sessionTypeComboBox.SelectedIndex = 0;
        }
        durationInput.Value = Math.Clamp(
            session.DurationMinutes,
            (int)durationInput.Minimum,
            (int)durationInput.Maximum);
        roundsInput.Value = Math.Clamp(
            session.SparringRounds,
            (int)roundsInput.Minimum,
            (int)roundsInput.Maximum);
        techniquesInput.Text = session.Techniques;
        notesInput.Text = session.Notes;
        updateButton.Enabled = true;
        deleteButton.Enabled = true;
    }

    private bool TryGetTechniques(out string techniques)
    {
        techniques = techniquesInput.Text.Trim();
        if (!string.IsNullOrWhiteSpace(techniques))
        {
            return true;
        }

        MessageBox.Show(
            "Enter at least one technique practised.",
            "Missing information",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
        techniquesInput.Focus();
        return false;
    }

    private void RefreshSessionList(Guid? sessionIdToSelect = null)
    {
        var validDates = !fromDateFilter.Checked || !toDateFilter.Checked ||
                         fromDateFilter.Value.Date <= toDateFilter.Value.Date;
        var search = techniqueSearch.Text.Trim();
        var type = typeFilter.SelectedItem?.ToString();
        List<TrainingSession> matching = validDates
            ? sessions.Where(item =>
                (string.IsNullOrEmpty(search) || item.Techniques.Contains(search, StringComparison.OrdinalIgnoreCase)) &&
                (type is null or "All types" || item.SessionType == type) &&
                (!fromDateFilter.Checked || item.SessionDate.Date >= fromDateFilter.Value.Date) &&
                (!toDateFilter.Checked || item.SessionDate.Date <= toDateFilter.Value.Date))
                .OrderByDescending(item => item.SessionDate)
                .ToList()
            : [];

        sessionList.BeginUpdate();
        try
        {
            sessionList.Items.Clear();
            foreach (var session in matching)
            {
                sessionList.Items.Add(session);
            }
            if (sessionIdToSelect.HasValue)
            {
                var selected = matching.FirstOrDefault(item => item.Id == sessionIdToSelect.Value);
                if (selected is not null) sessionList.SelectedItem = selected;
            }
        }
        finally
        {
            sessionList.EndUpdate();
        }

        if (sessionIdToSelect.HasValue && sessionList.SelectedItem is null)
        {
            ClearForm();
        }
        filterStatus.Text = validDates
            ? $"Showing {matching.Count} of {sessions.Count}"
            : "From date must be before to date";
    }

    private static void ShowSaveError(Exception exception)
    {
        MessageBox.Show(
            "The session changes could not be saved. " + exception.Message,
            "Save error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    private void LoadSavedSessions()
    {
        try
        {
            sessions = dataService.LoadSessions();
            RefreshSessionList();
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
        selectedSessionId = null;
        sessionList.ClearSelected();
        updateButton.Enabled = false;
        deleteButton.Enabled = false;
        sessionDatePicker.Value = DateTime.Today;
        sessionTypeComboBox.SelectedIndex = 0;
        durationInput.Value = 60;
        roundsInput.Value = 0;
        techniquesInput.Clear();
        notesInput.Clear();
        techniquesInput.Focus();
    }
}
