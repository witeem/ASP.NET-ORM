# ASP.NET-ORM
asp.net—自定义轻量级ORM
DataFieldAttribute.cs：实体映射表字段 特性（用于标注实体类中成员属性对应数据库中的字段名和字段类型）

PropertyAttribute.cs  ：实体映射数据库表 特性（用于标注实体类对应数据库中哪个表）

DBCrateFactory.cs    ：创建数据库对象的工厂（用于创建哪种数据库对象   MS SQL   还是  ORACLE）

SQLHelper.cs            ：这是一个抽象函数。DBWorks文件夹下所有类都继承该抽象函数，这些子类就必须实现SQLHelper中的抽象方法同时也可以使用该抽象函数的公用方法

IWiteem.cs                ： 对外接口

Witeem.cs   　　　　：继承并实现IWiteem接口

CommonHelper.cs     ：通用工具类

DBWorks文件夹下存放的是数据库操作类（因为是DEMO，所以只设置了MS SQL和ORACLE）

Enum文件夹下存放的是需要使用到的一些枚举类（ColumnKeyType.cs  字段状态， DBEnum.cs 数据库类型）
