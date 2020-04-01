using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using llt.qq;
namespace QQ_LOCAL
{
    class Program
    {
        static HttpListener LIS;
        static LCBOT bot;
        static void Main(string[] args)
        {
            Console.WriteLine("[Main]:program started");
            Console.WriteLine("[Main]:starting Mirai Bot");
            bot = new LCBOT();
            bot.init(1234567890, "123456", "C:\\apidb\\QQ","http://localhost/qqv3/x-local-recv");
            //       账号         密码       数据目录         回掉链接
            Console.WriteLine("[Main]:Mirai Bot started");
            Console.WriteLine("[Main]:starting Http Listener");
            LIS = new HttpListener();
            LIS.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            LIS.Prefixes.Add("http://+:4888/X_LLT_QQ/v3/");
            LIS.Start();
            Console.WriteLine("[Main]:Http Listener started at" + LIS.Prefixes);
            Task.Run(() =>
            {
                Listen();
            });
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
        static void Listen()
        {
            while (true)
            {
                HttpListenerContext ctx = LIS.GetContext();
                StreamWriter writer = new StreamWriter(ctx.Response.OutputStream);
                StreamReader reader = new StreamReader(ctx.Request.InputStream);
                string inStr;
                if (!ctx.Request.IsLocal)
                {
                    Console.WriteLine("[HTTP]:recv unauth request");
                    writer.Write("UNAUTH REQUEST");
                    writer.Close();
                    ctx.Response.OutputStream.Close();
                }
                else
                {
                    inStr = reader.ReadToEnd();
                    string action = ctx.Request.QueryString["action"];
                    string id = ctx.Request.QueryString["id"];
                    if (action == "sendfriendmsg")
                    {
                        Console.WriteLine("[HTTP]:recv send friend message request");
                        //string id
                        try
                        {
                            Console.WriteLine("[Bot]:send friend request");
                            Task.Run(() =>
                            {
                                bot.sendFriendMsg(long.Parse(id), inStr);
                            });
                            Console.WriteLine("[Bot]:sent friend request");
                        }
                        catch
                        {
                            Console.WriteLine("[Bot]:fail to send friend request");
                        }
                        writer.Write("Friend request");
                        writer.Close();
                        ctx.Response.OutputStream.Close();
                    }
                    else
                    {
                        if (action == "sendgroupmsg")
                        {
                            Console.WriteLine("[HTTP]:recv send group message request");
                            try
                            {
                                Console.WriteLine("[Bot]:send group request");
                                Task.Run(() =>
                                {

                                    bot.sendGroupMsg(long.Parse(id), inStr);
                                });
                                Console.WriteLine("[Bot]:sent group request");
                            }
                            catch
                            {
                                Console.WriteLine("[Bot]:fail to send group request");
                            }
                            writer.Write("Group request");
                            writer.Close();
                            ctx.Response.OutputStream.Close();
                        }
                        else
                        {
                            if (action == "reset")
                            {
                                Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                                Process.GetCurrentProcess().Kill();
                            }
                            Console.WriteLine("recv wrong request");
                            writer.Write("Wrong request");
                            writer.Close();
                            ctx.Response.OutputStream.Close();
                        }
                    }
                }
            }
        }
    }
}
