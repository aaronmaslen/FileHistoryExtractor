using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHistoryExtractor
{
	class Program
	{
		enum Mode
		{
			Default,
			Copy,
			Move
		}

		static void Main(string[] args)
		{
			var mode = Mode.Default;
			var sourceDir = new DirectoryInfo(Environment.CurrentDirectory);
			var destinationDir = new DirectoryInfo(Environment.CurrentDirectory);
			DirectoryInfo backupDir = null;

			for (var i = 0; i < args.Length; i++)
			{
				switch (args[i])
				{
					case "-c":
						mode = Mode.Copy;
						break;
					case "-m":
						mode = Mode.Move;
						break;
					case "-s":
						sourceDir = new DirectoryInfo(args[++i].TrimEnd('\\'));
						break;
					case "-d":
						destinationDir = new DirectoryInfo(args[++i].TrimEnd('\\'));
						break;
					case "-b":
						backupDir = new DirectoryInfo(args[++i].TrimEnd('\\'));
						break;
					case "-h":
					default:
						PrintHelp();
						return;
				}
			}

			if (!Directory.Exists(destinationDir.FullName))
				Directory.CreateDirectory(destinationDir.FullName);

			foreach (var f in GetAllFiles(sourceDir, sourceDir))
			{
				var renamer = new FileHistoryFileRenamer(f, destinationDir);

				Console.WriteLine("{0} -> {1}", renamer.SourceFile.File.FullName, renamer.DestinationFile.FullName);

				if (mode == Mode.Copy | mode == Mode.Default)
					renamer.CopyFile(backupDir?.FullName ?? "");
				else
					renamer.MoveFile(backupDir?.FullName ?? "");
			}
		}

		private static void PrintHelp()
		{
			Console.WriteLine("Usage: FileHistoryExtactor [-c|-m] [-s sourceDir] [-d destDir] [-b duplicateBackupDir]");
			Console.WriteLine("-h						: Print this message");
			Console.WriteLine("-c						: Copy mode (default)");
			Console.WriteLine("-m						: Move/Rename mode");
			Console.WriteLine("-s sourceDir				: Source Directory");
			Console.WriteLine("-d destDir				: Destination Directory");
			Console.WriteLine("-b duplicateBackupDir	: Duplicate backups destination directory");
		}

		internal static IEnumerable<FileHistoryFileInfo> GetAllFiles(DirectoryInfo dir, DirectoryInfo rootDir)
		{
			var fhRootDir = new FileHistoryDirectory(dir, rootDir);

			return fhRootDir.Files.OrderBy(f => f.BackupDateTime).Reverse().Concat(fhRootDir.Directories.SelectMany(d => GetAllFiles(d.Directory, rootDir).OrderBy(f => f.BackupDateTime).Reverse()));
		}
	}
}
