using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ValidationInterface;

namespace Web_Studio.Utils
{
    /// <summary>
    /// Custon JSON resolver for Newtonsoft JSON
    /// </summary>
    public class SetPropertiesResolver : DefaultContractResolver
    {
        /// <summary>
        /// Creates an instance
        /// </summary>
        public static readonly SetPropertiesResolver Instance = new SetPropertiesResolver();

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