namespace NeXt.Twitch.API
{
    /// <summary>
    /// A list of valid sorting directions.
    /// </summary>
    public enum SortDirection
    {
        /// <summary>Descending sort direction.</summary>
        Descending,
        /// <summary>Ascending sort direction.</summary>
        Ascending
    }

    /// <summary>Enum representing sort keys available for /follows/channels/</summary>
    public enum SortKey
    {
        /// <summary>SortKey representing the date/time of account creation</summary>
        CreatedAt,
        /// <summary>SortKey representing the date/time of the last broadcast of a channel</summary>
        LastBroadcaster,
        /// <summary>SortKey representing the alphabetical sort based on usernames</summary>
        Login
    }


    /// <summary>
    /// A list of valid commercial lengths.
    /// </summary>
    public enum CommercialLength
    {
        /// <summary>30 second commercial</summary>
        Seconds30 = 30,
        /// <summary>60 second commercial</summary>
        Seconds60 = 60,
        /// <summary>90 second commercial</summary>
        Seconds90 = 90,
        /// <summary>120 second commercial</summary>
        Second120 = 120,
        /// <summary>150 second commercial</summary>
        Seconds150 = 150,
        /// <summary>180 second commercial</summary>
        Seconds180 = 180
    }
}