using System;
using System.Collections.Generic;
using System.Text;

namespace Socialley.Common
{
    public static class AttributesConstraints
    {
        #region User
        public const byte NameMaxLength = 50;
        public const byte EmailMaxLength = 32;
        public const byte PhoneNumberMaxLength = 10;
        public const short AboutMeMaxLength = short.MaxValue;
        #endregion

        #region City, Region, Country
        public const byte CityNameMaxLength = 80;
        public const byte RegionNameMaxLength = 80;
        public const byte CountryNameMaxLength = 60;
        #endregion

        #region ChatMessage
        public const byte MessageTextMaxLength = byte.MaxValue;
        #endregion

        #region Group
        public const byte GroupNameMaxLength = 20;
        #endregion

        #region Post
        public const short ContentMaxLength = 500;
        #endregion
    }
}
