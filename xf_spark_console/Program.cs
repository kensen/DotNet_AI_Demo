// See https://aka.ms/new-console-template for more information

using xf_spark_console;

var t=ConfigRead.GetApiConfigDto();

Console.WriteLine($"Hello, World!欢迎使用讯飞火星大模型");
Console.WriteLine("请选择一个大模型版本：");

var index = 1;
foreach (var m in t.ModelUrls)
{
    Console.WriteLine($"{t.ModelUrls.IndexOf(m)}: [{m.ModelName}]");
    index++;
}

string userInput = Console.ReadLine();

Console.WriteLine("您输入的内容是：");
Console.WriteLine(userInput);
