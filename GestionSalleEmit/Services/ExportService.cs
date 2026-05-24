using ClosedXML.Excel;
using GestionSalleEmit.DTOs.EmploiDuTemps;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GestionSalleEmit.Services
{
    public class ExportService : IExportService
    {

        public byte[] ExportToExcel(List<EmploiDuTempsResponseDTO> data)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("EmploiDuTemps");

            // Header
            ws.Cell(1, 1).Value = "Jour";
            ws.Cell(1, 2).Value = "Heure Debut";
            ws.Cell(1, 3).Value = "Heure Fin";
            ws.Cell(1, 4).Value = "Salle";
            ws.Cell(1, 5).Value = "Enseignant";
            ws.Cell(1, 6).Value = "Matière";

            int row = 2;

            foreach (var item in data)
            {
                ws.Cell(row, 1).Value = item.Jour;
                ws.Cell(row, 2).Value = item.HeureDebut;
                ws.Cell(row, 3).Value = item.HeureFin;
                ws.Cell(row, 4).Value = item.Salle;
                ws.Cell(row, 5).Value = item.Enseignant;
                ws.Cell(row, 6).Value = item.Matiere;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        public byte[] ExportToPdf(List<EmploiDuTempsResponseDTO> data)
        {
            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/emit.png");
            var logoBytes = File.Exists(logoPath) ? File.ReadAllBytes(logoPath) : null;
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);

                    page.Header().Row(row =>
                    {
                        row.ConstantItem(80).Height(60).Image(logoBytes);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("ECOLE DE MANAGEMENT ET D'INNOVATION TECHNOLOGIQUE")
                                .FontSize(14)
                                .Bold();

                            col.Item().Text("EMPLOI DU TEMPS")
                                .FontSize(20)
                                .Bold()
                                .FontColor(Colors.Blue.Medium);

                        });
                    });

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(80);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        // HEADER STYLÉ
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Jour").FontColor(Colors.White);

                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Début").FontColor(Colors.White);

                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Fin").FontColor(Colors.White);

                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Salle").FontColor(Colors.White);

                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Enseignant").FontColor(Colors.White);

                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Matière").FontColor(Colors.White);
                        });

                        // LIGNES
                        foreach (var item in data)
                        {
                            table.Cell().BorderBottom(1).Padding(5).Text(item.Jour);
                            table.Cell().BorderBottom(1).Padding(5).Text(item.HeureDebut);
                            table.Cell().BorderBottom(1).Padding(5).Text(item.HeureFin);
                            table.Cell().BorderBottom(1).Padding(5).Text(item.Salle ?? "");
                            table.Cell().BorderBottom(1).Padding(5).Text(item.Enseignant ?? "");
                            table.Cell().BorderBottom(1).Padding(5).Text(item.Matiere ?? "");
                        }
                    });
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                    });
                });

            return document.GeneratePdf();
        }
    }
}
