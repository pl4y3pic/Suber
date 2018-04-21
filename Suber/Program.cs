/*
 * Created by SharpDevelop.
 * User: Kent
 * Date: 2018/01/01
 * Time: 19:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;

namespace Suber
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			string fn = null;
			if (args.Length == 0) {
				var ofd = new OpenFileDialog();
				ofd.Filter = "Supported files|*.avi;*.mp4;*.mpv;*.mpg;*.mpeg;*.mkv;*.mov;*.rmvb;*.srt|All files|*.*";
				if (ofd.ShowDialog() != DialogResult.OK) return;
				fn = ofd.FileName;
			} else {
				fn = args[0];
			}

			Application.Run(new MainForm(fn));
		}
		
	}
}
