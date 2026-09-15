namespace BJJTrainingTracker;

public class MainForm : Form
{
    public MainForm()
    {
        Text = "BJJ Training Tracker";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(900, 600);

        var titleLabel = new Label
        {
            Text = "BJJ Training Tracker",
            AutoSize = true,
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            Location = new Point(24, 20)
        };

        var statusLabel = new Label
        {
            Text = "Project setup complete. Training-session features will be added next.",
            AutoSize = true,
            Font = new Font("Segoe UI", 11),
            Location = new Point(28, 72)
        };

        Controls.Add(titleLabel);
        Controls.Add(statusLabel);
    }
}
