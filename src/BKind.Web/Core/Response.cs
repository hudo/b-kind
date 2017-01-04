using System.Collections.Generic;
using System.Linq;

namespace BKind.Web.Core
{
    public class Response
    {
        private readonly IList<ResponseMessage> _messages = new List<ResponseMessage>();

        public bool HasInformation { get { return _messages.Any(x => x.MessageType == ResponseMessageType.Information); } }
        public bool HasWarnings { get { return _messages.Any(x => x.MessageType == ResponseMessageType.Warning); } }
        public bool HasErrors { get { return _messages.Any(x => x.MessageType == ResponseMessageType.Error); } }
        public bool HasInformationOrWarnings => HasInformation || HasWarnings;

        public IEnumerable<ResponseMessage> Successes { get { return _messages.Where(x => x.MessageType == ResponseMessageType.Success).ToList().AsReadOnly(); } }
        public IEnumerable<ResponseMessage> Information { get { return _messages.Where(x => x.MessageType == ResponseMessageType.Information).ToList().AsReadOnly(); } }
        public IEnumerable<ResponseMessage> Warnings { get { return _messages.Where(x => x.MessageType == ResponseMessageType.Warning).ToList().AsReadOnly(); } }
        public IEnumerable<ResponseMessage> Errors { get { return _messages.Where(x => x.MessageType == ResponseMessageType.Error).ToList().AsReadOnly(); } }
        public IEnumerable<ResponseMessage> AllMessages => _messages;

        public void AddMessage(string key, string message, ResponseMessageType type)
        {
            _messages.Add(new ResponseMessage { Key = key, Message = message, MessageType = type });
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

        public T Result { get; }
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
                
    }
}