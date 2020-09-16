using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandler
{
    public static class Config
    {
        public static readonly string TimeServer = "http://api.m.taobao.com/rest/api3.do?api=mtop.common.getTimestamp";

        public static readonly string PATH = AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string SETTING_DIRECTORY_PATH = $"{PATH}setting\\";

        public static readonly string HEAD_IMAGE_URI = $"{SETTING_DIRECTORY_PATH}HeadImage.png";

        public static readonly string SETTING_DATA_URI = $"{SETTING_DIRECTORY_PATH}setting.dat";

        public static readonly string SETTING_USER_DATA_URI = $"{SETTING_DIRECTORY_PATH}user.dat";

        public static readonly string SETTING_GROUP_DATA_URI = $"{SETTING_DIRECTORY_PATH}group.dat";

        public static readonly string DATABASE_DIRECTORY_PATH = $"{PATH}data\\";

        public static readonly string DATABASE_FILE_PATH = $"{DATABASE_DIRECTORY_PATH}localdata.db";
    }
}
