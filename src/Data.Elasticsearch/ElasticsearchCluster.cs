using Nest;
using Domain.Model;
using Domain.Model.Data;
using Data.Elasticsearch.Plugins;
using Newtonsoft.Json.Linq;
using Elasticsearch.Net;

namespace Data.Elasticsearch
{
    public sealed class ElasticsearchCluster : IElasticsearchCluster
    {
        private readonly IElasticClient elasticClient;

        public ElasticsearchCluster()
        {
            this.elasticClient = this.GetElasticsearchClusterInstance();
        }

        public async Task<ISearchResponse<Product>> SearchAsync(int tenantId, string contextId, bool isFreeTextSearch)
        {
            var response = elasticClient.Search<Product>(s => s
                .Query(q => q
                    .FunctionScore(fs => fs
                        .Functions(new List<IScoreFunction>
                        {
                            new LiveScorerScoreFunction(new CustomPluginFields(){
                                TargetFieldName = "product_id",
                                Endpoint = "http://test.com",
                                Params = new Dictionary<string, string> { { "parameter", "value" } }
                            })
                            {
                                Weight = 1
                            }
                        })
                    )
                )
            );

            return response;
        }


        public void WriteLineQuery(int tenantId, string contextId, bool isFreeTextSearch)
        {

            var queryContainer = Query<Product>
                                 .FunctionScore(fs => fs
                                    .Functions(new List<IScoreFunction>
                                    {
                                        new LiveScorerScoreFunction(new CustomPluginFields(){
                                            TargetFieldName = "product_id",
                                            Endpoint = "http://test.com",
                                            Params = new Dictionary<string, string> { { "parameter", "value" } }
                                        })
                                        {
                                            Weight = 1
                                        }
                                    })
                                    .ScoreMode(FunctionScoreMode.Max)
                                );


            var searchDescriptor = new SearchDescriptor<Product>()
                .Index("product_10000")
                .Size(1000)
                .StoredFields("_none_")
                .DocValueFields(t => t.Field("product_id"))
                .Source(false)
                .Query(q =>
                    q.Bool(s => s.Must(queryContainer)));

            try
            {
                Console.WriteLine("=============== Apply the RequestResponseSerializer =================");
                var query = JObject.Parse(this.elasticClient.ConnectionSettings.RequestResponseSerializer.SerializeToString(searchDescriptor)).ToString();
                Console.WriteLine("Result: " + query);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("INNER EXCEPTION " + ex.InnerException);
            }
            finally
            {
                Console.WriteLine("##################################################");
            }

        }


        private ElasticClient GetElasticsearchClusterInstance()
        {
            var localHost = new SingleNodeConnectionPool(new Uri("http://localhost:9200/"));
            var inMemoryConnection = new InMemoryConnection();

            var connectionSettings = GetConnectionSettingsWith(localHost);

            return new ElasticClient(connectionSettings.DefaultIndex("product_10000"));
        }

        private ConnectionSettings GetConnectionSettingsWith(SingleNodeConnectionPool pool)
        {
            return
                new ConnectionSettings(pool)
                .EnableDebugMode()
                   .OnRequestCompleted(apiCallDetails =>
                   {
                       Console.WriteLine("############### Default #################");
                       Console.WriteLine(apiCallDetails.DebugInformation);
                   });
        }

        private ConnectionSettings GetConnectionSettingsWith(InMemoryConnection pool)
        {
            return
                new ConnectionSettings(pool)
                .EnableDebugMode()
                   .OnRequestCompleted(apiCallDetails =>
                   {
                       Console.WriteLine("############### Default #################");
                       Console.WriteLine(apiCallDetails.DebugInformation);
                   });
        }
    }
}