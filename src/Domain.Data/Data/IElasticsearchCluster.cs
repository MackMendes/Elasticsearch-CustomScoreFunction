using Nest;

namespace Domain.Model.Data
{
    public interface IElasticsearchCluster
    {
        Task<ISearchResponse<Product>> SearchAsync(int tenantId, string contextId, bool isFreeTextSearch);

        void WriteLineQuery(int tenantId, string contextId, bool isFreeTextSearch);
    }
}
