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
        /// Print list of form Id;Amount
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

            //PrintDialog printDialog1 = new PrintDialog();
            PrintPreviewDialog printDialog1 = new PrintPreviewDialog();
            PrintDocument printDocumentProxyCardList = new PrintDocument();

            printDialog1.Document = printDocumentProxyCardList;
            printDocumentProxyCardList.PrintPage += new PrintPageEventHandler(printDocumentProxyCardList_PrintPage);

            DialogResult result = printDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                printDocumentProxyCardList.Print();
            }    
        }

        private void PrintDeckList()
        {
            deckCardListIndex = 0;

            //PrintDialog printDialog1 = new PrintDialog();
            PrintPreviewDialog printDialog1 = new PrintPreviewDialog();
            PrintDocument printDocumentDeckList = new PrintDocument();

            printDialog1.Document = printDocumentDeckList;
            printDocumentDeckList.PrintPage += new PrintPageEventHandler(printDocumentDeckList_PrintPage);

            DialogResult result = printDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                printDocumentDeckList.Print();
            }  
        }

        private void printDocumentProxyCardList_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;

            e.PageSettings.PaperSize = new PaperSize("A4", 850, 1100);

            float pageWidth = e.PageSettings.PrintableArea.Width;
            float pageHeight = e.PageSettings.PrintableArea.Height;

            int startX = 40;
            int startY = 30;
            int offsetY = 40;

            int lastId = -1;
            Image lastImg = null;
            for (int i = proxyCardListIndex; i < proxyCardList.Count; i++)
            {
                proxyCardListIndex = i;

                if (lastId != proxyCardList[i])
                {
                    lastId = proxyCardList[i];
                    lastImg = Helper.GetMagicImage(proxyCardList[i].ToString());
                }

                graphic.DrawImage(lastImg, startX + 20, startY + offsetY);

                offsetY += lastImg.Height + 10;

                if (offsetY >= pageHeight)
                {
                    e.HasMorePages = true;
                    return;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }
        }

        private void printDocumentDeckList_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;
            SolidBrush brush = new SolidBrush(Color.Black);

            Font font = new Font("Courier New", 12);
            float fontHeight = font.GetHeight();

            e.PageSettings.PaperSize = new PaperSize("A4", 850, 1100);

            float pageWidth = e.PageSettings.PrintableArea.Width;
            float pageHeight = e.PageSettings.PrintableArea.Height;

            int startX = 40;
            int startY = 30;
            int offsetY = 40;

            for (int i = deckCardListIndex; i < deckCardList.Count; i++)
            {
                deckCardListIndex = i;

                // Amount
                graphic.DrawString(deckCardList[i].Amount.ToString(), font, brush, startX, startY + offsetY);
                // Name
                graphic.DrawString(deckCardList[i].Name, font, brush, startX + 20, startY + offsetY);
                // ?

                offsetY += (int)fontHeight;

                if (offsetY >= pageHeight)
                {
                    e.HasMorePages = true;
                    return;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }
        }
    }
}
