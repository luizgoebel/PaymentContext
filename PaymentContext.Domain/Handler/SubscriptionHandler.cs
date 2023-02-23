using Flunt.Notifications;
using Flunt.Validations;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enuns;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;
using System.Diagnostics.Contracts;

namespace PaymentContext.Domain.Handler;

public class SubscriptionHandler :
    Notifiable<Notification>,
    IHandler<CreateBoletoSubscriptionCommand>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IEmailService _emailService;

    public SubscriptionHandler(IStudentRepository studentRepository, IEmailService emailService)
    {
        _studentRepository = studentRepository;
        _emailService = emailService;
    }

    public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
    {
        //fail fast validation
        command.Validate();
        if (!command.IsValid)
        {
            AddNotifications(command);
            return new CommandResult(false, "Não possível realizar seu cadastro");
        }

        //verificar se documento ja esta cadastrado
        if (_studentRepository.DocumentExists(command.Document))
            AddNotification("Document", "Esta CPF já esta em uso");

        // verificar se email ja esta cadastrado
        if (_studentRepository.EmailExists(command.Email))
            AddNotification("Email", "Este Email já esta em uso");

        // gerar VOs
        Name name = new(command.FirstName, command.LastName);
        Email email = new(command.Email);
        Document document = new(command.Document, EDocumentType.CPF);
        Address address = new(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

        //gerar entidades
        Student studant = new(name, document, email);
        Subscription subscription = new(DateTime.Now.AddMonths(1));
        BoletoPayment payment = new(
            command.BarCode,
            command.BoletoNumber,
            command.PaidDate,
            command.ExpireDate,
            command.Total,
            command.TotalPaid,
            command.Payer,
            new Document(command.PayerDocument, command.PayerDocumentType),
            address,
            email);

        //relacionamentos
        subscription.AddPayment(payment);
        studant.AddSubscription(subscription);

        //agrupar as validações
        AddNotifications(name, document, email, address, studant, subscription, payment);

        // salvar informações
        _studentRepository.CreateSubscription(studant);

        //enviar email de boas vindas
        _emailService.Send(
            studant.Name.ToString(),
            studant.Email.Address,
            "Bem vindo ao curso.",
            "Sua assinatura foi criada com sucesso."
            );

        // retornar informações
        return new CommandResult(true, "Assinatura realizada com sucesso");
    }
}
