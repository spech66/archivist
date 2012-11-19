using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using Archivist.MagicObjects;
using System.Drawing;

namespace Archivist
{
    public class PrintDeck
    {
        private List<int> proxyCardList;
        private int proxyCardListIndex;
        private List<Card> deckCardList;
        private int deckCardListIndex;

        /// <summary>
        /// Print list of images
        /// </summary>
        /// <param name="cardList"></param>
        public static void PrintProxyDeck(List<int> cardList)
        {
            PrintDeck pd = new PrintDeck();
            pd.proxyCardList = cardList;
            pd.PrintProxyDeck();
        }

        /// <summary>
        /// Print list of cards without image
        /// </summary>
        /// <param name="cardList"></param>
        public static void PrintDeckList(List<Card> cardList)
        {
            PrintDeck pd = new PrintDeck();
            pd.deckCardList = cardList;
            pd.PrintDeckList();
        }

        private void PrintProxyDeck()
        {
            proxyCardListIndex = 0;

            using(PrintDialog printDialog1 = new PrintDialog())
			//using (PrintPreviewDialog printDialog1 = new PrintPreviewDialog())
			{
				PrintDocument printDocumentProxyCardList = new PrintDocument();
				SetPrinterSettings(printDocumentProxyCardList);

				printDialog1.Document = printDocumentProxyCardList;

				printDocumentProxyCardList.PrintPage += new PrintPageEventHandler(printDocumentProxyCardList_PrintPage);

				if (printDialog1.ShowDialog() == DialogResult.OK)
					printDocumentProxyCardList.Print();
			}    
        }

        private void PrintDeckList()
        {
            deckCardListIndex = 0;
			
			using (PrintDialog printDialog1 = new PrintDialog())
			//using (PrintPreviewDialog printDialog1 = new PrintPreviewDialog())
			{
				PrintDocument printDocumentDeckList = new PrintDocument();
				SetPrinterSettings(printDocumentDeckList);

				printDialog1.Document = printDocumentDeckList;

				printDocumentDeckList.PrintPage += new PrintPageEventHandler(printDocumentDeckList_PrintPage);

				if (printDialog1.ShowDialog() == DialogResult.OK)
					printDocumentDeckList.Print();
			}  
        }

		/// <summary>
		/// Set paper format
		/// </summary>
		/// <param name="doc"></param>
		private void SetPrinterSettings(PrintDocument doc)
		{
			foreach (PaperSize ps in doc.PrinterSettings.PaperSizes)
			{
				if (ps.Kind == PaperKind.A4)
				{
					doc.DefaultPageSettings.PaperSize = ps;
				}
			}
		}

        private void printDocumentProxyCardList_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;

            float pageWidth = e.PageSettings.PrintableArea.Right;
            float pageHeight = e.PageSettings.PrintableArea.Bottom;

			float startX = e.PageSettings.PrintableArea.Left;
			float startY = e.PageSettings.PrintableArea.Top;
			float offsetX = 0;
			float offsetY = 0;
			int heighestCardInRow = 0;
			float border = 8;

            int lastId = -1;
            Image img = null;
            for (int i = proxyCardListIndex; i < proxyCardList.Count; i++)
            {
				proxyCardListIndex = i;

                if (lastId != proxyCardList[i])
                {
                    lastId = proxyCardList[i];
                    img = Helper.GetMagicImage(proxyCardList[i].ToString());
                }

				if (startX + offsetX + img.Width > pageWidth)
				{
					offsetX = 0;
					offsetY += heighestCardInRow + border;
				}

				if (startY + offsetY + img.Height > pageHeight)
				{
					heighestCardInRow = img.Height;
					e.HasMorePages = true;
					return;
				}

				if (img.Height > heighestCardInRow)
					heighestCardInRow = img.Height;

				graphic.DrawImage(img, startX + offsetX, startY + offsetY, img.Width, img.Height);
				offsetX += img.Width + border;
            }
        }

        private void printDocumentDeckList_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;
            SolidBrush brush = new SolidBrush(Color.Black);

            Font font = new Font("Courier New", 12);
			float fontHeight = font.GetHeight();
			
			float pageHeight = e.PageSettings.PrintableArea.Bottom;
			float startX = e.PageSettings.PrintableArea.Left;
			float startY = e.PageSettings.PrintableArea.Top;
			float offsetY = 0;
			float space = 8;

			float sideboard = graphic.MeasureString(deckCardList.Any(sel => sel.IsInSideboard) ? "SB " : "", font).Width;
			float lengthAmount = graphic.MeasureString(deckCardList.Max(sel => sel.Amount).ToString(), font).Width + sideboard;

            for (int i = deckCardListIndex; i < deckCardList.Count; i++)
            {
                deckCardListIndex = i;

				if (startY + offsetY + fontHeight > pageHeight)
				{
					e.HasMorePages = true;
					return;
				}

				if (deckCardList[i].IsInSideboard)
				{
					graphic.DrawString("SB ", font, brush, startX, startY + offsetY);
				}
                // Amount
				graphic.DrawString(deckCardList[i].Amount.ToString(), font, brush, startX + sideboard, startY + offsetY);
                // Name
				graphic.DrawString(deckCardList[i].Name, font, brush, startX + lengthAmount + space, startY + offsetY);

                offsetY += fontHeight;
            }
        }
    }
}
