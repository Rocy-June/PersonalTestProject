using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Model.Data;
using WebSocketForm.Model.Enum;
using WebSocketForm.Model.File;
using WebSocketForm.Model.View;

namespace WebSocketForm.Helper
{
    static class ModelHelper
    {
        #region 用户信息转换

        public static User FileUserToViewUser(File_User user)
        {
            return new User
            {
                IP = new IPAddress(user.IP),
                Name = user.Name,
                NickName = user.NickName,
                OnlineStatus = OnlineStatus.Offline,
                LastResponsedTime = new DateTime(user.LastResponsedTime),
                HeadImage = ImageHelper.BytesToBitmapImage(user.HeadImage),
                IsTop = user.IsTop,
                IsMuted = user.IsMuted
            };
        }

        public static User DataUserToViewUser(Data_User user)
        {
            return new User
            {
                IP = new IPAddress(user.IP),
                Name = user.Name,
                OnlineStatus = (OnlineStatus)user.OnlineStatus,
                LastResponsedTime = new DateTime(user.RequestTime),
                HeadImage = ImageHelper.BytesToBitmapImage(user.HeadImage),
                IsTop = false,
                IsMuted = false
            };
        }

        public static File_User ViewUserToFileUser(User user)
        {
            return new File_User
            {
                IP = user.IP.GetAddressBytes(),
                Name = user.Name,
                NickName = user.NickName,
                OnlineStatus = (int)user.OnlineStatus,
                LastResponsedTime = user.LastResponsedTime.Ticks,
                HeadImage = ImageHelper.BitmapImageToBytes(user.HeadImage),
                IsTop = user.IsTop
            };
        }

        public static Data_User FileUserToDataUser(File_User user)
        {
            return new Data_User
            {
                IP = user.IP,
                HeadImage = user.HeadImage,
                Name = user.Name,
                OnlineStatus = user.OnlineStatus,
                RequestTime = DateTime.Now.Ticks
            };
        }

        #endregion
        
        #region 群组信息转换

        public static GroupChat FileGroupChatToViewGroupChat(File_GroupChat group)
        {
            return new GroupChat
            {
                OwnerID = new IPAddress(group.OwnerID),
                ID = new DateTime(group.ID),
                Members = group.Members.Select(e => FileUserToViewUser(e)).ToList(),
                Name = group.Name,
                HeadImage = ImageHelper.BytesToBitmapImage(group.HeadImage),
                IsTop = group.IsTop,
                IsMuted = group.IsMuted
            };
        }

        public static GroupChat DataGroupChatToViewGroupChat(Data_GroupChat group)
        {
            return new GroupChat
            {
                Name = group.Name,
                OwnerID = new IPAddress(group.OwnerID),
                ID = new DateTime(group.ID),
                Members = group.Members.Select(e => DataUserToViewUser(e)).ToList(),
                HeadImage = ImageHelper.BytesToBitmapImage(group.HeadImage),
                IsTop = false,
                IsMuted = false
            };
        }

        public static File_GroupChat ViewGroupChatToFileGroupChat(GroupChat group)
        {
            return new File_GroupChat
            {
                OwnerID = group.OwnerID.GetAddressBytes(),
                ID = group.ID.Ticks,
                Members = group.Members.Select(e => ViewUserToFileUser(e)).ToList(),
                Name = group.Name,
                HeadImage = ImageHelper.BitmapImageToBytes(group.HeadImage),
                IsTop = group.IsTop
            };
        } 

        #endregion
    }
}
