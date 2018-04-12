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

namespace ConfigSharp
{
    internal class StringUtils
    {
        public static int GetValueI( String text, int def = 0)
        {
            if (Int32.TryParse(text, out int r))
                return r;
            return def;
        }

        public static float GetValueF(String text, float def = 0)
        {
            return (float)GetValueD(text);
        }


        public static double GetValueD(String text, double def = 0)
        {
            if (Double.TryParse(text, out double r))
                return r;
            return def;
        }

        public static String GetValueS( int val )
        {
            return val.ToString();
        }

        public static string ToString(byte[] arr)
        {
            string str = "";
            foreach (int a in arr)
                str += (char)a;
            return str;
        }

        public static byte[] ToBytes(string inp)
        {
            byte[] barr = new byte[inp.Length + 1];
            int i = 0;
            for (i = 0; i < inp.Length; ++i)
                barr[i] = (byte)inp[i];
            barr[i] = 0;
            return barr;
        }
    }
}
