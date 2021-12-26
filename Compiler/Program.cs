using Compiler.Lexical;
using Compiler.Syntax;
using System;

namespace LexicalAnalyzer
{
    class Program
    {
        static void Main()
        {
            LexAnalizer lex = new LexAnalizer("readFile.txt");
            SyntaxAnalizer syntax = new SyntaxAnalizer(lex);
            syntax.FullProgrammCheck();
            Console.ReadKey();

        }
    }
}
