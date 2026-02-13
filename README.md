# WiteemFramework

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.x-purple.svg)]()

一款轻量级的 ASP.NET ORM 框架，支持多种数据库操作，简单易用。

## ✨ 特性

- 🚀 轻量级设计，易于集成
- 💾 支持 MS SQL Server 和 Oracle 数据库
- 🔧 支持泛型 CRUD 操作
- 📦 支持批量操作
- 🔄 支持事务处理
- 📄 支持分页查询
- 🎯 基于特性映射的对象关系映射

## 📋 功能列表

### 基础操作
- **新增（Add）**: 单条/批量插入数据
- **更新（Update）**: 单条/批量更新数据
- **删除（Delete）**: 单条/批量删除数据
- **查询（Select）**: 条件查询、分页查询

### 高级功能
- 支持主键自动生成和手动指定
- 支持存储过程调用
- 支持事务批处理
- 支持自定义 SQL 查询
- 支持返回 DataTable 或强类型对象

## 🚀 快速开始

### 安装

将 `WiteemFramework.dll` 引用到您的项目中。

### 配置

在 `web.config` 或 `app.config` 中配置数据库连接字符串：

```xml
<connectionStrings>
  <add name="MSSQL" connectionString="Server=.;Database=YourDB;Uid=sa;Pwd=123456;" />
</connectionStrings>
```

### 基本使用

```csharp
using WiteemFramework;
using WiteemFramework.Enum;

// 初始化
var db = new WiteemSQL(); // 默认使用 MSSQL
// 或指定数据库类型
var db = new WiteemSQL(DBEnum.MSSQL, "MSSQL");

// 新增
var user = new User { Name = "张三", Age = 25 };
db.Add(user);

// 查询
var user = db.GetModel<User>(1);

// 更新
user.Age = 26;
db.Update(user);

// 删除
db.Delete(user);

// 分页查询
int totalCount = 0;
var dbSearch = new DBSearchBase 
{ 
    PageIndex = 1, 
    PageSize = 10 
};
var list = db.ExecuteQueryPage<User>(dbSearch, ref totalCount);
```

### 实体类标注

```csharp
using WiteemFramework.Filter;

[DataField(TableName = "Users")]
public class User
{
    [PropertyAttribute(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
    
    [PropertyAttribute(ColumnName = "UserName")]
    public string Name { get; set; }
    
    public int Age { get; set; }
}
```

## 📖 支持的数据库

- ✅ Microsoft SQL Server
- ✅ Oracle

## 🛠️ 技术栈

- .NET Framework 4.x
- C#
- ADO.NET


## 📝 许可证

本项目基于 MIT 许可证开源。详见 [LICENSE](LICENSE) 文件。

## ⚠️ 免责声明

1. **使用风险**: 本软件按"原样"提供，不提供任何明示或暗示的保证，包括但不限于适销性、特定用途适用性和非侵权性的保证。

2. **责任限制**: 在任何情况下，作者或版权持有人均不对因使用本软件或其他交易而产生的任何索赔、损害或其他责任负责，无论是在合同诉讼、侵权行为还是其他方面。

3. **生产环境**: 本框架为学习和研究目的而创建，建议在生产环境使用前进行充分测试。

4. **数据安全**: 使用者应自行负责数据备份和安全防护，开发者不对任何数据丢失或损坏承担责任。

---

⭐ 如果这个项目对您有帮助，欢迎 Star！
