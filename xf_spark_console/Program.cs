// See https://aka.ms/new-console-template for more information

using xf_spark_console;
using xf_spark_console.SparkDesk;

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
//Console.WriteLine(userInput);
SparkDeskApi sparkDesk = new SparkDeskApi(t);

var msg = sparkDesk.Chat(userInput, t.ModelUrls[0].ModelName).Result;

Console.WriteLine(msg);
