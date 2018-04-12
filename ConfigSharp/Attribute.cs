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


namespace ConfigSharp
{
    public class Attribute
    {
        private string m_key;
        private string m_value;

        public Attribute(string key, string value)
        {
            m_key = key;
            m_value = value;
        }

        public string Key { get => m_key; }
        public string Value { get => m_value; }

        public string[] Array
        {
            get {
                string[] arr  = m_value.Split(',');
                if (arr != null && arr.Length > 0)
                    return arr;
                return null;
            }
        }

        public string SwizzleL
        {
            get {
                if (IsSwizzle())
                {
                    string[] swiz = m_value.Split('.');
                    if (swiz != null && swiz.Length > 0)
                        return swiz[0];
                }
                return "";
            }
        }
        public string SwizzleR
        {
            get
            {
                if (IsSwizzle())
                {
                    string[] swiz = m_value.Split('.');
                    if (swiz != null && swiz.Length > 1)
                        return swiz[1];
                }
                return "";
            }
        }

        public bool IsArray()
        {
            if (m_value != null)
                return m_value.Contains(",");
            return false;
        }


        public bool IsSwizzle()
        {
            if (m_value != null)
            {
                if (Lexer.IsAlpha(m_value[0]))
                    return m_value.Contains(".");
            }
            return false;
        }


        public bool IsIdentifier()
        {
            if (m_value != null)
            {
                if (Lexer.IsAlpha(m_value[0]))
                    return true;
            }
            return false;
        }


        public float Float  { get => StringUtils.GetValueF(m_value); }
        public int Int      { get => StringUtils.GetValueI(m_value); }
    }
}