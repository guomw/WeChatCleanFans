using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeChatCleanFans.Controls;
using WwChatHttpCore.HTTP;
using WwChatHttpCore.Objects;

namespace WeChatCleanFans
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 主界面等待提示
        /// </summary>
        private Label _lblWait;

        /// <summary>
        /// 当前登录微信用户
        /// </summary>
        private WXUser _me;

        /// <summary>
        /// 通讯录
        /// </summary>
        private WFriendsList _chat2friend;

        //private WFriendsList _waitCleanfriend;

        private List<object> _contact_all = new List<object>();
        private List<object> _contact_latest = new List<object>();

        List<object> contact_all = new List<object>();
        List<object> contact_group = new List<object>();
        List<object> contact_friends = new List<object>();

        List<WXUser> SendResult = new List<WXUser>();

        public MainForm(Image img)
        {
            InitializeComponent();
            pbHeadImg.Image = img;


            _chat2friend = new WFriendsList();
            _chat2friend.Dock = DockStyle.Fill;
            _chat2friend.Visible = false;
            _chat2friend.FriendInfoView += _chat2friend_FriendInfoView;
            Controls.Add(_chat2friend);


            //_waitCleanfriend = new WFriendsList();
            //_waitCleanfriend.Dock = DockStyle.Fill;
            //_waitCleanfriend.Visible = false;
            //Controls.Add(_waitCleanfriend);


            _lblWait = new Label();
            _lblWait.Text = "数据加载...";
            _lblWait.AutoSize = false;
            _lblWait.Size = this.ClientSize;
            _lblWait.TextAlign = ContentAlignment.MiddleCenter;
            _lblWait.Location = new Point(0, 0);
            Controls.Add(_lblWait);
        }

        private void _chat2friend_FriendInfoView(WXUser user)
        {
            //throw new NotImplementedException();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DoMainLogic();
        }


        #region 主逻辑
        /// <summary>
        /// 
        /// </summary>
        private void DoMainLogic()
        {
            _lblWait.BringToFront();
            ((Action)(delegate ()
            {
                WXService wxs = new WXService();
                JObject init_result = wxs.WxInit();  //初始化
                contact_all = new List<object>();
                contact_group = new List<object>();
                contact_friends = new List<object>();

                if (init_result != null)
                {
                    _me = new WXUser();
                    _me.UserName = init_result["User"]["UserName"].ToString();
                    _me.City = "";
                    _me.HeadImgUrl = init_result["User"]["HeadImgUrl"].ToString();
                    _me.NickName = init_result["User"]["NickName"].ToString();
                    _me.Province = "";
                    _me.PYQuanPin = init_result["User"]["PYQuanPin"].ToString();
                    _me.RemarkName = init_result["User"]["RemarkName"].ToString();
                    _me.RemarkPYQuanPin = init_result["User"]["RemarkPYQuanPin"].ToString();
                    _me.Sex = init_result["User"]["Sex"].ToString();
                    _me.Signature = init_result["User"]["Signature"].ToString();
                }

                JObject contact_result = wxs.GetContact(); //通讯录
                if (contact_result != null)
                {
                    foreach (JObject contact in contact_result["MemberList"])  //完整好友名单
                    {
                        if (contact["UserName"].ToString().Contains("@") && !contact["UserName"].ToString().Contains("@@") && contact["VerifyFlag"].ToString() == "0")
                        {
                            WXUser user = new WXUser();
                            user.UserName = contact["UserName"].ToString();
                            user.City = contact["City"].ToString();
                            user.HeadImgUrl = contact["HeadImgUrl"].ToString();
                            user.NickName = contact["NickName"].ToString();
                            user.Province = contact["Province"].ToString();
                            user.PYQuanPin = contact["PYQuanPin"].ToString();
                            user.RemarkName = contact["RemarkName"].ToString();
                            user.RemarkPYQuanPin = contact["RemarkPYQuanPin"].ToString();
                            user.Sex = contact["Sex"].ToString();
                            user.Signature = contact["Signature"].ToString();
                            user.IsOwner = Convert.ToInt32(contact["IsOwner"].ToString());
                            contact_all.Add(user);
                        }
                        if (contact["UserName"].ToString().Contains("@@"))
                        {
                            WXUser user = new WXUser();
                            user.UserName = contact["UserName"].ToString();
                            user.City = contact["City"].ToString();
                            user.HeadImgUrl = contact["HeadImgUrl"].ToString();
                            user.NickName = contact["NickName"].ToString();
                            user.Province = contact["Province"].ToString();
                            user.PYQuanPin = contact["PYQuanPin"].ToString();
                            user.RemarkName = contact["RemarkName"].ToString();
                            user.RemarkPYQuanPin = contact["RemarkPYQuanPin"].ToString();
                            user.Sex = contact["Sex"].ToString();
                            user.Signature = contact["Signature"].ToString();
                            user.IsOwner = Convert.ToInt32(contact["IsOwner"].ToString());
                            contact_group.Add(user);
                        }
                    }
                }
                IOrderedEnumerable<object> list_all = contact_all.OrderBy(e => (e as WXUser).ShowPinYin);

                WXUser wx; string start_char;
                foreach (object o in list_all)
                {
                    wx = o as WXUser;
                    start_char = wx.ShowPinYin == "" ? "" : wx.ShowPinYin.Substring(0, 1);
                    if (!_contact_all.Contains(start_char.ToUpper()))
                    {
                        _contact_all.Add(start_char.ToUpper());
                    }
                    _contact_all.Add(o);
                }

                this.BeginInvoke((Action)(delegate ()  //等待结束
                {
                    _lblWait.Visible = false;
                    wFriendsList1.Items.AddRange(_contact_all.ToArray());  //通讯录

                }));


                string sync_flag = "";
                JObject sync_result;
                while (true)
                {
                    sync_flag = wxs.WxSyncCheck();  //同步检查
                    if (sync_flag == null)
                    {
                        continue;
                    }
                    //这里应该判断 sync_flag中selector的值
                    else //有消息
                    {
                        sync_result = wxs.WxSync();  //进行同步
                        if (sync_result != null)
                        {
                            if (sync_result["AddMsgCount"] != null && sync_result["AddMsgCount"].ToString() != "0")
                            {
                                foreach (JObject m in sync_result["AddMsgList"])
                                {
                                    string from = m["FromUserName"].ToString();
                                    string to = m["ToUserName"].ToString();
                                    string content = m["Content"].ToString();
                                    string type = m["MsgType"].ToString();
                                    if (type == "10000")
                                    {
                                        if (content.Contains("开启了朋友验证") || content.Contains("消息已发出，但被对方拒收"))
                                        {
                                            WXUser user = SendResult.Find(item =>
                                             {
                                                 return item.UserName == from;
                                             });
                                            if (user != null)
                                            {
                                                this.BeginInvoke((Action)(delegate ()  //等待结束
                                                {
                                                    wFriendsList2.Items.Add(user);
                                                }));
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(10);
                }

            })).BeginInvoke(null, null);
        }
        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void pbSet_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private bool isStart = false;
        private void btnStartCleanFans_Click(object sender, EventArgs e)
        {
            wFriendsList2.Items.Clear();
            if (!isStart)
            {
                string Msg = rtbContent.Text;
                isStart = true;
                rtbMsg.Clear();
                btnStartCleanFans.Text = "正在扫描中";
                btnStartCleanFans.Enabled = false;
                wFriendsList2.Items.Add("");
                ((Action)(delegate ()
               {
                   SetText(">>>" + DateTime.Now.ToString("HH:mm:ss") + " 开始扫描僵尸粉..." + "\r\n");
                   foreach (var item in contact_all)
                   {
                       WXUser _friendUser = item as WXUser;

                       SetText(">>>" + DateTime.Now.ToString("HH:mm:ss") + " 正在扫描【" + _friendUser.ShowName + "】...");
                       WXMsg msg = new WXMsg();
                       msg.From = _me.UserName;
                       msg.Msg = Msg;
                       msg.Readed = false;
                       msg.To = _friendUser.UserName;
                       msg.Type = 1;
                       msg.Time = DateTime.Now;
                       SendResult.Add(_friendUser);
                       _friendUser.SendMsg(msg, false);
                       SetText("ok." + "\r\n");
                       System.Threading.Thread.Sleep(2000);
                   }
                   SetText(">>>" + DateTime.Now.ToString("HH:mm:ss") + " 扫描完成." + "\r\n");
                   isStart = false;


                   this.BeginInvoke((Action)(delegate ()  //等待结束
                   {
                       btnStartCleanFans.Text = "开始扫描僵尸粉";
                       btnStartCleanFans.Enabled = true;
                   }));
               })).BeginInvoke(null, null);


            }


        }

        private void SetText(string text)
        {
            if (rtbMsg.InvokeRequired)
            {
                this.Invoke(new Action<string>(SetText), new object[] { text });
            }
            else
            {
                rtbMsg.AppendText(text);
                rtbMsg.Refresh();
                rtbMsg.ScrollToCaret();
            }
        }


        private bool isMouseDown = false;
        private Point FormLocation;     //form的location
        private Point mouseOffset;      //鼠标的按下位置

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                FormLocation = this.Location;
                mouseOffset = Control.MousePosition;
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            int _x = 0;
            int _y = 0;
            if (isMouseDown)
            {
                Point pt = Control.MousePosition;
                _x = mouseOffset.X - pt.X;
                _y = mouseOffset.Y - pt.Y;

                this.Location = new Point(FormLocation.X - _x, FormLocation.Y - _y);
            }
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
