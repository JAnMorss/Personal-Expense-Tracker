using SpendWise.SharedKernel.Domain.Entities;
using SpendWise.SharedKernel.ErrorHandling;
using System.Net.Mail;

namespace SpendWise.Domain.Users.ValueObjects;

public sealed class Email(string Value) : ValueObject
{

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<Email>(new Error(
                "Email.Empty",
                "EmailAddress cannot be empty."));
        }

        try
        {
            var addr = new MailAddress(email.Trim());
            return Result.Success(new Email(addr.Address));
        }
        catch
        {
            return Result.Failure<Email>(new Error(
                "Email.Invalid",
                "Invalid email address format."));
        }
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString()
        => Value;

}

