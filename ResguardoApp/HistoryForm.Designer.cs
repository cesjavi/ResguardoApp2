namespace ResguardoApp
{
    partial class HistoryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox historyListBox;
        private System.Windows.Forms.Button clearHistoryButton;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            historyListBox = new System.Windows.Forms.ListBox();
            clearHistoryButton = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // historyListBox
            // 
            historyListBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            historyListBox.FormattingEnabled = true;
            historyListBox.ItemHeight = 25;
            historyListBox.Location = new System.Drawing.Point(12, 12);
            historyListBox.Name = "historyListBox";
            historyListBox.Size = new System.Drawing.Size(460, 354);
            historyListBox.TabIndex = 0;
            // 
            // clearHistoryButton
            // 
            clearHistoryButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            clearHistoryButton.Location = new System.Drawing.Point(351, 372);
            clearHistoryButton.Name = "clearHistoryButton";
            clearHistoryButton.Size = new System.Drawing.Size(121, 38);
            clearHistoryButton.TabIndex = 1;
            clearHistoryButton.Text = "Limpiar";
            clearHistoryButton.UseVisualStyleBackColor = true;
            // 
            // HistoryForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(484, 422);
            Controls.Add(clearHistoryButton);
            Controls.Add(historyListBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "HistoryForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Historial de Resguardos";
            ResumeLayout(false);
        }

        #endregion
    }
}
