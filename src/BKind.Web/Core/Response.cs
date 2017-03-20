using System.Collections.Generic;
using System.Linq;

namespace BKind.Web.Core
{
    public class Response
    {
        protected readonly IList<ResponseMessage> Messages = new List<ResponseMessage>();

        public bool HasInformation { get { return Messages.Any(x => x.MessageType == ResponseMessageType.Information); } }
        public bool HasWarnings { get { return Messages.Any(x => x.MessageType == ResponseMessageType.Warning); } }
        public bool HasErrors { get { return Messages.Any(x => x.MessageType == ResponseMessageType.Error); } }
        public bool HasInformationOrWarnings => HasInformation || HasWarnings;

        public IEnumerable<ResponseMessage> Successes { get { return Messages.Where(x => x.MessageType == ResponseMessageType.Success).ToList().AsReadOnly(); } }
        public IEnumerable<ResponseMessage> Information { get { return Messages.Where(x => x.MessageType == ResponseMessageType.Information).ToList().AsReadOnly(); } }
        public IEnumerable<ResponseMessage> Warnings { get { return Messages.Where(x => x.MessageType == ResponseMessageType.Warning).ToList().AsReadOnly(); } }
        public IEnumerable<ResponseMessage> Errors { get { return Messages.Where(x => x.MessageType == ResponseMessageType.Error).ToList().AsReadOnly(); } }
        public IEnumerable<ResponseMessage> AllMessages => Messages;
        
        public static Response<T> From<T>(T value)
        {
            return new Response<T>(value);
        }

        public static Response Empty() => new Response();
        public static Response<T> Empty<T>() => new Response<T>(default(T));

        public Response AddMessage(string key, string message, ResponseMessageType type)
        {
            Messages.Add(new ResponseMessage { Key = key, Message = message, MessageType = type });
            return this;
        }

        public Response AddError(string key, string message)
        {
            Messages.Add(new ResponseMessage { Key = key, Message = message, MessageType = ResponseMessageType.Error });
            return this;
        }
    }

    public class Response<T> : Response
    {
        public Response(T result)
        {
            Result = result;
        }

        public Response()
        {
            Result = default(T);
        }

        public T Result { get; set; }

        public bool HasResult => Result != null;
    }

   
    public enum ResponseMessageType
    {
        Success, 
        Error,
        Information,
        Warning
    }

    public class ResponseMessage
    {
        public ResponseMessageType MessageType { get; set; }
        public string Key { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return Message;
        }
    }
}