using Compiler.Lexical;
using Compiler.TableEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Syntax
{
  public  class SyntaxAnalizer
    {
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
        public bool MathOperationCheck()
        {
            E();
            return true;
        }

        //Арифметика
        void E()
        {
            T();
            nextToken(false);

            while (!((token.pointer == 'S' && token.num == (int)SingleSeparators.EndRoundBracket) || token.pointer == '#' || (token.pointer == 'S' && token.num ==(int)SingleSeparators.Semicolon)))
            {
                nextToken(false);
                if (token.pointer == 'S' && token.num == (int)SingleSeparators.Add)
                {//+
                    nextToken(true);

                    T();
                }
                else if (token.pointer == 'S' && token.num == (int)SingleSeparators.Substract)
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

            while (!(token.pointer == '#' || (token.pointer == 'S' && token.num == (int)SingleSeparators.EndRoundBracket) || (token.pointer == 'S' && token.num == (int)SingleSeparators.Substract) || (token.pointer == 'S' && token.num == (int)SingleSeparators.Add) || (token.pointer == 'S' && token.num == (int)SingleSeparators.Semicolon)))
            {
                nextToken(false);

                if (token.pointer == 'S' && token.num == (int)SingleSeparators.Multiply)
                {//*
                    nextToken(true);

                    F();
                }
                else if (token.pointer == 'S' && token.num == (int)SingleSeparators.Divide)
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
            else if (token.pointer == 'S' && token.num == (int)SingleSeparators.OpenRoundBracket)
            {
                nextToken(true);

                E();
                nextToken(true);

                if (token.pointer == 'S' && token.num == (int)SingleSeparators.EndRoundBracket) { }
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
        public void LogicaCheck()
        {
            Z();
        }
        void EL()
        {
            TL();
            nextToken(false);

            while (!((token.pointer == 'S' && token.num == (int)SingleSeparators.EndRoundBracket) || token.pointer == '#' || (token.pointer == 'S' && token.num == 8 || token.num == 9) || (token.pointer == 'D' && token.num == 0 || token.num == 12) || (token.pointer == 'S' && token.num == 20)))
            {
                nextToken(false);
                if (token.pointer == 'S' && token.num == (int)SingleSeparators.Add)
                {//+
                    nextToken(true);

                    TL();
                }
                else if (token.pointer == 'S' && token.num == (int)SingleSeparators.Substract)
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

            while (!(token.pointer == '#' || (token.pointer == 'S' && token.num == (int)SingleSeparators.EndRoundBracket) || (token.pointer == 'S' && token.num == 6) || (token.pointer == 'S' && token.num == 7) || (token.pointer == 'S' && token.num == 8 || token.num == 9) || (token.pointer == 'D' && token.num == 0 || token.num == 12) || (token.pointer == 'S' && token.num == 20)))
            {
                nextToken(false);

                if (token.pointer == 'S' && token.num == (int)SingleSeparators.Multiply)
                {//*
                    nextToken(true);

                    FL();
                }
                else if (token.pointer == 'S' && token.num == (int)SingleSeparators.Divide)
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
            else if (token.pointer == 'S' && token.num == (int)SingleSeparators.OpenRoundBracket)
            {
                nextToken(true);

                EL();
                nextToken(true);

                if (token.pointer == 'S' && token.num == (int)SingleSeparators.EndRoundBracket) { }
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
         void Z()
        {
            A();
            nextToken(false);

            while (!((token.pointer == 'S' && token.num == (int)SingleSeparators.EndRoundBracket) || token.pointer == '#'))
            {
                nextToken(false);
                if (token.pointer == 'D' && token.num == (int)DoubleSeparators.DoubleAmpersand)
                {//&&
                    nextToken(true);

                    A();
                }
                else if (token.pointer == 'D' && token.num == (int)DoubleSeparators.DoubleVerticalLine)
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
                if (token.pointer == 'S' && token.num == (int)SingleSeparators.OpenRoundBracket)// (
                {
                    nextToken(true);
                    Z();
                    nextToken(true);
                    if (token.pointer == 'S' && token.num == (int)SingleSeparators.EndRoundBracket) { } // )
                    else
                    {
                        Console.WriteLine("error not )");
                    }
                }
                else if (token.pointer == 'K' && (token.num == (int)KeyWords.TRUE || token.num == (int)KeyWords.FALSE))
                {
                    nextToken(true);
                }
                else if (token.pointer == 'V')
                {
                    nextToken(true);
                }
                else if (token.pointer == 'S' && token.num == (int)SingleSeparators.OpenSquareBracket)
                {
                    nextToken(true);
                    C();

                    if (token.pointer == 'S' && token.num == (int)SingleSeparators.EndSquareBracket)
                    {
                        nextToken(true);
                    }
                    else
                    {
                        Console.WriteLine("error not ]");
                    }
                }
                else if (token.pointer == 'S' && token.num == (int)SingleSeparators.ExclamationMark)//!
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
         void C()
        {
            EL();


            if ((token.pointer == 'S' && (token.num == (int)SingleSeparators.More || token.num == (int)SingleSeparators.Less)) || (token.pointer == 'D' && (token.num == (int)DoubleSeparators.DoubleEquals || token.num == 12  /*!=*/)))
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
        public void FullProgrammCheck()
        {
            SP();
            nextToken(false);
            BOIP();

        }
        public void DeclareAndProgrammCheck()
        {
            SP();
        }
        void SP()
        {
            nextToken(false);
            if (token.pointer == 'K' && token.num == (int)KeyWords.Programma)
            {
                nextToken(true);
            }
            else
            {
                Console.WriteLine("Отсутствует блок Programm");
                return;
            }
            nextToken(false);
            if (token.pointer == 'V')
            {
                nextToken(true);
            }
            else
            {
                Console.WriteLine("Отсутствует название программы");
                return;
            }
            nextToken(false);
            if (token.pointer == 'S' && token.num == (int)SingleSeparators.Semicolon)
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

            while (!(token.pointer == 'K' && token.num == (int)KeyWords.Start) && !(token.pointer == '#'))
            {
                nextToken(false);
                if (token.pointer == 'K' && (token.num == (int)KeyWords.Char || token.num == (int)KeyWords.Int || token.num == (int)KeyWords.Bool))
                {
                    nextToken(true);
                    ST();
                }
                else if (token.pointer == 'S' && token.num == (int)SingleSeparators.Semicolon)
                {
                    Console.WriteLine("Error SD ;");
                }
            }
        }
        void ST()
        {
            nextToken(false);

            while (!(token.pointer == 'S' && token.num == (int)SingleSeparators.Semicolon))
            {
                nextToken(false);
                if (token.pointer == 'V')
                {
                    nextToken(true);
                    SM();
                    nextToken(false);
                    if (token.pointer == 'S' && token.num == (int)SingleSeparators.Comma)
                    {
                        nextToken(true);
                    }
                    else if (token.pointer == 'S' && token.num == (int)SingleSeparators.Semicolon)
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
            if (token.pointer == 'S' && token.num == (int)SingleSeparators.OpenSquareBracket)
            {
                nextToken(true);
                EL();
                if (token.pointer == 'S' && token.num == (int)SingleSeparators.EndSquareBracket)
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
        public void BlockOperatorovAndPrisvoeniaCheck()
        {
            BOIP();
        }
         void BOIP()
        {
            nextToken(false);

            if (token.pointer == 'K' && token.num == (int)KeyWords.Start)//start
            {
                nextToken(true);
                BP();
                nextToken(true);
                if (token.pointer == 'K' && token.num == (int)KeyWords.End)//end
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

            while (!(token.pointer == 'K' && token.num == (int)KeyWords.End) && (token.pointer != '#'))//end
            {
                nextToken(false);
                if (token.pointer == 'V')//переменная
                {
                    nextToken(true);
                    BV();
                    nextToken(false);
                    if (token.pointer == 'S' && token.num == (int)SingleSeparators.Semicolon)
                    {
                        nextToken(true);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка отсутствует ;");
                    }
                }
                else if (token.pointer == 'K' && token.num == (int)KeyWords.IF) // if
                {
                    nextToken(true);
                    BIF();
                    nextToken(false);
                    if (token.pointer == 'K' && token.num == (int)KeyWords.ENDIF) { nextToken(true); }
                    else
                    {
                        Console.WriteLine("error endif not found");
                    }
                }
                else if (token.pointer == 'K' && token.num == (int)KeyWords.While)// while
                {
                    nextToken(true);
                    BWhile();
                    nextToken(false);
                    if (token.pointer == 'K' && token.num == (int)KeyWords.ENDWhile) { nextToken(true); }
                    else
                    {
                        Console.WriteLine("error endwhile not found");
                    }
                }
                else if (token.pointer == 'K' && token.num == (int)KeyWords.goWrite)//goWrite
                {
                    nextToken(true);
                    BW();
                    nextToken(false);
                    if (token.pointer == 'S' && token.num == (int)SingleSeparators.Semicolon)
                    {
                        nextToken(true);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка отсутствует ;");
                    }

                }
                else if (token.pointer == 'K' && token.num == (int)KeyWords.goRead)//goRead
                {
                    nextToken(true);
                    BR();
                    nextToken(false);
                    if (token.pointer == 'S' && token.num == (int)SingleSeparators.Semicolon)
                    {
                        nextToken(true);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка отсутствует ;");
                    }
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
            if (token.pointer == 'S' && token.num == (int)SingleSeparators.OpenRoundBracket)
            {
                nextToken(true);
                Z();
                nextToken(false);
                if (token.pointer == 'S' && token.num == (int)SingleSeparators.EndRoundBracket)
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
            while (!(token.pointer == 'K' && token.num == 33) && token.pointer != '#')
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
                    nextToken(true);
                    BW();
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
                else if (token.pointer == 'K' && token.num == 31)//goRead
                {
                    nextToken(true);
                    BR();
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
            if (token.pointer == 'S' && token.num == (int)SingleSeparators.OpenRoundBracket)
            {
                nextToken(true);
                Z();
                nextToken(false);
                if (token.pointer == 'S' && token.num == (int)SingleSeparators.EndRoundBracket)
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
                    nextToken(true);
                    BW();
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
                else if (token.pointer == 'K' && token.num == 31)//goRead
                {
                    nextToken(true);
                    BR();
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
            if (token.pointer == 'S' && token.num == (int)SingleSeparators.Equals)
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
        //блок goRead
        void BR()
        {
            nextToken(false);
            if (token.pointer == 'D' && token.num == (int)DoubleSeparators.DoubleMore)
            {
                nextToken(true);
                nextToken(false);
                if (token.pointer == 'V')
                {
                    nextToken(true);
                    nextToken(false);
                }
                else
                {
                    Console.WriteLine("Ошибка отсутствует переменная");
                    return;
                }
                while (!(token.pointer == 'S' && token.num == (int)SingleSeparators.Semicolon) && token.pointer != '#')
                {
                    nextToken(false);
                    if (token.pointer == 'D' && token.num == (int)DoubleSeparators.DoubleMore)
                    {
                        nextToken(true);
                        nextToken(false);
                        if (token.pointer == 'V')
                        {
                            nextToken(true);
                            nextToken(false);
                        }
                        else
                        {
                            Console.WriteLine("Ошибка отсутствует переменная");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка отсутствует >>");
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("Ошибка отсутствует >>");
            }
        }
        //блок goWrite
        void BW()
        {
            nextToken(false);
            if (token.pointer == 'D' && token.num == (int)DoubleSeparators.DoubleLess)
            {
                nextToken(true);
                nextToken(false);
                if (token.pointer == 'V' || token.pointer == 'N' || token.pointer == 'L' || (token.pointer == 'K' && (token.num == (int)KeyWords.TRUE || token.num == (int)KeyWords.FALSE)))
                {
                    nextToken(true);
                    nextToken(false);
                }
                else
                {
                    Console.WriteLine("Ошибка отсутствует переменная");
                    return;
                }
                while (!(token.pointer == 'S' && token.num == (int)SingleSeparators.Semicolon) && token.pointer != '#')
                {
                    nextToken(false);
                    if (token.pointer == 'D' && token.num == (int)DoubleSeparators.DoubleLess)
                    {
                        nextToken(true);
                        nextToken(false);
                        if (token.pointer == 'V' || token.pointer == 'N' || token.pointer == 'L' || (token.pointer == 'K' && (token.num == (int)KeyWords.TRUE || token.num == (int)KeyWords.FALSE)))
                        {
                            nextToken(true);
                            nextToken(false);
                        }
                        else
                        {
                            Console.WriteLine("Ошибка отсутствует переменная");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка отсутствует <<");
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("Ошибка отсутствует <<");
            }
        }
    }
}
