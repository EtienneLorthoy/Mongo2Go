using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo2Go
{
    public enum StorageEngineType
    {
        /// <summary>
        /// Will use the default storage engine specified by the version of Mongo.
        /// Starting in MongoDB 3.2, wiredTiger is the default.
        /// </summary>
        VersionDefault,

        /// <summary>
        /// In memory engine for mongo.
        /// https://docs.mongodb.com/manual/core/inmemory/
        /// </summary>
        InMemory,

        /// <summary>
        /// WiredTiger uses document-level concurrency control for write operations. As a result, multiple clients can modify different documents of a collection at the same time.
        /// https://docs.mongodb.com/manual/core/wiredtiger/
        /// </summary>
        WiredTiger,

        /// <summary>
        /// MMAPv1 is MongoDB’s original storage engine
        /// https://docs.mongodb.com/manual/core/mmapv1/
        /// </summary>
        Mmapv1
    }
}
