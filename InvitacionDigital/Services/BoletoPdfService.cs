using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InvitacionDigital.Services
{
    public class BoletoPdfService
    {
        public static byte[] GenerarBoletoPdf(string nombreFamilia, string nombreInvitado, string rutaImagenWwwroot)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var colorFondoHex = "#f5f3e2";

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Dimensiones proporcionales a 993x567 px (convertidos a puntos pt)
                    page.Size(new PageSize(745, 425));
                    page.Margin(0);
                    page.PageColor(colorFondoHex);

                    page.Content().Layers(layers =>
                    {
                        // Capa 1 (Fondo): La imagen del boleto
                        var fondo = layers.PrimaryLayer().Width(745).Height(425);
                        if (System.IO.File.Exists(rutaImagenWwwroot))
                        {
                            fondo.Image(rutaImagenWwwroot).FitArea();
                        }

                        // Capa 2 (Superpuesta): Nombre del invitado en la esquina inferior derecha
                        layers.Layer()
                            .PaddingLeft(110)
                            .PaddingBottom(30)
                            .AlignBottom()
                            .AlignLeft()
                            .Column(col =>
                            {
                                col.Item().Text(nombreInvitado)
                                    .FontSize(13)
                                    .FontColor("#5c5c5c")
                                    .AlignLeft(); // Corregido: AlignRight en lugar de RightAlign

                                col.Item().PaddingTop(4).Text($"Familia {nombreFamilia}")
                                    .FontSize(12)
                                    .FontColor("#d7b590")
                                    .AlignLeft(); // Corregido: AlignRight en lugar de RightAlign
                                    
                            });
                    });
                });
            }).GeneratePdf();
        }
    }
}