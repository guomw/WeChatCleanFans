namespace WeChatCleanFans
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbSet = new System.Windows.Forms.PictureBox();
            this.pbHeadImg = new System.Windows.Forms.PictureBox();
            this.btnStartCleanFans = new System.Windows.Forms.Button();
            this.rtbContent = new System.Windows.Forms.RichTextBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.rtbMsg = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.wFriendsList2 = new WeChatCleanFans.Controls.WFriendsList();
            this.wFriendsList1 = new WeChatCleanFans.Controls.WFriendsList();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeadImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.pbSet);
            this.panel1.Controls.Add(this.pbHeadImg);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(53, 643);
            this.panel1.TabIndex = 1;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            // 
            // pbSet
            // 
            this.pbSet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbSet.Image = global::WeChatCleanFans.Properties.Resources.set;
            this.pbSet.Location = new System.Drawing.Point(12, 602);
            this.pbSet.Name = "pbSet";
            this.pbSet.Size = new System.Drawing.Size(26, 27);
            this.pbSet.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSet.TabIndex = 1;
            this.pbSet.TabStop = false;
            this.pbSet.Click += new System.EventHandler(this.pbSet_Click);
            // 
            // pbHeadImg
            // 
            this.pbHeadImg.Location = new System.Drawing.Point(11, 8);
            this.pbHeadImg.Name = "pbHeadImg";
            this.pbHeadImg.Size = new System.Drawing.Size(27, 29);
            this.pbHeadImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbHeadImg.TabIndex = 0;
            this.pbHeadImg.TabStop = false;
            // 
            // btnStartCleanFans
            // 
            this.btnStartCleanFans.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnStartCleanFans.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartCleanFans.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnStartCleanFans.Location = new System.Drawing.Point(636, 34);
            this.btnStartCleanFans.Name = "btnStartCleanFans";
            this.btnStartCleanFans.Size = new System.Drawing.Size(175, 68);
            this.btnStartCleanFans.TabIndex = 2;
            this.btnStartCleanFans.Text = "开始扫描僵尸粉";
            this.btnStartCleanFans.UseVisualStyleBackColor = false;
            this.btnStartCleanFans.Click += new System.EventHandler(this.btnStartCleanFans_Click);
            // 
            // rtbContent
            // 
            this.rtbContent.BackColor = System.Drawing.SystemColors.Menu;
            this.rtbContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbContent.Location = new System.Drawing.Point(327, 34);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.Size = new System.Drawing.Size(307, 68);
            this.rtbContent.TabIndex = 5;
            this.rtbContent.Text = "";
            this.rtbContent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.rtbContent.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.rtbContent.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            // 
            // pbClose
            // 
            this.pbClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbClose.Image = global::WeChatCleanFans.Properties.Resources.ico_close;
            this.pbClose.Location = new System.Drawing.Point(867, 3);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(15, 14);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbClose.TabIndex = 4;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            // 
            // rtbMsg
            // 
            this.rtbMsg.BackColor = System.Drawing.SystemColors.Menu;
            this.rtbMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMsg.Location = new System.Drawing.Point(327, 150);
            this.rtbMsg.Name = "rtbMsg";
            this.rtbMsg.ReadOnly = true;
            this.rtbMsg.Size = new System.Drawing.Size(307, 473);
            this.rtbMsg.TabIndex = 7;
            this.rtbMsg.Text = "";
            this.rtbMsg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.rtbMsg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.rtbMsg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(325, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "清理发送内容：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(325, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "清理日志：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(650, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "僵尸粉列表：";
            // 
            // wFriendsList2
            // 
            this.wFriendsList2.BackColor = System.Drawing.SystemColors.MenuBar;
            this.wFriendsList2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.wFriendsList2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.wFriendsList2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.wFriendsList2.FormattingEnabled = true;
            this.wFriendsList2.Location = new System.Drawing.Point(652, 149);
            this.wFriendsList2.Name = "wFriendsList2";
            this.wFriendsList2.Size = new System.Drawing.Size(219, 473);
            this.wFriendsList2.TabIndex = 6;
            this.wFriendsList2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.wFriendsList2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.wFriendsList2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            // 
            // wFriendsList1
            // 
            this.wFriendsList1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.wFriendsList1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.wFriendsList1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.wFriendsList1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.wFriendsList1.FormattingEnabled = true;
            this.wFriendsList1.Location = new System.Drawing.Point(54, 1);
            this.wFriendsList1.Name = "wFriendsList1";
            this.wFriendsList1.Size = new System.Drawing.Size(256, 642);
            this.wFriendsList1.TabIndex = 0;
            this.wFriendsList1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.wFriendsList1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.wFriendsList1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(889, 640);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbMsg);
            this.Controls.Add(this.wFriendsList2);
            this.Controls.Add(this.rtbContent);
            this.Controls.Add(this.pbClose);
            this.Controls.Add(this.btnStartCleanFans);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.wFriendsList1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeadImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.WFriendsList wFriendsList1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pbHeadImg;
        private System.Windows.Forms.PictureBox pbSet;
        private System.Windows.Forms.Button btnStartCleanFans;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.RichTextBox rtbContent;
        private Controls.WFriendsList wFriendsList2;
        private System.Windows.Forms.RichTextBox rtbMsg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}