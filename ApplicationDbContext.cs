using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class ApplicationDbContext : DbContext
{
    public DbSet<Message> Messages { get; set; }
    public DbSet<Intent> Intents { get; set; }
    public DbSet<SentEmail> SentEmails { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}

public class Message
{
    public int MessageID { get; set; }
    public int ConversationID { get; set; }
    public required string Sender { get; set; }
    public required string MessageText { get; set; }
    public DateTime Timestamp { get; set; }
}

public class Intent
{
    public int IntentID { get; set; }
    public required string IntentName { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SentEmail
{
    [Key]
    public required string SentTo { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
    public DateTime SentAt { get; set; }
}
