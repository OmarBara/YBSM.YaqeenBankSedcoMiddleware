using Newtonsoft.Json.Serialization;

namespace YBSM.API.Middleware
{
    public class CapitalizePropertyNamesResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return propertyName;

            return char.ToUpper(propertyName[0]) + propertyName.Substring(1);
        }
    }
}
