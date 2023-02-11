using Flunt.Validations;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Entities;
using System.Diagnostics.Contracts;

namespace PaymentContext.Domain.Entities;

public class Student : Entity
{
    private IList<Subscription> _subscriptions;
    public Student(Name name, Document document, Email email)
    {
        Name = name;
        Document = document;
        Email = email;
        _subscriptions = new List<Subscription>();

        AddNotifications(name, document, email);
    }

    public Name Name { get; private set; }
    public Document Document { get; private set; }
    public Email Email { get; private set; }
    public Address? Address { get; private set; }
    public IReadOnlyCollection<Subscription> Subscriptions
    {
        get
        {
            return _subscriptions.ToArray();
        }
    }

    public void AddSubscription(Subscription subscription)
    {
        bool hasSubscritiionActive = false;
        foreach (Subscription sub in Subscriptions)
            if (sub.Active)
                hasSubscritiionActive = true;

        AddNotifications(new Contract<Student>()
            .Requires()
            .IsFalse(hasSubscritiionActive, "Student.Subscriptions", "Você já possui uma assinatura ativa")
            .AreEquals(0, subscription.Payments.Count, "Student.Subscription.Payments", "Esta assinatura não possui pagamentos")
            );
    }
}
