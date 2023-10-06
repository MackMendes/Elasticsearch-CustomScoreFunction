using Data.Elasticsearch;

Console.WriteLine("All dependecies were resolved.");

var instance = new ElasticsearchCluster();

instance.WriteLineQuery(10000, "teste", true);

var result = await instance.SearchAsync(10000, "teste", true);

Console.WriteLine(result.DebugInformation);
Console.WriteLine("This program finish!");