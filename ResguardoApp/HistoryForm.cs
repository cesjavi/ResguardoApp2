using SharedLib;
using System;
using System.Windows.Forms;

namespace ResguardoApp
{
    public partial class HistoryForm : Form
    {
        public HistoryForm()
        {
            InitializeComponent();
            LoadHistory();
            clearHistoryButton.Click += ClearHistoryButton_Click;
        }

        private void LoadHistory()
        {
            var records = BackupHistoryService.GetRecords();
            historyListBox.Items.Clear();
            foreach (var record in records)
            {
                historyListBox.Items.Add($"{record.Timestamp:u} | {record.Status} | {record.Details}");
            }
        }

        private void ClearHistoryButton_Click(object? sender, EventArgs e)
        {
            BackupHistoryService.Clear();
            LoadHistory();
        }
    }
}
