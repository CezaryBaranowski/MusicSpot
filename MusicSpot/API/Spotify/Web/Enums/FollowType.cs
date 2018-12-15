using System;

namespace MusicSpot.API.Spotify.Web.Enums
{
    [Flags]
    public enum FollowType
    {
        [String("artist")]
        Artist = 1,

        [String("user")]
        User = 2
    }
}