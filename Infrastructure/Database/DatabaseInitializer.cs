using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(MessengerContext db)
    {
        var sql = """
        create or replace function update_updated_column_at()
        returns trigger as
        $$
        begin
            NEW.updated_at = current_timestamp;
            return NEW;
        end;
        $$ language plpgsql;

        do $$
        begin
            if not exists (select 1 from pg_type where typname = 'conversation_type') then
                create type conversation_type as enum ('private', 'group');
            end if;

            if not exists (select 1 from pg_type where typname = 'participation_role') then
                create type participation_role as enum ('member', 'admin');
            end if;

            if not exists (select 1 from pg_type where typname = 'user_status') then
                create type user_status as enum ('offline', 'online');
            end if;
        end
        $$;

        create table if not exists users
        (
            id int primary key generated always as identity,
            username varchar(255) not null unique,
            password varchar(255) not null,
            status user_status default 'offline',
            created_at timestamp with time zone default current_timestamp
        );

        create table if not exists conversations
        (
            id int primary key generated always as identity,
            type conversation_type default 'private',
            name varchar(255) null,
            created_at timestamp with time zone default current_timestamp,
            updated_at timestamp with time zone default current_timestamp
        );

        create table if not exists messages
        (
            id int primary key generated always as identity,
            conversation_id int not null references conversations(id) on delete cascade,
            sender_id int not null references users(id) on delete cascade,
            message_text text not null,
            is_edited bool default false,
            is_read bool default false,
            created_at timestamp with time zone default current_timestamp,
            updated_at timestamp with time zone default current_timestamp
        );

        create table if not exists conversation_participants
        (
            conversation_id int not null references conversations(id) on delete cascade,
            participant_id int not null references users(id) on delete cascade,
            role participation_role default 'member',
            joined_at timestamp with time zone default current_timestamp,
            primary key (conversation_id, participant_id)
        );

        create table if not exists friend_list
        (
            user_id int not null references users(id) on delete cascade,
            friend_id int not null references users(id) on delete cascade,
            primary key (user_id, friend_id)
        );

        drop trigger if exists update_conversations_updated_at on conversations;
        create trigger update_conversations_updated_at
            before update on conversations
            for each row
        execute function update_updated_column_at();

        drop trigger if exists update_messages_updated_at on messages;
        create trigger update_messages_updated_at
            before update on messages
            for each row
        execute function update_updated_column_at();
        """;

        await db.Database.ExecuteSqlRawAsync(sql);
    }
}