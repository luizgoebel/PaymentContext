using PaymentContext.Domain.Entities;

namespace PaymentContext.Domain.Repositories;

public interface IStudentRepository
{
    bool DocumentExists(string Document);
    bool EmailExists(string Email);
    void CreateSubscription(Student student);
}
