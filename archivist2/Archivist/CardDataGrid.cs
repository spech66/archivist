﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Archivist
{
	public class CardDataGrid :DataGridView
	{
		public enum GridType { Cards, Library, Deck }
		private GridType type = GridType.Cards;

		private bool isInitialized = false;
		
		private DataGridViewTextBoxColumn colLibId;
		private DataGridViewTextBoxColumn colLibAmount;
		private DataGridViewTextBoxColumn colLibName;
		private DataGridViewTextBoxColumn colLibCosts;
		private DataGridViewTextBoxColumn colLibType;
		private DataGridViewTextBoxColumn colLibExtension;
		private DataGridViewTextBoxColumn colLibRarity;
		private DataGridViewImageColumn colLibImage;
        private DataGridViewCheckBoxColumn colLibIsInSideboard;
            
		public CardDataGrid()
		{
		}

		public void SetGridType(GridType t)
		{
			type = t;
		}

		private void InitializeControls()
		{
			colLibId = new DataGridViewTextBoxColumn();
			colLibAmount = new DataGridViewTextBoxColumn();
			colLibName = new DataGridViewTextBoxColumn();
			colLibCosts = new DataGridViewTextBoxColumn();
			colLibType = new DataGridViewTextBoxColumn();
			colLibImage = new DataGridViewImageColumn();
			colLibExtension = new DataGridViewTextBoxColumn();
			colLibRarity = new DataGridViewTextBoxColumn();
            colLibIsInSideboard = new DataGridViewCheckBoxColumn();

			SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			AllowUserToAddRows = false;
			AllowUserToDeleteRows = false;
			AllowUserToResizeRows = false;
			
			AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			MultiSelect = false;

			RowHeadersVisible = false;

			// 
			// colLibId
			// 
			this.colLibId.HeaderText = "Id";
			this.colLibId.Name = "colLibId";
			this.colLibId.ReadOnly = true;
			this.colLibId.Visible = false;
			// 
			// colLibAmount
			// 
			this.colLibAmount.HeaderText = "Amount";
			this.colLibAmount.Name = "colLibAmount";
			this.colLibAmount.SortMode = DataGridViewColumnSortMode.Automatic;
			// 
			// colLibName
			// 
			this.colLibName.HeaderText = "Name";
			this.colLibName.Name = "colLibName";
			this.colLibName.ReadOnly = true;
			this.colLibName.SortMode = DataGridViewColumnSortMode.Automatic;
			// 
			// colLibCosts
			// 
			this.colLibCosts.HeaderText = "Costs";
			this.colLibCosts.Name = "colLibCosts";
			this.colLibCosts.ReadOnly = true;
			this.colLibCosts.SortMode = DataGridViewColumnSortMode.Automatic;
			// 
			// colLibType
			// 
			this.colLibType.HeaderText = "Type";
			this.colLibType.Name = "colLibType";
			this.colLibType.ReadOnly = true;
			this.colLibType.SortMode = DataGridViewColumnSortMode.Automatic;
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
			this.colLibExtension.SortMode = DataGridViewColumnSortMode.Automatic;
			// 
			// colLibName
			// 
			this.colLibRarity.HeaderText = "Rarity";
			this.colLibRarity.Name = "colLibRarity";
			this.colLibRarity.ReadOnly = true;
			this.colLibRarity.SortMode = DataGridViewColumnSortMode.Automatic;
			//
            // colLibIsInSideboard
            //
            this.colLibIsInSideboard.HeaderText = "Sideboard";
            this.colLibIsInSideboard.Name = "colLibIsInSideboard";
			this.colLibIsInSideboard.SortMode = DataGridViewColumnSortMode.Automatic;

			Columns.AddRange(new DataGridViewColumn[] {
				this.colLibId,
				this.colLibAmount,
				this.colLibIsInSideboard,
				this.colLibName,
				this.colLibCosts,
				this.colLibType,
				this.colLibExtension,
				this.colLibRarity,
				this.colLibImage});

			isInitialized = true;
		}

		public void BindDatasource(object datasource)
		{
			if (!isInitialized)
			{
                InitializeControls();

                AutoGenerateColumns = false;

                colLibId.DataPropertyName = "Multiverseid";
                colLibAmount.DataPropertyName = "Amount";
                colLibIsInSideboard.DataPropertyName = "IsInSideboard";
                colLibName.DataPropertyName = "Name";
                colLibCosts.DataPropertyName = "ManaCost";
                colLibType.DataPropertyName = "Type";
                colLibExtension.DataPropertyName = "Extension";
                colLibRarity.DataPropertyName = "Rarity";

                foreach (DataGridViewColumn col in Columns)
                {
                    if (col.Name != colLibId.Name)
                    {
                        col.Visible = true;
                    }
                }

                switch (type)
                {
                    case CardDataGrid.GridType.Cards:
                        colLibImage.Visible = false;
                        colLibAmount.Visible = false;
                        colLibType.Visible = false;
                        colLibImage.Visible = false;
                        colLibRarity.Visible = false;
                        colLibIsInSideboard.Visible = false;
                        break;
                    case CardDataGrid.GridType.Deck:
                        colLibImage.Visible = Properties.Settings.Default.ShowImagesDeck;
                        colLibExtension.Visible = false;
                        break;
                    case CardDataGrid.GridType.Library:
                        colLibImage.Visible = Properties.Settings.Default.ShowImagesLibrary;
                        colLibIsInSideboard.Visible = false;
                        break;
                }
			}

			DataSource = datasource;
		}

		protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
		{
			if (!isInitialized)
			{
				base.OnCellFormatting(e);
				return;
			}

			int idColumnIdx = colLibId.Index;
			int colorColumnIdx = colLibCosts.Index;
			int imageColumnIdx = colLibImage.Index;

			if (e.ColumnIndex == colorColumnIdx)
			{
				if (Rows[e.RowIndex].Cells[colorColumnIdx].Value == null)
				{
					base.OnCellFormatting(e);
					return;
				}

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
						Rows[e.RowIndex].Cells[colorColumnIdx].Style.ForeColor = Color.Black;
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
			}
			else if (e.ColumnIndex == imageColumnIdx && colLibImage.Visible)
			{
				if (Rows[e.RowIndex].Cells[idColumnIdx].Value == null || Rows[e.RowIndex].Cells[imageColumnIdx].Value != null)
				{
					base.OnCellFormatting(e);
					return;
				}

				Image img = Helper.GetMagicImage(Rows[e.RowIndex].Cells[idColumnIdx].Value.ToString());
				Rows[e.RowIndex].Cells[imageColumnIdx].Value = img;
			}

			base.OnCellFormatting(e);
		}
		
		protected override void OnCellValidating(DataGridViewCellValidatingEventArgs e)
		{
			if (e.ColumnIndex == colLibAmount.Index)
			{
				int i;
				if (!int.TryParse(Convert.ToString(e.FormattedValue), out i))
				{
					MessageBox.Show("Value must be numeric.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
					base.OnCellValidating(e);
					return;
				}

				if (i < 1)
				{
					MessageBox.Show("Value must be greater than 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
					base.OnCellValidating(e);
					return;
				}
			}

			base.OnCellValidating(e);
		}
	}
}
