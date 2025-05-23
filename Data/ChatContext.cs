using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data;

public class ChatContext(DbContextOptions<ChatContext> options) : DbContext(options)
{
    public DbSet<ChatMessage> ChatMessages { get; set; }
}
