using System;
namespace NetflixGPT.infra.DB
{
	public class MongoDbSettings
	{
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }
	}
}

