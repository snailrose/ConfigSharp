# ConfigSharp
Flexible configuration parser

<pre>

/*
Multi line comment
*/
type name {
    key1  = "value1";
    key2  = "value2";
    
    // Single line comment
    subtype name {
      key1 = "123";
      key2 = "5,7,8,9,1";
    }

    // comma seperated list 
    somearray [
        a, b, c, d, e, f, g,
        1, 2, 3, 4, 5, 6, 7,
        "A", "B", "C", "D", "E"
    ]
    
    key_chunk =>>>
      #include "stdio.h"
      
      int main (int argc, char **argv)  
      {
          printf("Hello World\n");
          return 0;
      }
    <<<=;

}
</pre>

type, name, key1, key2, and key_chunk may be any alpha numeric identifier as long as the first character is a letter.
<br/>
All attribute values need to defined within quotation marks, and terminated with the semicolon `;` character. 
<br/>
It’s up to you to convert anything in the string to suit your needs.
<br/>
The assignment `=>>>` allows you to put everything in the attribute's value up to the terminating `<<<=;` tag.

