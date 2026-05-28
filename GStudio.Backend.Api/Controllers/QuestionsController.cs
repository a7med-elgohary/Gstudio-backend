using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GStudio.Backend.Infrastructure.Persistence;
using GStudio.Backend.Domain.Entities;

namespace GStudio.Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class QuestionItem
        {
            public string Label { get; set; } = string.Empty;
            public string Placeholder { get; set; } = string.Empty;
            public string Subtext { get; set; } = string.Empty;
        }

        private static readonly Dictionary<string, QuestionItem> DefaultQuestions = new Dictionary<string, QuestionItem>
        {
            { "clientName", new QuestionItem { Label = "اسم العميل *", Placeholder = "الاسم بالكامل..." } },
            { "phone", new QuestionItem { Label = "رقم الهاتف *", Placeholder = "رقم التواصل..." } },
            { "company", new QuestionItem { Label = "اسم الشركة وعنوانها *", Placeholder = "مثال: شركة تيك للأبحاث، الرياض..." } },
            { "location", new QuestionItem { Label = "أين يتواجد المستخدمون؟ وأين يقع نشاطك التجاري؟ *", Placeholder = "المدينة، الدولة، نطاق الاستهداف..." } },
            { "tagline", new QuestionItem { Label = "هل لدى علامتك التجارية جملة تعريفية (Tagline)؟ *", Placeholder = "نعم/لا - اكتبها إن وجدت..." } },
            { "category", new QuestionItem { Label = "إلى أي فئة ينتمي منتجك أو خدمتك؟ *", Placeholder = "عقارات، تقنية، طعام..." } },
            { "brandName", new QuestionItem { Label = "ما الاسم الأساسي لعلامتك التجارية؟ *", Placeholder = "اسم البراند..." } },
            { "nameMeaning", new QuestionItem { Label = "ما معنى الاسم ولماذا اخترته؟ *", Placeholder = "سبب التسمية..." } },
            { "logoName", new QuestionItem { Label = "ما الاسم الذي ترغب في استخدامه على الشعار؟ (وبأي لغات؟) *", Placeholder = "الاسم بالشعار واللغات المفضلة..." } },
            { "decisionMakers", new QuestionItem { Label = "من هم صناع القرار؟ وما العوامل التي تؤثر على قراراتهم؟ *", Placeholder = "العملاء المستهدفين والأسباب المؤثرة في الشراء..." } },
            { "industryTraits", new QuestionItem { Label = "ما هي خصائص المجال أو الصناعة التي ينتمي إليها منتجك؟ *", Placeholder = "أبرز تحديات ومميزات السوق الخاص بك..." } },
            { "deadline", new QuestionItem { Label = "متى تحتاج تسليم المشروع؟ وهل التواريخ مرنة؟ *", Placeholder = "المدد الزمنية والموعد النهائي..." } },
            { "vision", new QuestionItem { Label = "ما هي رؤية نشاطك التجاري؟ وأين تريد أن تصل به مستقبلاً؟ *", Placeholder = "تطلعاتك للمستقبل ورؤية العمل..." } },
            { "foundingDate", new QuestionItem { Label = "متى تم تأسيس عملك؟ *", Placeholder = "" } },
            { "launchDate", new QuestionItem { Label = "التاريخ المتوقع لإطلاق الهوية الجديدة؟ *", Placeholder = "" } },
            { "scope", new QuestionItem { Label = "ما المخرجات التصميمية التي تحتاجها بالتحديد؟ *", Placeholder = "اكتب المخرجات بالتفصيل...", Subtext = "(مثل الشعار، الهوية البصرية، ملفات السوشيال ميديا، إلخ...)" } },
            { "targetAudience", new QuestionItem { Label = "من هم المستخدمون أو العملاء المستهدفون؟ *", Placeholder = "تفاصيل العملاء المستهدفين...", Subtext = "صفهم من حيث الجنس، الفئة العمرية، المهنة، الاحتياجات، نقاط الألم، والجنسية." } },
            { "competitors", new QuestionItem { Label = "من هم منافسوك الرئيسيون؟ *", Placeholder = "المنافسين والروابط...", Subtext = "أدرج أسماءهم وروابط مواقعهم وحساباتهم على وسائل التواصل الاجتماعي." } },
            { "logoTypes", new QuestionItem { Label = "قم بإختيار نوع اللوجو المفضل لديك *", Placeholder = "أخرى (حدد إن وجد)..." } },
            { "colors", new QuestionItem { Label = "الألوان المفضلة *", Placeholder = "ألوان أخرى..." } },
            { "selectedTraits", new QuestionItem { Label = "شخصية الجمهور / العلامة *", Placeholder = "", Subtext = "يمكنك اختيار أكثر من صفة" } },
            { "likedLogosReason", new QuestionItem { Label = "شعارات تعجبك؟ *", Placeholder = "لماذا تعجبك هذه الشعارات...", Subtext = "أدرج بعض الشعارات التي تعجبك من علامات أخرى، واذكر سبب الإعجاب." } },
            { "dislikes", new QuestionItem { Label = "أشياء تُعجبك أو لا تُعجبك في الشعارات؟ *", Placeholder = "تفاصيل ما تفضله وتكرهه...", Subtext = "اذكر رموز نحتاج لاعتبارها، وعناصر يجب ألا نستخدمها نهائياً." } },
            { "additionalInfo", new QuestionItem { Label = "هل هناك أي تفاصيل إضافية؟ *", Placeholder = "تفاصيل إضافية...", Subtext = "أي أفكار وملاحظات مهمة لتصميم الهوية البصرية." } },
            { "supportFiles", new QuestionItem { Label = "هل هناك أي صور أو ملفات تود إرفاقها؟", Placeholder = "", Subtext = "نماذج أعجبتك، صور ملهمة، أو أي ملفات مساعدة." } }
        };

        [HttpGet]
        public async Task<IActionResult> GetQuestions()
        {
            try
            {
                var dbQuestions = await _context.CustomQuestions.ToListAsync();
                var dbDict = dbQuestions.ToDictionary(q => q.Key, q => new QuestionItem 
                { 
                    Label = q.Label, 
                    Placeholder = q.Placeholder, 
                    Subtext = q.Subtext 
                });

                var merged = new Dictionary<string, QuestionItem>();
                foreach (var kvp in DefaultQuestions)
                {
                    if (dbDict.TryGetValue(kvp.Key, out var val))
                    {
                        merged[kvp.Key] = val;
                    }
                    else
                    {
                        merged[kvp.Key] = kvp.Value;
                    }
                }
                return Ok(merged);
            }
            catch
            {
                return Ok(DefaultQuestions);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateQuestions([FromBody] Dictionary<string, QuestionItem> questions)
        {
            if (questions == null) return BadRequest("Questions data is null");

            var dbQuestions = await _context.CustomQuestions.ToListAsync();

            foreach (var kvp in questions)
            {
                var existing = dbQuestions.FirstOrDefault(q => q.Key == kvp.Key);
                if (existing != null)
                {
                    existing.Label = kvp.Value.Label ?? string.Empty;
                    existing.Placeholder = kvp.Value.Placeholder ?? string.Empty;
                    existing.Subtext = kvp.Value.Subtext ?? string.Empty;
                    existing.ModifiedAt = DateTime.UtcNow;
                    _context.CustomQuestions.Update(existing);
                }
                else
                {
                    var newQ = new CustomQuestion
                    {
                        Key = kvp.Key,
                        Label = kvp.Value.Label ?? string.Empty,
                        Placeholder = kvp.Value.Placeholder ?? string.Empty,
                        Subtext = kvp.Value.Subtext ?? string.Empty,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _context.CustomQuestions.AddAsync(newQ);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Questions updated successfully" });
        }
    }
}
