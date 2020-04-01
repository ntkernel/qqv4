using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LLTAPI_v3.Modules;
using Newtonsoft.Json.Linq;
using System.IO;

namespace LLTAPI_v3.Controllers
{
    [Route("qqv3")]
    [ApiController]
    public class QQv3Controller : ControllerBase
    {
        v3Auth vA = new v3Auth();
        QQv3R vR = new QQv3R();
        WX_T wxt = new WX_T();
        Settings settings = new Settings();
        [Route("sendfriendmsg")]
        [HttpPost]
        public string sendfriendmsg([FromQuery]long id,[FromQuery]string token)
        {
            
            StreamReader reader = new StreamReader(Request.Body);
            vR.sendFriendMsg(id, reader.ReadToEnd());
            return "sent";
        }
        [Route("sendgroupmsg")]
        [HttpPost]
        public string sendgroupmsg([FromQuery]long id, [FromQuery]string token)
        {
            //TODO
            if (!vA.checkToken(token))
            {
                return "-1";
            }
            StreamReader reader = new StreamReader(Request.Body);
            vR.sendGroupMsg(id, reader.ReadToEnd());
            return "sent";
        }
        [Route("forcereset")]
        [HttpGet]
        public string forcereset([FromQuery]string token)
        {
            
            vR.reset();
            return "ok";
        }
        [Route("x-local-recv")]
        [HttpPost]
        public string localconnect([FromQuery]long id,[FromQuery]string action)
        {
            StreamReader reader = new StreamReader(Request.Body);
            string instr = reader.ReadToEnd();
            if (action == "friend")
            {
                //TODO
                
            }
            if (action == "group")
            {
                //TODO
            }
            if(action == "reset" && id == 4888 && instr == "RESET" && authDB.resetable)
            {
                vR.reset();
            }
            return "X";
        }
    }
}