using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;

namespace ProductVersions
{
	class Program
	{
		static void FindVersion(string path)
		{
			FileVersionInfo vinfo = FileVersionInfo.GetVersionInfo(path);
			if (vinfo.FileVersion == null ||
				vinfo.ProductVersion == null ||
				vinfo.FileName == null ||
				vinfo.ProductName == null ||
				vinfo.Comments == null ||
				vinfo.FileDescription == null ||
				vinfo.InternalName == null ||
				vinfo.PrivateBuild == null ||
				vinfo.ProductName == null ||
				vinfo.ProductPrivatePart == null ||
				vinfo.SpecialBuild == null)
				return;

			if (!vinfo.FileVersion.Contains("23") &&
				!vinfo.ProductVersion.Contains("23") &&
				!vinfo.FileName.Contains("23") &&
				!vinfo.ProductName.Contains("23") &&
				!vinfo.Comments.Contains("23") &&
				!vinfo.FileDescription.Contains("23") &&
				!vinfo.InternalName.Contains("23") &&
				!vinfo.PrivateBuild.Contains("23") &&
				!vinfo.ProductName.Contains("23") &&
				vinfo.ProductPrivatePart != 23 &&
				!vinfo.SpecialBuild.Contains("23")
				)
				return;

			Console.WriteLine("{0}: {1} / {2}", Path.GetFileName(path), vinfo.FileVersion, vinfo.ProductVersion);
		}

		static void Main(string[] args)
		{
			string medocpath = @"C:\Program Files\Medoc\Medoc";
			Console.WriteLine(".exe:");
			foreach(string exe in Directory.GetFiles(medocpath, "*.exe", SearchOption.AllDirectories))
			{
				FindVersion(exe);
			}
			Console.WriteLine();

			Console.WriteLine(".dll:");
			foreach (string dll in Directory.GetFiles(medocpath, "*.dll", SearchOption.AllDirectories))
			{
				FindVersion(dll);
			}
			Console.WriteLine();

			Console.WriteLine("Done");
			Console.ReadLine();
		}
	}
}
