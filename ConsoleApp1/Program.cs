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
using System;



namespace ConsoleApp1
{
    class Program
    {
        static string def = "type xyx {\n"+
                            " key1 = \"value1\";\n"+
                            " key2 = \"value2\";\n" +
                            " some_node_type pdq { \n" +
                            "   some_other_attribute = \"value\";\n" +
                            " }\n" +
                            "}";



        static void Main(string[] args)
        {

            Console.WriteLine("Parsing config script ");
            Console.WriteLine("");
            Console.WriteLine(def);

            ConfigSharp.Parser p = new ConfigSharp.Parser();
            p.ParseString(def);

            ConfigSharp.Node nd = p.Tree.FindNodeByName("xyx");
            if (nd != null)
            {
                ConfigSharp.Attribute key1 = nd.FindAttribute("key1");
                if (key1!=null)
                    Console.WriteLine("Parsed key1 = " + key1.Value);

                ConfigSharp.Attribute key2 = nd.FindAttribute("key2");
                if (key1 != null)
                    Console.WriteLine("Parsed key2 = " + key2.Value);
            }
        }
    }
}
