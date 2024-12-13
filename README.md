# 專案介紹
> 於 DotnetConf 2024 的 Session 中介紹的範例

- 有兩個專案，Web 以及 Service，直接呼叫 Web，然後 Web 會自己轉打 Service
- 使用 serilog 搭配 seq 做 log 的收集，透過 seq 來查資料流的串接

# 如何使用

## seq
使用 docker 起一個 seq 的 container，並且將 port 指定為 `5341`
> 若 port 不同的時候，要調整 `appsettings.json` 的設定
``` shell
docker run --name seq -d -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest
```

## web
``` shell
cd .\Web
dotnet run
```

## service
```shell
cd .\Service
dotnet run
```

## 呼叫 API
呼叫 web 服務的 weatherforecast
```
curl http://localhost:5234/weatherforecast --header 'x-correlation-id: dotnetconf2024'
```

## 查看 seq
用 `CorrelationId` 做過濾，可以看到資料流從 Web 到 Service
![image](https://github.com/user-attachments/assets/00d06b3c-8ffe-4306-9816-79ee09564e53)
