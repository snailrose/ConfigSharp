/*
-------------------------------------------------------------------------------
    This file is part of ConfigSharp.

    Copyright (c) Charles Carley.

    Contributor(s): none yet.
-------------------------------------------------------------------------------
  This software is provided 'as-is', without any express or implied
  warranty. In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.
-------------------------------------------------------------------------------
*/
namespace ConfigSharp
{
    public class Token
    {
        public enum TokenType {

            TokenIdentifier = 0,
            OpenBracket,
            Equals,
            String,
            Terminator,
            CloseBracket,
            SyntaxError,
            InlineExtra,
            EOF
        }

        TokenType m_type;
        string m_value;

        public TokenType Type { get => m_type; set => m_type = value; }
        public string Value { get => m_value; set => m_value = value; }


        public Token( TokenType tok, string val )
        {
            m_type = tok;
            m_value = val;
        }
    }
}
