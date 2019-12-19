namespace PortPinger
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button_Start = new System.Windows.Forms.Button();
            this.progressBar_Wait = new System.Windows.Forms.ProgressBar();
            this.label_PlzWait = new System.Windows.Forms.Label();
            this.panel_info = new System.Windows.Forms.Panel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.MenuItem_Functions = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_CleanDIsconnected = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_BlockUser = new System.Windows.Forms.ToolStripMenuItem();
            this.StripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_ReconnectAlert = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SetNickName = new System.Windows.Forms.ToolStripMenuItem();
            this.StripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_StatusThumb = new System.Windows.Forms.ToolStripMenuItem();
            this.StripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_ShowTop = new System.Windows.Forms.ToolStripMenuItem();
            this.panel_info.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(36, 50);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(110, 23);
            this.button_Start.TabIndex = 0;
            this.button_Start.Text = "开始更新";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // progressBar_Wait
            // 
            this.progressBar_Wait.Location = new System.Drawing.Point(36, 50);
            this.progressBar_Wait.MarqueeAnimationSpeed = 20;
            this.progressBar_Wait.Name = "progressBar_Wait";
            this.progressBar_Wait.Size = new System.Drawing.Size(110, 23);
            this.progressBar_Wait.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar_Wait.TabIndex = 1;
            // 
            // label_PlzWait
            // 
            this.label_PlzWait.AutoSize = true;
            this.label_PlzWait.Location = new System.Drawing.Point(71, 35);
            this.label_PlzWait.Name = "label_PlzWait";
            this.label_PlzWait.Size = new System.Drawing.Size(41, 12);
            this.label_PlzWait.TabIndex = 2;
            this.label_PlzWait.Text = "请稍后";
            this.label_PlzWait.Visible = false;
            // 
            // panel_info
            // 
            this.panel_info.BackColor = System.Drawing.SystemColors.Control;
            this.panel_info.Controls.Add(this.label_PlzWait);
            this.panel_info.Controls.Add(this.button_Start);
            this.panel_info.Controls.Add(this.progressBar_Wait);
            this.panel_info.Location = new System.Drawing.Point(12, 27);
            this.panel_info.Name = "panel_info";
            this.panel_info.Size = new System.Drawing.Size(182, 115);
            this.panel_info.TabIndex = 3;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_Functions,
            this.MenuItem_ShowTop});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(206, 25);
            this.menuStrip.TabIndex = 4;
            this.menuStrip.Text = "菜单";
            // 
            // MenuItem_Functions
            // 
            this.MenuItem_Functions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_CleanDIsconnected,
            this.MenuItem_BlockUser,
            this.StripSeparator1,
            this.MenuItem_ReconnectAlert,
            this.MenuItem_SetNickName,
            this.StripSeparator2,
            this.MenuItem_StatusThumb,
            this.StripSeparator3,
            this.MenuItem_Refresh});
            this.MenuItem_Functions.Name = "MenuItem_Functions";
            this.MenuItem_Functions.Size = new System.Drawing.Size(44, 21);
            this.MenuItem_Functions.Text = "功能";
            // 
            // MenuItem_CleanDIsconnected
            // 
            this.MenuItem_CleanDIsconnected.Name = "MenuItem_CleanDIsconnected";
            this.MenuItem_CleanDIsconnected.Size = new System.Drawing.Size(148, 22);
            this.MenuItem_CleanDIsconnected.Text = "清理离线用户";
            // 
            // MenuItem_BlockUser
            // 
            this.MenuItem_BlockUser.Name = "MenuItem_BlockUser";
            this.MenuItem_BlockUser.Size = new System.Drawing.Size(148, 22);
            this.MenuItem_BlockUser.Text = "固定用户显示";
            // 
            // StripSeparator1
            // 
            this.StripSeparator1.Name = "StripSeparator1";
            this.StripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // MenuItem_ReconnectAlert
            // 
            this.MenuItem_ReconnectAlert.Name = "MenuItem_ReconnectAlert";
            this.MenuItem_ReconnectAlert.Size = new System.Drawing.Size(148, 22);
            this.MenuItem_ReconnectAlert.Text = "重连提醒";
            // 
            // MenuItem_SetNickName
            // 
            this.MenuItem_SetNickName.Name = "MenuItem_SetNickName";
            this.MenuItem_SetNickName.Size = new System.Drawing.Size(148, 22);
            this.MenuItem_SetNickName.Text = "设定名称";
            // 
            // StripSeparator2
            // 
            this.StripSeparator2.Name = "StripSeparator2";
            this.StripSeparator2.Size = new System.Drawing.Size(145, 6);
            // 
            // MenuItem_StatusThumb
            // 
            this.MenuItem_StatusThumb.Name = "MenuItem_StatusThumb";
            this.MenuItem_StatusThumb.Size = new System.Drawing.Size(148, 22);
            this.MenuItem_StatusThumb.Text = "状态缩略图";
            // 
            // StripSeparator3
            // 
            this.StripSeparator3.Name = "StripSeparator3";
            this.StripSeparator3.Size = new System.Drawing.Size(145, 6);
            // 
            // MenuItem_Refresh
            // 
            this.MenuItem_Refresh.Name = "MenuItem_Refresh";
            this.MenuItem_Refresh.Size = new System.Drawing.Size(148, 22);
            this.MenuItem_Refresh.Text = "刷新";
            // 
            // MenuItem_ShowTop
            // 
            this.MenuItem_ShowTop.Checked = true;
            this.MenuItem_ShowTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuItem_ShowTop.Name = "MenuItem_ShowTop";
            this.MenuItem_ShowTop.Size = new System.Drawing.Size(80, 21);
            this.MenuItem_ShowTop.Text = "置顶显示 √";
            this.MenuItem_ShowTop.Click += new System.EventHandler(this.MenuItem_ShowTop_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(206, 154);
            this.Controls.Add(this.panel_info);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "局域网测试";
            this.TopMost = true;
            this.panel_info.ResumeLayout(false);
            this.panel_info.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.ProgressBar progressBar_Wait;
        private System.Windows.Forms.Label label_PlzWait;
        private System.Windows.Forms.Panel panel_info;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Functions;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_CleanDIsconnected;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_BlockUser;
        private System.Windows.Forms.ToolStripSeparator StripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_ReconnectAlert;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SetNickName;
        private System.Windows.Forms.ToolStripSeparator StripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_StatusThumb;
        private System.Windows.Forms.ToolStripSeparator StripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Refresh;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_ShowTop;
    }
}

