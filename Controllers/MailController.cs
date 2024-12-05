using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class MailController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly EmailService _emailService;

    public MailController(ApplicationDbContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost("CheckMessages")]
    public IActionResult CheckMessages()
    {
        // Lấy tất cả messages và intents từ DB
        var messages = _context.Messages.ToList();
        var intents = _context.Intents.Select(i => i.IntentName.ToLower()).ToHashSet();

        // Kiểm tra message không có trong intent
        var missingIntents = messages
            .GroupBy(m => m.MessageText.ToLower())
            .Where(g => g.Count() >= 10 && !intents.Contains(g.Key))
            .Select(g => g.Key)
            .ToList();

        if (missingIntents.Any())
        {
            string body = $"Các từ sau không tồn tại trong intents: {string.Join(", ", missingIntents)}";
            string subject = "Cập nhật Intent";
            string recipient = "2154810034@vaa.edu.vn";

            // Kiểm tra nếu email đã được gửi với nội dung tương tự
            bool emailExists = _context.SentEmails.Any(e =>
                e.SentTo == recipient &&
                e.Subject == subject &&
                e.Body == body);

            if (!emailExists)
            {
                // Gửi email
                _emailService.SendEmail(recipient, subject, body);

                // Lưu thông tin email đã gửi vào DB
                _context.SentEmails.Add(new SentEmail
                {
                    SentTo = recipient,
                    Subject = subject,
                    Body = body,
                    SentAt = DateTime.UtcNow
                });
                _context.SaveChanges();

                return Ok("Email đã được gửi.");
            }

            return Ok("Email đã được gửi trước đó.");
        }

        return Ok("Không có từ nào cần cập nhật.");
    }

}
