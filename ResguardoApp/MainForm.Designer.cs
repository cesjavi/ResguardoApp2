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
            this.backupFoldersListBox = new System.Windows.Forms.ListBox();
            this.addFolderButton = new System.Windows.Forms.Button();
            this.removeFolderButton = new System.Windows.Forms.Button();
            this.saveConfigButton = new System.Windows.Forms.Button();
            this.portableDisksListBox = new System.Windows.Forms.ListBox();
            this.detectDrivesButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.backupButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Carpetas para respaldar:";
            //
            // backupFoldersListBox
            //
            this.backupFoldersListBox.FormattingEnabled = true;
            this.backupFoldersListBox.ItemHeight = 15;
            this.backupFoldersListBox.Location = new System.Drawing.Point(12, 27);
            this.backupFoldersListBox.Name = "backupFoldersListBox";
            this.backupFoldersListBox.Size = new System.Drawing.Size(358, 154);
            this.backupFoldersListBox.TabIndex = 1;
            //
            // addFolderButton
            //
            this.addFolderButton.Location = new System.Drawing.Point(376, 27);
            this.addFolderButton.Name = "addFolderButton";
            this.addFolderButton.Size = new System.Drawing.Size(120, 23);
            this.addFolderButton.TabIndex = 2;
            this.addFolderButton.Text = "Agregar Carpeta";
            this.addFolderButton.UseVisualStyleBackColor = true;
            //
            // removeFolderButton
            //
            this.removeFolderButton.Location = new System.Drawing.Point(376, 56);
            this.removeFolderButton.Name = "removeFolderButton";
            this.removeFolderButton.Size = new System.Drawing.Size(120, 23);
            this.removeFolderButton.TabIndex = 3;
            this.removeFolderButton.Text = "Quitar Carpeta";
            this.removeFolderButton.UseVisualStyleBackColor = true;
            //
            // saveConfigButton
            //
            this.saveConfigButton.Location = new System.Drawing.Point(376, 85);
            this.saveConfigButton.Name = "saveConfigButton";
            this.saveConfigButton.Size = new System.Drawing.Size(120, 23);
            this.saveConfigButton.TabIndex = 4;
            this.saveConfigButton.Text = "Guardar Config.";
            this.saveConfigButton.UseVisualStyleBackColor = true;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Discos extraíbles:";
            //
            // portableDisksListBox
            //
            this.portableDisksListBox.FormattingEnabled = true;
            this.portableDisksListBox.ItemHeight = 15;
            this.portableDisksListBox.Location = new System.Drawing.Point(12, 213);
            this.portableDisksListBox.Name = "portableDisksListBox";
            this.portableDisksListBox.Size = new System.Drawing.Size(358, 94);
            this.portableDisksListBox.TabIndex = 6;
            //
            // detectDrivesButton
            //
            this.detectDrivesButton.Location = new System.Drawing.Point(376, 213);
            this.detectDrivesButton.Name = "detectDrivesButton";
            this.detectDrivesButton.Size = new System.Drawing.Size(120, 23);
            this.detectDrivesButton.TabIndex = 7;
            this.detectDrivesButton.Text = "Detectar Discos";
            this.detectDrivesButton.UseVisualStyleBackColor = true;
            //
            // backupButton
            //
            this.backupButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.backupButton.Location = new System.Drawing.Point(376, 242);
            this.backupButton.Name = "backupButton";
            this.backupButton.Size = new System.Drawing.Size(120, 65);
            this.backupButton.TabIndex = 8;
            this.backupButton.Text = "Realizar Resguardo";
            this.backupButton.UseVisualStyleBackColor = true;
            //
            // backupTimePicker
            //
            this.backupTimePicker = new System.Windows.Forms.DateTimePicker();
            this.backupTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.backupTimePicker.Location = new System.Drawing.Point(12, 330);
            this.backupTimePicker.Name = "backupTimePicker";
            this.backupTimePicker.Size = new System.Drawing.Size(120, 23);
            this.backupTimePicker.TabIndex = 9;
            //
            // installServiceButton
            //
            this.installServiceButton = new System.Windows.Forms.Button();
            this.installServiceButton.Location = new System.Drawing.Point(376, 114);
            this.installServiceButton.Name = "installServiceButton";
            this.installServiceButton.Size = new System.Drawing.Size(120, 23);
            this.installServiceButton.TabIndex = 10;
            this.installServiceButton.Text = "Instalar Servicio";
            this.installServiceButton.UseVisualStyleBackColor = true;
            //
            // label3
            //
            this.label3 = new System.Windows.Forms.Label();
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 312);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "Hora de Respaldo:";
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 365);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.installServiceButton);
            this.Controls.Add(this.backupTimePicker);
            this.Controls.Add(this.backupButton);
            this.Controls.Add(this.detectDrivesButton);
            this.Controls.Add(this.portableDisksListBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.saveConfigButton);
            this.Controls.Add(this.removeFolderButton);
            this.Controls.Add(this.addFolderButton);
            this.Controls.Add(this.backupFoldersListBox);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "ResguardoApp";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox backupFoldersListBox;
        private System.Windows.Forms.Button addFolderButton;
        private System.Windows.Forms.Button removeFolderButton;
        private System.Windows.Forms.Button saveConfigButton;
        private System.Windows.Forms.ListBox portableDisksListBox;
        private System.Windows.Forms.Button detectDrivesButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button backupButton;
        private System.Windows.Forms.DateTimePicker backupTimePicker;
        private System.Windows.Forms.Button installServiceButton;
        private System.Windows.Forms.Label label3;
    }
}
