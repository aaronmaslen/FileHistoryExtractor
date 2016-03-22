using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileHistoryExtractor
{
	public class FileHistoryFileInfo
	{
		//1st capture group contains the unmangled filename and the 3rd contains the extension. The 2nd contains the backup date.
		protected const string MangledFilenamePattern = @"(.*) \((\d{4}_\d{2}_\d{2} \d{2}_\d{2}_\d{2} UTC)\)(\..*)?";
		//YYYY_MM_DD HH_MM_SS UTC
		protected const string BackupDatePattern = @"(\d{4})_(\d{2})_(\d{2}) (\d{2})_(\d{2})_(\d{2}) UTC";

		public static string UnmangleFilenameDefault(string filename, string mangledFilenamePattern = MangledFilenamePattern)
		{
			var match = Regex.Match(filename, mangledFilenamePattern);

			if (match == Match.Empty)
				return filename;

			return match.Groups[1].Value + match.Groups[3].Value;
		}

		public static string GetBackupDateString(string filename, string mangledFilenamePattern = MangledFilenamePattern)
		{
			var match = Regex.Match(filename, mangledFilenamePattern);

			if (match == Match.Empty)
				throw new ArgumentException("No matches found");

			return match.Groups[2].Value;
		}

		public static DateTime GetBackupDateTime(string backupDateString, string pattern = BackupDatePattern)
		{
			var match = Regex.Match(backupDateString, pattern);

			if (match == Match.Empty)
				throw new ArgumentException("No matches found");

			var year = int.Parse(match.Groups[1].Value);
			var month = int.Parse(match.Groups[2].Value);
			var day = int.Parse(match.Groups[3].Value);

			var hour = int.Parse(match.Groups[4].Value);
			var minute = int.Parse(match.Groups[5].Value);
			var second = int.Parse(match.Groups[6].Value);

			return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
		}

		public readonly FileHistoryDirectory Directory;
		public readonly FileInfo File;

		public FileHistoryFileInfo(FileInfo file, FileHistoryDirectory directory)
		{
			Directory = directory;
			File = file;
		}

		public virtual string UnmangleFilename()
		{
			return UnmangleFilenameDefault(File.Name);
		}

		public virtual DateTime BackupDateTime => GetBackupDateTime(GetBackupDateString(File.Name));
	}
}
