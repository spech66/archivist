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
            this.components = new System.ComponentModel.Container();
            this.zgManaCurve = new ZedGraph.ZedGraphControl();
            this.zgDistribution = new ZedGraph.ZedGraphControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpCards = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dgDeck = new Archivist.CardDataGrid();
            this.cmDeck = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cardInfoDeck = new Archivist.CardInfo();
            this.tpManaCurve = new System.Windows.Forms.TabPage();
            this.tpManaSymbols = new System.Windows.Forms.TabPage();
            this.zgManaSymbols = new ZedGraph.ZedGraphControl();
            this.tpDistribution = new System.Windows.Forms.TabPage();
            this.tpDraw = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDrawMulligan = new System.Windows.Forms.Button();
            this.btnDrawNewHand = new System.Windows.Forms.Button();
            this.pbDrawImage = new System.Windows.Forms.PictureBox();
            this.lbDrawStartingHand = new System.Windows.Forms.ListBox();
            this.lbDrawLibrary = new System.Windows.Forms.ListBox();
            this.tpFormat = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxFormat = new System.Windows.Forms.ComboBox();
            this.btnFormatCheck = new System.Windows.Forms.Button();
            this.pbFormat = new System.Windows.Forms.PictureBox();
            this.lvFormatSummary = new System.Windows.Forms.ListView();
            this.bwLoadDeck = new System.ComponentModel.BackgroundWorker();
            this.tabControl1.SuspendLayout();
            this.tpCards.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDeck)).BeginInit();
            this.cmDeck.SuspendLayout();
            this.tpManaCurve.SuspendLayout();
            this.tpManaSymbols.SuspendLayout();
            this.tpDistribution.SuspendLayout();
            this.tpDraw.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDrawImage)).BeginInit();
            this.tpFormat.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbFormat)).BeginInit();
            this.SuspendLayout();
            // 
            // zgManaCurve
            // 
            this.zgManaCurve.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zgManaCurve.IsEnableHPan = false;
            this.zgManaCurve.IsEnableHZoom = false;
            this.zgManaCurve.IsEnableVPan = false;
            this.zgManaCurve.IsEnableVZoom = false;
            this.zgManaCurve.IsEnableWheelZoom = false;
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
            this.tabControl1.Controls.Add(this.tpCards);
            this.tabControl1.Controls.Add(this.tpManaCurve);
            this.tabControl1.Controls.Add(this.tpManaSymbols);
            this.tabControl1.Controls.Add(this.tpDistribution);
            this.tabControl1.Controls.Add(this.tpDraw);
            this.tabControl1.Controls.Add(this.tpFormat);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(824, 597);
            this.tabControl1.TabIndex = 1;
            // 
            // tpCards
            // 
            this.tpCards.Controls.Add(this.tableLayoutPanel2);
            this.tpCards.Location = new System.Drawing.Point(23, 4);
            this.tpCards.Name = "tpCards";
            this.tpCards.Padding = new System.Windows.Forms.Padding(3);
            this.tpCards.Size = new System.Drawing.Size(797, 589);
            this.tpCards.TabIndex = 0;
            this.tpCards.Text = "Cards";
            this.tpCards.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel2.Controls.Add(this.dgDeck, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cardInfoDeck, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(791, 583);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // dgDeck
            // 
            this.dgDeck.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgDeck.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDeck.ContextMenuStrip = this.cmDeck;
            this.dgDeck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDeck.Location = new System.Drawing.Point(3, 3);
            this.dgDeck.Name = "dgDeck";
            this.dgDeck.Size = new System.Drawing.Size(535, 577);
            this.dgDeck.TabIndex = 0;
            this.dgDeck.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgDeck_CellValueChanged);
            this.dgDeck.SelectionChanged += new System.EventHandler(this.dgDeck_SelectionChanged);
            // 
            // cmDeck
            // 
            this.cmDeck.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.cmDeck.Name = "cmDeck";
            this.cmDeck.Size = new System.Drawing.Size(118, 26);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // cardInfoDeck
            // 
            this.cardInfoDeck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardInfoDeck.Location = new System.Drawing.Point(544, 3);
            this.cardInfoDeck.Name = "cardInfoDeck";
            this.cardInfoDeck.Size = new System.Drawing.Size(244, 577);
            this.cardInfoDeck.TabIndex = 1;
            // 
            // tpManaCurve
            // 
            this.tpManaCurve.Controls.Add(this.zgManaCurve);
            this.tpManaCurve.Location = new System.Drawing.Point(23, 4);
            this.tpManaCurve.Name = "tpManaCurve";
            this.tpManaCurve.Padding = new System.Windows.Forms.Padding(3);
            this.tpManaCurve.Size = new System.Drawing.Size(797, 589);
            this.tpManaCurve.TabIndex = 1;
            this.tpManaCurve.Text = "Mana Curve";
            this.tpManaCurve.UseVisualStyleBackColor = true;
            // 
            // tpManaSymbols
            // 
            this.tpManaSymbols.Controls.Add(this.zgManaSymbols);
            this.tpManaSymbols.Location = new System.Drawing.Point(23, 4);
            this.tpManaSymbols.Name = "tpManaSymbols";
            this.tpManaSymbols.Padding = new System.Windows.Forms.Padding(3);
            this.tpManaSymbols.Size = new System.Drawing.Size(797, 589);
            this.tpManaSymbols.TabIndex = 4;
            this.tpManaSymbols.Text = "Mana Symbols";
            this.tpManaSymbols.UseVisualStyleBackColor = true;
            // 
            // zgManaSymbols
            // 
            this.zgManaSymbols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zgManaSymbols.Location = new System.Drawing.Point(3, 3);
            this.zgManaSymbols.Name = "zgManaSymbols";
            this.zgManaSymbols.ScrollGrace = 0D;
            this.zgManaSymbols.ScrollMaxX = 0D;
            this.zgManaSymbols.ScrollMaxY = 0D;
            this.zgManaSymbols.ScrollMaxY2 = 0D;
            this.zgManaSymbols.ScrollMinX = 0D;
            this.zgManaSymbols.ScrollMinY = 0D;
            this.zgManaSymbols.ScrollMinY2 = 0D;
            this.zgManaSymbols.Size = new System.Drawing.Size(791, 583);
            this.zgManaSymbols.TabIndex = 0;
            // 
            // tpDistribution
            // 
            this.tpDistribution.Controls.Add(this.zgDistribution);
            this.tpDistribution.Location = new System.Drawing.Point(23, 4);
            this.tpDistribution.Name = "tpDistribution";
            this.tpDistribution.Padding = new System.Windows.Forms.Padding(3);
            this.tpDistribution.Size = new System.Drawing.Size(797, 589);
            this.tpDistribution.TabIndex = 2;
            this.tpDistribution.Text = "Distribution";
            this.tpDistribution.UseVisualStyleBackColor = true;
            // 
            // tpDraw
            // 
            this.tpDraw.Controls.Add(this.tableLayoutPanel1);
            this.tpDraw.Location = new System.Drawing.Point(23, 4);
            this.tpDraw.Name = "tpDraw";
            this.tpDraw.Padding = new System.Windows.Forms.Padding(3);
            this.tpDraw.Size = new System.Drawing.Size(797, 589);
            this.tpDraw.TabIndex = 3;
            this.tpDraw.Text = "Draw";
            this.tpDraw.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnDrawMulligan, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnDrawNewHand, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.pbDrawImage, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbDrawStartingHand, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbDrawLibrary, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(791, 583);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(389, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Starting Hand";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 261);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(389, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Library";
            // 
            // btnDrawMulligan
            // 
            this.btnDrawMulligan.Location = new System.Drawing.Point(3, 525);
            this.btnDrawMulligan.Name = "btnDrawMulligan";
            this.btnDrawMulligan.Size = new System.Drawing.Size(75, 23);
            this.btnDrawMulligan.TabIndex = 2;
            this.btnDrawMulligan.Text = "Mulligan";
            this.btnDrawMulligan.UseVisualStyleBackColor = true;
            this.btnDrawMulligan.Click += new System.EventHandler(this.btnDrawMulligan_Click);
            // 
            // btnDrawNewHand
            // 
            this.btnDrawNewHand.Location = new System.Drawing.Point(3, 555);
            this.btnDrawNewHand.Name = "btnDrawNewHand";
            this.btnDrawNewHand.Size = new System.Drawing.Size(75, 23);
            this.btnDrawNewHand.TabIndex = 3;
            this.btnDrawNewHand.Text = "New Hand";
            this.btnDrawNewHand.UseVisualStyleBackColor = true;
            this.btnDrawNewHand.Click += new System.EventHandler(this.btnDrawNewHand_Click);
            // 
            // pbDrawImage
            // 
            this.pbDrawImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbDrawImage.Location = new System.Drawing.Point(398, 3);
            this.pbDrawImage.Name = "pbDrawImage";
            this.tableLayoutPanel1.SetRowSpan(this.pbDrawImage, 6);
            this.pbDrawImage.Size = new System.Drawing.Size(390, 577);
            this.pbDrawImage.TabIndex = 4;
            this.pbDrawImage.TabStop = false;
            // 
            // lbDrawStartingHand
            // 
            this.lbDrawStartingHand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDrawStartingHand.FormattingEnabled = true;
            this.lbDrawStartingHand.Location = new System.Drawing.Point(3, 23);
            this.lbDrawStartingHand.Name = "lbDrawStartingHand";
            this.lbDrawStartingHand.Size = new System.Drawing.Size(389, 235);
            this.lbDrawStartingHand.TabIndex = 5;
            this.lbDrawStartingHand.SelectedIndexChanged += new System.EventHandler(this.lbDrawStartingHand_SelectedIndexChanged);
            // 
            // lbDrawLibrary
            // 
            this.lbDrawLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDrawLibrary.FormattingEnabled = true;
            this.lbDrawLibrary.Location = new System.Drawing.Point(3, 284);
            this.lbDrawLibrary.Name = "lbDrawLibrary";
            this.lbDrawLibrary.Size = new System.Drawing.Size(389, 235);
            this.lbDrawLibrary.TabIndex = 6;
            this.lbDrawLibrary.SelectedIndexChanged += new System.EventHandler(this.lbDrawLibrary_SelectedIndexChanged);
            // 
            // tpFormat
            // 
            this.tpFormat.Controls.Add(this.tableLayoutPanel3);
            this.tpFormat.Location = new System.Drawing.Point(23, 4);
            this.tpFormat.Name = "tpFormat";
            this.tpFormat.Padding = new System.Windows.Forms.Padding(3);
            this.tpFormat.Size = new System.Drawing.Size(797, 589);
            this.tpFormat.TabIndex = 5;
            this.tpFormat.Text = "Format";
            this.tpFormat.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel3.Controls.Add(this.comboBoxFormat, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnFormatCheck, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.pbFormat, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.lvFormatSummary, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(791, 583);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // comboBoxFormat
            // 
            this.comboBoxFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFormat.FormattingEnabled = true;
            this.comboBoxFormat.Location = new System.Drawing.Point(3, 3);
            this.comboBoxFormat.Name = "comboBoxFormat";
            this.comboBoxFormat.Size = new System.Drawing.Size(637, 21);
            this.comboBoxFormat.TabIndex = 0;
            // 
            // btnFormatCheck
            // 
            this.btnFormatCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFormatCheck.Location = new System.Drawing.Point(646, 3);
            this.btnFormatCheck.Name = "btnFormatCheck";
            this.btnFormatCheck.Size = new System.Drawing.Size(114, 22);
            this.btnFormatCheck.TabIndex = 1;
            this.btnFormatCheck.Text = "Check";
            this.btnFormatCheck.UseVisualStyleBackColor = true;
            this.btnFormatCheck.Click += new System.EventHandler(this.btnFormatCheck_Click);
            // 
            // pbFormat
            // 
            this.pbFormat.Location = new System.Drawing.Point(766, 3);
            this.pbFormat.MaximumSize = new System.Drawing.Size(20, 20);
            this.pbFormat.MinimumSize = new System.Drawing.Size(20, 20);
            this.pbFormat.Name = "pbFormat";
            this.pbFormat.Size = new System.Drawing.Size(20, 20);
            this.pbFormat.TabIndex = 2;
            this.pbFormat.TabStop = false;
            // 
            // lvFormatSummary
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.lvFormatSummary, 3);
            this.lvFormatSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFormatSummary.Location = new System.Drawing.Point(3, 31);
            this.lvFormatSummary.Name = "lvFormatSummary";
            this.lvFormatSummary.Size = new System.Drawing.Size(785, 549);
            this.lvFormatSummary.TabIndex = 3;
            this.lvFormatSummary.UseCompatibleStateImageBehavior = false;
            this.lvFormatSummary.View = System.Windows.Forms.View.List;
            // 
            // bwLoadDeck
            // 
            this.bwLoadDeck.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwLoadDeck_DoWork);
            this.bwLoadDeck.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwLoadDeck_RunWorkerCompleted);
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
            this.tpCards.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDeck)).EndInit();
            this.cmDeck.ResumeLayout(false);
            this.tpManaCurve.ResumeLayout(false);
            this.tpManaSymbols.ResumeLayout(false);
            this.tpDistribution.ResumeLayout(false);
            this.tpDraw.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDrawImage)).EndInit();
            this.tpFormat.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbFormat)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private CardDataGrid dgDeck;
		private ZedGraph.ZedGraphControl zgManaCurve;
		private ZedGraph.ZedGraphControl zgDistribution;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tpCards;
		private System.Windows.Forms.TabPage tpManaCurve;
		private System.Windows.Forms.TabPage tpDistribution;
        private System.Windows.Forms.TabPage tpDraw;
		private System.Windows.Forms.ContextMenuStrip cmDeck;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDrawMulligan;
        private System.Windows.Forms.Button btnDrawNewHand;
        private System.Windows.Forms.PictureBox pbDrawImage;
        private System.Windows.Forms.ListBox lbDrawStartingHand;
        private System.Windows.Forms.ListBox lbDrawLibrary;
        private System.Windows.Forms.TabPage tpManaSymbols;
        private ZedGraph.ZedGraphControl zgManaSymbols;
		private System.ComponentModel.BackgroundWorker bwLoadDeck;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private CardInfo cardInfoDeck;
        private System.Windows.Forms.TabPage tpFormat;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ComboBox comboBoxFormat;
        private System.Windows.Forms.Button btnFormatCheck;
        private System.Windows.Forms.PictureBox pbFormat;
        private System.Windows.Forms.ListView lvFormatSummary;
	}
}
