namespace Modsen.Application
{
public class EmailService
{
    private readonly SendEmailUseCase _sendEmailUseCase;

    public EmailService(SendEmailUseCase sendEmailUseCase)
    {
        _sendEmailUseCase = sendEmailUseCase;
    }

    public void SendEventUpdatedEmail(string userEmail, int eventId)
    {
        string subject = "Event Updated";
        string body = $"Event with id {eventId} has been successfully updated.";
        _sendEmailUseCase.Execute(userEmail, subject, body);
    }
}

}