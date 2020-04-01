# qqv4

基于mirai的qq消息转发工具，mirai: https://github.com/mamoe/mirai

准备：
> 1. VS2019
> 2. IntelliJ Idea / Android Studio

用法：
> 1. 用ij/as打开kt文件夹中的LCBOT项目
> 2. 用build.gradle的shadowJar编译
> 3. 用ikvmc转换生成的LCBOT-4.0-all.jar为DLL文件
> 4. 用vs打开cs文件夹的QQ_LOCAL项目
> 5. 引用生成的dll
> 6. 修改登陆参数并编译

大致过程：
> 1. mirai（kotlin部分）收到消息，post到localhost的.netcore服务器
> 2. 你要发信息时，localhost.netcore服务器post到c#部分，调用kotlin的mirai向qq发消息
