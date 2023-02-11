using Flunt.Validations;
using System.Net;

namespace PaymentContext.Domain.ValueObjects;

public class Name : ValueObject
{
    public Name(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;

        AddNotifications(new Contract<Name>()
        .Requires()
        );
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
}
