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
        public Node( string type, string name )
        {
            Name = name;
            Type = type;
            Parent = null;
            Children = new List<Node>();
            Attributes = new Dictionary<string, Attribute>();
        }

        public void AddChild( Node nd )
        {
            if( nd != null ) {
                Children.Add( nd );
                nd.Parent = this;
            }
        }

        public void AddAttribute( Attribute attr )
        {
            if( attr != null )
                Attributes.Add(attr.Key, attr );
        }

        public Attribute FindAttribute( string name )
        {
            if (Attributes.ContainsKey(name))
                return Attributes[name];
            return null;
        }


        public Node FindNodeByName( string name )
        {
            foreach( Node nd in Children ) {
                if( nd.Name == name )
                    return nd;
            }
            return null;
        }

        public Node FindNodeByType( string type )
        {
            foreach( Node nd in Children ) {
                if( nd.Type == type )
                    return nd;
            }
            return null;
        }
        

        public string           Name        { get; private set; }
        public string           Type        { get; private set; }
        public List<Node>       Children    { get; private set; }
        public Node             Parent      { get; private set; }

        public Dictionary<string, Attribute>  Attributes  { get; set; }
    }
}
