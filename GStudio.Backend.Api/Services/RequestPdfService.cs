using GStudio.Backend.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GStudio.Backend.Api.Services
{
    public class RequestPdfService
    {
        public RequestPdfService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerateRequestPdf(ProjectRequest r)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    
                    // Main font for Arabic support
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(11).FontColor(Colors.Grey.Darken4));

                    // ─── Header ──────────────────────────────────────────
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("G.STUDIO").FontSize(32).ExtraBold().FontColor(Colors.Purple.Medium).LetterSpacing(0.05f);
                            col.Item().Text("PROJECT BRIEF REPORT").FontSize(12).SemiBold().FontColor(Colors.Grey.Medium);
                            col.Item().PaddingTop(5).Text($"ID: #{r.Id} | Date: {r.CreatedAt:yyyy-MM-dd}").FontSize(10).FontColor(Colors.Grey.Lighten1);
                        });

                        row.ConstantItem(150).Column(col => {
                            col.Item().AlignRight().Text("طلب مشروع جديد").FontSize(18).Bold().FontColor(Colors.Purple.Medium);
                            col.Item().AlignRight().Text(r.Category).FontSize(14).SemiBold();
                        });
                    });

                    // ─── Content ─────────────────────────────────────────
                    page.Content().PaddingVertical(20).Column(column =>
                    {
                        column.Spacing(25);

                        // 1. Client Info Section
                        column.Item().Component(new PdfSection("بيانات التواصل الأساسية", Colors.Purple.Lighten5));
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns => {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Cell().Component(new PdfDataField("اسم العميل", r.ClientName));
                            table.Cell().Component(new PdfDataField("رقم الجوال", r.Phone));
                            table.Cell().Component(new PdfDataField("الشركة / المؤسسة", r.Company));
                            table.Cell().Component(new PdfDataField("الموقع / المدينة", r.Location));
                        });

                        // 2. Project Info Section
                        column.Item().Component(new PdfSection("هوية المشروع والبراند", Colors.Purple.Lighten5));
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns => {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Cell().Component(new PdfDataField("اسم البراند", r.BrandName));
                            table.Cell().Component(new PdfDataField("تصنيف المشروع", r.Category));
                            table.Cell().Component(new PdfDataField("الموعد النهائي", r.Deadline));
                            table.Cell().Component(new PdfDataField("أصحاب القرار", r.DecisionMakers));
                            
                            table.Cell().ColumnSpan(2).Component(new PdfDataField("شرح معنى الاسم", r.NameMeaning));
                            table.Cell().ColumnSpan(2).Component(new PdfDataField("التاغ لاين (Tagline)", r.Tagline));
                        });

                        // 3. Business & Vision Section
                        column.Item().Component(new PdfSection("الرؤية والعمل", Colors.Purple.Lighten5));
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns => {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Cell().Component(new PdfDataField("تاريخ التأسيس", r.FoundingDate));
                            table.Cell().Component(new PdfDataField("موعد الإطلاق", r.LaunchDate));
                            table.Cell().ColumnSpan(2).Component(new PdfDataField("نطاق العمل (Scope)", r.Scope));
                            table.Cell().ColumnSpan(2).Component(new PdfDataField("الرؤية والرسالة", r.Vision));
                        });

                        // 4. Audience & Competition
                        column.Item().Component(new PdfSection("الجمهور والمنافسين", Colors.Purple.Lighten5));
                        column.Item().Column(c => {
                            c.Item().Component(new PdfDataField("الجمهور المستهدف", r.TargetAudience));
                            c.Item().Component(new PdfDataField("المنافسين", r.Competitors));
                            c.Item().Component(new PdfDataField("سمات الصناعة", r.IndustryTraits));
                        });

                        // 5. Design Preferences
                        column.Item().Component(new PdfSection("تفضيلات التصميم", Colors.Purple.Lighten5));
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns => {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Cell().Component(new PdfDataField("أنواع الشعارات", r.LogoTypes));
                            table.Cell().Component(new PdfDataField("الألوان المفضلة", r.Colors));
                            table.Cell().Component(new PdfDataField("لون محدد (كود)", r.CustomColor));
                            table.Cell().Component(new PdfDataField("السمات المختارة", r.SelectedTraits));
                            
                            table.Cell().ColumnSpan(2).Component(new PdfDataField("لماذا أعجبتك النماذج المختارة؟", r.LikedLogosReason));
                            table.Cell().ColumnSpan(2).Component(new PdfDataField("أشياء لا تحبها في التصميم", r.Dislikes));
                        });

                        // 6. Additional Info
                        if (!string.IsNullOrEmpty(r.AdditionalInfo))
                        {
                            column.Item().Component(new PdfSection("ملاحظات إضافية", Colors.Purple.Lighten5));
                            column.Item().PaddingTop(5).Text(r.AdditionalInfo).FontSize(12).LineHeight(1.5f);
                        }
                    });

                    // ─── Footer ──────────────────────────────────────────
                    page.Footer().Column(col =>
                    {
                        col.Item().PaddingTop(10).BorderTop(1).BorderColor(Colors.Grey.Lighten3).Row(row =>
                        {
                            row.RelativeItem().Text("Generated by G.Studio Admin Management System").FontSize(9).FontColor(Colors.Grey.Lighten1);
                            row.RelativeItem().AlignRight().Text(x =>
                            {
                                x.Span("Page ").FontSize(9);
                                x.CurrentPageNumber().FontSize(9);
                                x.Span(" of ").FontSize(9);
                                x.TotalPages().FontSize(9);
                            });
                        });
                    });
                });
            }).GeneratePdf();
        }
    }

    // Helper Component for Sections
    public class PdfSection : IComponent
    {
        private string Title { get; }
        private string BgColor { get; }

        public PdfSection(string title, string bgColor)
        {
            Title = title;
            BgColor = bgColor;
        }

        public void Compose(IContainer container)
        {
            container
                .PaddingBottom(5)
                .Background(BgColor)
                .PaddingHorizontal(12)
                .PaddingVertical(8)
                .Row(row =>
                {
                    row.RelativeItem().AlignRight().Text(Title).FontSize(14).Bold().FontColor(Colors.Purple.Medium);
                });
        }
    }

    // Helper Component for Data Fields
    public class PdfDataField : IComponent
    {
        private string Label { get; }
        private string Value { get; }

        public PdfDataField(string label, string value)
        {
            Label = label;
            Value = string.IsNullOrEmpty(value) ? "---" : value;
        }

        public void Compose(IContainer container)
        {
            container.PaddingVertical(5).Column(col =>
            {
                col.Item().AlignRight().Text(Label).FontSize(10).Bold().FontColor(Colors.Grey.Medium);
                col.Item().AlignRight().PaddingTop(2).Text(Value).FontSize(12).FontColor(Colors.Black).LineHeight(1.2f);
            });
        }
    }
}
