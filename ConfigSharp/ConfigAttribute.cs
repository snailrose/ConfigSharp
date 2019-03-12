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
    public class Attribute
    {
        public Attribute( string key, string value, Token.TokenType type = Token.TokenType.String )
        {
            Key = key;
            Value = value;
            Type = type;
        }

        public string Key       { get; private set; }
        public string Value     { get; private set; }

        public string[] Array {
            get {
                string[] arr  = Value.Split( ',' );
                if( arr != null && arr.Length > 0 )
                    return arr;
                return null;
            }
        }

        public string SwizzleL {
            get {
                if( IsSwizzle() )
                {
                    string[] swiz = Value.Split( '.' );
                    if( swiz != null && swiz.Length > 0 )
                        return swiz[0];
                }
                return "";
            }
        }
        public string SwizzleR {
            get
            {
                if( IsSwizzle() )
                {
                    string[] swiz = Value.Split( '.' );
                    if( swiz != null && swiz.Length > 1 )
                        return swiz[1];
                }
                return "";
            }
        }

        public bool IsArray()
        {
            if( Value != null )
                return Value.Contains( "," );
            return false;
        }


        public bool IsSwizzle()
        {
            if( Value != null ) {
                if( Lexer.IsAlpha( Value[0] ) )
                    return Value.Contains( "." );
            }
            return false;
        }


        public bool IsIdentifier()
        {
            if( Value != null ) {
                if( Lexer.IsAlpha( Value[0] ) )
                    return true;
            }
            return false;
        }


        public float Float => GetValueF( Value );
        public int Int     => GetValueI( Value );
        public Token.TokenType Type { get; private set; }

        public float[] Rect {
            get
            {
                float[] rect = new float[4] { 0, 0, 0, 0 };

                string[] arr = Value.Split( ',' );
                if( arr.Length == 4 )
                {
                    rect[0] = GetValueF( arr[0] );
                    rect[1] = GetValueF( arr[1] );
                    rect[2] = GetValueF( arr[2] );
                    rect[3] = GetValueF( arr[3] );
                }
                return rect;
            }
        }


        public float[] Vec2 {
            get
            {
                float[] v = new float[2] { 0, 0 };

                string[] arr = Value.Split( ',' );
                if( arr.Length == 2 )
                {
                    v[0] = GetValueF( arr[0] );
                    v[1] = GetValueF( arr[1] );
                }
                return v;
            }
        }


        public bool AsBool => ( Value == "true" || Value == "yes" || Int != 0 );

        internal static int GetValueI( String text, int def = 0 )
        {
            if( Int32.TryParse( text, out int r ) )
                return r;
            return def;
        }

        internal static float GetValueF( String text, float def = 0 )
        {
            return ( float )GetValueD( text );
        }


        internal static double GetValueD( String text, double def = 0 )
        {
            if( Double.TryParse( text, out double r ) )
                return r;
            return def;
        }
    }
}
