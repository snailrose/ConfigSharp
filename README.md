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
  
  key_chunk =>>>
      #include "stdio.h"
      
      int main (int argc, char **argv)  
      {
          printf("defines this large block of text to be stored in the key_chunk attribute\n");
          return 0;
      }
    <<<=;

}
</pre>

type, name, key1, key2, and key_chunk may be any alpha numeric identifier as long as the first character is a letter.
All attribute values need to defined within quotation marks. It’s up to you to convert the string for your needs.
<br/>
The assignment `=>>>` allows you to put everything in the attribute up to the terminating `<<<=;` tag.


