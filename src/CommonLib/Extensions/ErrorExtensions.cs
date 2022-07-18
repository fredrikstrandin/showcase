using CommonLib.Models;
using HotChocolate;
using HotChocolate.Language;
using System.Net;

namespace CommonLib.Extensions
{
    public static class ErrorExtensions
    {
        public static Error Create(this ErrorItem error,
        HotChocolate.Path? path = null,
        IReadOnlyList<HotChocolate.Location>? locations = null,
        IReadOnlyDictionary<string, object?>? extensions = null,
        Exception? exception = null,
        ISyntaxNode? syntaxNode = null)
        {
            HttpStatusCode code = (HttpStatusCode)error.HttpStatusCode;
            
            return new Error(error.Message, code.ToString(), path, locations, extensions, exception, syntaxNode);
        }
    }
}
