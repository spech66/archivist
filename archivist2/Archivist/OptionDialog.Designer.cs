namespace Archivist
{
	partial class OptionDialog
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
            this.button1 = new System.Windows.Forms.Button();
            this.cbShowImagesLibrary = new System.Windows.Forms.CheckBox();
            this.cbShowImagesDeck = new System.Windows.Forms.CheckBox();
            this.cbDownload = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cbShowIconsInRule = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(125, 227);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbShowImagesLibrary
            // 
            this.cbShowImagesLibrary.AutoSize = true;
            this.cbShowImagesLibrary.Checked = global::Archivist.Properties.Settings.Default.ShowImagesLibrary;
            this.cbShowImagesLibrary.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Archivist.Properties.Settings.Default, "ShowImagesLibrary", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbShowImagesLibrary.Location = new System.Drawing.Point(12, 35);
            this.cbShowImagesLibrary.Name = "cbShowImagesLibrary";
            this.cbShowImagesLibrary.Size = new System.Drawing.Size(340, 17);
            this.cbShowImagesLibrary.TabIndex = 4;
            this.cbShowImagesLibrary.Text = "Show Images in Librarytable (Restart required, Slows down startup)";
            this.cbShowImagesLibrary.UseVisualStyleBackColor = true;
            // 
            // cbShowImagesDeck
            // 
            this.cbShowImagesDeck.AutoSize = true;
            this.cbShowImagesDeck.Checked = global::Archivist.Properties.Settings.Default.ShowImagesDeck;
            this.cbShowImagesDeck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Archivist.Properties.Settings.Default, "ShowImagesDeck", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbShowImagesDeck.Location = new System.Drawing.Point(12, 12);
            this.cbShowImagesDeck.Name = "cbShowImagesDeck";
            this.cbShowImagesDeck.Size = new System.Drawing.Size(284, 17);
            this.cbShowImagesDeck.TabIndex = 3;
            this.cbShowImagesDeck.Text = "Show Images in Decktable (Newly opened decks only)";
            this.cbShowImagesDeck.UseVisualStyleBackColor = true;
            // 
            // cbDownload
            // 
            this.cbDownload.AutoSize = true;
            this.cbDownload.Checked = global::Archivist.Properties.Settings.Default.DownloadImages;
            this.cbDownload.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Archivist.Properties.Settings.Default, "DownloadImages", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbDownload.Location = new System.Drawing.Point(12, 58);
            this.cbDownload.Name = "cbDownload";
            this.cbDownload.Size = new System.Drawing.Size(111, 17);
            this.cbDownload.TabIndex = 2;
            this.cbDownload.Text = "Download Images";
            this.cbDownload.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(13, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Warning";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(16, 170);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(299, 51);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "Checking download images and show images in deck/library causes download of all i" +
                "mages of the whole deck!";
            // 
            // cbShowIconsInRule
            // 
            this.cbShowIconsInRule.AutoSize = true;
            this.cbShowIconsInRule.Checked = global::Archivist.Properties.Settings.Default.ShowIconsInRule;
            this.cbShowIconsInRule.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Archivist.Properties.Settings.Default, "ShowIconsInRule", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbShowIconsInRule.Location = new System.Drawing.Point(12, 81);
            this.cbShowIconsInRule.Name = "cbShowIconsInRule";
            this.cbShowIconsInRule.Size = new System.Drawing.Size(208, 17);
            this.cbShowIconsInRule.TabIndex = 7;
            this.cbShowIconsInRule.Text = "Show icons (Mana, Tap, ...) in rule text";
            this.cbShowIconsInRule.UseVisualStyleBackColor = true;
            // 
            // OptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 262);
            this.Controls.Add(this.cbShowIconsInRule);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbShowImagesLibrary);
            this.Controls.Add(this.cbShowImagesDeck);
            this.Controls.Add(this.cbDownload);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox cbDownload;
		private System.Windows.Forms.CheckBox cbShowImagesDeck;
		private System.Windows.Forms.CheckBox cbShowImagesLibrary;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox cbShowIconsInRule;
	}
}