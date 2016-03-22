using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHistoryExtractor
{
	class FileHistoryFileRenamer
	{
		public readonly FileHistoryFileInfo SourceFile;
		public readonly DirectoryInfo DestinationDirectory;
		protected readonly DirectoryInfo DestinationRootDirectory;

		public FileInfo DestinationFile => new FileInfo(DestinationDirectory.FullName + Path.DirectorySeparatorChar + SourceFile.UnmangleFilename());

		public FileHistoryFileRenamer(FileHistoryFileInfo file, DirectoryInfo destRootDir)
		{
			SourceFile = file;
			DestinationRootDirectory = destRootDir;
			
			DestinationDirectory = new DirectoryInfo(destRootDir.FullName + Path.DirectorySeparatorChar + file.Directory.RelativePath);
		}

		public void CopyFile(string backupDir = "")
		{
			if (File.Exists(DestinationFile.FullName))
			{
				if (backupDir != "")
				{
					FileInfo backupFilePath =
						new FileInfo(backupDir + Path.DirectorySeparatorChar + SourceFile.BackupDateTime.ToString("yy-MM-dd hh-mm-ss") +
						             Path.DirectorySeparatorChar + SourceFile.Directory.RelativePath + Path.DirectorySeparatorChar + SourceFile.UnmangleFilename());
					if (backupFilePath.Directory != null && !backupFilePath.Directory.Exists)
						Directory.CreateDirectory(backupFilePath.Directory.FullName);

					File.Copy(SourceFile.File.FullName, backupFilePath.FullName);
				}

			}
			else
			{
				if (!DestinationDirectory.Exists)
					Directory.CreateDirectory(DestinationDirectory.FullName);

				File.Copy(SourceFile.File.FullName, DestinationFile.FullName);
			}
		}

		public void MoveFile(string backupDir = "")
		{
			if (File.Exists(DestinationFile.FullName))
			{
				if (backupDir != "")
				{
					FileInfo backupFilePath =
						new FileInfo(backupDir + Path.DirectorySeparatorChar + SourceFile.BackupDateTime.ToString("yy-MM-dd hh-mm-ss") +
									 Path.DirectorySeparatorChar + SourceFile.Directory.RelativePath + Path.DirectorySeparatorChar + SourceFile.UnmangleFilename());
					if (backupFilePath.Directory != null && !backupFilePath.Directory.Exists)
						Directory.CreateDirectory(backupFilePath.Directory.FullName);

					File.Move(SourceFile.File.FullName, backupFilePath.FullName);
				}
			}
			else
			{
				if (!DestinationDirectory.Exists)
					Directory.CreateDirectory(DestinationDirectory.FullName);

				File.Move(SourceFile.File.FullName, DestinationFile.FullName);
			}
		}
	}
}
