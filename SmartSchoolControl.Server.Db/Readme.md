# SmartSchoolControl.Server.Db

## 常用命令

* 创建迁移
```shell
dotnet ef migrations add InitializeDb -s ..\SmartSchoolControl.Server.Backend
```

* Release Database
```shell
dotnet ef database update --configuration Release -s ..\SmartSchoolControl.Server.Bac
kend -- --environment Production
```