// See https://aka.ms/new-console-template for more information
using SemanticKernel_Demo.chapter1;
using SemanticKernel_Demo.Dto;

System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

Console.WriteLine("Hello, World!");


//等待用户输入
//Console.ReadLine();

////只有用户输入 Q 才退出控制台
//if (Console.ReadLine() == "Q")
//{
//    Environment.Exit(0);
//}else
//{
//    Console.ReadLine();
//}

chapter1_demo chapter1 = new chapter1_demo("volces");
//chapter1_demo chapter1 = new chapter1_demo();

while (true)
{
    string input = Console.ReadLine(); // 读取用户输入的字符串

    if (input.Equals("Q", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("\n用户输入了 'Q'，程序即将退出。");
        break; // 退出循环
    }
    else
    {
       // chapter1.RunChat(input);
        chapter1.RunChatStream(input);
    
    }
      
    
}


