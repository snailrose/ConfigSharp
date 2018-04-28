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

Nodes are defined as follows
<pre>

node          ::=  node_decl
node_decl     ::=  identifier  identifier  '{' decl '}'
                    | identifier '{' decl '}'
                    | identifier identifier ';'
                    | decl
decl          ::=    identifier '=' '"' [any - \n] '"' ';'
                    | identifier '=>>>' [any] '<<<='; 
                    | node_decl
                    
</pre>
