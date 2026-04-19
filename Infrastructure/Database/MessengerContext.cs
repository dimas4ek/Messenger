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
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<ChatParticipant> ChatParticipants => Set<ChatParticipant>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Friendship> Friends => Set<Friendship>();
    public DbSet<FriendRequest> FriendRequests => Set<FriendRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Username).IsUnique();
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(u => u.Status)
                .HasColumnType("user_status")
                .HasDefaultValue(UserStatus.Offline);
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Type)
                .HasColumnType("chat_type")
                .HasDefaultValue(ChatType.Private);
            entity.Property(c => c.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(c => c.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<ChatParticipant>(entity =>
        {
            entity.HasKey(p => new { p.ChatId, p.ParticipantId });
            entity.Property(p => p.Role)
                .HasColumnType("chat_participation_role")
                .HasDefaultValue(ChatParticipationRole.Member);
            entity.Property(p => p.JoinedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(p => p.Chat)
                .WithMany(c => c.Participants)
                .HasForeignKey(p => p.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Participant)
                .WithMany(u => u.Participations)
                .HasForeignKey(p => p.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.IsEdited).HasDefaultValue(false);
            entity.Property(m => m.IsRead).HasDefaultValue(false);
            entity.Property(m => m.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(m => m.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
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

        modelBuilder.Entity<FriendRequest>(entity =>
        {
            entity.HasKey(fr => fr.Id);

            entity.HasOne(fr => fr.Sender)
                .WithMany()
                .HasForeignKey(fr => fr.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(fr => fr.Receiver)
                .WithMany()
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(fr => fr.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        base.OnModelCreating(modelBuilder);
    }
}