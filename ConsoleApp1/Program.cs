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


        static string def = "" +
        "/*\n" +
        "Multi line comment\n" +
        "*/\n" +
        "type name\n" +
        "{\n" +
        "   key1  = \"value1\";\n" +
        "   key2  = \"value2\";\n" +
        "\n" +
        "   // Single line comment\n" +
        "    subtype name\n" +
        "    {\n" +
        "        key1 = \"123\";\n" +
        "        key2 = \"5,7,8,9,1\";\n" +
        "    }\n" +
        "\n" +
        "key_chunk =>>>\n" +
        "  #include \"stdio.h\"\n" +
        "\n" +
        "  int main(int argc, char** argv)\n" +
        "  {\n" +
        "       printf(\"defines this large block of text to be stored in the key_chunk attribute\n\");\n" +
        "       return 0;\n" +
        "   }\n" +
        "<<<=;\n" +
        "}\n";


        static void Main(string[] args)
        {

            Console.WriteLine("========================================");
            Console.WriteLine("Parsing config script ");
            Console.WriteLine("");
            Console.WriteLine(def);
            Console.WriteLine("========================================");

            ConfigSharp.Parser p = new ConfigSharp.Parser();
            p.ParseString(def);

            ConfigSharp.Node nd = p.Tree.FindNodeByName("name");
            if (nd != null)
            {
                ConfigSharp.Attribute key1 = nd.FindAttribute("key1");
                if (key1!=null)
                    Console.WriteLine("Parsed key1 = " + key1.Value);

                ConfigSharp.Attribute key2 = nd.FindAttribute("key2");
                if (key2 != null)
                    Console.WriteLine("Parsed key2 = " + key2.Value);

                ConfigSharp.Attribute key_chunk = nd.FindAttribute("key_chunk");
                if (key_chunk != null)
                    Console.WriteLine("Parsed key_chunk = " + key_chunk.Value);
            }
        }
    }
}
