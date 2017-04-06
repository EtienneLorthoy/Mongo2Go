using System;
using System.IO;

namespace Mongo2Go.Helper
{

    public class NugetPackageMongoBinaryLocator : IMongoBinaryLocator
    {
        private readonly string _nugetPrefix = Path.Combine("packages", "Mongo2Go*");
        public const string DefaultWindowsSearchPattern = @"tools\mongodb-win32*\bin";
        public const string DefaultLinuxSearchPattern = "tools/mongodb-linux*/bin";
        public const string DefaultOsxSearchPattern = "tools/mongodb-osx*/bin";
        private string _binFolder = string.Empty;
        private readonly string _searchPattern;

        public NugetPackageMongoBinaryLocator(string searchPatternOverride)
        {
            if (string.IsNullOrEmpty(searchPatternOverride))
            {
                var operatingSystem = Environment.OSVersion.Platform;
                switch (operatingSystem)
                {
                    case PlatformID.MacOSX:
                        _searchPattern = DefaultOsxSearchPattern;
                        break;
                    case PlatformID.Unix:
                        _searchPattern = DefaultLinuxSearchPattern;
                        break;
                    default:
                        _searchPattern = DefaultWindowsSearchPattern;
                        break;
                }
            }
            else
            {
                _searchPattern = searchPatternOverride;
            }
        }

        public string Directory
        {
            get
            {
                if (string.IsNullOrEmpty(_binFolder))
                {
                    return _binFolder = ResolveBinariesDirectory();
                }
                else
                {
                    return _binFolder;
                }
            }
        }

        private string ResolveBinariesDirectory()
        {
            var binariesFolder =
                // First try path when installed via nuget
                FolderSearch.CurrentExecutingDirectory().FindFolderUpwards(Path.Combine(_nugetPrefix, _searchPattern)) ??
                // second try path when started from solution
                FolderSearch.CurrentExecutingDirectory().FindFolderUpwards(_searchPattern);

            if (binariesFolder == null)
            {
                throw new MonogDbBinariesNotFoundException(string.Format(
                    "Could not find Mongo binaries using the search pattern of \"{0}\". We walked up the directories {1} levels from {2} and {3}.  You can override the search pattern when calling MongoDbRunner.Start.  We have detected the OS as {4}",
                    _searchPattern,
                    FolderSearch.MaxLevelOfRecursion,
                    FolderSearch.CurrentExecutingDirectory(),
                    Path.Combine(_nugetPrefix, _searchPattern),
                    Environment.OSVersion.Platform));
            }
            return binariesFolder;
        }
    }
}