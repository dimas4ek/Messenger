using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.Database;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(MessengerContext db)
    {
        const string sql = """
                           create or replace function update_updated_column_at()
                               returns trigger as
                           $$
                           begin
                               NEW.updated_at = current_timestamp;
                               return NEW;
                           end;
                           $$ language plpgsql;

                           do
                           $$
                               begin
                                   if not exists (select 1 from pg_type where typname = 'chat_type') then
                                       create type chat_type as enum ('private', 'group');
                                   end if;

                                   if not exists (select 1 from pg_type where typname = 'chat_participation_role') then
                                       create type chat_participation_role as enum ('member', 'admin');
                                   end if;

                                   if not exists (select 1 from pg_type where typname = 'user_status') then
                                       create type user_status as enum ('offline', 'online');
                                   end if;

                                   if not exists (select 1 from pg_type where typname = 'image_content_type') then
                                       create type image_content_type as enum (
                                           'image/jpeg',
                                           'image/png',
                                           'image/gif',
                                           'image/webp',
                                           'image/bmp',
                                           'image/svg+xml',
                                           'image/tiff',
                                           'image/ico'
                                           );
                                   end if;
                               end
                           $$;

                           create table if not exists images
                           (
                               id           int primary key generated always as identity,
                               name         varchar(255)       not null,
                               content_type image_content_type not null,
                               data         bytea              not null,
                               created_at   timestamp with time zone default current_timestamp
                           );

                           create table if not exists users
                           (
                               id         int primary key generated always as identity,
                               username   varchar(255) not null unique,
                               password   varchar(255) not null,
                               status     user_status              default 'offline',
                               avatar_id  int          references images (id) on delete set null,
                               created_at timestamp with time zone default current_timestamp
                           );

                           create table if not exists chats
                           (
                               id         int primary key generated always as identity,
                               type       chat_type                default 'private',
                               name       varchar(255) null,
                               image_id  int          references images (id) on delete set null,
                               created_at timestamp with time zone default current_timestamp,
                               updated_at timestamp with time zone default current_timestamp
                           );

                           create table if not exists messages
                           (
                               id           int primary key generated always as identity,
                               chat_id      int  not null references chats (id) on delete cascade,
                               sender_id    int  not null references users (id) on delete cascade,
                               message_text text not null,
                               is_edited    bool                     default false,
                               is_read      bool                     default false,
                               created_at   timestamp with time zone default current_timestamp,
                               updated_at   timestamp with time zone default current_timestamp
                           );

                           create table if not exists chat_participants
                           (
                               chat_id        int not null references chats (id) on delete cascade,
                               participant_id int not null references users (id) on delete cascade,
                               role           chat_participation_role  default 'member',
                               joined_at      timestamp with time zone default current_timestamp,
                               primary key (chat_id, participant_id)
                           );

                           create table if not exists friend_list
                           (
                               user_id   int not null references users (id) on delete cascade,
                               friend_id int not null references users (id) on delete cascade,
                               primary key (user_id, friend_id)
                           );

                           create table if not exists friend_requests
                           (
                               id          int primary key generated always as identity,
                               sender_id   int not null references users (id) on delete cascade,
                               receiver_id int not null references users (id) on delete cascade,
                               created_at  timestamp with time zone default current_timestamp
                           );

                           drop trigger if exists update_chats_updated_at on chats;
                           create trigger update_chats_updated_at
                               before update
                               on chats
                               for each row
                           execute function update_updated_column_at();

                           drop trigger if exists update_messages_updated_at on messages;
                           create trigger update_messages_updated_at
                               before update
                               on messages
                               for each row
                           execute function update_updated_column_at();
                           """;

        await db.Database.ExecuteSqlRawAsync(sql);

        // On a fresh database the enum types (user_status, chat_type, ...) are created
        // by the SQL above, but Npgsql has already cached the database's type catalog
        // from its first connection — without those enums. Reload the type cache so the
        // freshly-created enums are recognized right away; otherwise the first enum query
        // fails until the process is restarted.
        var connection = (NpgsqlConnection)db.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();
        await connection.ReloadTypesAsync();
    }
}