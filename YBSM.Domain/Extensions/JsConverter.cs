using System.Net;
using Core.Domain.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Domain.Extensions
{
    public static class JsConverter
    {
        public static string ToJson(this object value)
        {
            if (value == null)  throw new APIException(
                "json value is null..", HttpStatusCode.NotAcceptable);

            var serializeObject = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            });
            return serializeObject;
        }
    }
}