using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoImporter._Dependencies;
using Moq;
using Moq.Language.Flow;
using System.Collections.Generic;

namespace PhotoImporter.Tests;

public abstract class _TestBase {
    protected Mock<IConsoleWriter> _consoleWriter;
    protected Mock<IConfigReader> _configReader;
    protected Mock<IFilesystem> _filesystem;
    protected Mock<IDependencyFactory> _dependencies;
    protected Mock<IPhotoImporter> _photoImporter;

    protected ISetup<IConsoleWriter> _writeLine;
    protected List<string> _writeLineResults;
    
    protected int _index;

    [TestInitialize]
    public void _GlobalSetup() {
        _consoleWriter = new Mock<IConsoleWriter>();
        _configReader = new Mock<IConfigReader>();
        _filesystem = new Mock<IFilesystem>();
        _photoImporter = new Mock<IPhotoImporter>();

        _dependencies = new Mock<IDependencyFactory>();
        _dependencies.Setup(x => x.GetConsoleWriter()).Returns(_consoleWriter.Object);
        _dependencies.Setup(x => x.GetConfigReader()).Returns(_configReader.Object);
        _dependencies.Setup(x => x.GetFilesystem()).Returns(_filesystem.Object);
        _dependencies.Setup(x => x.GetMessenger()).Returns(new Messenger(_dependencies.Object));
        _dependencies.Setup(x => x.GetPhotoImporter()).Returns(_photoImporter.Object);

        _writeLine = _consoleWriter.Setup(x => x.WriteLine(It.IsAny<string>()));
        _writeLineResults = new List<string>();
        _writeLine.Callback((string line) => _writeLineResults.Add(line));

        _index = 0;
    }

    protected void verifySingleMessage(string message) {
        Assert.AreEqual(1, _writeLineResults.Count);
        Assert.AreEqual(message, _writeLineResults[_index++]);
    }
}