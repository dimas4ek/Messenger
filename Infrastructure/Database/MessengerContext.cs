using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class MessengerContext : DbContext
{
    public MessengerContext()
    {
    }

    public MessengerContext(DbContextOptions<MessengerContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<ConversationParticipant> ConversationParticipants => Set<ConversationParticipant>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Friendship> Friends => Set<Friendship>();

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = "Host=localhost;Port=5432;Database=messenger_db;Username=postgres;Password=root";

            optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MapEnum<UserStatus>("user_status");
                npgsqlOptions.MapEnum<ParticipationRole>("participation_role");
                npgsqlOptions.MapEnum<ConversationType>("conversation_type");
            });

            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            optionsBuilder.EnableSensitiveDataLogging(); //DEV!!!
        }
    }*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Status)
                .HasColumnType("user_status")
                .HasDefaultValue(UserStatus.Offline);
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type)
                .HasColumnType("conversation_type")
                .HasDefaultValue(ConversationType.Private);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<ConversationParticipant>(entity =>
        {
            entity.HasKey(e => new { e.ConversationId, e.ParticipantId });
            entity.Property(e => e.Role)
                .HasColumnType("participation_role")
                .HasDefaultValue(ParticipationRole.Member);
            entity.Property(e => e.JoinedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.Conversation)
                .WithMany(c => c.Participants)
                .HasForeignKey(e => e.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Participant)
                .WithMany(c => c.Participations)
                .HasForeignKey(e => e.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IsEdited).HasDefaultValue(false);
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(e => e.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(e => e.SenderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Friendship>(entity =>
        {
            entity.HasKey(fl => new { fl.UserId, fl.FriendId });

            entity.HasOne(fl => fl.User)
                .WithMany(fl => fl.Friends)
                .HasForeignKey(fl => fl.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(fl => fl.Friend)
                .WithMany(fl => fl.AddedByFriends)
                .HasForeignKey(fl => fl.FriendId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        base.OnModelCreating(modelBuilder);
    }
}