using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace LexicalAnalyzer
{
    public class LexAnalizer
    {
        private int position = 0;
        FileStream fstream;
        int numIndex = 0;
        int varIndex = 0;
        int stringIndex = 0;
        bool nextComment = false;
        //   bool nextString = false;
        int iterErrorCount = 0;
        public string Lex(bool next)
        {
            int K = 0;
            string bufer = "";
            string str;
            bool end = false;
            fstream.Position = position;
            str = ((char)fstream.ReadByte()).ToString();
            bool start = true;
            string result = null;
            while (!end)
            {
                if (fstream.Position == fstream.Length)
                {
                    iterErrorCount++;
                }
                if (iterErrorCount > 2)
                {
                    return "End";
                }
                if (str == "\uffff")
                {
                    Console.WriteLine("Файл закончился без символа конца файла. (Символ конца файла \"#\")");
                }
                else
                {
                    //  Console.WriteLine(str);
                }

                int y;
                switch (K)
                {
                    case 0:
                        if (start)
                        {
                            start = false;
                        }
                        else
                        {
                            if (next)
                            {
                                position = (int)fstream.Position;
                            }
                            end = true;
                            break;
                        }

                        if (nextComment == true)
                        {
                            nextComment = false;
                            K = 5;
                            break;
                        }

                        if (int.TryParse(str, out y))
                        {
                            K = 1;
                            bufer = str;
                        }
                        else if (Regex.IsMatch(str, @"^[a-zA-Z]|[_]$"))
                        {
                            K = 2;
                            bufer = str;
                        }
                        else if (Regex.IsMatch(str, @"[,.+\-|=/<>&*!{}();]|\[|\]|[""]$"))
                        {
                            if (str == "/")
                            {
                                K = 4;
                                bufer = str;

                            }
                            else if (str == "\"")
                            {
                                K = 7;
                                bufer = str;
                            }
                            else
                            {
                                K = 3;
                                bufer = str;
                            }
                        }
                        else if (str == "#")
                        {
                            Console.WriteLine("end file.");
                            return "End";
                        }
                        else
                        {
                            K = 0;
                        }
                        break;
                    case 1:
                        str = ((char)fstream.ReadByte()).ToString();

                        if (int.TryParse(str, out y))
                        {
                            K = 1;
                            bufer += str;
                        }
                        else
                        {
                            K = 0;
                            result = ReadToFile(ref bufer, "num.txt", " ", " число", "N");
                            if (bufer != "")
                            {
                                result = WriteToFile(bufer, "num.txt", " ", ref numIndex, " число", "N");
                            }
                            bufer = "";
                            fstream.Position -= 1;
                        }

                        break;
                    case 2:
                        str = ((char)fstream.ReadByte()).ToString();
                        if (Regex.IsMatch(str, @"^[a-zA-Z0-9]|[_]$"))
                        {
                            K = 2;
                            bufer += str;
                        }
                        else
                        {
                            K = 0;
                            result = ReadToFile(ref bufer, "keyword.txt", " ", " ключевое слово", "K");

                            if (bufer != "")
                            {
                                result = ReadToFile(ref bufer, "nameVar.txt", " ", " переменная", "V");

                                if (bufer != "")
                                {
                                    result = WriteToFile(bufer, "nameVar.txt", " ", ref varIndex, " переменная", "V");
                                }
                                bufer = "";
                            }
                            fstream.Position -= 1;
                        }
                        break;
                    case 3:
                        str = ((char)fstream.ReadByte()).ToString();
                        if (Regex.IsMatch(str, @"^[a-zA-Z0-9]|[_]|[ ]|[\s]$"))
                        {
                            K = 0;
                            result = ReadToFile(ref bufer, "singleSeparator.txt", " ", " одналитерный символ", "S");
                            fstream.Position -= 1;
                        }
                        else if (Regex.IsMatch(str, @"[+-|=/<>&*]$"))
                        {
                            K = 0;
                            bufer += str;
                            result = ReadToFile(ref bufer, "doubleSeparator.txt", " ", " двулитерный символ", "D");

                            if (bufer != "")
                            {
                                bufer = bufer[0].ToString();
                                result = ReadToFile(ref bufer, "singleSeparator.txt", " ", " одналитерный символ", "S");
                                fstream.Position -= 1;
                            }
                        }
                        else
                        {
                            K = 0;
                            result = ReadToFile(ref bufer, "singleSeparator.txt", " ", " одналитерный символ", "S");
                            fstream.Position -= 1;

                        }

                        break;

                    case 4:
                        str = ((char)fstream.ReadByte()).ToString();
                        if (str == "*")
                        {
                            bufer += str;
                            result = ReadToFile(ref bufer, "doubleSeparator.txt", " ", " двулитерный символ", "D");
                            if (next == true)
                                nextComment = true;
                            K = 0;

                        }
                        else
                        {
                            fstream.Position -= 1;
                            K = 3;
                        }
                        break;

                    case 5:
                        str = ((char)fstream.ReadByte()).ToString();

                        if (str != "*")
                        {
                            continue;
                        }
                        else
                        {
                            K = 6;
                        }
                        break;

                    case 6:
                        str = ((char)fstream.ReadByte()).ToString();
                        if (str == "/")
                        {
                            K = 0;
                            bufer = "*/";
                            result = ReadToFile(ref bufer, "doubleSeparator.txt", " ", " двулитерный символ", "D");
                        }
                        else
                        {
                            K = 5;
                        }

                        break;
                    case 7:
                        str = ((char)fstream.ReadByte()).ToString();
                        if (str == "\"")
                        {
                            bufer += str;
                            K = 8;
                        }
                        else
                        {
                            K = 7;
                            bufer += str;
                        }
                        break;
                    case 8:
                        K = 0;
                        result = ReadToFile(ref bufer, "String.txt", "==", " строка", "L");
                        if (bufer != "")
                        {
                            result = WriteToFile(bufer, "String.txt", "==", ref stringIndex, " строка", "L");
                        }
                        bufer = "";
                        break;
                }
            }
            return result;
        }

        private string ReadToFile(ref string bufer, string fileName, string separator, string writeToConsole, string fileCategory)
        {
            using (StreamReader streamReader = File.OpenText(fileName))
            {
                while (!streamReader.EndOfStream)
                {
                    string[] sr = streamReader.ReadLine().Split(separator);
                    if (bufer == sr[0])
                    {
                        Console.WriteLine(bufer + " " + sr[1] + writeToConsole);
                        bufer = "";
                        return fileCategory + sr[1];

                    }
                }
            }
            return null;
        }

        private string WriteToFile(string bufer, string fileName, string separator, ref int index, string writeToConsole, string fileCategory)
        {
            using (StreamWriter streamWriter = new StreamWriter(fileName, true))
            {
                streamWriter.Write(bufer + separator + index + Environment.NewLine);
                Console.WriteLine(bufer + " " + index + writeToConsole);
                return fileCategory + index++;

            }
        }

        public LexAnalizer(string file)
        {
            fstream = new FileStream(file, FileMode.Open);
            fstream.Position = position;
        }

        ~LexAnalizer()
        {
            fstream.Close();
        }
    }


    public class SyntaxAnalizer
    {
        struct Token
        {
            public int num;
            public char pointer;
        }

        private Queue<Token> Tokens;
        private Token token;
        private LexAnalizer LexAnalizer;
        List<string> strs = new List<string>();
        int iterator = 0;
        public SyntaxAnalizer(LexAnalizer lexAnalizer)
        {
            LexAnalizer = lexAnalizer;
            string res;
            while ((res = LexAnalizer.Lex(true)) != "End" || res == null)
            {
                if (res != null)
                    strs.Add(res);
            }
            strs.Add("End");
        }
        public void nextToken(bool next)
        {
            string token = strs[iterator];
            if (token == "End")
            {
                this.token = new Token()
                {
                    pointer = '#',
                    num = 0
                };
                return;
            }
            if (next)
            {
                iterator++;
            }
            this.token = new Token()
            {
                pointer = token[0],
                num = int.Parse(token.Substring(1))
            };
        }
        public SyntaxAnalizer(List<string> tokens)
        {
            Tokens = new Queue<Token>();
            foreach (var token in tokens)
            {
                Tokens.Enqueue(new Token()
                {
                    pointer = token[0],
                    num = int.Parse(token.Substring(1))
                });
            }
        }

        public bool CheckMathOperation()
        {
            E();
            return true;
        }

        //Арифметика
        void E()
        {
            T();
            nextToken(false);

            while (!((token.pointer == 'S' && token.num == 17) || token.pointer == '#' || (token.pointer == 'S' && token.num == 14)))
            {
                nextToken(false);
                if (token.pointer == 'S' && token.num == 6)
                {//+
                    nextToken(true);

                    T();
                }
                else if (token.pointer == 'S' && token.num == 7)
                {//-
                    nextToken(true);

                    T();
                }
                else
                {
                    Console.WriteLine("Error E1");
                    return;
                }
                nextToken(false);

            }
        }

        void T()
        {
            F();
            nextToken(false);

            while (!(token.pointer == '#' || (token.pointer == 'S' && token.num == 17) || (token.pointer == 'S' && token.num == 6) || (token.pointer == 'S' && token.num == 7) || (token.pointer == 'S' && token.num == 14)))
            {
                nextToken(false);

                if (token.pointer == 'S' && token.num == 1)
                {//*
                    nextToken(true);

                    F();
                }
                else if (token.pointer == 'S' && token.num == 2)
                {//
                    nextToken(true);

                    F();
                }
                else
                {
                    Console.WriteLine("Error T1");
                    return;
                }
                nextToken(false);

            }
        }

        void F()
        {
            nextToken(false);

            if (token.pointer == 'N')
            {
                nextToken(true);

            }
            else if (token.pointer == 'V')
            {
                nextToken(true);

            }
            else if (token.pointer == 'S' && token.num == 16)
            {
                nextToken(true);

                E();
                nextToken(true);

                if (token.pointer == 'S' && token.num == 17) { }
                else
                {
                    Console.WriteLine("Error F1");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Error F2");
                return;
            }
        }

        //Логика
        void EL()
        {
            TL();
            nextToken(false);

            while (!((token.pointer == 'S' && token.num == 17) || token.pointer == '#' || (token.pointer == 'S' && token.num == 8 || token.num == 9) || (token.pointer == 'D' && token.num == 0 || token.num == 12) || (token.pointer == 'S' && token.num == 20)))
            {
                nextToken(false);
                if (token.pointer == 'S' && token.num == 6)
                {//+
                    nextToken(true);

                    TL();
                }
                else if (token.pointer == 'S' && token.num == 7)
                {//-
                    nextToken(true);

                    TL();
                }
                else
                {
                    Console.WriteLine("Error E1");
                    return;
                }
                nextToken(false);

            }
        }
        void TL()
        {
            FL();
            nextToken(false);

            while (!(token.pointer == '#' || (token.pointer == 'S' && token.num == 17) || (token.pointer == 'S' && token.num == 6) || (token.pointer == 'S' && token.num == 7) || (token.pointer == 'S' && token.num == 8 || token.num == 9) || (token.pointer == 'D' && token.num == 0 || token.num == 12) || (token.pointer == 'S' && token.num == 20)))
            {
                nextToken(false);

                if (token.pointer == 'S' && token.num == 1)
                {//*
                    nextToken(true);

                    FL();
                }
                else if (token.pointer == 'S' && token.num == 2)
                {//
                    nextToken(true);

                    FL();
                }
                else
                {
                    Console.WriteLine("Error T1");
                    return;
                }
                nextToken(false);

            }
        }
        void FL()
        {
            nextToken(false);

            if (token.pointer == 'N')
            {
                nextToken(true);

            }
            else if (token.pointer == 'V')
            {
                nextToken(true);

            }
            else if (token.pointer == 'S' && token.num == 16)
            {
                nextToken(true);

                EL();
                nextToken(true);

                if (token.pointer == 'S' && token.num == 17) { }
                else
                {
                    Console.WriteLine("Error F1");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Error F2");
                return;
            }
        }
        public void Z()
        {
            A();
            nextToken(false);

            while (!((token.pointer == 'S' && token.num == 17) || token.pointer == '#'))
            {
                nextToken(false);
                if (token.pointer == 'D' && token.num == 14)
                {//&&
                    nextToken(true);

                    A();
                }
                else if (token.pointer == 'D' && token.num == 13)
                {//||
                    nextToken(true);

                    A();
                }
                else
                {
                    Console.WriteLine("Error Z1");
                    return;
                }
                nextToken(false);

            }
        }
        public void A()
        {
            nextToken(false);

            if (token.pointer != '#')
            {
                if (token.pointer == 'S' && token.num == 16)// (
                {
                    nextToken(true);
                    Z();
                    nextToken(true);
                    if (token.pointer == 'S' && token.num == 17) { } // )
                    else
                    {
                        Console.WriteLine("error not )");
                    }
                }
                else if (token.pointer == 'K' && (token.num == 24 || token.num == 25))
                {
                    nextToken(true);
                }
                else if (token.pointer == 'V')
                {
                    nextToken(true);
                }
                else if (token.pointer == 'S' && token.num == 19)
                {
                    nextToken(true);
                    C();

                    if (token.pointer == 'S' && token.num == 20)
                    {
                        nextToken(true);
                    }
                    else
                    {
                        Console.WriteLine("error not ]");
                    }
                }
                else if (token.pointer == 'S' && token.num == 18)//!
                {
                    nextToken(true);
                    A();
                }
                else
                {
                    Console.WriteLine("Error Type A1");
                }

            }
        }
        public void C()
        {
            EL();


            if ((token.pointer == 'S' && (token.num == 8 || token.num == 9)) || (token.pointer == 'D' && (token.num == 0 || token.num == 12)))
            {
                nextToken(true);
            }
            else
            {
                Console.WriteLine("Error C1 not <>");
            }
            EL();

        }

        //Скелет
        public void S()
        {
            SP();
            nextToken(false);

        }
        void SP()
        {
            nextToken(false);
            if(token.pointer == 'K' && token.num == 27)
            {
                nextToken(true);
            }
            else
            {
                Console.WriteLine("Отсутствует блок Programm");
                return;
            }
            nextToken(false);
            if(token.pointer == 'V')
            {
                nextToken(true);
            }
            else
            {
                Console.WriteLine("Отсутствует название программы");
                return;
            }
            nextToken(false);
            if (token.pointer == 'S' && token.num == 14)
            {
                nextToken(true);
            }
            else
            {
                Console.WriteLine("Error end line ;");
                return;
            }

            SD();
        }
        void SD()
        {
            nextToken(false);

            while(!(token.pointer == 'K' && token.num == 28) && !(token.pointer == '#'))
            {
                nextToken(false);
                if(token.pointer == 'K' && (token.num == 8 || token.num == 15 || token.num == 3))
                {
                    nextToken(true);
                    ST();
                }
                else if(token.pointer == 'S' && token.num == 14)
                {
                    Console.WriteLine("Error SD ;");
                }
            }
        }
        void ST()
        {
            nextToken(false);

            while(!(token.pointer == 'S' && token.num == 14))
            {
                nextToken(false);
                if (token.pointer == 'V')
                {
                    nextToken(true);
                    SM();
                    nextToken(false);
                    if(token.pointer == 'S' && token.num == 5)
                    {
                        nextToken(true);
                    }
                    else if(token.pointer == 'S' && token.num == 14)
                    {
                        nextToken(true);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Error not , or ;");
                    }
                }
            }
        }
        void SM()
        {
            nextToken(false);
            if(token.pointer == 'S' && token.num == 19)
            {
                nextToken(true);
                EL();
                if(token.pointer == 'S' && token.num == 20) 
                {
                    nextToken(true);
                }
                else
                {
                    Console.WriteLine("Error SM ]");
                }
            }
        }

        //Блок операторов и присвоения
        void BWAP()
        {
            //вызов определения переменных и вызов определение блока операторов и присвоения 
        }

       public void BOIP()
        {
            nextToken(false);

            if(token.pointer == 'K' && token.num == 28)//start
            {
                nextToken(true);
                BP();
                nextToken(true);
                if(token.pointer == 'K' && token.num == 29)//end
                {

                }
                else
                {
                    Console.WriteLine("Отсутствует ключевое слово End");
                }
            }
            else
            {
                Console.WriteLine("Отсутствует ключевое слово Start");
            }
        }
        void BP()
        {
            nextToken(false);

            while (!(token.pointer == 'K' && token.num == 29) && (token.pointer != '#'))//end
            {
                nextToken(false);
                if(token.pointer == 'V')//переменная
                {
                    nextToken(true);
                    BV();
                    nextToken(false);
                    if (token.pointer == 'S' && token.num == 14)
                    {
                        nextToken(true);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка отсутствует ;");
                    }
                }
                else if(token.pointer == 'K' && token.num == 32) // if
                {
                    nextToken(true);
                    BIF();
                    nextToken(false);
                    if(token.pointer == 'K' && token.num == 33) { nextToken(true); }
                    else
                    {
                        Console.WriteLine("error endif not found");
                    }
                }
                else if(token.pointer == 'K' && token.num == 22)// while
                {
                    nextToken(true);
                    BWhile();
                    nextToken(false);
                    if (token.pointer == 'K' && token.num == 34) { nextToken(true); }
                    else
                    {
                        Console.WriteLine("error endwhile not found");
                    }
                }
                else if(token.pointer == 'K' && token.num == 30)//goWrite
                {

                }
                else if(token.pointer == 'K' && token.num == 31)//goRead
                {

                }
                else
                {
                    Console.WriteLine("error не известный ключ");
                    return;
                }
                nextToken(false);
            }
        }


        //блок if
        void BIF()
        {
            nextToken(false);
            if(token.pointer == 'S' && token.num == 16)
            {
                nextToken(true);
                Z();
                nextToken(false);
                if(token.pointer == 'S' && token.num == 17)
                {
                    nextToken(true);
                    BOIF();
             
                }
            }
            else
            {
                Console.WriteLine("error ( not found");
            }
        }
       void BOIF()
        {
            nextToken(false);
            while(!(token.pointer == 'K' && token.num == 33) && token.pointer != '#')
            {
                nextToken(false);
                if (token.pointer == 'V')//переменная
                {
                    nextToken(true);
                    BV();
                    nextToken(false);
                    if (token.pointer == 'S' && token.num == 14)
                    {
                        nextToken(true);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка отсутствует ;");
                    }
                }
                else if (token.pointer == 'K' && token.num == 32) // if
                {
                    nextToken(true);
                    BIF();
                    nextToken(false);
                    if (token.pointer == 'K' && token.num == 33) { nextToken(true); }
                    else
                    {
                        Console.WriteLine("error endif not found");
                    }
                }
                else if (token.pointer == 'K' && token.num == 22)// while
                {
                    nextToken(true);
                    BWhile();
                    nextToken(false);
                    if (token.pointer == 'K' && token.num == 34) { nextToken(true); }
                    else
                    {
                        Console.WriteLine("error endwhile not found");
                    }
                }
                else if (token.pointer == 'K' && token.num == 30)//goWrite
                {

                }
                else if (token.pointer == 'K' && token.num == 31)//goRead
                {

                }
                else
                {
                    Console.WriteLine("error не известный ключ");
                    return;
                }
                nextToken(false);
            }
        }

        //блок while
        void BWhile()
        {
            nextToken(false);
            if (token.pointer == 'S' && token.num == 16)
            {
                nextToken(true);
                Z();
                nextToken(false);
                if (token.pointer == 'S' && token.num == 17)
                {
                    nextToken(true);
                    BOWhile();

                }
            }
            else
            {
                Console.WriteLine("error ( not found");
            }
        }

        void BOWhile()
        {
            nextToken(false);
            while (!(token.pointer == 'K' && token.num == 34) && token.pointer != '#')
            {
                nextToken(false);
                if (token.pointer == 'V')//переменная
                {
                    nextToken(true);
                    BV();
                    nextToken(false);
                    if (token.pointer == 'S' && token.num == 14)
                    {
                        nextToken(true);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка отсутствует ;");
                    }
                }
                else if (token.pointer == 'K' && token.num == 32) // if
                {
                    nextToken(true);
                    BIF();
                    nextToken(false);
                    if (token.pointer == 'K' && token.num == 33) { nextToken(true); }
                    else
                    {
                        Console.WriteLine("error endif not found");
                    }
                }
                else if (token.pointer == 'K' && token.num == 22)// while
                {
                    nextToken(true);
                    BWhile();
                    nextToken(false);
                    if (token.pointer == 'K' && token.num == 34) { nextToken(true); }
                    else
                    {
                        Console.WriteLine("error endwhile not found");
                    }
                }
                else if (token.pointer == 'K' && token.num == 30)//goWrite
                {

                }
                else if (token.pointer == 'K' && token.num == 31)//goRead
                {

                }
                else
                {
                    Console.WriteLine("error не известный ключ");
                    return;
                }
                nextToken(false);
            }
        }

        //блок переменных
        void BV()
        {
            nextToken(false);
            if (token.pointer == 'S' && token.num == 3)
            {
                nextToken(true);
                E();
                nextToken(false);
            }
            else
            {
                Console.WriteLine("Ошибка блока присваивания =");
            }
        }
        public void show()
        {
            foreach (var token in Tokens)
            {
                Console.WriteLine(token.num + " " + token.pointer);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            LexAnalizer lex = new LexAnalizer("readFile.txt");
            //List<string> codeResult = new List<string>();
            string res;
            //while ((res = lex.Lex(true)) != "End" || res == null)
            //{
            //    if (res != null)
            //        codeResult.Add(res);
            //}
            //foreach (var code in codeResult)
            //{
            //    Console.WriteLine(code);
            //}
            //bool next = false;
            //string a;
            //while ((res = lex.Lex(next)) != "End" || res == null)
            //{

            //    if (res != null)
            //        Console.WriteLine(res);
            //    a = Console.ReadLine();
            //    if (a == "1")
            //    {
            //        next = true;
            //    }
            //    else if (a == "0")
            //    {
            //        next = false;
            //    }
            //}


            SyntaxAnalizer syntax = new SyntaxAnalizer(lex);
            // syntax.show();
            // Console.WriteLine(syntax.CheckMathOperation());

            syntax.BOIP();
            Console.ReadKey();

        }
    }
}
