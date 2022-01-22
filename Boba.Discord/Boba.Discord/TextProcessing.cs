
using Discord;
using System.Linq;

namespace Boba.Discord;

internal static class Utilities
{
    /// <summary>
    /// Checks if a user with a given ID is mentioned by a message
    /// </summary>
    internal static bool Mentions(this IUserMessage message, ulong userId) =>
        message.MentionedUserIds.Any(id => id == userId);


    internal static bool StartsWith(this string message, string phrase, out string remaining)
    {
        var trimmed = message.TrimStart();
        bool starts_with = trimmed.StartsWith(phrase);
        remaining = starts_with ? trimmed[phrase.Length..] : message;
        return starts_with;
    }


}
