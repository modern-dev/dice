//
// This file\code is part of InTouch project.
//
// InTouch - is a .NET wrapper for the vk.com API.
// https://github.com/virtyaluk/InTouch
//
// Copyright (c) 2016 Bohdan Shtepan
// http://modern-dev.com/
//
// Licensed under the MIT license.
//

namespace ModernDev.Dice
{
    /// <summary>
    /// Color formats.
    /// </summary>
    public enum ColorFormats
    {
        /// <summary>
        /// Hex e.g. #79c157.
        /// </summary>
        Hex,

        /// <summary>
        /// Short hex.
        /// </summary>
        ShortHex,

        /// <summary>
        /// RGB e.g. rgb(110,52,164).
        /// </summary>
        RGB,

        /// <summary>
        /// 0x e.g. 0x67ae0b.
        /// </summary>
        Ox,

        /// <summary>
        /// RGBA e.g. rgba(110, 52, 164, 0.2545).
        /// </summary>
        RGBA,

        /// <summary>
        /// Named e.g. Red.
        /// </summary>
        Named
    }
}
