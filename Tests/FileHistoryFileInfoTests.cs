using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHistoryExtractor;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
    public class FileHistoryFileInfoTests
    {
		[Test]
		public void UnmangleFilename()
		{
			Assert.That(FileHistoryFileInfo.UnmangleFilenameDefault("Test (2016_01_01 22_10_05 UTC).txt"), Is.EqualTo("Test.txt"));
		}

		[Test]
		public void GetBackupDateTime()
		{
			Assert.That(FileHistoryFileInfo.GetBackupDateString("Test (2016_01_01 22_10_05 UTC).txt"), Is.EqualTo("2016_01_01 22_10_05 UTC"));

			Assert.That(FileHistoryFileInfo.GetBackupDateTime("Test (2016_01_02 00_01_02 UTC).txt"), Is.EqualTo(new DateTime(2016, 1, 2, 0, 1, 2, DateTimeKind.Utc)));
		}
    }
}
