# GameFramework

## 1.sealed

英 [siːld]  v.密封(容器)

当sealed修饰类时，则该类不能被继承;
当sealed修饰方法或者属性时，防止子类重写该方法或属性
		（前提是该方法是虚方法或者虚属性，可以被子类重写）;

## 2.partial

英 [ˈpɑːʃl]  adj.部分的

partial 是局部类型的标志。
		局部类型可以实现将一个类、结构或接口分成几个部分，分别放在在几个不同的.cs文件中（当然也可以放在同一个.cs文件中）。
		在程序进行编译之后，将会合并成一个完整的类。
		（1）只适用于类、接口、结构，不支持委托和枚举。 
		（2）同一个类型的各个部分必须有修饰符partial。
		（3）使用局部类型时，一个类型的各个部分必须位于相同的命名空间中。 
		（4）一个类型的各个部分必须同时被编译。 
		 ...
		
## 3.internal

英 [ɪnˈtɜːnl]  adj.内部的

internal（内部）：限定的是只有在同一程序集中可访问，可以跨类

protected（受保护）：限定的是只有在继承的子类中可访问，可以跨程序集

protected internal：受保护"或"内部修饰符修饰成员,
	当父类与子类在同一个程序集中，internal成员可见。
	当父类与子类不在同一个程序集中，子类不能访问父类internal成员，而子类可以访问父类的ptotected internal成员，
	即从当前程序集或从包含类派生的类型，可以访问具有访问修饰符 protected internal 的类型或成员。

## 4.readonly
[readonly (C# Reference)](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/readonly)

readonly 关键字是一个修饰符，readonly可以在四种情况下使用

1.在字段声明中，readonly表示对该字段的赋值只能作为声明的一部分或在同一类的构造函数中发生。
可以在字段声明和构造函数中多次分配和重新分配只读字段。

readonly构造函数退出后无法分配字段。此规则对值类型和引用类型有不同的含义：

a.因为值类型直接包含它们的数据，所以作为 readonly值类型的字段是不可变的。

b.因为引用类型包含对其数据的引用，所以作为readonly引用类型的字段必须始终引用同一个对象。该对象不是不可变的。
readonly 修饰符防止字段被引用类型的不同实例替换。但是，该修饰符不会阻止该字段的实例数据通过只读字段进行修改。

2.在readonly struct类型定义中，readonly表示结构类型是不可变的。

3.在结构类型内的实例成员声明中，readonly指示实例成员不修改结构的状态。

4.在ref readonly方法 return中，readonly修饰符指示该方法返回一个引用并且不允许对该引用进行写入。

## 5.const
[const (C# Reference)](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/const)

const在定义的时候初始化,以后不能改变他的值;

常量可以是数字、布尔值、字符串或空引用。

常量表达式是可以在编译时完全计算的表达式。因此，引用类型常量的唯一可能值是字符串和空引用。

常量声明中不允许使用 static 修饰符

常量可以参与常量表达式


## 6.













