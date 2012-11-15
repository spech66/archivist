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
			this.cbDownload = new System.Windows.Forms.CheckBox();
			this.cbShowImages = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(197, 227);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// cbDownload
			// 
			this.cbDownload.AutoSize = true;
			this.cbDownload.Checked = global::Archivist.Properties.Settings.Default.DownloadImages;
			this.cbDownload.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Archivist.Properties.Settings.Default, "DownloadImages", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.cbDownload.Location = new System.Drawing.Point(12, 35);
			this.cbDownload.Name = "cbDownload";
			this.cbDownload.Size = new System.Drawing.Size(111, 17);
			this.cbDownload.TabIndex = 2;
			this.cbDownload.Text = "Download Images";
			this.cbDownload.UseVisualStyleBackColor = true;
			// 
			// cbShowImages
			// 
			this.cbShowImages.AutoSize = true;
			this.cbShowImages.Checked = global::Archivist.Properties.Settings.Default.ShowImages;
			this.cbShowImages.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbShowImages.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Archivist.Properties.Settings.Default, "ShowImages", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.cbShowImages.Location = new System.Drawing.Point(12, 12);
			this.cbShowImages.Name = "cbShowImages";
			this.cbShowImages.Size = new System.Drawing.Size(90, 17);
			this.cbShowImages.TabIndex = 3;
			this.cbShowImages.Text = "Show Images";
			this.cbShowImages.UseVisualStyleBackColor = true;
			// 
			// OptionDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.cbShowImages);
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
		private System.Windows.Forms.CheckBox cbShowImages;
	}
}