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
    public class Node
    {
        string m_name;
        string m_type;

        List<Node> m_children;
        List<Attribute> m_attributes;
        Node m_parent;

        public Node( string type, string name )
        {
            m_name = name;
            m_type = type;
            m_parent = null;
            m_children = new List<Node>();
            m_attributes = new List<Attribute>();
        }

        public void AddChild( Node nd )
        {
            if( nd != null ) {
                m_children.Add( nd );
                nd.m_parent = this;
            }
        }

        public void AddAttribute( Attribute attr )
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
            foreach( Node nd in m_children ) {
                if( nd.m_name == name )
                    return nd;
            }
            return null;
        }

        public Node FindNodeByType( string type )
        {
            foreach( Node nd in m_children ) {
                if( nd.m_type == type )
                    return nd;
            }
            return null;
        }

        public Attribute this[int key]
        {
            get
            {
                if (key >= 0 && key < m_attributes.Count)
                    return m_attributes[key];
                return null;
            }
        }


        public string Name { get => m_name; }
        public string Type { get => m_type; }
        public List<Node> Children { get => m_children;}
        public Node Parent { get => m_parent; }
        public List<Attribute> Attributes { get => m_attributes; set => m_attributes = value; }
    }
}
