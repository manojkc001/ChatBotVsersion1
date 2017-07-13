using System.Runtime.Serialization;
using System.Collections.Generic;


namespace Bot_Application.Serialization
{
    [DataContract]
    public class Utterance
    {
        [DataMember]
        public string query { get; set; }
        [DataMember]
        public List<Intent> intents { get; set; }
        [DataMember]
        public List<Entity1> entities { get; set; }
    }
}