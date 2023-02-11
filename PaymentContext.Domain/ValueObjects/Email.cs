using Flunt.Validations;

namespace PaymentContext.Domain.ValueObjects;

public class Email : ValueObject
{
    public Email(string address)
    {
        Address = address;

        AddNotifications(new Contract<Email>()
            .Requires()
            .IsEmail(address, "Email.Address", "E-mail inválido"));
    }

    public string Address { get; private set; }
}
