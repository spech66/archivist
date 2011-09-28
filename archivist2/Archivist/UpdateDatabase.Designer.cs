namespace Archivist
{
    partial class UpdateDatabase
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listStatus = new System.Windows.Forms.ListBox();
            this.labelTaskStatus = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.progressBarTaskStatus = new System.Windows.Forms.ProgressBar();
            this.progressBarStatus = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.listStatus, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelTaskStatus, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelStatus, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.progressBarTaskStatus, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.progressBarStatus, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.button1, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(701, 322);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // listStatus
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.listStatus, 2);
            this.listStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listStatus.FormattingEnabled = true;
            this.listStatus.Location = new System.Drawing.Point(3, 3);
            this.listStatus.Name = "listStatus";
            this.listStatus.Size = new System.Drawing.Size(695, 199);
            this.listStatus.TabIndex = 0;
            // 
            // labelTaskStatus
            // 
            this.labelTaskStatus.AutoSize = true;
            this.labelTaskStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaskStatus.Location = new System.Drawing.Point(3, 212);
            this.labelTaskStatus.Name = "labelTaskStatus";
            this.labelTaskStatus.Size = new System.Drawing.Size(114, 40);
            this.labelTaskStatus.TabIndex = 1;
            this.labelTaskStatus.Text = "Task Staus:";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStatus.Location = new System.Drawing.Point(3, 252);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(114, 40);
            this.labelStatus.TabIndex = 2;
            this.labelStatus.Text = "Status";
            // 
            // progressBarTaskStatus
            // 
            this.progressBarTaskStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBarTaskStatus.Location = new System.Drawing.Point(123, 215);
            this.progressBarTaskStatus.Name = "progressBarTaskStatus";
            this.progressBarTaskStatus.Size = new System.Drawing.Size(575, 23);
            this.progressBarTaskStatus.TabIndex = 3;
            // 
            // progressBarStatus
            // 
            this.progressBarStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBarStatus.Location = new System.Drawing.Point(123, 255);
            this.progressBarStatus.Name = "progressBarStatus";
            this.progressBarStatus.Size = new System.Drawing.Size(575, 23);
            this.progressBarStatus.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(623, 295);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UpdateDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 322);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateDatabase";
            this.Text = "UpdateDatabase";
            this.Load += new System.EventHandler(this.UpdateDatabase_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox listStatus;
        private System.Windows.Forms.Label labelTaskStatus;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ProgressBar progressBarTaskStatus;
        private System.Windows.Forms.ProgressBar progressBarStatus;
        private System.Windows.Forms.Button button1;
    }
}