using System;

namespace Example 
{
    public class CSharpClass : IHello
    {
        public string SayHello() 
        {
            return "I am a C# class!";
        }

        public int AnswerEverything()
        {
            return 42;
        }
    }
}