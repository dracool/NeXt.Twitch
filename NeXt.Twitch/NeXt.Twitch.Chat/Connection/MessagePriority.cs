namespace NeXt.Twitch.Chat.Connection
{
    /// <summary>
    /// The priority a message is sent with
    /// </summary>
    public enum MessagePriority
    {
#pragma warning disable 1591 // (missing xml comment) would be pointless, everybody understands what this means
        //TODO: possibly explain why these values were chosen?
        Critical = -5,
        High = -2,
        Default = 0,
        Low = 1,
        Idle = 10,
#pragma warning restore 1591
    }
}