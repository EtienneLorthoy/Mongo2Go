using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Mongo2Go.Helper
{

    public class CurrentExecFolderMongoBinaryLocator : IMongoBinaryLocator
    {
        private readonly string[] _mongodExeFilenames = new[] 
        {
            MongoDbDefaults.MongodExecutable,
            MongoDbDefaults.MongoExportExecutable,
            MongoDbDefaults.MongoImportExecutable
        };

        private readonly Lazy<string> _binFolder;

        public CurrentExecFolderMongoBinaryLocator()
        {
            _binFolder = new Lazy<string>(ResolveBinariesDirectory);
        }

        public string Directory => _binFolder.Value;

        private string ResolveBinariesDirectory()
        {
            // When running from a CI, mstest instead of building tests and run it from there, it will deploy into 
            // unknown another folder (can be in another disk!). It will resolve file needed just with lib ref and 
            // only content file with a deployment item pointing at it. 
            // (ref: Remarks section at https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.testtools.unittesting.deploymentitemattribute.aspx )
            var binariesFolderCandidate = FolderSearch.CurrentExecutingDirectory();

            foreach (var exeFilename in _mongodExeFilenames)
            {
                if (!File.Exists(Path.Combine(binariesFolderCandidate, exeFilename + ".exe")))
                {
                    throw new MonogDbBinariesNotFoundException(string.Format("Could not find Mongo binaries using the 'current execution dir' search pattern. If you're running on a CI make sure you use DeploymentItem decorator Resolved current dir \"{0}\".", binariesFolderCandidate));
                }
            }

            return binariesFolderCandidate;
        }
    }
}