using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler.Lexical
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
}
