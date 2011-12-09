// -----------------------------------------------------------------------
// <copyright file="VoterList.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.PdfGenerator
{
    using System;
    using PdfSharp;
    using PdfSharp.Drawing.Layout;
    using PdfSharp.Pdf;
    using PdfSharp.Drawing;
    using System.Diagnostics.Contracts;

    using SmallTuba.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VoterList
    {
        private const int topMargin = 100, bottonMargin = 50, rightMargin = 50, leftMargin = 50;
        private uint count, rows;
        private double nameFieldX, cprnrFieldX, voternrFieldX, rowDistance;
        private string electionName, electionDate;
        private int pollingTable;
        private XFont font;
        private XGraphics gfx;
        private PdfDocument document;

        //May I have a new voting list for this election?
        public VoterList(uint rows, string electionName, string electionDate, int pollingTable)
        {
            Contract.Requires(rows > 20);
            Contract.Requires(electionName != null);
            Contract.Requires(electionDate != null);
            Contract.Requires(pollingTable > 0);

            this.rows = rows;
            this.electionName = electionName;
            this.electionDate = electionDate;
            this.pollingTable = pollingTable;
            document = new PdfDocument();
            this.AddPage();
            font = this.CreateFont();
        }

        private XFont CreateFont()
        {
            int fontsize = 1;
            XFont font = new XFont("Arial", fontsize, XFontStyle.Regular);
            while (gfx.MeasureString("ABC", font).Height < rowDistance - 2 && fontsize <= 12)
            {
                font = new XFont("Arial", fontsize, XFontStyle.Regular);
                fontsize++;
            }
            return font;
        }

        private void AddPage()
        {
            if (gfx != null)
            {
                gfx.Dispose();
            }
            PdfPage page = document.AddPage();
            page.Size = PageSize.A4;
            gfx = XGraphics.FromPdfPage(page);
            count = 0;
            this.DrawTemplate(page);
        }

        private void DrawTemplate(PdfPage page)
        {
            XRect rect = this.DrawOuterFrame(page);
            this.DrawInnerFrame(rect);
            this.DrawColumnDescriptions(rect);
            this.SetDescriptionFieldsPostitions(rect);
            this.DrawElectionInformation();
        }

        private XRect DrawOuterFrame(PdfPage page)
        {
            XPen penBold = new XPen(XColor.FromName("Black"), 1.0);
            XRect rect = new XRect(leftMargin, topMargin, page.Width - rightMargin - leftMargin, page.Height - bottonMargin - topMargin);
            gfx.DrawRectangle(penBold, rect);
            return rect;
        }

        private void DrawInnerFrame(XRect rect)
        {
            XPen penRegular = new XPen(XColor.FromName("Black"), 0.5);
            gfx.DrawLine(penRegular, rect.TopLeft.X + rect.Width / 2, rect.TopLeft.Y, rect.TopLeft.X + rect.Width / 2, rect.BottomLeft.Y);
            gfx.DrawLine(penRegular, rect.TopLeft.X + rect.Width * 0.75, rect.TopLeft.Y, rect.TopLeft.X + rect.Width * 0.75, rect.BottomLeft.Y);
            for (double i = rect.TopLeft.Y + (rect.Height / rows); i < rect.BottomLeft.Y; i += rect.Height / rows)
            {
                gfx.DrawLine(penRegular, rect.TopLeft.X, i, rect.TopRight.X, i);
            }
        }

        private void DrawColumnDescriptions(XRect rect)
        {
            XFont font = new XFont("Arial", 10, XFontStyle.Bold);
            gfx.DrawString("Navn", font, XBrushes.Black, 160, rect.TopLeft.Y - 2);
            gfx.DrawString("CPR nr.", font, XBrushes.Black, 340, rect.TopLeft.Y - 2);
            gfx.DrawString("Vælgernr.", font, XBrushes.Black, 460, rect.TopLeft.Y - 2);
        }

        private void SetDescriptionFieldsPostitions(XRect rect)
        {
            rowDistance = rect.Height / rows;
            nameFieldX = leftMargin + 3;
            cprnrFieldX = rect.TopLeft.X + rect.Width / 2 + 3;
            voternrFieldX = rect.TopLeft.X + rect.Width * 0.75 + 3;
        }

        private void AddPageNumbers()
        {
            this.gfx.Dispose();
            PdfPages pages = document.Pages;
            for (int i = 0; i < document.PageCount; i++)
            {
                int pageNumber = i + 1;
                PdfPage page = pages[i];
                String text = "Side " + pageNumber + " af " + document.PageCount;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                gfx.DrawString(text, font, XBrushes.Black, page.Width/2-(gfx.MeasureString(text, font).Width/2) , page.Height-bottonMargin/2);
            }
        }

        private void DrawElectionInformation()
        {
            XFont font = new XFont("Arial", 12, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);
            String text = electionName + System.Environment.NewLine + this.electionDate;
            tf.DrawString(text, font, XBrushes.Black, new XRect(leftMargin,topMargin/3,200,50));
            tf.DrawString("bord "+ this.pollingTable.ToString(), font, XBrushes.Black, new XRect(gfx.PageSize.Width/2, topMargin/3, 200, 50));
        }

        //Can you save the voting list to this location on the harddrive?
        public void AddVoter(Person person)
        {
            Contract.Requires(person != null);

            if (count == rows)
            {
                AddPage();
            }
            count++;
            double positionY = topMargin + (rowDistance * count) - 2;
            gfx.DrawString(person.FirstName + " " + person.LastName, font, XBrushes.Black, nameFieldX, positionY);
            gfx.DrawString(person.Cpr.ToString(), font, XBrushes.Black, cprnrFieldX, positionY);
            gfx.DrawString(person.VoterId.ToString(), font, XBrushes.Black, voternrFieldX, positionY);
        }

        //Can you save the voting list to this location on the harddrive?
        public void SaveToDisk(String path)
        {
            Contract.Requires(path != null);
            this.AddPageNumbers();
            Console.WriteLine(path);
            document.Save(path);
        }
     
    }
}
