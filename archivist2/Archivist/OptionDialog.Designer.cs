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
			this.cbShowImagesLibrary.Size = new System.Drawing.Size(306, 17);
			this.cbShowImagesLibrary.TabIndex = 4;
			this.cbShowImagesLibrary.Text = "Show Images Library (Restart required, Slows down startup)";
			this.cbShowImagesLibrary.UseVisualStyleBackColor = true;
			// 
			// cbShowImagesDeck
			// 
			this.cbShowImagesDeck.AutoSize = true;
			this.cbShowImagesDeck.Checked = global::Archivist.Properties.Settings.Default.ShowImagesDeck;
			this.cbShowImagesDeck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Archivist.Properties.Settings.Default, "ShowImagesDeck", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.cbShowImagesDeck.Location = new System.Drawing.Point(12, 12);
			this.cbShowImagesDeck.Name = "cbShowImagesDeck";
			this.cbShowImagesDeck.Size = new System.Drawing.Size(119, 17);
			this.cbShowImagesDeck.TabIndex = 3;
			this.cbShowImagesDeck.Text = "Show Images Deck";
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
			// OptionDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(327, 262);
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
	}
}