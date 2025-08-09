namespace ResguardoApp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            configPanel = new Panel();
            panel1 = new Panel();
            installServiceButton = new Button();
            backupTimePicker = new DateTimePicker();
            label3 = new Label();
            applyConfigButton = new Button();
            removeFolderButton = new Button();
            addFolderButton = new Button();
            label1 = new Label();
            portableDisksListBox = new ListBox();
            detectDrivesButton = new Button();
            backupButton = new Button();
            backupFoldersListBox = new ListBox();
            configPanel.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // configPanel
            // 
            configPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            configPanel.BackColor = SystemColors.ActiveCaptionText;
            configPanel.Controls.Add(detectDrivesButton);
            configPanel.Controls.Add(backupButton);
            configPanel.Controls.Add(panel1);
            configPanel.Controls.Add(installServiceButton);
            configPanel.Controls.Add(portableDisksListBox);
            configPanel.Controls.Add(backupTimePicker);
            configPanel.Controls.Add(label3);
            configPanel.Controls.Add(applyConfigButton);
            configPanel.Controls.Add(removeFolderButton);
            configPanel.Controls.Add(addFolderButton);
            configPanel.Controls.Add(label1);
            configPanel.Location = new Point(0, 0);
            configPanel.Margin = new Padding(4, 5, 4, 5);
            configPanel.Name = "configPanel";
            configPanel.Size = new Size(855, 608);
            configPanel.TabIndex = 12;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlDarkDark;
            panel1.Controls.Add(backupFoldersListBox);
            panel1.ForeColor = Color.Coral;
            panel1.Location = new Point(12, 23);
            panel1.Name = "panel1";
            panel1.Size = new Size(615, 283);
            panel1.TabIndex = 12;
            // 
            // installServiceButton
            // 
            installServiceButton.Location = new Point(658, 268);
            installServiceButton.Margin = new Padding(4, 5, 4, 5);
            installServiceButton.Name = "installServiceButton";
            installServiceButton.Size = new Size(171, 38);
            installServiceButton.TabIndex = 10;
            installServiceButton.Text = "Instalar Servicio";
            installServiceButton.UseVisualStyleBackColor = true;
            // 
            // backupTimePicker
            // 
            backupTimePicker.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            backupTimePicker.Format = DateTimePickerFormat.Time;
            backupTimePicker.Location = new Point(658, 53);
            backupTimePicker.Margin = new Padding(4, 5, 4, 5);
            backupTimePicker.Name = "backupTimePicker";
            backupTimePicker.Size = new Size(170, 31);
            backupTimePicker.TabIndex = 9;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.ForeColor = Color.Coral;
            label3.Location = new Point(658, 23);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(158, 25);
            label3.TabIndex = 11;
            label3.Text = "Hora de Respaldo:";
            // 
            // applyConfigButton
            // 
            applyConfigButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            applyConfigButton.Location = new Point(657, 94);
            applyConfigButton.Margin = new Padding(4, 5, 4, 5);
            applyConfigButton.Name = "applyConfigButton";
            applyConfigButton.Size = new Size(171, 38);
            applyConfigButton.TabIndex = 4;
            applyConfigButton.Text = "Aplicar Config.";
            applyConfigButton.UseVisualStyleBackColor = true;
            // 
            // removeFolderButton
            // 
            removeFolderButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            removeFolderButton.Location = new Point(410, 314);
            removeFolderButton.Margin = new Padding(4, 5, 4, 5);
            removeFolderButton.Name = "removeFolderButton";
            removeFolderButton.Size = new Size(171, 38);
            removeFolderButton.TabIndex = 3;
            removeFolderButton.Text = "Quitar Carpeta";
            removeFolderButton.UseVisualStyleBackColor = true;
            // 
            // addFolderButton
            // 
            addFolderButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            addFolderButton.Location = new Point(58, 314);
            addFolderButton.Margin = new Padding(4, 5, 4, 5);
            addFolderButton.Name = "addFolderButton";
            addFolderButton.Size = new Size(171, 38);
            addFolderButton.TabIndex = 2;
            addFolderButton.Text = "Agregar Carpeta";
            addFolderButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 15);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(203, 25);
            label1.TabIndex = 0;
            label1.Text = "Carpetas para respaldar:";
            // 
            // portableDisksListBox
            // 
            portableDisksListBox.FormattingEnabled = true;
            portableDisksListBox.ItemHeight = 25;
            portableDisksListBox.Location = new Point(13, 440);
            portableDisksListBox.Margin = new Padding(4, 5, 4, 5);
            portableDisksListBox.Name = "portableDisksListBox";
            portableDisksListBox.Size = new Size(372, 154);
            portableDisksListBox.TabIndex = 6;
            // 
            // detectDrivesButton
            // 
            detectDrivesButton.Location = new Point(13, 392);
            detectDrivesButton.Margin = new Padding(4, 5, 4, 5);
            detectDrivesButton.Name = "detectDrivesButton";
            detectDrivesButton.Size = new Size(171, 38);
            detectDrivesButton.TabIndex = 7;
            detectDrivesButton.Text = "Detectar Discos";
            detectDrivesButton.UseVisualStyleBackColor = true;
            // 
            // backupButton
            // 
            backupButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            backupButton.Location = new Point(393, 440);
            backupButton.Margin = new Padding(4, 5, 4, 5);
            backupButton.Name = "backupButton";
            backupButton.Size = new Size(226, 154);
            backupButton.TabIndex = 8;
            backupButton.Text = "Realizar Resguardo Manual";
            backupButton.UseVisualStyleBackColor = true;
            // 
            // backupFoldersListBox
            // 
            backupFoldersListBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            backupFoldersListBox.FormattingEnabled = true;
            backupFoldersListBox.ItemHeight = 25;
            backupFoldersListBox.Location = new Point(7, 14);
            backupFoldersListBox.Margin = new Padding(4, 5, 4, 5);
            backupFoldersListBox.Name = "backupFoldersListBox";
            backupFoldersListBox.Size = new Size(600, 254);
            backupFoldersListBox.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(852, 608);
            Controls.Add(configPanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "MainForm";
            Text = "ResguardoApp";
            configPanel.ResumeLayout(false);
            configPanel.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button addFolderButton;
        private System.Windows.Forms.Button removeFolderButton;
        private System.Windows.Forms.Button applyConfigButton;
        private System.Windows.Forms.ListBox portableDisksListBox;
        private System.Windows.Forms.Button detectDrivesButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button backupButton;
        private System.Windows.Forms.DateTimePicker backupTimePicker;
        private System.Windows.Forms.Button installServiceButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel configPanel;
        private Panel panel1;
        private ListBox backupFoldersListBox;
    }
}
