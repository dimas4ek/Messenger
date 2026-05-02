namespace Contracts;

public static class HubMethods
{
    public static class Groups
    {
        public const string JoinUserGroup = "JoinUserGroup";
        public const string JoinFriendGroup = "JoinFriendGroup";
        public const string JoinChatGroup = "JoinChatGroup";
        public const string LeaveChatGroup = "LeaveChatGroup";
    }

    public static class Chats
    {
        public const string GroupChatCreated = "GroupChatCreated";
        public const string GroupChatUpdated = "GroupChatUpdated";
        public const string GroupChatDeleted = "GroupChatDeleted";
    }

    public static class Messages
    {
        public const string MessageReceived = "MessageReceived";
        public const string MessageUpdated = "MessageUpdated";
        public const string MessageDeleted = "MessageDeleted";
    }

    public static class Friends
    {
        public const string FriendAdded = "FriendAdded";
        public const string FriendUpdated = "FriendUpdated";
        public const string FriendStatusUpdated = "FriendStatusUpdated";
    }
}