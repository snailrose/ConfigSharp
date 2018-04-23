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
using System.Collections.Generic;

namespace ConfigSharp
{
    public class Tree
    {
        List<Node> m_nodes;
        List<Attribute> m_attributes;
        private int m_depth;

        public Tree()
        {
            m_nodes = new List<Node>();
            m_attributes = new List<Attribute>();
            m_depth = 0;
        }

        public void AddNode( Node nd )
        {
            if( nd != null )
                m_nodes.Add( nd );
        }

        public void AddAtribute( Attribute attr )
        {
            if( attr != null )
                m_attributes.Add( attr );
        }



        public Attribute FindAttribute( string name )
        {
            foreach( Attribute attr in m_attributes ) {
                if( attr.Key == name )
                    return attr;
            }
            return null;
        }

        public Node FindNodeByName( string name )
        {
            foreach( Node nd in m_nodes ) {
                if( nd.Name == name )
                    return nd;
            }
            return null;
        }


        private Node LookupNode(Node root, string name)
        {
            if (root == null) return null;

            foreach (Node nd in root.Children)
            {
                if (nd.Name == name)
                    return nd;
                else
                {
                    Node rv = LookupNode(nd, name);
                    if (rv != null)
                        return rv;
                }
            }
            return null;
        }

        public Node LookupNode(string name)
        {
            foreach (Node nd in m_nodes)
            {
                if (nd.Name == name)
                    return nd;
            }

            // not found search recursivly
            foreach (Node nd in m_nodes)
            {
                Node rv = LookupNode(nd, name);
                if (rv != null) return rv;
            }
            return null;
        }

        public Node FindNodeByType( string type )
        {
            foreach( Node nd in m_nodes ) {
                if( nd.Type == type )
                    return nd;
            }
            return null;
        }


        public Attribute LookupAttribute(Attribute forAttr, string attr)
        {
            Node l = LookupNode(forAttr.SwizzleL);
            if (l != null)
            {
                Node r = l.FindNodeByName(forAttr.SwizzleR);
                if (r != null)
                    return r.FindAttribute(attr);
            }
            return null;
        }

        public List<Node> Nodes { get => m_nodes; }
        public List<Attribute> Attributes { get => m_attributes; }



        private string WriteNewLine()
        {
            return "\n";
        }

        private string WriteSpace()
        {
            string str = "";
            for (int i = 0; i < m_depth; ++i)
                str += " ";
            return str;
        }


        private string AsPrettyPrint(Node nd)
        {
            string result = "";
            if (nd == null)
                return result;

            result += WriteSpace();
            result += nd.Type + " " + nd.Name;
            result += WriteNewLine();
            result += WriteSpace() + "{";
            result += WriteNewLine();
            m_depth += 4;

            foreach (Attribute attr in nd.Attributes)
            {
                if (attr.Type == Token.TokenType.InlineExtra)
                {
                    result += WriteSpace();
                    result += attr.Key + "  =>>>";
                    result += attr.Value;
                    result += "<<<=;";
                    result += WriteNewLine();
                }
                else if (attr.Type == Token.TokenType.Array)
                {
                    result += WriteSpace();
                    result += attr.Key + " [";
                    result += WriteNewLine();
                    m_depth += 4;

                    string[] arr = attr.Array;
                    bool first = true;

                    foreach (string s in arr)
                    {
                        if (!first) {
                            result += ',';
                            result += WriteNewLine();
                        }
                        result += WriteSpace();
                        result += s;
                        first = false;
                    }
                    result += WriteNewLine();
                    m_depth -= 4;
                    result += WriteSpace();
                    result += "]";
                    result += WriteNewLine();
                }
                else
                {
                    result += WriteSpace();
                    result += attr.Key + "  = \"" + attr.Value + "\";";
                    result += WriteNewLine();
                }
            }
            foreach (Node chnd in nd.Children)
                result += AsPrettyPrint(chnd);

            m_depth -= 4;
            result += WriteSpace();
            result += "}" + WriteNewLine();
            return result;
        }


        public string AsPrettyPrint()
        {
            string result = "";
            result += WriteSpace();
            result += WriteNewLine();
            m_depth = 0;

            foreach (Node nd in m_nodes)
                result += AsPrettyPrint(nd);
            return result;
        }



        private string AsCompactPrint(Node nd)
        {
            string result = "";
            if (nd == null)
                return result;

            result += nd.Type + " " + nd.Name + "{";
            foreach (Attribute attr in nd.Attributes)
            {
                if (attr.Type == Token.TokenType.InlineExtra)
                {
                    result += attr.Key + "=>>>";
                    result += attr.Value;
                    result += "<<<=;";
                }
                else if (attr.Type == Token.TokenType.Array)
                {
                    result += attr.Key + "[";
                    string[] arr = attr.Array;
                    bool first = true;
                    foreach (string s in arr)
                    {
                        if (!first)
                            result += ',';
                        result += s;
                        first = false;
                    }
                    result += "]";
                }
                else
                    result += attr.Key + "=\"" + attr.Value + "\";";
            }
            foreach (Node chnd in nd.Children)
                result += AsCompactPrint(chnd);
            result += "}";
            return result;
        }

        public string AsCompactPrint()
        {
            string result = "";
            m_depth = 0;

            foreach (Node nd in m_nodes)
                result += AsCompactPrint(nd);
            return result;
        }

        public string AsCompactByteArray()
        {
            string result = "", val = AsCompactPrint();
            if (val.Length > 0)
            {
                int nl = (int)'\n';
                int cr = (int)'\r';


                int i = 0;
                foreach (char c in val)
                {
                    int ci = c;
                    if ((ci >= 32 && ci <= 127)|| ci == nl || ci == cr )
                    {
                        result += ci.ToString();
                        if (i + 1 < val.Length)
                            result += ",";
                        ++i;
                    }
                }
            }
            return result;
        }

        public override string ToString() { return AsPrettyPrint(); }
    }
}
