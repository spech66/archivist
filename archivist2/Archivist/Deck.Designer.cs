namespace Archivist
{
	partial class Deck
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.zgManaCurve = new ZedGraph.ZedGraphControl();
			this.zgDistribution = new ZedGraph.ZedGraphControl();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.cardDataGrid1 = new Archivist.CardDataGrid();
			this.btnSaveAs = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cardDataGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// zgManaCurve
			// 
			this.zgManaCurve.Dock = System.Windows.Forms.DockStyle.Fill;
			this.zgManaCurve.Location = new System.Drawing.Point(3, 3);
			this.zgManaCurve.Name = "zgManaCurve";
			this.zgManaCurve.ScrollGrace = 0D;
			this.zgManaCurve.ScrollMaxX = 0D;
			this.zgManaCurve.ScrollMaxY = 0D;
			this.zgManaCurve.ScrollMaxY2 = 0D;
			this.zgManaCurve.ScrollMinX = 0D;
			this.zgManaCurve.ScrollMinY = 0D;
			this.zgManaCurve.ScrollMinY2 = 0D;
			this.zgManaCurve.Size = new System.Drawing.Size(791, 583);
			this.zgManaCurve.TabIndex = 1;
			// 
			// zgDistribution
			// 
			this.zgDistribution.Dock = System.Windows.Forms.DockStyle.Fill;
			this.zgDistribution.Location = new System.Drawing.Point(3, 3);
			this.zgDistribution.Name = "zgDistribution";
			this.zgDistribution.ScrollGrace = 0D;
			this.zgDistribution.ScrollMaxX = 0D;
			this.zgDistribution.ScrollMaxY = 0D;
			this.zgDistribution.ScrollMaxY2 = 0D;
			this.zgDistribution.ScrollMinX = 0D;
			this.zgDistribution.ScrollMinY = 0D;
			this.zgDistribution.ScrollMinY2 = 0D;
			this.zgDistribution.Size = new System.Drawing.Size(791, 583);
			this.zgDistribution.TabIndex = 2;
			// 
			// tabControl1
			// 
			this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(824, 597);
			this.tabControl1.TabIndex = 1;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.tableLayoutPanel1);
			this.tabPage1.Location = new System.Drawing.Point(23, 4);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(797, 589);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Cards";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.zgManaCurve);
			this.tabPage2.Location = new System.Drawing.Point(23, 4);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(797, 589);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Mana Curve";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.zgDistribution);
			this.tabPage3.Location = new System.Drawing.Point(23, 4);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(797, 589);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Distribution";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// tabPage4
			// 
			this.tabPage4.Location = new System.Drawing.Point(23, 4);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(797, 589);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Info";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.tableLayoutPanel1.Controls.Add(this.cardDataGrid1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnClose, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnSave, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnSaveAs, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(791, 583);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(634, 556);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(74, 23);
			this.btnSave.TabIndex = 1;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(714, 556);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(74, 23);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// cardDataGrid1
			// 
			this.cardDataGrid1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.cardDataGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.tableLayoutPanel1.SetColumnSpan(this.cardDataGrid1, 4);
			this.cardDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cardDataGrid1.Location = new System.Drawing.Point(3, 3);
			this.cardDataGrid1.Name = "cardDataGrid1";
			this.cardDataGrid1.Size = new System.Drawing.Size(785, 547);
			this.cardDataGrid1.TabIndex = 0;
			// 
			// btnSaveAs
			// 
			this.btnSaveAs.Location = new System.Drawing.Point(554, 556);
			this.btnSaveAs.Name = "btnSaveAs";
			this.btnSaveAs.Size = new System.Drawing.Size(74, 23);
			this.btnSaveAs.TabIndex = 3;
			this.btnSaveAs.Text = "Save As ...";
			this.btnSaveAs.UseVisualStyleBackColor = true;
			this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
			// 
			// Deck
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.Name = "Deck";
			this.Size = new System.Drawing.Size(824, 597);
			this.Load += new System.EventHandler(this.Deck_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cardDataGrid1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private CardDataGrid cardDataGrid1;
		private ZedGraph.ZedGraphControl zgManaCurve;
		private ZedGraph.ZedGraphControl zgDistribution;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSaveAs;
	}
}
