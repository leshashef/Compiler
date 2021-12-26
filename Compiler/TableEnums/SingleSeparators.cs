using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.TableEnums
{
    public enum SingleSeparators : int
    {
        Multiply = 1,
        Divide = 2,
        Equals = 3,
        Comma = 5,
        Substract = 6,
        Add = 7,
        More = 8,
        Less = 9,
        Semicolon = 14,
        OpenRoundBracket = 16,
        EndRoundBracket = 17,
        ExclamationMark = 18,
        OpenSquareBracket = 19,
        EndSquareBracket = 20,
    }
}
