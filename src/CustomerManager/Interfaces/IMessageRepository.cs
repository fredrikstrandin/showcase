using Google.Protobuf;
using System.Threading.Tasks;

namespace CustomerManager.Interfaces;

public interface IMessageRepository
{
    Task SendMessageAsync<K, V>(K key, V message, string topicName) where V : IMessage<V>, new();
}
