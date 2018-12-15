﻿using System;

namespace MusicSpot.API.Spotify.Web.Enums
{
    /// <summary>
    ///     Only one value allowed
    /// </summary>
    [Flags]
    public enum TimeRangeType
    {
        [String("long_term")]
        LongTerm = 1,

        [String("medium_term")]
        MediumTerm = 2,

        [String("short_term")]
        ShortTerm = 4
    }
}