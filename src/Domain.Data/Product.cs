using Nest;
using Newtonsoft.Json;

namespace Domain.Model
{
    [ElasticsearchType(IdProperty = "id")]
    public sealed class Product
    {
        public Product()
        {
            this.Codes = new List<string>();
        }

        [Keyword(Name = "id")]
        public int Id { get; set; }

        [Number(NumberType.Integer, Name = "product_id")]
        public int ProductId { get; set; }

        [Date(Name = "start_online_date")]
        public DateTime? StartOnlineDate { get; set; }

        [Date(Name = "update_datetime")]
        public DateTime UpdateDateTime { get; set; }

        [Number(NumberType.Long, Name = "version_number")]
        public long VersionNumber { get; set; }

        [Object(Name = "collection_v2")]
        public object CollectionV2 { get; set; }

        [Object(Name = "department")]
        public object Department { get; set; }

        [Object(Name = "designer")]
        public object Designer { get; set; }

        [Object(Name = "filters")]
        public object Filters { get; set; }

        [Object(Name = "season")]
        public object Season { get; set; }

        [Object(Name = "style")]
        public object Style { get; set; }

        [Object(Name = "gender")]
        public object Gender { get; set; }

        public Dictionary<string, string> ShortDescription { get; set; }

        public Dictionary<string, string> FullDescription { get; set; }

        [Keyword(Name = "visibility_by_benefits")]
        public List<string> VisibilityByBenefits { get; set; }

        [Keyword(Name = "available_at")]
        public List<string> AvailableAt { get; set; }

        public List<string> Codes { get; set; }

        [Object(Name = "property_attributes")]
        public List<object> PropertyAttributes { get; set; }

        [Object(Name = "materials")]
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public List<object> Materials { get; set; }

        [Object(Name = "categories")]
        public List<object> Categories { get; set; }

        [Object(Name = "colors")]
        public List<object> Colors { get; set; }
    }
}
