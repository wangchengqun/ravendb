using System.Threading;
using System.Threading.Tasks;

using Raven.Abstractions.Util;
using Raven.Client;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Smuggler.Database.Impl.Remote
{
	public class DatabaseSmugglerRemoteDocumentActions : IDatabaseSmugglerDocumentActions
	{
		private readonly BulkInsertOperation _bulkInsert;

		public DatabaseSmugglerRemoteDocumentActions(IDocumentStore store)
		{
			_bulkInsert = store.BulkInsert();
		}

		public void Dispose()
		{
			_bulkInsert?.Dispose();
		}

		public Task WriteDocumentAsync(RavenJObject document, CancellationToken cancellationToken)
		{
			var metadata = document.Value<RavenJObject>("@metadata");
			document.Remove("@metadata");

			var id = metadata.Value<string>("@id");

			_bulkInsert.Store(document, metadata, id);
			return new CompletedTask();
		}
	}
}