using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHistoryExtractor
{
	public class FileHistoryDirectory
	{
		public readonly DirectoryInfo Directory;
		public readonly DirectoryInfo RootDirectory;

		public FileHistoryDirectory(DirectoryInfo directory, DirectoryInfo root)
		{
			Directory = directory;
			RootDirectory = root;
		}

		public string RelativePath
		{
			get
			{
				var result = "";

				for (int i = 0; i < Directory.FullName.Length; i++)
				{
					if (i < RootDirectory.FullName.Length && RootDirectory.FullName[i] == Directory.FullName[i])
						continue;

					result += Directory.FullName[i];
				}

				return result;
			}
		}

		public IEnumerable<FileHistoryFileInfo> Files
		{
			get { return Directory.GetFiles().Select(f => new FileHistoryFileInfo(f, this)); }
		}

		public IEnumerable<FileHistoryDirectory> Directories
		{
			get { return Directory.GetDirectories().Select(d => new FileHistoryDirectory(d, RootDirectory)); }
		}
	}
}
