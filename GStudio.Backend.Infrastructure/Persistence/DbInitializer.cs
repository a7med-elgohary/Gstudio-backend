using GStudio.Backend.Domain.Entities;
using GStudio.Backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GStudio.Backend.Infrastructure;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // 1. Smart Seed Works
        var worksToSeed = new List<Work>
        {
            new Work { Title = "Al Nmaa Branding", Client = "Al Nmaa", Category = "Branding", Year = "2024", Services = "Branding, UI/UX", Description = "Premium branding for local construction.", Image = "/uploads/images/projects/alnmaa.jpg" },
            new Work { Title = "Dar El Bnaa", Client = "Dar El Bnaa", Category = "Real Estate", Year = "2024", Services = "Web Design", Description = "Luxury real estate platform.", Image = "/uploads/images/projects/dar_elbnaa.jpg" },
            new Work { Title = "GeoPro Identity", Client = "GeoPro", Category = "Corporate", Year = "2023", Services = "Identity", Description = "Global corporate identity.", Image = "/uploads/images/projects/geopro.jpg" },
            new Work { Title = "Kookje App", Client = "Kookje", Category = "E-Commerce", Year = "2025", Services = "Mobile App", Description = "Innovative e-commerce experience.", Image = "/uploads/images/projects/kookje.jpg" },
            new Work { Title = "Zuzu Fashion", Client = "Zuzu", Category = "Fashion", Year = "2024", Services = "Marketing", Description = "Minimalist fashion campaign.", Image = "/uploads/images/projects/zuzu.jpg" }
        };

        foreach (var work in worksToSeed)
        {
            if (!await context.Works.AnyAsync(w => w.Title == work.Title))
            {
                context.Works.Add(work);
            }
        }

        // 2. Smart Seed Services
        var existingServicesCount = await context.Services.CountAsync();
        if (existingServicesCount != 6)
        {
            context.Services.RemoveRange(context.Services);
            await context.SaveChangesAsync();

            var servicesToSeed = new List<ServiceItem>
            {
                new ServiceItem { Num = "01", Title = "Branding", Desc = "Building a complete and distinctive brand identity that reflects your vision and connects with your audience." },
                new ServiceItem { Num = "02", Title = "Brand Guidelines", Desc = "Clear and structured guidelines to ensure consistency across all brand touchpoints and communications." },
                new ServiceItem { Num = "03", Title = "Brand Strategy", Desc = "Defining your brand positioning, voice, and direction to create a strong and lasting impact." },
                new ServiceItem { Num = "04", Title = "Website Design", Desc = "Designing modern, user-focused websites that reflect your brand and deliver a seamless experience." },
                new ServiceItem { Num = "05", Title = "Product Photography", Desc = "Capturing high-quality visuals that highlight your products and elevate your brand image." },
                new ServiceItem { Num = "06", Title = "Packaging", Desc = "Packaging is more than a cover, it’s your product’s first impression. We design packaging that tells your brand’s story before the box is opened." }
            };
            context.Services.AddRange(servicesToSeed);
        }

        // 3. Smart Seed Client Logos (All 37 Artboards)
        for (int i = 1; i <= 37; i++)
        {
            string url = $"/uploads/images/clients/Artboard_{i}.png";
            if (!await context.ClientLogos.AnyAsync(l => l.ImageUrl == url))
            {
                context.ClientLogos.Add(new ClientLogo { ImageUrl = url });
            }
        }

        // 4. Seed or Update Site Content
        var siteContent = await context.SiteContents.FirstOrDefaultAsync();
        if (siteContent == null)
        {
            context.SiteContents.Add(new SiteContent
            {
                HeroVideoUrl = "/uploads/videos/1-jpg.mp4",
                HeroVideoUrlMobile = "/uploads/videos/hero-mobile.mp4",
                HeroImageUrl = "/uploads/images/heroimg.jpg",
                AboutTagline = "GLOBAL CREATIVE STUDIO",
                AboutTitleLine1 = "creative strategy &",
                AboutTitleLine2 = "",
                AboutTitleAccent = "experiences",
                AboutDescription = "We are a boutique creative agency specializing in branding, web development, and digital marketing. We help brands stand out through storytelling.",
                ContactEmail = "hello@gstudio.com",
                ContactWhatsapp = "201012345678",
                ContactLocation = "Cairo, Egypt",
                Stat1Number = "+5",
                Stat1Label = "Years Experience",
                Stat2Number = "+80",
                Stat2Label = "Projects Done",
                Stat3Number = "48",
                Stat3Label = "Active Clients",
                Stat4Number = "7",
                Stat4Label = "Countries"
            });
        }
        else 
        {
            // Force update to prove it's connected to backend
            siteContent.Stat1Number = "+10"; 
            siteContent.Stat1Label = "Years of Magic";
            siteContent.Stat2Number = "+80";
            siteContent.Stat3Number = "48";
            siteContent.Stat4Number = "7";
            siteContent.AboutTagline = "GLOBAL CREATIVE STUDIO";
            
            // Ensure Contact Info is also seeded if empty
            if (string.IsNullOrEmpty(siteContent.ContactEmail)) siteContent.ContactEmail = "hello@gstudio.com";
            if (string.IsNullOrEmpty(siteContent.ContactWhatsapp)) siteContent.ContactWhatsapp = "201012345678";
            if (string.IsNullOrEmpty(siteContent.ContactLocation)) siteContent.ContactLocation = "Cairo, Egypt";

            context.SiteContents.Update(siteContent);
        }

        // 5. Seed Partner Countries
        if (!await context.PartnerCountries.AnyAsync())
        {
            context.PartnerCountries.AddRange(new List<PartnerCountry>
            {
                new PartnerCountry { Name = "Italy", Code = "it" },
                new PartnerCountry { Name = "Canada", Code = "ca" },
                new PartnerCountry { Name = "United Kingdom", Code = "gb" },
                new PartnerCountry { Name = "USA", Code = "us" },
                new PartnerCountry { Name = "Syria", Code = "sy" },
                new PartnerCountry { Name = "Saudi Arabia", Code = "sa" },
                new PartnerCountry { Name = "Egypt", Code = "eg" }
            });
        }

        await context.SaveChangesAsync();

        // 6. Dynamically ensure CustomQuestions table is created and seed default questions
        try
        {
            var createTableSql = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomQuestions]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE [dbo].[CustomQuestions] (
                        [Id] INT IDENTITY(1,1) NOT NULL,
                        [Key] NVARCHAR(450) NOT NULL,
                        [Label] NVARCHAR(MAX) NOT NULL,
                        [Placeholder] NVARCHAR(MAX) NOT NULL,
                        [Subtext] NVARCHAR(MAX) NOT NULL,
                        [CreatedAt] DATETIME2 NOT NULL DEFAULT (GETUTCDATE()),
                        [ModifiedAt] DATETIME2 NULL,
                        CONSTRAINT [PK_CustomQuestions] PRIMARY KEY CLUSTERED ([Id] ASC)
                    );
                END";
            await context.Database.ExecuteSqlRawAsync(createTableSql);

            if (!await context.CustomQuestions.AnyAsync())
            {
                var defaultQuestions = new List<CustomQuestion>
                {
                    new CustomQuestion { Key = "clientName", Label = "اسم العميل *", Placeholder = "الاسم بالكامل...", Subtext = "" },
                    new CustomQuestion { Key = "phone", Label = "رقم الهاتف *", Placeholder = "رقم التواصل...", Subtext = "" },
                    new CustomQuestion { Key = "company", Label = "اسم الشركة وعنوانها *", Placeholder = "مثال: شركة تيك للأبحاث، الرياض...", Subtext = "" },
                    new CustomQuestion { Key = "location", Label = "أين يتواجد المستخدمون؟ وأين يقع نشاطك التجاري؟ *", Placeholder = "المدينة، الدولة، نطاق الاستهداف...", Subtext = "" },
                    new CustomQuestion { Key = "tagline", Label = "هل لدى علامتك التجارية جملة تعريفية (Tagline)؟ *", Placeholder = "نعم/لا - اكتبها إن وجدت...", Subtext = "" },
                    new CustomQuestion { Key = "category", Label = "إلى أي فئة ينتمي منتجك أو خدمتك؟ *", Placeholder = "عقارات، تقنية، طعام...", Subtext = "" },
                    new CustomQuestion { Key = "brandName", Label = "ما الاسم الأساسي لعلامتك التجارية؟ *", Placeholder = "اسم البراند...", Subtext = "" },
                    new CustomQuestion { Key = "nameMeaning", Label = "ما معنى الاسم ولماذا اخترته؟ *", Placeholder = "سبب التسمية...", Subtext = "" },
                    new CustomQuestion { Key = "logoName", Label = "ما الاسم الذي ترغب في استخدامه على الشعار؟ (وبأي لغات؟) *", Placeholder = "الاسم بالشعار واللغات المفضلة...", Subtext = "" },
                    new CustomQuestion { Key = "decisionMakers", Label = "من هم صناع القرار؟ وما العوامل التي تؤثر على قراراتهم؟ *", Placeholder = "العملاء المستهدفين والأسباب المؤثرة في الشراء...", Subtext = "" },
                    new CustomQuestion { Key = "industryTraits", Label = "ما هي خصائص المجال أو الصناعة التي ينتمي إليها منتجك؟ *", Placeholder = "أبرز تحديات ومميزات السوق الخاص بك...", Subtext = "" },
                    new CustomQuestion { Key = "deadline", Label = "متى تحتاج تسليم المشروع؟ وهل التواريخ مرنة؟ *", Placeholder = "المدد الزمنية والموعد النهائي...", Subtext = "" },
                    new CustomQuestion { Key = "vision", Label = "ما هي رؤية نشاطك التجاري؟ وأين تريد أن تصل به مستقبلاً؟ *", Placeholder = "تطلعاتك للمستقبل ورؤية العمل...", Subtext = "" },
                    new CustomQuestion { Key = "foundingDate", Label = "متى تم تأسيس عملك؟ *", Placeholder = "", Subtext = "" },
                    new CustomQuestion { Key = "launchDate", Label = "التاريخ المتوقع لإطلاق الهوية الجديدة؟ *", Placeholder = "", Subtext = "" },
                    new CustomQuestion { Key = "scope", Label = "ما المخرجات التصميمية التي تحتاجها بالتحديد؟ *", Placeholder = "اكتب المخرجات بالتفصيل...", Subtext = "(مثل الشعار، الهوية البصرية، ملفات السوشيال ميديا، إلخ...)" },
                    new CustomQuestion { Key = "targetAudience", Label = "من هم المستخدمون أو العملاء المستهدفون؟ *", Placeholder = "تفاصيل العملاء المستهدفين...", Subtext = "صفهم من حيث الجنس، الفئة العمرية، المهنة، الاحتياجات، نقاط الألم، والجنسية." },
                    new CustomQuestion { Key = "competitors", Label = "من هم منافسوك الرئيسيون؟ *", Placeholder = "المنافسين والروابط...", Subtext = "أدرج أسماءهم وروابط مواقعهم وحساباتهم على وسائل التواصل الاجتماعي." },
                    new CustomQuestion { Key = "logoTypes", Label = "قم بإختيار نوع اللوجو المفضل لديك *", Placeholder = "أخرى (حدد إن وجد)...", Subtext = "" },
                    new CustomQuestion { Key = "colors", Label = "الألوان المفضلة *", Placeholder = "ألوان أخرى...", Subtext = "" },
                    new CustomQuestion { Key = "selectedTraits", Label = "شخصية الجمهور / العلامة *", Placeholder = "", Subtext = "يمكنك اختيار أكثر من صفة" },
                    new CustomQuestion { Key = "likedLogosReason", Label = "شعارات تعجبك؟ *", Placeholder = "لماذا تعجبك هذه الشعارات...", Subtext = "أدرج بعض الشعارات التي تعجبك من علامات أخرى، واذكر سبب الإعجاب." },
                    new CustomQuestion { Key = "dislikes", Label = "أشياء تُعجبك أو لا تُعجبك في الشعارات؟ *", Placeholder = "تفاصيل ما تفضله وتكرهه...", Subtext = "اذكر رموز نحتاج لاعتبارها، وعناصر يجب ألا نستخدمها نهائياً." },
                    new CustomQuestion { Key = "additionalInfo", Label = "هل هناك أي تفاصيل إضافية؟ *", Placeholder = "تفاصيل إضافية...", Subtext = "أي أفكار وملاحظات مهمة لتصميم الهوية البصرية." },
                    new CustomQuestion { Key = "supportFiles", Label = "هل هناك أي صور أو ملفات تود إرفاقها؟", Placeholder = "", Subtext = "نماذج أعجبتك، صور ملهمة، أو أي ملفات مساعدة." }
                };

                await context.CustomQuestions.AddRangeAsync(defaultQuestions);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception)
        {
            // Fail silently or log if needed, similar to the original code in Program.cs
        }
    }
}
