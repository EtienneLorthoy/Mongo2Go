using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Mongo2Go.Helper
{
    /// <summary>
    /// Will forward binary directory from first successfull of all given IMongoBinaryLocator
    /// </summary>
    public class MongoBinaryLocator : IMongoBinaryLocator
    {
        private readonly IList<IMongoBinaryLocator> _locatorStrategies;
        private readonly Lazy<string> _binFolder;

        public MongoBinaryLocator(List<IMongoBinaryLocator> locatorStrategies)
        {
            _locatorStrategies = locatorStrategies;
            _binFolder = new Lazy<string>(ResolveBinariesDirectory);
        }

        public string Directory => _binFolder.Value;

        private string ResolveBinariesDirectory()
        {
            var exceptionList = new List<MonogDbBinariesNotFoundException>();

            foreach (var strat in _locatorStrategies)
            {
                try
                {
                    return strat.Directory;
                }
                catch (MonogDbBinariesNotFoundException ex)
                {
                    exceptionList.Add(ex);

                    if (_locatorStrategies.IndexOf(strat) == _locatorStrategies.Count - 1)
                    {
                        throw new AggregateException("Unable to find Mongo binaries. See inner exceptions for more info", exceptionList);
                    }
                }
            }

            // Unreachable code
            return null;
        }
    }
}