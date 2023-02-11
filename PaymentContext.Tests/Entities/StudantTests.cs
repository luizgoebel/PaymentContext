using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enuns;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests.Entities;

[TestClass]
public class StudantTests
{
    private readonly Student _studant;
    private readonly Subscription _subscription;
    private readonly Name _name;
    private readonly Email _email;
    private readonly Address _address;
    private readonly Document _document;

    public StudantTests()
    {
        this._name = new("Luiz", "Eduardo");
        this._email = new("goebel.adm@gmail.com");
        this._document = new Document("99016092080", EDocumentType.CPF);
        this._address = new("Fernão dias", "68", "Salto Weissbach", "Blumenau", "Santa Catarina", "Brasil", "89032170");
        this._studant = new Student(_name, _document, _email);
        this._subscription = new Subscription(null);

    }

    [TestMethod]
    public void Deve_Retornar_Erro_Quando_Possui_Subscription_Active()
    {
        PayPalPayment payPalPayment = new("12345678", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, "WayneCorp", _document, _address, _email);
        this._subscription.AddPayment(payPalPayment);
        this._studant.AddSubscription(this._subscription);
        this._studant.AddSubscription(this._subscription);

        Assert.IsTrue(!this._studant.IsValid);
    }
    [TestMethod]
    public void Deve_Retornar_Erro_Quando_Subscription_Sem_Pagamento()
    {
        this._studant.AddSubscription(this._subscription);
        Assert.IsTrue(!this._studant.IsValid);
    }
    [TestMethod]
    public void Deve_Retornar_Sucesso_Quando_Adicionar_Subscription()
    {
        PayPalPayment payPalPayment = new("12345678", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, "WayneCorp", _document, _address, _email);
        this._subscription.AddPayment(payPalPayment);
        this._studant.AddSubscription(this._subscription);

        Assert.IsTrue(_studant.IsValid);
    }
}