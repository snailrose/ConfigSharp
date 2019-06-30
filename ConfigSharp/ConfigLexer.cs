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
    public class Lexer
    {
        private string m_buffer;
        private int m_cur;
        private int m_len;

        public int Line { get; private set; }

        public Lexer(string str)
        {
            Line = 0;
            m_cur = 0;
            m_len = str.Length;
            m_buffer = str;
        }

        public Token Lex()
        {
            while (!EOF())
            {
                char c = m_buffer[m_cur];
                if (c == '/')
                {
                    c = m_buffer[++m_cur];
                    if (c == '/')
                    {
                        while (c != '\n' && c != '\r' && !EOF())
                            c = m_buffer[++m_cur];
                    }
                    else if (c == '*')
                    {
                        while (c != '/' && m_cur < m_len)
                        {
                            c = m_buffer[++m_cur];
                            if (c == '\n' || c == '\r')
                                Line++;
                        }
                    }
                    continue;
                }
                else if (IsWS(c))
                {
                    while (IsWS(m_buffer[++m_cur]) && !EOF()) ;
                    continue;
                }
                else if (c == '=')
                {
                    ++m_cur;
                    return new Token(Token.TokenType.Equals, "=");
                }
                else if (c == ';')
                {
                    ++m_cur;
                    return new Token(Token.TokenType.Terminator, ";");
                }
                else if (c == '"')
                {
                    string value = "";
                    while ((c = m_buffer[++m_cur]) != '"' && !EOF())
                        value += c;
                    if (EOF())
                        break;
                    m_cur++;
                    return new Token(Token.TokenType.String, value);
                }
                else if (c == '\n' || c == '\r')
                {
                    ++m_cur;
                    if (c == '\n')
                        Line++;
                    continue;
                }
                else if (c == '{')
                {
                    ++m_cur;
                    return new Token(Token.TokenType.OpenBracket, "{");
                }
                else if (c == '[')
                {
                    c = m_buffer[++m_cur];

                    string buf = "";
                    while (!EOF() && c != ']')
                    {
                        if (!IsWS(c) && !IsNewLine(c))
                            buf += c;
                        c = m_buffer[++m_cur];
                    }

                    // move past ']'
                    ++m_cur;

                    if (EOF())
                        return new Token(Token.TokenType.EOF, "end of file found while scanning array.");
                    return new Token(Token.TokenType.Array, buf);
                }
                else if (c == '}')
                {
                    ++m_cur;
                    return new Token(Token.TokenType.CloseBracket, "}");
                }
                else if (c == '>')
                {
                    int cnt = 1;
                    while ((c = m_buffer[++m_cur]) == '>' && !EOF())
                        ++cnt;
                    if (cnt == 3)
                    {
                        string alpha = "";
                        while (!EOF())
                        {
                            c = m_buffer[m_cur];
                            if (c == '<')
                            {
                                if ((m_cur + 4) < m_len)
                                {
                                    if (m_buffer.Substring(m_cur, 4) == "<<<=")
                                    {
                                        m_cur += 4;
                                        return new Token(Token.TokenType.InlineExtra, alpha);
                                    }
                                    else
                                        alpha += c;
                                }
                            }
                            else
                                alpha += c;
                            ++m_cur;
                        }
                    }
                }
                else if (IsAlpha(c))
                {
                    string alpha = "";
                    alpha += c;
                    while (ValidIdentifier(c) && !EOF())
                    {
                        c = m_buffer[++m_cur];
                        if (ValidIdentifier(c))
                            alpha += c;
                    }
                    return new Token(Token.TokenType.TokenIdentifier, alpha);
                }
                else
                    return new Token(Token.TokenType.SyntaxError, " starting at '" + c + "'");
            }
            return new Token(Token.TokenType.EOF, "EOF");
        }

        private bool ValidIdentifier(char c)
        {
            return (IsAlpha(c) || IsNumeric(c) || c == '_' || c == '.');
        }

        public static bool IsAlpha(char c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
        }

        public static bool IsNumeric(char c)
        {
            return (c >= '1' && c <= '9') || c == '0';
        }

        public static bool IsWS(char c)
        {
            return (c == ' ' || c == '\t');
        }
        public static bool IsNewLine(char c)
        {
            return (c == '\r' || c == '\n');
        }

        bool EOF() { return m_cur >= m_len; }

    }

}
