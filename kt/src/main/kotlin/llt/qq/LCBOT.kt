package llt.qq



import net.mamoe.mirai.alsoLogin
import net.mamoe.mirai.contact.sendMessage
import net.mamoe.mirai.qqandroid.Bot
import net.mamoe.mirai.qqandroid.QQAndroid
import kotlinx.coroutines.*
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.RequestBody.Companion.toRequestBody
import kotlinx.serialization.json.Json
import kotlinx.serialization.json.JsonObject
import net.mamoe.mirai.Bot
import net.mamoe.mirai.event.events.BotOfflineEvent
import net.mamoe.mirai.event.subscribeAlways
import net.mamoe.mirai.message.FriendMessage
import net.mamoe.mirai.message.GroupMessage
import net.mamoe.mirai.message.data.Image
import net.mamoe.mirai.message.data.Message
import net.mamoe.mirai.message.data.MessageChain
import net.mamoe.mirai.message.data.PlainText
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody
import okhttp3.Response
import java.io.File
import java.lang.Exception
import java.text.SimpleDateFormat
import java.util.*
import kotlin.system.exitProcess


class LCBOT{
    private var cProcess: Int = 0
    private var path: String = ""
    private var uri: String = ""
    var bot: Bot? = null
    fun init(id:Long, pwd:String, path:String, uri:String){
        //bot = QQAndroid.Bot(id,pwd){randomDeviceName=false}.alsoLogin()
        this.path = path
        this.uri = uri
        cProcess=1
        GlobalScope.launch {
            bot = QQAndroid.Bot(id,pwd){}.alsoLogin()
            //bot =
            cProcess=0
        }
        while (cProcess==1){
            Thread.sleep(100)
        }
        bot?.subscribeAlways<FriendMessage> {
            saveMsg(message,false, sender.id)
            println(message)

            //message[][0]
            post(uri+"?action=friend&id="+sender.id,message.toString())
            println("[KTMT]:Friendmsg "+uri+"?action=friend&id="+sender.id+" "+message.toString())
        }
        bot?.subscribeAlways<GroupMessage>{
            saveMsg(message,true, group.id)
            println(message)
            post(uri+"?action=group&id="+group.id,message.toString())
            println("[KTMT]:Groupmsg "+uri+"?action=group&id="+group.id+" "+message.toString())
        }
        bot?.subscribeAlways<BotOfflineEvent.Force>{
            println("offline")
            post("$uri?action=reset&id=4888","RESET")
        }
    }
    fun sendFriendMsg(id:Long, msg:String){
        cProcess=1
        GlobalScope.launch {
            try{
                bot!!.getFriend(id).sendMessage(msg)
                saveSentMsg(id,false,msg)
            }catch (e:Exception){
                //todo
            }
            cProcess=0
        }
        while (cProcess==1){
            Thread.sleep(100)
        }
    }
    fun sendGroupMsg(id:Long,msg:String){
        cProcess=1
        GlobalScope.launch {
            try{
                bot!!.getGroup(id).sendMessage(msg)
                saveSentMsg(id,true,msg)
            }catch (E:Exception){
                //todo
            }
            cProcess=0
        }
        while (cProcess==1){
            Thread.sleep(100)
        }
    }
    @Suppress("SimpleDateFormat")
    private fun saveSentMsg(id: Long,isGroup: Boolean,content: String){
        var cT:String = SimpleDateFormat("yyyyMMddHHmmssSSS").format(Date())
        var iPath:String = path
        if(!iPath.endsWith("\\")){
            iPath += "\\"
        }
        if(isGroup){
            iPath += "Group\\"
        }else{
            iPath += "Friend\\"
        }
        iPath = iPath + id + "\\"
        iPath += cT
        iPath += "\\"
        File(iPath).mkdirs()
        iPath += "1TEXT"
        File(iPath).writeText(content)
    }
    @Suppress("SimpleDateFormat")
    private fun saveMsg(msg: MessageChain, isGroup: Boolean, id: Long){
        var cC:Int = 1
        var cT:String = SimpleDateFormat("yyyyMMddHHmmssSSS").format(Date())
        var iPath:String = path
        if(!iPath.endsWith("\\")){
            iPath += "\\"
        }
        if(isGroup){
            iPath += "Group\\"
        }else{
            iPath += "Friend\\"
        }
        iPath = iPath + id + "\\"
        iPath += cT
        iPath += "\\"
        //C:\example\Friend\cT\
        var cType: String
        msg.forEach{
            cType = "UR"
            if(it is PlainText){
                cType = "TEXT"
            }
            if(it is Image){
                cType = "IMAGE"
            }
            var pPath:String = iPath
            File(pPath).mkdirs()
            File(iPath + cC + cType).writeText(it.toString())

            cC++
        }
        /*旧版本mirai MessageChain
        while(cC<msg.size){
            //var cType
            if(msg[cC] is PlainText){
                cType = "TEXT"
            }
            if(msg[cC] is Image){
                cType = "IMAGE"
            }
            var pPath:String = iPath
            File(pPath).mkdirs()
            File(iPath + cC + cType).writeText(msg[cC].toString())

            cC++
        }*/
    }
    private fun post(uri:String,content:String): String? {
        try{
            var cli = OkHttpClient()
            val requestBody:RequestBody = content.toRequestBody()
            var request:Request = Request.Builder().url(uri).post(requestBody).build()
            var rsp:Response=cli.newCall(request).execute()
            return rsp.body?.string()
        }catch (ex:Exception){
            println("==========error")
            println(uri)
            println(content)
            println(ex.toString())
            return ex.toString()
        }
    }
}



private fun posts(uri:String,content:String): String? {
    try{
        var cli = OkHttpClient()
        val requestBody:RequestBody = content.toRequestBody()
        var request:Request = Request.Builder().url(uri).post(requestBody).build()
        var rsp:Response=cli.newCall(request).execute()
        return rsp.body?.string()
    }catch (ex:Exception){
        return ex.toString()
    }
}
