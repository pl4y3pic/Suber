/*
 * Created by SharpDevelop.
 * User: Kent
 * Date: 2018/01/08
 * Time: 22:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Suber
{
	/// <summary>
	/// Description of TransControl.
	/// </summary>
	public partial class TransControl : UserControl
	{
		const int WS_EX_TRANSPARENT = 0x20;
		const int WM_NCHITTEST = 0x84;
		static IntPtr HTTRANSPARENT = new IntPtr(-1);

		public TransControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		protected override CreateParams CreateParams
		{
			get {
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= WS_EX_TRANSPARENT;
				return cp;
			}
		}
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_NCHITTEST) {
				m.Result = HTTRANSPARENT;
				return;
			}
			base.WndProc(ref m);
		}
		protected override void OnPaint(PaintEventArgs e)
		{
		}
		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}
	}
}
