namespace Mongo2Go.Helper
{
    public interface IMongoDbProcessStarter
    {
        IMongoDbProcess Start(string binariesDirectory, string dataDirectory, StorageEngineType storageEngineType, int port);

        IMongoDbProcess Start(string binariesDirectory, string dataDirectory, StorageEngineType storageEngineType, int port, bool doNotKill);
    }
}