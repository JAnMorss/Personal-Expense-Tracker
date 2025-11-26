using SpendWise.SharedKernel.Domain.Entities;
using SpendWise.SharedKernel.ErrorHandling;

namespace SpendWise.Domain.Users.ValueObjects;

public sealed class Avatar(string Value) : ValueObject
{
    public static Result<Avatar> Create(string avatar) 
    {
        if (!Uri.TryCreate(avatar, UriKind.Absolute, out var uriResult) || 
            (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
        {
            return Result.Failure<Avatar>(new Error(
                "Avatar.InvalidUrl",
                "Invalid avatar URL."));
        }

        return new Avatar(avatar);
    }

    public static Guid? ExtractFileIdFromUrl(string url) 
    {
        try
        {
            var segments = new Uri(url).Segments;
            if (segments.Length == 0) return null;

            var fileName = segments.Last().Trim('/');
            return Guid.TryParse(fileName, out var fileId) ? fileId : null;
        }
        catch
        {
            return null;
        }
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
