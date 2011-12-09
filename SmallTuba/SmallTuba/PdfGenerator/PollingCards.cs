// -----------------------------------------------------------------------
// <copyright file="PDFGenerator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.PdfGenerator
{
    using PdfSharp.Drawing.BarCodes;
    using PdfSharp.Drawing.Layout;
    using PdfSharp.Pdf;
    using PdfSharp.Drawing;
    using System.Diagnostics.Contracts;

    using SmallTuba.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PollingCards
    {
        private const int width = 210, height = 100;
        private PdfDocument document;
        private XForm template;

        //May I have a new polling cards generator for this election?
        public PollingCards(string electionName, string electionDate, string electionTime)
        {
            Contract.Requires(electionName != null);
            Contract.Requires(electionDate != null);
            Contract.Requires(electionTime != null);

            document = new PdfDocument();
            template = new XForm(document,XUnit.FromMillimeter(width), XUnit.FromMillimeter(height));

            XGraphics gfx = XGraphics.FromForm(template);
            AddWatermark(gfx);
            DrawGraphics(gfx);
            ElectionDetails(gfx, electionName, electionDate, electionTime);
            Descriptions(gfx);
            
            gfx.Dispose();
       
        }

        //Create a polling card for this person!
        public void CreatePollingCard(Person person, Address municipality, Address pollingVenue)
        {
            Contract.Requires(person != null);

            PdfPage page = document.AddPage();
            page.Width = XUnit.FromMillimeter(width);
            page.Height = XUnit.FromMillimeter(height);
            XGraphics gfx = XGraphics.FromPdfPage(page);
            
            gfx.DrawImage(template, 0,0);
            FromField(gfx, municipality.Name, municipality.Street, municipality.City);
            ToField(gfx, person.FirstName + " " + person.LastName, person.Street, person.City);
            VotingTable(gfx, person.PollingTable);
            VotingNumber(gfx, person.VoterId.ToString());         
            
            VoterVenue(gfx, pollingVenue.Name, pollingVenue.Street, pollingVenue.City);
            gfx.Dispose();
        }

        //Can you save all the polling card on this location on the harddrive?
        public void SaveToDisk(string path)
        {
            Contract.Requires(path != null);
            document.Save(path);
        }

        private void AddWatermark(XGraphics gfx)
        {
            gfx.RotateTransform(-40);
            XFont font = new XFont("Arial Rounded MT Bold", 60, XFontStyle.Regular);
            XBrush brush = new XSolidBrush(XColor.FromArgb(70, 255, 0, 0));
            gfx.DrawString("VALGKORT", font, brush, -120, 250);
            gfx.RotateTransform(40);
            
        }

        private void FromField(XGraphics gfx, string line1, string line2, string line3)
        {
            XFont font = new XFont("Lucida Console", 8, XFontStyle.Italic);
            XTextFormatter tf = new XTextFormatter(gfx);
            string adress = line1 + System.Environment.NewLine + line2 + System.Environment.NewLine + line3;
            tf.DrawString(adress, font, XBrushes.Black, new XRect(310, 95, 100, 50));
        }

        private void ToField(XGraphics gfx, string line1, string line2, string line3)
        {
            XFont font = new XFont("Lucida Console", 8, XFontStyle.Regular);
            XTextFormatter tf = new XTextFormatter(gfx);
            string adress = line1 + System.Environment.NewLine + line2 + System.Environment.NewLine + line3;
            tf.DrawString(adress, font, XBrushes.Black, new XRect(310, 155, 100, 50));
        }

        private void Descriptions(XGraphics gfx)
        {
            XFont font = new XFont("Arial", 5, XFontStyle.Regular);
            gfx.DrawString("Afstemningssted:", font, XBrushes.Black, 40, 90);
            gfx.DrawString("Valgbord:", font, XBrushes.Black, 40, 162);
            gfx.DrawString("Vælgernr.:", font, XBrushes.Black, 40, 192);
            gfx.DrawString("Afstemningstid:", font, XBrushes.Black, 40, 222);
            gfx.DrawString("Afsender:", font, XBrushes.Black, 305, 90);
            gfx.DrawString("Modtager:", font, XBrushes.Black, 305, 150);
        }

        private void ElectionDetails(XGraphics gfx, string electionName, string electionDate, string electionTime)
        {
            XFont font = new XFont("Arial", 12, XFontStyle.Bold);
            gfx.DrawString(electionName, font, XBrushes.Black, 35, 40);
            gfx.DrawString(electionDate, font, XBrushes.Black, 35, 55);

            ElectionTime(gfx, electionTime);

            XFont font2 = new XFont("Arial", 8, XFontStyle.BoldItalic);
            gfx.DrawString("Medbring kortet ved afstemningen", font2, XBrushes.Black, 35, 75);
            
        }

        private void VoterVenue(XGraphics gfx, string line1, string line2, string line3)
        {
            XFont font = new XFont("Arial", 9, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);
            string adress = line1 + System.Environment.NewLine + line2 + System.Environment.NewLine + line3;
            tf.DrawString(adress, font, XBrushes.Black, new XRect(45, 95, 100, 50));
        }

        private void VotingTable(XGraphics gfx, string table)
        {
            XFont font = new XFont("Arial", 9, XFontStyle.Bold);
            gfx.DrawString(table, font, XBrushes.Black, 80, 162);
        }

        private void VotingNumber(XGraphics gfx, string votingNumber)
        {
            XFont font = new XFont("Arial", 9, XFontStyle.Bold);
            gfx.DrawString(votingNumber, font, XBrushes.Black, 80, 192);
            Barcode(gfx, votingNumber);
        }

        private void ElectionTime(XGraphics gfx, string time)
        {
            XFont font = new XFont("Arial", 9, XFontStyle.Bold);
            gfx.DrawString(time, font, XBrushes.Black, 80, 222);
        }

        private void Barcode(XGraphics gfx, string votingNumber)
        {
            BarCode barcode = new Code3of9Standard();
            barcode.Text = votingNumber;
            barcode.StartChar = '*';
            barcode.EndChar = '*';
            barcode.Size = new XSize(new XPoint(120, 20));
            gfx.DrawBarCode(barcode, XBrushes.Black, new XPoint(310, 40));

            XFont font = new XFont("Lucida Console", 7, XFontStyle.Regular);
            gfx.DrawString(votingNumber, font, XBrushes.Black, 310, 35);
        }

        private void DrawGraphics(XGraphics gfx)
        {
            XPen pen = new XPen(XColor.FromName("Black"), 0.5);
            gfx.DrawRectangle(pen, 35, 80, 220, 60);

            gfx.DrawRectangle(pen, 35, 150, 220, 20);
            gfx.DrawRectangle(pen, 35, 180, 220, 20);
            gfx.DrawRectangle(pen, 35, 210, 220, 20);

            gfx.DrawLine(pen, 300, 20, 300, 250);
            gfx.DrawLine(pen, 35, 250, gfx.PageSize.Width - 20, 250);
                
                
            gfx.DrawLine(pen, 300, 80, gfx.PageSize.Width - 20, 80);
            gfx.DrawLine(pen, 300, 140, gfx.PageSize.Width - 20, 140);
            gfx.DrawLine(pen, 300, 80, 450, 140);
            gfx.DrawLine(pen, 300, 140, 450, 80);
        }

       
    }
}
