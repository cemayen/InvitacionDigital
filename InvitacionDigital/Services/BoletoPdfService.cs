using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InvitacionDigital.Services
{
    public class BoletoPdfService
    {
        public static byte[] GenerarBoletoPdf(string nombreFamilia, string nombreInvitado)
        {
            // Configurar licencia comunitaria gratuita de QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Tamaño personalizado para el boleto (ej. Pase tipo tarjeta vertical u horizontal)
                    page.Size(PageSizes.A6.Landscape());
                    page.Margin(15);
                    page.PageColor(Colors.Grey.Lighten4);
                    page.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Grey.Darken3));

                    page.Content().Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .Background(Colors.White)
                        .Padding(20)
                        .Column(column =>
                        {
                            column.Spacing(10);

                            // Encabezado
                            column.Item().Text("PASE DERECHO DE ENTRADA")
                                .FontSize(16)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Amber.Darken3);

                            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            // Nombre de la Familia
                            column.Item().Text(text =>
                            {
                                text.Span("Familia: ").Bold();
                                text.Span(nombreFamilia);
                            });

                            // Nombre del Invitado (Asistente)
                            column.Item().Background(Colors.Grey.Lighten3)
                                .Padding(10)
                                .Column(invCol =>
                                {
                                    invCol.Item().Text("INVITADO CONFIRMADO:").FontSize(9).Bold().FontColor(Colors.Grey.Darken1);
                                    invCol.Item().Text(nombreInvitado).FontSize(16).Bold().FontColor(Colors.Black);
                                });

                            column.Item().Text("Presenta este pase el día del evento.").FontSize(9).Italic().AlignCenter();
                        });
                });
            });

            return documento.GeneratePdf();
        }
    }
}