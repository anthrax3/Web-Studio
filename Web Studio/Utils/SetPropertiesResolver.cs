using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Web_Studio.Utils
{
    /// <summary>
    /// Custon JSON resolver for Newtonsoft JSON
    /// </summary>
    internal class SetPropertiesResolver : DefaultContractResolver
    {  
        /// <summary>
        /// Only serialize to JSON writables properties
        /// </summary>
        /// <param name="member"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override JsonProperty CreateProperty(MemberInfo member,
                                     MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization); 
            if (!property.Writable) property.ShouldSerialize = instance => false;


            return property;
        }
    }
}