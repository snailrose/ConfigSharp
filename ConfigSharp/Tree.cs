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

        public Tree()
        {
            m_nodes = new List<Node>();
            m_attributes = new List<Attribute>();

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
    }
}
