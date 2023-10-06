using Nest;
using System.Runtime.Serialization;

namespace Data.Elasticsearch.Plugins
{
    public class LiveScorerScoreFunction : IScoreFunction
    {
        public LiveScorerScoreFunction(CustomPluginFields fields) => Plugin = fields;

        [DataMember(Name = "live_scorer")]
        public CustomPluginFields Plugin { get; }

        public QueryContainer Filter { get; set; }

        public double? Weight { get; set; }
    }

    public class CustomPluginFields
    {
        [DataMember(Name = "target_field_name")]
        public string TargetFieldName { get; set; }

        [DataMember(Name = "endpoint")]
        public string Endpoint { get; set; }

        [DataMember(Name = "params")]
        public Dictionary<string, string> Params { get; set; }
    }
}
