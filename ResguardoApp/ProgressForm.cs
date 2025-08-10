using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SharedLib;

namespace ResguardoApp
{
    public class ProgressForm : Form
    {
        private readonly ProgressBar _progressBar;
        private readonly Label _percentLabel;
        private readonly Label _timeLabel;
        private readonly Button _cancelButton;
        private readonly CancellationTokenSource _cts = new();

        public ProgressForm(string title)
        {
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Width = 400;
            Height = 160;

            _progressBar = new ProgressBar { Dock = DockStyle.Top, Height = 30, Minimum = 0, Maximum = 100 };
            _percentLabel = new Label { Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter };
            _timeLabel = new Label { Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter };
            _cancelButton = new Button { Dock = DockStyle.Bottom, Text = "Cancelar" };
            _cancelButton.Click += (s, e) => _cts.Cancel();

            Controls.Add(_cancelButton);
            Controls.Add(_timeLabel);
            Controls.Add(_percentLabel);
            Controls.Add(_progressBar);
        }

        public CancellationToken CancellationToken => _cts.Token;

        public void UpdateProgress(BackupProgress progress)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<BackupProgress>(UpdateProgress), progress);
                return;
            }

            if (progress.TotalBytes > 0)
            {
                int percent = (int)(progress.ProcessedBytes * 100 / progress.TotalBytes);
                percent = Math.Max(0, Math.Min(100, percent));
                _progressBar.Value = percent;
                _percentLabel.Text = $"{percent}%";
            }

            if (progress.EstimatedTimeRemaining.HasValue)
            {
                var remaining = progress.EstimatedTimeRemaining.Value;
                _timeLabel.Text = $"Tiempo restante: {remaining:hh\\:mm\\:ss}";
            }
        }
    }
}
