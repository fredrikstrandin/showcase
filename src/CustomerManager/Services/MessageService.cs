using CustomerManager.Interfaces;
using CustomerManager.Model;
using Northstar.Message;
using System.Threading.Tasks;

namespace CustomerManager.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;

    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public Task SendNewUserAsync(CustomerCreateRequest request)
    {
        NewUserMessage newUser = new NewUserMessage()
        {
            Id = request.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Street = request.Street,
            Zip = request.Zip,
            City = request.City
        };
        
        CustomerKafkaMessage message = new CustomerKafkaMessage() { NewUserMessage = newUser };
        
        return _messageRepository.SendMessageAsync(request.Id, message, "customermanager");
    }
}