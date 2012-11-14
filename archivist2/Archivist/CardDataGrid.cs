using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Archivist
{
	public class CardDataGrid :DataGridView
	{
		private bool isInitialized = false;

		private DataGridViewTextBoxColumn colLibAmount;
		private DataGridViewTextBoxColumn colLibName;
		private DataGridViewTextBoxColumn colLibCosts;
		private DataGridViewTextBoxColumn colLibType;
		private DataGridViewTextBoxColumn colLibExtension;
		private DataGridViewTextBoxColumn colLibRarity;
		private DataGridViewImageColumn colLibImage;

		public CardDataGrid()
		{
		}

		private void InitializeControls()
		{
			colLibAmount = new DataGridViewTextBoxColumn();
			colLibName = new DataGridViewTextBoxColumn();
			colLibCosts = new DataGridViewTextBoxColumn();
			colLibType = new DataGridViewTextBoxColumn();
			colLibImage = new DataGridViewImageColumn();
			colLibExtension = new DataGridViewTextBoxColumn();
			colLibRarity = new DataGridViewTextBoxColumn();

			SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			AllowUserToAddRows = false;
			AllowUserToDeleteRows = false;
			AllowUserToResizeRows = false;
			AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

			// 
			// colLibAmount
			// 
			this.colLibAmount.HeaderText = "Amount";
			this.colLibAmount.Name = "colLibAmount";
			// 
			// colLibName
			// 
			this.colLibName.HeaderText = "Name";
			this.colLibName.Name = "colLibName";
			this.colLibName.ReadOnly = true;
			// 
			// colLibCosts
			// 
			this.colLibCosts.HeaderText = "Costs";
			this.colLibCosts.Name = "colLibCosts";
			this.colLibCosts.ReadOnly = true;
			// 
			// colLibType
			// 
			this.colLibType.HeaderText = "Type";
			this.colLibType.Name = "colLibType";
			this.colLibType.ReadOnly = true;
			// 
			// colLibImage
			// 
			this.colLibImage.HeaderText = "Image";
			this.colLibImage.Name = "colLibImage";
			this.colLibImage.ReadOnly = true;
			// 
			// colLibExtension
			// 
			this.colLibExtension.HeaderText = "Extension";
			this.colLibExtension.Name = "colLibExtension";
			this.colLibExtension.ReadOnly = true;
			// 
			// colLibName
			// 
			this.colLibRarity.HeaderText = "XRarity";
			this.colLibRarity.Name = "colLibRarity";
			this.colLibRarity.ReadOnly = true;

			Columns.AddRange(new DataGridViewColumn[] {
				this.colLibAmount,
				this.colLibName,
				this.colLibCosts,
				this.colLibType,
				this.colLibExtension,
				this.colLibRarity,
				this.colLibImage});

			isInitialized = true;
		}

		public void BindDatasource(object datasource, bool simple)
		{
			if (!isInitialized)
			{
				InitializeControls();
			}
			
			AutoGenerateColumns = false;

			DataSource = datasource;

			colLibName.DataPropertyName = "Name";
			colLibCosts.DataPropertyName = "ManaCost"; //mc.ManaCost
			colLibType.DataPropertyName = "Type";
			colLibExtension.DataPropertyName = "Extension";
			colLibRarity.DataPropertyName = "Rarity";

			if (simple)
			{
				colLibAmount.Visible = false;
				colLibType.Visible = false;
				colLibImage.Visible = false;
				colLibRarity.Visible = false;
			}
			else
			{
				foreach (DataGridViewColumn col in Columns)
				{
					col.Visible = true;
				}
			}
		}

		protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
		{
			if (!isInitialized)
			{
				base.OnCellFormatting(e);
				return;
			}

			int colorColumnIdx = colLibCosts.Index;

			string costs = Rows[e.RowIndex].Cells[colorColumnIdx].Value.ToString();
			int colorCount = 0;
			string color = "";

			string[] colors = { "U", "B", "W", "R", "G" };
			foreach (string c in colors)
			{
				if (costs.Contains(c))
				{
					colorCount++;
					color = c;
				}
			}

			if (colorCount == 0) // Artifacts and lands, ...
			{
				Rows[e.RowIndex].Cells[colorColumnIdx].Style.BackColor = Color.Gray;
			}
			else if (colorCount == 1) // Mono color cards
			{
				if (color == "U")
				{
					Rows[e.RowIndex].Cells[colorColumnIdx].Style.BackColor = Color.Blue;
					Rows[e.RowIndex].Cells[colorColumnIdx].Style.ForeColor = Color.White;
				}
				else if (color == "W")
				{
					Rows[e.RowIndex].Cells[colorColumnIdx].Style.BackColor = Color.White;
				}
				else if (color == "R")
				{
					Rows[e.RowIndex].Cells[colorColumnIdx].Style.BackColor = Color.Red;
				}
				else if (color == "G")
				{
					Rows[e.RowIndex].Cells[colorColumnIdx].Style.BackColor = Color.DarkGreen;
					Rows[e.RowIndex].Cells[colorColumnIdx].Style.ForeColor = Color.White;
				}
				else if (color == "B")
				{
					Rows[e.RowIndex].Cells[colorColumnIdx].Style.BackColor = Color.Black;
					Rows[e.RowIndex].Cells[colorColumnIdx].Style.ForeColor = Color.White;
				}
			}
			else // Multicolor cards
			{
				Rows[e.RowIndex].Cells[colorColumnIdx].Style.BackColor = Color.Gold;
			}

			base.OnCellFormatting(e);
		}
	}
}
