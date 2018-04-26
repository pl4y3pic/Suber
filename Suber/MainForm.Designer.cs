/*
 * Created by SharpDevelop.
 * User: Kent
 * Date: 2018/01/01
 * Time: 19:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Suber
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private Suber.TransControl DropBox;
		private System.Windows.Forms.SplitContainer Split;
		private System.Windows.Forms.Label SubText;
		private AxWMPLib.AxWindowsMediaPlayer Player;
		private System.Windows.Forms.ListBox Subs;
		private System.Windows.Forms.Timer Timer;
		private System.Windows.Forms.FontDialog fontDialog;
		private System.Windows.Forms.ComboBox Aspect;
		private System.Windows.Forms.TextBox EndT;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox StartT;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
        private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.DropBox = new Suber.TransControl();
			this.Split = new System.Windows.Forms.SplitContainer();
			this.SubText = new System.Windows.Forms.Label();
			this.Player = new AxWMPLib.AxWindowsMediaPlayer();
			this.EndT = new System.Windows.Forms.TextBox();
			this.Aspect = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.StartT = new System.Windows.Forms.TextBox();
			this.Subs = new System.Windows.Forms.ListBox();
			this.Timer = new System.Windows.Forms.Timer(this.components);
			this.fontDialog = new System.Windows.Forms.FontDialog();
			((System.ComponentModel.ISupportInitialize)(this.Split)).BeginInit();
			this.Split.Panel1.SuspendLayout();
			this.Split.Panel2.SuspendLayout();
			this.Split.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Player)).BeginInit();
			this.SuspendLayout();
			// 
			// DropBox
			// 
			this.DropBox.AllowDrop = true;
			this.DropBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DropBox.Location = new System.Drawing.Point(0, 0);
			this.DropBox.Name = "DropBox";
			this.DropBox.Size = new System.Drawing.Size(1303, 671);
			this.DropBox.TabIndex = 9;
			this.DropBox.TabStop = false;
			this.DropBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.DropBoxDragDrop);
			this.DropBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.DropBoxDragEnter);
			// 
			// Split
			// 
			this.Split.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Split.Location = new System.Drawing.Point(0, 0);
			this.Split.Name = "Split";
			// 
			// Split.Panel1
			// 
			this.Split.Panel1.BackColor = System.Drawing.Color.DimGray;
			this.Split.Panel1.Controls.Add(this.SubText);
			this.Split.Panel1.Controls.Add(this.Player);
			// 
			// Split.Panel2
			// 
			this.Split.Panel2.Controls.Add(this.EndT);
			this.Split.Panel2.Controls.Add(this.Aspect);
			this.Split.Panel2.Controls.Add(this.label1);
			this.Split.Panel2.Controls.Add(this.StartT);
			this.Split.Panel2.Controls.Add(this.Subs);
			this.Split.Size = new System.Drawing.Size(1303, 671);
			this.Split.SplitterDistance = 881;
			this.Split.TabIndex = 1;
			this.Split.TabStop = false;
			this.Split.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.SplitSplitterMoved);
			// 
			// SubText
			// 
			this.SubText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.SubText.BackColor = System.Drawing.Color.Black;
			this.SubText.Font = new System.Drawing.Font("经典繁仿圆", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.SubText.ForeColor = System.Drawing.Color.Yellow;
			this.SubText.Location = new System.Drawing.Point(4, 544);
			this.SubText.Name = "SubText";
			this.SubText.Size = new System.Drawing.Size(873, 64);
			this.SubText.TabIndex = 4;
			this.SubText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.SubText.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SubTextMouseClick);
			// 
			// Player
			// 
			this.Player.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.Player.Enabled = true;
			this.Player.Location = new System.Drawing.Point(4, 4);
			this.Player.Name = "Player";
			this.Player.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Player.OcxState")));
			this.Player.Size = new System.Drawing.Size(873, 663);
			this.Player.TabIndex = 3;
			this.Player.TabStop = false;
			this.Player.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.PlayerPlayStateChange);
			this.Player.Enter += new System.EventHandler(this.SubsFocus);
			// 
			// EndT
			// 
			this.EndT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.EndT.Location = new System.Drawing.Point(91, 637);
			this.EndT.Name = "EndT";
			this.EndT.Size = new System.Drawing.Size(64, 22);
			this.EndT.TabIndex = 8;
			// 
			// Aspect
			// 
			this.Aspect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.Aspect.FormattingEnabled = true;
			this.Aspect.Items.AddRange(new object[] {
			"Keep Aspect",
			"16/9",
			"4/3"});
			this.Aspect.Location = new System.Drawing.Point(176, 637);
			this.Aspect.Name = "Aspect";
			this.Aspect.Size = new System.Drawing.Size(105, 22);
			this.Aspect.TabIndex = 7;
			this.Aspect.Text = "Keep Aspect";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(76, 639);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(11, 14);
			this.label1.TabIndex = 5;
			this.label1.Text = "-";
			// 
			// StartT
			// 
			this.StartT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.StartT.Location = new System.Drawing.Point(8, 637);
			this.StartT.Name = "StartT";
			this.StartT.Size = new System.Drawing.Size(64, 22);
			this.StartT.TabIndex = 4;
			// 
			// Subs
			// 
			this.Subs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.Subs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.Subs.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Subs.FormattingEnabled = true;
			this.Subs.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.Subs.Location = new System.Drawing.Point(0, 0);
			this.Subs.Name = "Subs";
			this.Subs.Size = new System.Drawing.Size(418, 628);
			this.Subs.Sorted = true;
			this.Subs.TabIndex = 0;
			this.Subs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.SubsDrawItem);
			this.Subs.ClientSizeChanged += new System.EventHandler(this.SubsClientSizeChanged);
			this.Subs.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SubsMouseDoubleClick);
			// 
			// Timer
			// 
			this.Timer.Tick += new System.EventHandler(this.TimerTick);
			// 
			// fontDialog
			// 
			this.fontDialog.ShowColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1303, 671);
			this.Controls.Add(this.DropBox);
			this.Controls.Add(this.Split);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.KeyPreview = true;
			this.Name = "MainForm";
			this.Text = "Suber";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.Shown += new System.EventHandler(this.MainFormShown);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyDown);
			this.Split.Panel1.ResumeLayout(false);
			this.Split.Panel2.ResumeLayout(false);
			this.Split.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.Split)).EndInit();
			this.Split.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.Player)).EndInit();
			this.ResumeLayout(false);

		}
	}
}
