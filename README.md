# JSON
C# JSON parser / stringifier, or deserializer / serializer

I created this to be used with a project in Unity, as the included deserializer wasn't optimal for unstructured data. Of course there are others that do work with unstructed data and are probably better than mine, but I wanted to create my own.

To deserialize, use JSON.parse(int json).
This returns null object for "null", a boxed bool for "true" or "false", a boxed double for a number, string for string, List&lt;object> for array, and Dictionary&lt;string, object> for object.
  
Serialize with JSON.stringify.
Or, if you want nice formatting, JSON.prettyStringify
(prettyStringify also arguments where you can specify what string to use for indentations and new lines)
stringify accepts all the object types that JSON.parse returns, as well as object[] for arrays, and IDictionary&lt;string, object> for objects so you may use Dictionaries or SortedDictionaries.

Now also has functions for working with streams rather than strings. Use JSON.read, giving a string of a path to a file or a StreamReader to recieve and object. Likewise JSON.write (and JSON.prettyWrite) takes a path (or StreamWriter) and the object to write.
