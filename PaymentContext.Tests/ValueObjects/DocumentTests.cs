using PaymentContext.Domain.Enuns;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests.ValueObjects;

[TestClass]
public class DocumentTests
{
    //Red, Green, Refactor
    [TestMethod]
    public void Deve_Retornar_Erro_Quando_Cnpj_Invalido()
    {
        Document document = new("123", EDocumentType.CNPJ);
        Assert.IsTrue(!document.IsValid);
    }
    [TestMethod]
    public void Deve_Retornar_Sucesso_Quando_Cnpj_Valido()
    {
        Document document = new("66091990000129", EDocumentType.CNPJ);
        Assert.IsTrue(document.IsValid);
    }
    [TestMethod]
    public void Deve_Retornar_Erro_Quando_Cpf_Invalido()
    {
        Document document = new("123", EDocumentType.CPF);
        Assert.IsTrue(!document.IsValid);
    }
    [TestMethod]
    public void Deve_Retornar_Sucesso_Quando_Cpf_Valido()
    {
        Document document = new("99016092080", EDocumentType.CPF);
        Assert.IsTrue(document.IsValid);
    }
}
