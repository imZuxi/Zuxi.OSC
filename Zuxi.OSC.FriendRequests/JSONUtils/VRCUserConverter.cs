using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuxi.OSC.Modules.FriendRequests.JSONUtils
{
    internal class VRCUserConverter : CustomCreationConverter<VRCUser>
    {
        public override VRCUser Create(Type objectType)
        {
            return new VRCUser();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            VRCUser vrChatUser = base.ReadJson(reader, objectType, existingValue, serializer) as VRCUser;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = reader.Value.ToString();
                    if (reader.Read())
                    {
                        // Handle special date properties with the custom date format
                        if (propertyName == "accountDeletionDate" || propertyName == "accountDeletionLog")
                        {
                            DateTime dateValue;
                            if (DateTime.TryParseExact(reader.Value.ToString(), "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateValue))
                            {
                                vrChatUser.GetType().GetProperty(propertyName).SetValue(vrChatUser, dateValue);
                            }
                        }
                        else
                        {
                            vrChatUser.GetType().GetProperty(propertyName).SetValue(vrChatUser, reader.Value);
                        }
                    }
                }
                else if (reader.TokenType == JsonToken.EndObject)
                {
                    break;
                }
            }

            return vrChatUser;
        }
    }
}
