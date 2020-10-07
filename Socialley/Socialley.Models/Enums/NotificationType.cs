using System;
using System.Collections.Generic;
using System.Text;

namespace Socialley.Models.Enums
{
    public enum NotificationType
    {
        Unknown=0,
        Message = 1,
        ApprovedPost = 2,
        BannedPost = 3,
        UnbannedPost = 4,
        AddToFavorite = 5,
        RateProfile = 6,
        CreateNewBlogPost = 7,
        CommentPost = 8,
        ReplyToComment = 9,
        ApprovedComment = 10,
        BannedProfile = 11,
        UnbannedProfile = 12,
    }
}
