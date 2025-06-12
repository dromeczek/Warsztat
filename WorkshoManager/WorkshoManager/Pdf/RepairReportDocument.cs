using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WorkshoManager.Models;

public class RepairReportDocument : IDocument
{
    private readonly List<Order> _orders;

    public RepairReportDocument(List<Order> orders)
    {
        _orders = orders;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);
            page.Content().Column(column =>
            {
                column.Item().Text("Raport napraw klienta").FontSize(20).Bold().Underline().AlignCenter();

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.ConstantColumn(100);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Pojazd").Bold();
                        header.Cell().Text("Opis").Bold();
                        header.Cell().Text("Koszt").Bold();
                    });

                    foreach (var order in _orders)
                    {
                        var total = order.Tasks.Sum(t =>
                            t.LaborCost + (t.UsedParts?.Sum(up => up.Quantity * up.Part.UnitPrice) ?? 0));

                        table.Cell().Text(order.Vehicle.RegistrationNumber);
                        table.Cell().Text(order.Description);
                        table.Cell().Text($"{total} zł");
                    }
                });
            });
        });
    }
}