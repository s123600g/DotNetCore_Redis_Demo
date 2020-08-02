---
title: DotnetCore Redis Demo
tags: dotnetCore , Redis
description: 使用Docker建置Redis結合DotnetCore API
---

# DotNetCore_Redis_Demo
dotnet core 3.1配合使用 Docker建置Redis練習筆記


# Docker 操作
Redis Image <br/>
https://hub.docker.com/_/redis

從Docker Hub下載Redis Image
```shell=
docker pull redis
```

建立Redis Container
```shell=
docker run --name my-redis --restart always -p 6379:6379 -d redis:latest
```
官方預設建置開放Port是`6379`

可參考
https://github.com/docker-library/redis/blob/master/Dockerfile.template

測試連線是否正常運作
```shell=
docker exec -it my-redis bash
```

```shell=
root@b8baf1928543:/data# redis-cli
127.0.0.1:6379> ping
PONG
127.0.0.1:6379>
```

# Visual Studio專案建置操作
1. 建立ASP.Net Core Application專案
2. 選擇範本類型為API
3. 進階項目選擇啟用`Configure for HTTPS`與`Enable Docker Support`選擇*Linux*
4. 專案剛建立好會先執行建立該專案Image與專案Contianer，過程中會出現提示需要分享資料夾視窗，請點選`Share It`務必要讓專案用到資料夾都有分享到
5. 在專案Nuget套件管理器內安裝`StackExchange.Redis`
6. 在`Startup.cs`內`ConfigureServices`加入`Scoped` DI服務
```csharp=
// This method gets called by the runtime. Use this method to add services to the container.
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    // 加入AddScoped-使用Redis作為暫存區塊
    //using StackExchange.Redis;
    IConnectionMultiplexer redis = ConnectionMultiplexer.Connect("172.17.0.4");
    services.AddScoped(s => redis.GetDatabase());
}
```
需要注意連線IP，不能以本機IP作為連線，要用Docker內部分配到的IP

可以用以下指來查看Container分配到IP
```shell=
docker inspect my-redis
```

7. Controllers資料夾新增Controller-->API Controller (EMPTY)


# 測試
* GET
```
https://localhost:32768/api/Cache?Key=Welcome
```

```
https://localhost:32768/api/Cache?Key=Author
```

* POST

使用[Postman](https://www.postman.com/)
```
https://localhost:32768/api/Cache
```

raw(JSON)
```json=
{
    "Key":"Hello",
    "Value":"Hi"
}
```

