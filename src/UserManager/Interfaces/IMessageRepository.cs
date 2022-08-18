using Google.Protobuf;
using Northstar.Message;
using System.Threading.Tasks;

namespace UserManager.Interfaces;

public interface IMessageRepository
{
    //Task SendMessageAsync<K, V>(K key, V message, string topicName) where V : IMessage<V>, new();
    Task SendMessageAsync(string key, UserKafkaMessage message, string topicName);
}
