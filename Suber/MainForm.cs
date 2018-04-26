/*
 * Created by SharpDevelop.
 * User: Kent
 * Date: 2018/01/01
 * Time: 19:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WMPLib;

namespace Suber
{
    /// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		const string tUndef = "--:--:--,---";
		const int tMax = 99999999;
		const int tDelayS = 350; // start delay
		const int tDelayE = 250; // end delay
		static int tWidth = -1;
		//static double[] rates = { 1.0, 2.0, 0.5 };
		static double[] rates = { 1.0, 2.0 };
		int rate = 0; // index of rates
		Bitmap bg;
		Graphics g;

        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;　 //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte = (byte)((curByte << 1) & 0xff)) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }
        public static Encoding GetEncoding(FileStream fs)
        {
            var Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            var UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            var UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
            var reVal = Encoding.Default;
            var r = new BinaryReader(fs, Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;
        }
        public static Encoding GetEncoding(string filename)
        {
            var fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            Encoding r = GetEncoding(fs);
            fs.Close();
            return r;
        }
		static string Time2Str(int t, bool noms = false)
		{
			if (t == tMax) return tUndef;
			int h = t / 3600000;
			int m = t / 60000 % 60;
			int s = t / 1000 % 60;
			int ms = t % 1000;
			if (noms) return string.Format("{0}:{1:00}:{2:00}", h, m, s);
			return string.Format("{0:00}:{1:00}:{2:00},", h, m, s) + string.Format("{0:000}", ms);
		}
		static int Str2Time(string t)
		{
			if (t == tUndef) return tMax;
			try {
				int h = int.Parse(t.Substring(0, 2));
				int m = int.Parse(t.Substring(3, 2));
				int s = int.Parse(t.Substring(6, 2));
				int ms = int.Parse(t.Substring(9));
				return (h * 3600000) + (m * 60000) + (s * 1000) + ms;
			} catch {}
			return 0;
		}
		static int Index(string ss, string s)
		{
			int i = 0;
			int j = ss.IndexOf(s);
			while (j-- > 0) {
				if (ss[j] == ',') i++;
			}
			return i;
		}
		static string IndexStr(string s, int i)
		{
			int j = 0;
			while (true) {
				if (s[++j] == ',') {
					i--;
					if (i == 0) break;
				}
			}
			return s.Substring(++j);
		}
		static string AssText(string s)
		{
			s = s.Replace("        ", "|").Replace("\\N", "|");
			while (true)
			{
				int i, j;
				i = s.IndexOf('{');
				if (i < 0) break;
				j = s.IndexOf('}', i);
				if (j < 0) break;
				s = s.Substring(0, i) + s.Substring(j+1);
			}
			return s;
		}

		class Subtitle
		{
			public int Start; // ms
			public int End;
			public string Text;

			public Subtitle(int start, int end = tMax, string text = null)
			{
				Start = start;
				End = end;
				Text = text;
			}
			public override string ToString()
			{
				return Time2Str(Start) +" "+ Time2Str(End) + " "+ Text;
			}
		}
		
		void LoadFiles(string filename)
		{
			if (filename.EndsWith(".srt") || filename.EndsWith(".ass")) {
				string s = filename.Substring(0, filename.LastIndexOf('.') + 1);
				if (File.Exists(s + "mp4")) Player.URL = s + "mp4";
				else if (File.Exists(s + "mpv")) Player.URL = s + "mpv";
				else if (File.Exists(s + "mkv")) Player.URL = s + "mkv";
				else if (File.Exists(s + "mov")) Player.URL = s + "mov";
				else if (File.Exists(s + "avi")) Player.URL = s + "avi";
				else if (File.Exists(s + "rmvb")) Player.URL = s + "rmvb";
				Player.Tag = filename.Replace(".ass", ".srt");
			} else {
				Player.URL = filename;
				Player.Tag = filename = filename.Substring(0, filename.LastIndexOf('.') + 1) + "srt";
			}
			LoadSubs(filename);
		}
		
		public MainForm(string filename)
		{
 			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			//typeof(ListBox).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, Subs, new object[] { true });
			LoadFiles(filename);
		}

		// update player current position
		void UpdatePlayer(int t)
		{
			bool p = Player.playState != WMPPlayState.wmppsPlaying;
			if (p) Player.Ctlcontrols.play();
			Player.Ctlcontrols.currentPosition = t / 1000.0;
			if (p) {
				Player.Ctlcontrols.pause();
				TimerTick(null, null);
			}
		}
		// load subs from srt file
		void LoadSubs(string filename)
		{
			bool bSrt = filename.EndsWith(".srt");
			Subs.BeginUpdate();
			Subs.Items.Clear();
			if (File.Exists(filename)) {
				string ss;
				int si = 0, ei = 0, ti = 0;
				var f = new StreamReader(filename, GetEncoding(filename));
				while ((ss = f.ReadLine()) != null) { // eof
					Subtitle s;
					if (bSrt) {
						if (string.IsNullOrWhiteSpace(ss)) continue; // skip empty line & i
	
						ss = f.ReadLine(); // start --> end
						s = new Subtitle(Str2Time(ss.Substring(0, 12)));
						s.End = Str2Time(ss.Substring(17));
		
						while (!string.IsNullOrEmpty(ss = f.ReadLine())) {
							if (string.IsNullOrEmpty(s.Text))
								s.Text = ss;
							else
								s.Text += "|" + ss;
						}
					} else {
						if (ss.StartsWith("Format:")) {
							si = Index(ss, "Start");
							ei = Index(ss, "End");
							ti = Index(ss, "Text");
							continue;
						}
						if (!ss.StartsWith("Dialogue:")) continue;
						s = new Subtitle(Str2Time("0" + IndexStr(ss, si).Substring(0, 10) + "0"),
						                 Str2Time("0" + IndexStr(ss, ei).Substring(0, 10) + "0"),
						                 AssText(IndexStr(ss, ti)));
					}
					Subs.Items.Add(s);
				}
				f.Close();
			}
			Subs.EndUpdate();			
		}
		// save subs to srt file
		void SaveSubs(string filename)
		{
			if (Subs.Items.Count == 0
			    //|| (File.Exists(filename) && MessageBox.Show("Overwrite exists srt file?", "Suber", MessageBoxButtons.YesNo) == DialogResult.No)
			   ) return;

			File.Delete(filename);
			var f = File.CreateText(filename);
			int i = 1;
			foreach (Subtitle s in Subs.Items) {
				if (string.IsNullOrEmpty(s.Text)) continue;
				f.WriteLine(i++);
				f.WriteLine(Time2Str(s.Start) + " --> " + Time2Str(s.End));
				f.WriteLine(s.Text.Replace("|", "\r\n").TrimEnd() + "\r\n");
			}
			f.Close();
		}
		// adjust sub delay
		void AdjustSubs()
		{
			if (Subs.Items.Count == 0) return;

			int t = 0;
			int.TryParse(Interaction.InputBox("Input sub delay in ms:", Text, ""), out t);
			if (t == 0) return;
	
			Subs.BeginUpdate();
			foreach (Subtitle s in Subs.Items) {
				s.Start += t;
				s.End += t;
			}
			Subs.Invalidate();
			Subs.EndUpdate();
		}
		// input subtitle text
		bool InputText(Subtitle s, int i, bool p)
		{
			string ss = null;
			Player.Ctlcontrols.pause(); // force pause
#if false
			Point pt = Subs.GetItemRectangle(i).Location;
			pt.X += 4;
			pt.Y += Subs.GetItemHeight(i) + 4;
			pt = Subs.PointToScreen(pt);
			if ((pt.Y + 210) > Screen.PrimaryScreen.Bounds.Height) {
				pt = Subs.GetItemRectangle(i).Location;
				pt.X += 4;
				pt.Y -= 172;
				pt = Subs.PointToScreen(pt);
			}
			ss = Interaction.InputBox("Input subtitle text:", Text, s.Text, pt.X, pt.Y);
#else
			ss = Interaction.InputBox("Input subtitle text:", Text, s.Text);
#endif
			if (!p) Player.Ctlcontrols.play(); // resume play
			if (!string.IsNullOrEmpty(ss)) s.Text = ss;
			if (p = string.IsNullOrEmpty(s.Text)) Subs.Items.RemoveAt(i);
			return p;
		}

		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			SubText.Visible = false;
			SaveSubs(Player.Tag as string);
		}
		void SubsFocus(object sender, EventArgs e)
		{
			Subs.Focus();
		}
		void SubsDrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0) return;
			if (tWidth < 0) tWidth = (int)e.Graphics.MeasureString(tUndef + "-", e.Font).Width - 4;

			var s = Subs.Items[e.Index] as Subtitle;
			if (s.End != tMax) {
				g.FillRectangle(Brushes.HotPink, 0, 0, tWidth, e.Bounds.Height);
				g.FillRectangle(Brushes.MediumPurple, tWidth, 0, tWidth, e.Bounds.Height);
				g.FillRectangle(new SolidBrush(e.BackColor), tWidth * 2, 0, e.Bounds.Width - tWidth * 2, e.Bounds.Height);
			} else {
				g.FillRectangle(Brushes.DarkOrange, 0, 0, e.Bounds.Width, e.Bounds.Height);
			}
			g.DrawString(s.ToString(), e.Font, new SolidBrush(e.ForeColor), 0, 0);
			e.Graphics.DrawImageUnscaled(bg, e.Bounds.Left, e.Bounds.Top);
		}
		void SubsMouseDoubleClick(object sender, MouseEventArgs e)
		{
			int i = Subs.SelectedIndex;
			if (i < 0) return;

			var s = Subs.SelectedItem as Subtitle;
			SubText.Text = s.Text.Replace("|", "\r\n");
			if (e == null || e.X > tWidth * 2) {
				bool p = Player.playState != WMPPlayState.wmppsPlaying;
				if (p) Player.Ctlcontrols.play();
				Player.Ctlcontrols.currentPosition = s.Start / 1000.0;
				if (!InputText(s, i, p)) {
					Subs.Invalidate(Subs.GetItemRectangle(i));
					SubText.Text = s.Text.Replace("|", "\r\n");
				}
			}
			else if (e.X < tWidth)
				UpdatePlayer(s.Start);
			else if (s.End != tMax)
				UpdatePlayer(s.End);
		}
		void SubsClientSizeChanged(object sender, EventArgs e)
		{
			g = Graphics.FromImage(bg = new Bitmap(Subs.ClientSize.Width, 14));
		}
		void TimerTick(object sender, EventArgs e)
		{
			int t = (int)(Player.Ctlcontrols.currentPosition * 1000);
			string ss = "";
			Subtitle st = null;
			foreach (Subtitle s in Subs.Items) {
				if (s.Start <= t && s.End > t) {
					st = s;
					ss = s.Text.Replace("|", "\r\n");
					break;
				}
			}
			if (string.CompareOrdinal(SubText.Text, ss) != 0) {
				SubText.Text = ss;
				if (st != null) Subs.SelectedItem = st;
			}
		}
		void PlayerPlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
		{
			Timer.Enabled = (e.newState >= 3 && e.newState <= 5); // playing/ff/fb
			if (e.newState == 10) { // ready
				Player.Ctlcontrols.play();
				Player.stretchToFit = true;
				//Player.settings.rate = 2.0;
			}
		}
#if false
		Font FitFont(Font f)
		{
			while ((g.MeasureString("中\r\n中", f).Height - 6) > SubText.Height) {
				f = new Font(f.FontFamily, (float)(f.Size - 1));
			}
			return f;
		}
#endif
		void SubTextMouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) {
				if (fontDialog.ShowDialog() == DialogResult.OK) {
					SubText.Font = fontDialog.Font; //= FitFont(fontDialog.Font);
					SubText.ForeColor = fontDialog.Color;
				}
			} else //if (!string.IsNullOrEmpty(SubText.Text))
				SubsMouseDoubleClick(sender, null);
		}
		void MainFormShown(object sender, EventArgs e)
		{
			fontDialog.Font = SubText.Font; //= FitFont(SubText.Font);
			fontDialog.Color =SubText.ForeColor;
		}
		void DropBoxDragDrop(object sender, DragEventArgs e)
		{
			SaveSubs(Player.Tag as string);
			LoadFiles((e.Data.GetData("FileNameW") as string[])[0]);
		}
		void DropBoxDragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("FileNameW")) e.Effect = DragDropEffects.Copy;
		}
		void SplitSplitterMoved(object sender, SplitterEventArgs e)
		{
			Subs.Focus();
		}
		void MainFormKeyDown(object sender, KeyEventArgs e)
		{
			var s = Subs.SelectedItem as Subtitle;
			int i = Subs.SelectedIndex;
			bool p = Player.playState != WMPPlayState.wmppsPlaying;

			e.Handled = true;
			switch (e.KeyCode) {
			case Keys.D:
				StartT.Text = Time2Str((int)(Player.Ctlcontrols.currentPosition * 1000), true);
				break;
			case Keys.F:
				EndT.Text = Time2Str((int)(Player.Ctlcontrols.currentPosition * 1000), true);
				break;
			case Keys.C:
				string pa = "\"" + Player.URL + "\" " + StartT.Text +"-"+ EndT.Text;
				if (string.Compare(Aspect.Text, "Keep Aspect") != 0)
				{
					pa += " --aspect-ratio -1:" + Aspect.Text;
				}
				Player.Ctlcontrols.stop();
				Process.Start("F:/Tools/mc.bat", pa);
				Close();
				return;
			case Keys.Enter:
				if (e.Alt) {
					Player.fullScreen = !Player.fullScreen;
					return;
				}
				if (e.Control) {
					UpdatePlayer(s.Start);
					return;
				}
				if (p) {
					Player.Ctlcontrols.play();
					return;
				}
				if (s != null && s.End == tMax) {
					Split.Panel1.BackColor = Color.DimGray;
					s.End = (int)(Player.Ctlcontrols.currentPosition * 1000 - Player.settings.rate * tDelayE); // update end time
				} else {
					// new subtitle entry
					s = new Subtitle((int)(Player.Ctlcontrols.currentPosition * 1000 - Player.settings.rate * tDelayS));
					Subs.Items.Add(s);
					Subs.SelectedItem = s;
					i = Subs.SelectedIndex;
					Split.Panel1.BackColor = Color.DarkOrange;
					if (InputText(s, i, false))	{
						Split.Panel1.BackColor = Color.DimGray;
						try { Subs.SelectedIndex = (i > 0) ? --i : i; } catch {}
						return;
					}
				}
				break;
			case Keys.Escape:
				Close();
				return;
			// switch play speed
			case Keys.Back:
				rate = (rate + 1) % rates.Length;
				Player.settings.rate = rates[rate];
				return;
			// seek
			case Keys.Q:
			case Keys.Left:
				//Player.Ctlcontrols.currentPosition -= 3;
				UpdatePlayer((int)((Player.Ctlcontrols.currentPosition - 3) * 1000));
				return;
			case Keys.W:
			case Keys.Right:
				//Player.Ctlcontrols.currentPosition += 3;
				UpdatePlayer((int)((Player.Ctlcontrols.currentPosition + 3) * 1000));
				return;
			// play/pause
			case Keys.Space:
				if (p)
					Player.Ctlcontrols.play();
				else
					Player.Ctlcontrols.pause();
				return;
			case Keys.E:
				SaveSubs(Player.Tag as string);
				return;
			case Keys.R:
				AdjustSubs();
				return;
			}

			if (StartT.Focused || EndT.Focused || i < 0) {
				e.Handled = false;
				return;
			}

			int t = -1;
			int l;
			switch (e.KeyCode) {
			// start time
			case Keys.A:
				l = (i > 0) ? (Subs.Items[i-1] as Subtitle).End : 0;
				t = s.Start - 200;
				if (t <= l) t = l + 50;
				s.Start = t;
				break;
			case Keys.S:
				l = s.End;
				t = s.Start + 200;
				if (t >= l) t = l - 200;
				s.Start = t;
				break;
			// end time
			case Keys.Z:
				if (s.End == tMax) return;
				l = s.Start;
				t = s.End - 200;
				if (t <= l) t = l + 200;
				s.End = t;
				break;
			case Keys.X:
				if (s.End == tMax) return;
				l = (i < Subs.Items.Count-1) ? (Subs.Items[i+1] as Subtitle).Start : 99999999;
				t = s.End + 200;
				if (t >= l) t = l - 50;
				s.End = t;
				break;

			case Keys.Delete:
				Subs.Items.RemoveAt(i);
				try { Subs.SelectedIndex = (i > 0) ? --i : i; } catch {}
				return;

			default:
				e.Handled = false;
				break;
			}
			if (t >= 0) UpdatePlayer(p ? t : (t - 1000));
			Subs.Invalidate(Subs.GetItemRectangle(i));
		}
	}
}
