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
        private int m_depth;

        public Tree()
        {
            Nodes       = new List<Node>();
            Attributes  = new List<Attribute>();
            m_depth     = 0;
        }

        public void AddNode(Node nd)
        {
            if (nd != null)
                Nodes.Add(nd);
        }



        public void AddAtribute(string name, string value)
        {
            if (FindAttribute(name) == null)
                Attributes.Add(new Attribute(name, value));
        }

        public void AddAtribute(Attribute attr)
        {
            if (attr != null)
                Attributes.Add(attr);
        }



        public Attribute FindAttribute(string name)
        {
            foreach (Attribute attr in Attributes)
            {
                if (attr.Key == name)
                    return attr;
            }
            return null;
        }

        public Node FindNodeByName(string name)
        {
            foreach (Node nd in Nodes)
            {
                if (nd.Name == name)
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
            foreach (Node nd in Nodes)
            {
                if (nd.Name == name)
                    return nd;
            }

            // not found search recursively
            foreach (Node nd in Nodes)
            {
                Node rv = LookupNode(nd, name);
                if (rv != null) return rv;
            }
            return null;
        }

        public Node FindNodeByType(string type)
        {
            foreach (Node nd in Nodes)
            {
                if (nd.Type == type)
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

        public List<Node> Nodes { get; private set; }
        public List<Attribute> Attributes { get; private set; }

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
        
        private string AsPrettyPrint(List<Attribute> attributes)
        {
            string result = "";
            if (attributes == null)
                return result;

            foreach (Attribute attr in attributes)
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
                        if (!first)
                        {
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
            return result;
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
            result += AsPrettyPrint(nd.Attributes);
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
            result += AsPrettyPrint(Attributes);
            foreach (Node nd in Nodes)
                result += AsPrettyPrint(nd);
            return result;
        }

        private string AsCompactPrint(List<Attribute> attributes)
        {
            string result = "";
            if (attributes == null)
                return result;

            foreach (Attribute attr in attributes)
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
            return result;
        }

        private string AsCompactPrint(Node nd)
        {
            string result = "";
            if (nd == null)
                return result;

            result += nd.Type + " " + nd.Name + "{";
            result += AsCompactPrint(nd.Attributes);
            foreach (Node chnd in nd.Children)
                result += AsCompactPrint(chnd);
            result += "}";
            return result;
        }

        public string AsCompactPrint()
        {
            string result = "";
            m_depth = 0;
            result += AsCompactPrint(Attributes);
            foreach (Node nd in Nodes)
                result += AsCompactPrint(nd);
            return result;
        }

        public string AsCompactByteArray()
        {
            string result = "", val = AsCompactPrint();
            if (val.Length > 0)
            {
                int nl = '\n';
                int cr = '\r';
                int i = 0;
                foreach (char c in val)
                {
                    int ci = c;
                    if ((ci >= 32 && ci <= 127) || ci == nl || ci == cr)
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
        
        private static int CharHex2Int(char c)
        {
            int ival = 0;
            if (c >= 'A') ival = 10 + ((int)c) - 'A';
            else if (c >= '0') ival = ((int)c) - '0';
            return ival;
        }
        
        public string FromHex(string src)
        {

            if( src == null || src.Length == 0 )
                return string.Empty;

            int i, len = src.Length, dv,rv, iv;
            string res = "";
            
            for (i=0; i<len; i+=2)
            {
                dv = CharHex2Int(src[i]);
                rv = CharHex2Int(src[i+1]);
                iv = 16 * dv + rv;
                res += (char)iv;
            }
            return  res;
        }

        public string AsHex()
        {
            string result = "";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(AsCompactPrint());
            foreach( byte c in bytes)
                result += c.ToString("X2");
            return result;
        }
        public override string ToString() { return AsPrettyPrint(); }
    }
}
