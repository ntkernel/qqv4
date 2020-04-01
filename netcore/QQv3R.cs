using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
namespace LLTAPI_v3.Modules
{
    public class QQv3R
    {
        REST rest = new REST();
        Settings settings = new Settings();
        public void sendFriendMsg(long id,string content)
        {
            rest.g_strHttpPostOrPutRequest("POST", "http://localhost:4888/x_llt_qq/v3?action=sendfriendmsg&id=" + id, content);
        }
        public void sendGroupMsg(long id, string content)
        {
            rest.g_strHttpPostOrPutRequest("POST", "http://localhost:4888/x_llt_qq/v3?action=sendgroupmsg&id=" + id, content);
        }
        public void reset()
        {
            rest.g_strHttpPostOrPutRequest("POST", "http://localhost:4888/x_llt_qq/v3?action=reset&id=0", "x");
        }
        public string getFriendChatList()
        {
            string[] ChatL = Directory.GetDirectories(settings.v3FDir);
            int Dlc = ChatL.Length;
            JArray ja = new JArray();
            int cC = 0;
            while (cC < Dlc)
            {
                ja.Add(ChatL[cC].Substring(settings.v3FDir.Length));
                cC++;
            }
            return ja.ToString();
        }
        public string getGroupChatList()
        {
            string[] ChatL = Directory.GetDirectories(settings.v3GDir);
            int Dlc = ChatL.Length;
            JArray ja = new JArray();
            int cC = 0;
            while (cC < Dlc)
            {
                ja.Add(ChatL[cC].Substring(settings.v3GDir.Length));
                cC++;
            }
            return ja.ToString();
        }
        public string getChat(long id,bool isGroup)
        {
            string iPath = "";
            dynamic Joperator;
            if (isGroup)
            {
                iPath = settings.v3GDir;
            }
            else
            {
                iPath = settings.v3FDir;
            }
            iPath += id + "\\";
            if (!Directory.Exists(iPath))
            {
                return "WrongRQ";
            }
            JArray retJsonArr = new JArray();//level 0
            //get All Dir
            string[] perSentMsgList = Directory.GetDirectories(iPath);
            long cC1 = 0;//level 1
            while (cC1 < perSentMsgList.Length)
            {
                string lv1Path = perSentMsgList[cC1] + "\\";
                long cC2 = 0;//enter time dir and find all message such as 1TEXT
                JArray perMsgJsonList = new JArray();//level 1
                string[] childMsgList = Directory.GetFiles(lv1Path);
                while (cC2 < childMsgList.Length)
                {
                    string childMsgPath = childMsgList[cC2];
                    string childMsgName = childMsgPath.Substring(lv1Path.Length);
                    string type = "";
                    string childMsgId = "";
                    if (childMsgName.EndsWith("TEXT"))
                    {
                        type = "TEXT";
                        childMsgId = childMsgName.Substring(0, childMsgName.Length - 4);
                    }
                    if (childMsgName.EndsWith("IMAGE"))
                    {
                        type = "IMAGE";
                        childMsgId = childMsgName.Substring(0, childMsgName.Length - 5);
                    }
                    string msgContent = File.ReadAllText(childMsgPath);
                    JToken childMsgJson = new JObject();//level 2
                    Joperator = childMsgJson;
                    Joperator.type = type;
                    Joperator.content = msgContent;
                    perMsgJsonList.Add(childMsgJson);
                    cC2++;
                }
                retJsonArr.Add(perMsgJsonList);
                cC1++;
            }
            return retJsonArr.ToString();
        }
    }
}
