﻿/*
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
using System;
using System.Collections.Generic;

namespace ConfigSharp
{
    public class SyntaxError : Exception {
        int m_line;

        public SyntaxError(string message, int line) : base(message) {
            m_line = line;
        }

        public int Line  => m_line;
    }


    public class Parser
    {
        Tree m_tree;

        public Parser()
        {
            m_tree = new Tree();
        }

        public void ParseString( string buffer )
        {
            Lexer lex = new Lexer( buffer );

            Node cur_node = null;
            Stack<Node> stack = new Stack<Node>();

            int idx = 0;
            Token tok = null;
            while( ( tok = lex.Lex() ) != null && tok.Type != Token.TokenType.EOF ) {
                if( tok.Type == Token.TokenType.SyntaxError ) {
                    throw new SyntaxError("syntax error," + tok.Value, lex.Line);
                }
                else if( tok.Type == Token.TokenType.CloseBracket)
                {
                    if( stack.Count > 0 ) {
                        Node top = stack.Pop();
                        if( stack.Count == 0 )
                            m_tree.AddNode( top );
                        else
                            stack.Peek().AddChild( top );
                    }
                    continue;
                } else if( tok.Type == Token.TokenType.TokenIdentifier ) {
                    // rule TokenIdentifier {
                    // rule TokenIdentifier TokenIdentifier {
                    // rule TokenIdentifier TokenIdentifier;

                    Token d, a, b, c = null;
                    a = tok;
                    tok = lex.Lex();
                    if( tok != null &&
                        tok.Type == Token.TokenType.TokenIdentifier ||
                        tok.Type == Token.TokenType.OpenBracket ) {
                        b = tok;
                        if( b.Type != Token.TokenType.OpenBracket ) {
                            tok = lex.Lex();
                            if (tok.Type == Token.TokenType.Terminator)
                                c = tok;
                        }
                        cur_node = new Node( a.Value,
                                             b.Type != Token.TokenType.OpenBracket ?
                                             b.Value : String.Format( "__unnamed{0}__", idx++ )
                                           );
                        if( cur_node != null )
                        {
                            stack.Push(cur_node);

                            if (stack.Count > 0 && c != null)
                            {
                                Node top = stack.Pop();
                                if (stack.Count == 0)
                                    m_tree.AddNode(top);
                                else
                                    stack.Peek().AddChild(top);
                            }
                        }
 

                        continue;
                    } else if( tok != null && tok.Type == Token.TokenType.Equals ) {
                        d = a;
                        a = tok;
                        b = lex.Lex();

                        if ( b.Type != Token.TokenType.String && b.Type != Token.TokenType.InlineExtra ) {
                            throw new SyntaxError("syntax error, expecting string", lex.Line);
                        }
                        c = lex.Lex();
                        if( c.Type != Token.TokenType.Terminator ) {
                            throw new SyntaxError("syntax error, expecting ';'", lex.Line);
                        }
                        
                        if ( stack.Count == 0 )
                            m_tree.AddAtribute( new Attribute( d.Value, b.Value ));
                        else
                            stack.Peek().AddAttribute( new Attribute( d.Value, b.Value ));
                        continue;
                    }
                    else if (tok != null && tok.Type == Token.TokenType.EOF)
                        throw new SyntaxError("syntax error, premature end of file", lex.Line);
                }
            }
        }
        public Tree Tree { get => m_tree; }
    }
}
