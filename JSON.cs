using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public static class JSON
{
    public static object parse(string json)
    {
        int index = 0;
        return parse(json, ref index);
    }

    public static string stringify(object obj)
    {
        if (obj == null) return "null";
        if (obj is bool) return (bool)obj ? "true" : "false";
        if (obj is double) return ((double)obj).ToString(System.Globalization.CultureInfo.InvariantCulture);
        StringBuilder s = new StringBuilder();
        if (obj is string)
        {
            s.Append('"');
            foreach (char c in (string)obj)
            {
                switch (c)
                {
                    case '"': s.Append("\\\""); break;
                    case '\\': s.Append("\\\\"); break;
                    case '\n': s.Append("\\n"); break;
                    case '\r': s.Append("\\r"); break;
                    case '\t': s.Append("\\t"); break;
                    case '\b': s.Append("\\b"); break;
                    case '\f': s.Append("\\f"); break;
                    default:
                        if (c > '\u001f')
                            s.Append(c);
                        else
                        {
                            s.Append("\\u00");
                            int high = c / 16;
                            int low = c % 16;
                            if (high <= 9)
                                s.Append((char)('0' + high));
                            else
                                s.Append((char)('a' + high));
                            if (low <= 9)
                                s.Append((char)('0' + low));
                            else
                                s.Append((char)('a' + low));
                        }
                        break;
                }
            }
            s.Append('"');
            return s.ToString();
        }
        if (obj is IList<object>)
        {
            IList<object> list = (IList<object>)obj;
            s.Append('[');
            for (int i = 0; i < list.Count; ++i)
            {
                s.Append(stringify(list[i]));
                if (i < list.Count - 1)
                    s.Append(',');
            }
            s.Append(']');
            return s.ToString();
        }
        if (obj is object[])
        {
            object[] array = (object[])obj;
            s.Append('[');
            for (int i = 0; i < array.Length; ++i)
            {
                s.Append(stringify(array[i]));
                if (i < array.Length - 1)
                    s.Append(',');
            }
            s.Append(']');
            return s.ToString();
        }
        if (obj is IDictionary<string, object>)
        {
            s.Append('{');
            bool first = true;
            foreach (KeyValuePair<string, object> pair in (IDictionary<string, object>)obj)
            {
                if (first)
                    first = false;
                else
                    s.Append(',');
                s.Append(stringify(pair.Key));
                s.Append(':');
                s.Append(stringify(pair.Value));
            }
            s.Append('}');
            return s.ToString();
        }
        throw new Exception();
    }

    public static string prettyStringify(object obj, string indent = "\t", string newLine = "\n", int indentCount = 0, bool ignoreInitailIndent = false)
    {
        StringBuilder s = new StringBuilder();
        if (!ignoreInitailIndent)
            for (int i = 0; i < indentCount; ++i)
                s.Append(indent);
        if (obj == null)
        {
            s.Append("null");
            return s.ToString();
        }
        if (obj is bool) {
            s.Append((bool)obj ? "true" : "false");
            return s.ToString();
        }
        if (obj is double) {
            s.Append(((double)obj).ToString(System.Globalization.CultureInfo.InvariantCulture));
            return s.ToString();
        }
        if (obj is string)
        {
            s.Append('"');
            foreach (char c in (string)obj)
            {
                switch (c)
                {
                    case '"': s.Append("\\\""); break;
                    case '\\': s.Append("\\\\"); break;
                    case '\n': s.Append("\\n"); break;
                    case '\r': s.Append("\\r"); break;
                    case '\t': s.Append("\\t"); break;
                    case '\b': s.Append("\\b"); break;
                    case '\f': s.Append("\\f"); break;
                    default:
                        if (c > '\u001f')
                            s.Append(c);
                        else
                        {
                            s.Append("\\u00");
                            int high = c / 16;
                            int low = c % 16;
                            if (high <= 9)
                                s.Append((char)('0' + high));
                            else
                                s.Append((char)('a' + high));
                            if (low <= 9)
                                s.Append((char)('0' + low));
                            else
                                s.Append((char)('a' + low));
                        }
                        break;
                }
            }
            s.Append('"');
            return s.ToString();
        }
        if (obj is IList<object>)
        {
            IList<object> list = (IList<object>)obj;
            s.Append('[');
            s.Append(newLine);
            for (int i = 0; i < list.Count; ++i)
            {
                s.Append(prettyStringify(list[i], indent, newLine, indentCount + 1));
                if (i < list.Count - 1)
                    s.Append(',');
                s.Append(newLine);
            }
            for (int i = 0; i < indentCount; ++i)
                s.Append(indent);
            s.Append(']');
            return s.ToString();
        }
        if (obj is object[])
        {
            object[] array = (object[])obj;
            s.Append('[');
            s.Append(newLine);
            for (int i = 0; i < array.Length; ++i)
            {
                s.Append(prettyStringify(array[i], indent, newLine, indentCount + 1));
                if (i < array.Length - 1)
                    s.Append(',');
                s.Append(newLine);
            }
            for (int i = 0; i < indentCount; ++i)
                s.Append(indent);
            s.Append(']');
            return s.ToString();
        }
        if (obj is IDictionary<string, object>)
        {
            s.Append('{');
            s.Append(newLine);
            bool first = true;
            foreach (KeyValuePair<string, object> pair in (IDictionary<string, object>)obj)
            {
                if (first)
                    first = false;
                else
                {
                    s.Append(',');
                    s.Append(newLine);
                }
                s.Append(prettyStringify(pair.Key, indent, newLine, indentCount + 1));
                s.Append(": ");
                s.Append(prettyStringify(pair.Value, indent, newLine, indentCount + 1, true));
            }
            s.Append(newLine);
            for (int i = 0; i < indentCount; ++i)
                s.Append(indent);
            s.Append('}');
            return s.ToString();
        }
        throw new Exception();
    }

    public static object read(string path)
    {
        StreamReader reader = new StreamReader(path);
        object result = read(reader);
        reader.Close();
        reader.Dispose();
        return result;
    }

    public static object read(StreamReader reader)
    {
        int c = reader.Read();
        StringBuilder s = new StringBuilder();
        object result = parseValueFromStream(reader, ref c, s);
        if (c == -1)
            return result;
        else throw new Exception();
    }

    public static void write(string path, object obj)
    {
        StreamWriter writer = new StreamWriter(path);
        write(writer, obj);
        writer.Close();
        writer.Dispose();
    }

    public static void write(StreamWriter writer, object obj, StringBuilder s = null)
    {
        if (s == null) s = new StringBuilder();
        if (obj == null) writer.Write("null");
        else if (obj is bool) writer.Write((bool)obj ? "true" : "false");
        else if (obj is double) writer.Write(((double)obj).ToString(System.Globalization.CultureInfo.InvariantCulture));
        else if (obj is string)
        {
            s.Append('"');
            foreach (char c in (string)obj)
            {
                switch (c)
                {
                    case '"': s.Append("\\\""); break;
                    case '\\': s.Append("\\\\"); break;
                    case '\n': s.Append("\\n"); break;
                    case '\r': s.Append("\\r"); break;
                    case '\t': s.Append("\\t"); break;
                    case '\b': s.Append("\\b"); break;
                    case '\f': s.Append("\\f"); break;
                    default:
                        if (c > '\u001f')
                            s.Append(c);
                        else
                        {
                            s.Append("\\u00");
                            int high = c / 16;
                            int low = c % 16;
                            if (high <= 9)
                                s.Append((char)('0' + high));
                            else
                                s.Append((char)('a' + high));
                            if (low <= 9)
                                s.Append((char)('0' + low));
                            else
                                s.Append((char)('a' + low));
                        }
                        break;
                }
            }
            s.Append('"');
            writer.Write(s.ToString());
            s.Clear();
        }
        else if (obj is IList<object>)
        {
            writer.Write('[');
            bool first = true;
            foreach (object item in (IList<object>)obj)
            {
                if (first)
                    first = false;
                else
                    writer.Write(',');
                write(writer, item, s);
            }
            writer.Write(']');
        }
        else if (obj is object[])
        {
            writer.Write('[');
            bool first = true;
            foreach (object item in (object[])obj)
            {
                if (first)
                    first = false;
                else
                    writer.Write(',');
                write(writer, item, s);
            }
            writer.Write(']');
        }
        else if (obj is IDictionary<string, object>)
        {
            writer.Write('{');
            bool first = true;
            foreach (KeyValuePair<string, object> pair in (IDictionary<string, object>)obj)
            {
                if (first)
                    first = false;
                else
                    writer.Write(',');
                write(writer, pair.Key);
                writer.Write(':');
                write(writer, pair.Value);
            }
            writer.Write('}');
        }
        else throw new Exception();
    }

    public static void prettyWrite(string path, object obj, string indent = "\t", string newLine = "\n", int indentCount = 0, bool supressInitialIndent = false)
    {
        StreamWriter writer = new StreamWriter(path);
        prettyWrite(writer, obj, indent, newLine, indentCount, supressInitialIndent);
        writer.Close();
        writer.Dispose();
    }

    public static void prettyWrite(StreamWriter writer, object obj, string indent = "\t", string newLine = "\n", int indentCount = 0, bool supressInitialIndent = false, StringBuilder s = null)
    {
        if (s == null) s = new StringBuilder();
        if (!supressInitialIndent)
            for (int i = 0; i < indentCount; ++i)
                writer.Write(indent);

        if (s == null) s = new StringBuilder();
        if (obj == null) writer.Write("null");
        else if (obj is bool) writer.Write((bool)obj ? "true" : "false");
        else if (obj is double) writer.Write(((double)obj).ToString(System.Globalization.CultureInfo.InvariantCulture));
        else if (obj is string)
        {
            s.Append('"');
            foreach (char c in (string)obj)
            {
                switch (c)
                {
                    case '"': s.Append("\\\""); break;
                    case '\\': s.Append("\\\\"); break;
                    case '\n': s.Append("\\n"); break;
                    case '\r': s.Append("\\r"); break;
                    case '\t': s.Append("\\t"); break;
                    case '\b': s.Append("\\b"); break;
                    case '\f': s.Append("\\f"); break;
                    default:
                        if (c > '\u001f')
                            s.Append(c);
                        else
                        {
                            s.Append("\\u00");
                            int high = c / 16;
                            int low = c % 16;
                            if (high <= 9)
                                s.Append((char)('0' + high));
                            else
                                s.Append((char)('a' + high));
                            if (low <= 9)
                                s.Append((char)('0' + low));
                            else
                                s.Append((char)('a' + low));
                        }
                        break;
                }
            }
            s.Append('"');
            writer.Write(s.ToString());
            s.Clear();
        }
        else if (obj is IList<object>)
        {
            writer.Write('[');
            writer.Write(newLine);
            bool first = true;
            foreach (object item in (IList<object>)obj)
            {
                if (first)
                    first = false;
                else
                {
                    writer.Write(',');
                    writer.Write(newLine);
                }
                prettyWrite(writer, item, indent, newLine, indentCount + 1, false, s);
            }
            writer.Write(newLine);
            for (int i = 0; i < indentCount; ++i)
                writer.Write(indent);
            writer.Write(']');
        }
        else if (obj is object[])
        {
            writer.Write('[');
            writer.Write(newLine);
            bool first = true;
            foreach (object item in (object[])obj)
            {
                if (first)
                    first = false;
                else
                {
                    writer.Write(',');
                    writer.Write(newLine);
                }
                prettyWrite(writer, item, indent, newLine, indentCount + 1, false, s);
            }
            writer.Write(newLine);
            for (int i = 0; i < indentCount; ++i)
                writer.Write(newLine);
            writer.Write(']');
        }
        else if (obj is IDictionary<string, object>)
        {
            writer.Write('{');
            writer.Write(newLine);
            bool first = true;
            foreach (KeyValuePair<string, object> pair in (IDictionary<string, object>)obj)
            {
                if (first)
                    first = false;
                else
                {
                    writer.Write(',');
                    writer.Write(newLine);
                }
                prettyWrite(writer, pair.Key, indent, newLine, indentCount + 1, false, s);
                writer.Write(": ");
                prettyWrite(writer, pair.Value, indent, newLine, indentCount + 1, true, s);
            }
            writer.Write(newLine);
            for (int i = 0; i < indentCount; ++i)
                writer.Write(newLine);
            writer.Write('}');
        }
        else throw new Exception();
    }

    static object parse(string json, ref int index)
    {
        object result = parseElement(json, ref index);
        if (index == json.Length)
            return result;
        throw new Exception();
    }

    static void advancePastWhitespace(string json, ref int index)
    {
        while (index < json.Length && char.IsWhiteSpace(json[index]))
            ++index;
    }

    static object parseElement(string json, ref int index)
    {
        object result;
        advancePastWhitespace(json, ref index);
        if (index < json.Length)
            result = parseValue(json, ref index);
        else throw new Exception();
        advancePastWhitespace(json, ref index);
        return result;
    }

    static object parseValue(string json, ref int index)
    {
        char c = json[index];
        switch (c)
        {
            case 't':
                if (index + 3 < json.Length && json.Substring(index + 1, 3) == "rue")
                {
                    index += 4;
                    return true;
                }
                else throw new Exception();
            case 'f':
                if (index + 4 < json.Length && json.Substring(index + 1, 4) == "alse")
                {
                    index += 5;
                    return false;
                }
                else throw new Exception();
            case 'n':
                if (index + 3 < json.Length && json.Substring(index + 1, 3) == "ull")
                {
                    index += 4;
                    return null;
                }
                else throw new Exception();
            case '"':
                return parseString(json, ref index);
            case '[':
                return parseArray(json, ref index);
            case '{':
                return parseObject(json, ref index);
            default:
                break;
        }
        if (c == '-' || char.IsDigit(c))
            return parseNumber(json, ref index);

        // No valid value found
        throw new Exception(c.ToString());
    }

    static string parseString(string json, ref int index)
    {
        StringBuilder s = new StringBuilder();
        while (true)
        {
            if (++index == json.Length)
                throw new Exception();
            char c = json[index];
            if (c == '\\')
            {
                if (++index == json.Length)
                    throw new Exception();
                switch (json[index])
                {
                    case '"': s.Append('"'); break;
                    case '\\': s.Append('\\'); break;
                    case '/': s.Append('/'); break;
                    case 'b': s.Append('\b'); break;
                    case 'f': s.Append('\f'); break;
                    case 'n': s.Append('\n'); break;
                    case 'r': s.Append('\r'); break;
                    case 't': s.Append('\t'); break;
                    case 'u':
                        if (index + 4 < json.Length)
                        {
                            char unicodeChar = (char)0;
                            for (int i = 0; i < 4; ++i)
                            {
                                c = json[++index];
                                unicodeChar <<= 4;
                                if (char.IsDigit(c))
                                    unicodeChar |= (char)(c - '0');
                                else if (c >= 'a' && c <= 'f')
                                    unicodeChar |= (char)(10 + (c - 'a'));
                                else if (c >= 'A' && c <= 'F')
                                    unicodeChar |= (char)(10 + (c - 'A'));
                                else throw new Exception();
                            }
                            s.Append(unicodeChar);
                            break;
                        }
                        else throw new Exception();
                    default: throw new Exception();
                }
            }
            else if (c == '"')
            {
                ++index;
                return s.ToString();
            }
            else
                s.Append(c);
        }
    }

    static double parseNumber(string json, ref int index)
    {
        int start = index++;
        char c = json[start];
        if (c == '-')
            if (index < json.Length)
                c = json[index++];
            else throw new Exception();

        if (c == '0')
        {
            if (index < json.Length)
            {
                c = json[index++];
                if (c == '.')
                {
                    if (index < json.Length)
                    {
                        bool atLeastOneDigit = false;
                        while (true)
                        {
                            if (index < json.Length)
                            {
                                if (char.IsDigit(json[index]))
                                {
                                    atLeastOneDigit = true;
                                    ++index;
                                }
                                else break;
                            }
                            else break;
                        }
                        if (atLeastOneDigit)
                        {
                            if (index < json.Length)
                            {
                                c = json[index++];
                                if (c == 'e' || c == 'E')
                                    if (index < json.Length)
                                    {
                                        c = json[index++];
                                        if (c == '-' || c == '+')
                                            c = json[index++];
                                        atLeastOneDigit = false;
                                        while (char.IsDigit(c))
                                        {
                                            atLeastOneDigit = true;
                                            if (index < json.Length)
                                                c = json[index++];
                                            else if (atLeastOneDigit)
                                                return double.Parse(json.Substring(start, index - start), System.Globalization.CultureInfo.InvariantCulture);
                                        }
                                        return double.Parse(json.Substring(start, index - start), System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    else return double.Parse(json.Substring(start, index - start), System.Globalization.CultureInfo.InvariantCulture);
                                return double.Parse(json.Substring(start, index - start), System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else return double.Parse(json.Substring(start, index - start), System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else throw new Exception();
                    }
                    else throw new Exception();
                }
                else return double.Parse(json.Substring(start, index - start), System.Globalization.CultureInfo.InvariantCulture);
            }
            else return double.Parse(json.Substring(start, index - start), System.Globalization.CultureInfo.InvariantCulture);
        }
        else if (c >= '1' && c <= '9')
        {
            while (true)
            {
                if (index < json.Length)
                {
                    c = json[index];
                    if (char.IsDigit(c))
                        ++index;
                    else
                        break; ;
                } else return double.Parse(json.Substring(start), System.Globalization.CultureInfo.InvariantCulture);
            }
            if (c == '.')
            {
                if (index < json.Length)
                {
                    ++index;
                    bool atLeastOneDigit = false;
                    while (true)
                    {
                        if (index < json.Length)
                        {
                            c = json[index];
                            if (char.IsDigit(c))
                            {
                                atLeastOneDigit = true;
                                ++index;
                            }
                            else
                                break;
                        } else if (atLeastOneDigit)
                            return double.Parse(json.Substring(start), System.Globalization.CultureInfo.InvariantCulture);
                        else
                            throw new Exception();
                    }
                    if (atLeastOneDigit)
                    {
                        if (index < json.Length)
                        {
                            c = json[index++];
                            if (c == 'e' || c == 'E')
                                if (index < json.Length)
                                {
                                    c = json[index++];
                                    if (c == '-' || c == '+')
                                        c = json[index++];
                                    atLeastOneDigit = false;
                                    while (true)
                                    {
                                        if (index < json.Length)
                                        {
                                            if (char.IsDigit(json[index]))
                                            {
                                                atLeastOneDigit = true;
                                                ++index;
                                            }
                                            else break;
                                        }
                                        else return double.Parse(json.Substring(start, index - start), System.Globalization.CultureInfo.InvariantCulture); ;
                                    }
                                    return double.Parse(json.Substring(start, index - start), System.Globalization.CultureInfo.InvariantCulture);
                                }
                                else return double.Parse(json.Substring(start, index - start), System.Globalization.CultureInfo.InvariantCulture);
                            return double.Parse(json.Substring(start, index - start), System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else return double.Parse(json.Substring(start, index - start), System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else throw new Exception();
                }
                else throw new Exception();
            }
            else
                return double.Parse(json.Substring(start, index - start), System.Globalization.CultureInfo.InvariantCulture);
        }
        else throw new Exception();
    }

    static List<object> parseArray(string json, ref int index)
    {
        List<object> result = new List<object>();
        ++index;
        advancePastWhitespace(json, ref index);
        if (index < json.Length)
        {
            char c = json[index];
            if (c == ']')
            {
                ++index;
                return result;
            }
            else
            {
                result.Add(parseValue(json, ref index));
                advancePastWhitespace(json, ref index);
                while (true)
                {
                    if (index < json.Length)
                    {
                        c = json[index++];
                        switch (c)
                        {
                            case ',':
                                result.Add(parseElement(json, ref index));
                                break;
                            case ']':
                                return result;
                            default: throw new Exception();
                        }
                    }
                    else throw new Exception();
                }
            }

        }
        else throw new Exception();
    }

    static Dictionary<string, object> parseObject(string json, ref int index)
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        string key;
        ++index;
        advancePastWhitespace(json, ref index);
        if (index < json.Length)
        {
            switch (json[index])
            {
                case '"':
                    key = parseString(json, ref index);
                    advancePastWhitespace(json, ref index);
                    if (index < json.Length)
                    {
                        if (json[index] == ':')
                        {
                            if (++index < json.Length)
                            {
                                result.Add(key, parseElement(json, ref index));
                                if (index < json.Length)
                                {
                                    while (true)
                                    {
                                        switch (json[index])
                                        {
                                            case ',':
                                                if (++index < json.Length)
                                                {
                                                    advancePastWhitespace(json, ref index);
                                                    if (index < json.Length && json[index] == '"')
                                                    {
                                                        key = parseString(json, ref index);
                                                        advancePastWhitespace(json, ref index);
                                                        if (index < json.Length && json[index] == ':')
                                                        {
                                                            if (++index < json.Length) {}
                                                            result.Add(key, parseElement(json, ref index));
                                                            break;
                                                        }
                                                        else throw new Exception();
                                                    }
                                                    else throw new Exception();
                                                }
                                                else throw new Exception();
                                            case '}':
                                                ++index;
                                                return result;
                                            default: throw new Exception();
                                        }
                                    }
                                }
                                else throw new Exception();
                            }
                            else throw new Exception();

                        } else throw new Exception();
                    } else throw new Exception();
                case '}':
                    ++index;
                    return result;
                default: throw new Exception();
            }
        }
        else throw new Exception();
    }

    static object parseValueFromStream(StreamReader reader, ref int _c, StringBuilder s)
   {
       int c = _c;
       object result = null;
       bool valueFound = false;
       while (c != -1 && char.IsWhiteSpace((char)c))
           c = reader.Read();

       switch (c)
       {
           case -1: throw new Exception();
           case '"':
               valueFound = true;
               result = parseStringFromStream(reader, s);
               c = reader.Read();
               break;
           case 'n':
               valueFound = true;
               for (int i = 0; i < 3; ++i)
               {
                   c = reader.Read();
                   if (c != "ull"[i]) throw new Exception();
               }
               result = null;
               c = reader.Read();
               break;
           case 'f':
               valueFound = true;
               for (int i = 0; i < 4; ++i)
               {
                   c = reader.Read();
                   if (c != "alse"[i]) throw new Exception();
               }
               result = false;
               c = reader.Read();
               break;
           case 't':
               valueFound = true;
               for (int i = 0; i < 3; ++i)
               {
                   c = reader.Read();
                   if (c != "rue"[i]) throw new Exception();
               }
               result = true;
               c = reader.Read();
               break;
           case '[':
               valueFound = true;
               c = reader.Read();
               while (c != -1 && char.IsWhiteSpace((char)c))
                   c = reader.Read();
               result = new List<object>();
               while (true)
               {
                   if (c == ']')
                   {
                       c = reader.Read();
                       break; ;
                   }
                   ((List<object>)result).Add(parseValueFromStream(reader, ref c, s));
                   switch (c)
                   {
                       case ',':
                           c = reader.Read();
                           continue;
                       case ']': continue;
                       default: throw new Exception();
                   }
               }
               break;
           case '{':
               valueFound = true;
               c = reader.Read();
               while (c != -1 && char.IsWhiteSpace((char)c))
                   c = reader.Read();
               result = new Dictionary<string, object>();
               while (true)
               {
                   if (c == '}')
                   {
                       c = reader.Read();
                       break;
                   }
                   while (c != -1 && char.IsWhiteSpace((char)c))
                       c = reader.Read();
                   string key;
                   if (c == '"')
                       key = parseStringFromStream(reader, s);
                   else throw new Exception();
                   c = reader.Read();
                   if (c == ':')
                       c = reader.Read();
                   else throw new Exception();
                   ((Dictionary<string, object>)result).Add(key, parseValueFromStream(reader, ref c, s));
                   switch (c)
                   {
                       case ',':
                           c = reader.Read();
                           continue;
                       case '}': continue;
                       default: throw new Exception();
                   }
               }
               break;
       }
       if (char.IsDigit((char)c)) {
           valueFound = true;
           while (c != -1 && (char.IsDigit((char)c) || c == '.' || c == '-' || c == '+' || c == 'e' || c == 'E'))
           {
               s.Append((char)c);
               c = reader.Read();
           }
           result = double.Parse(s.ToString(), System.Globalization.CultureInfo.InvariantCulture);
           s.Clear();
       }
       if (!valueFound) throw new Exception();
       while (c != -1 && char.IsWhiteSpace((char)c))
           c = reader.Read();
       _c = c;
       return result;
   }

   static string parseStringFromStream(StreamReader reader, StringBuilder s)
   {
       int c;
       while (true)
       {
           c = reader.Read();
           switch (c)
           {
               case -1: throw new Exception();
               case '\\':
                   c = reader.Read();
                   switch (c)
                   {
                       case -1: throw new Exception();
                       case '"': s.Append('"'); break;
                       case '\\': s.Append('\\'); break;
                       case '/': s.Append('/'); break;
                       case 'b': s.Append('\b'); break;
                       case 'f': s.Append('\f'); break;
                       case 'n': s.Append('\n'); break;
                       case 'r': s.Append('\r'); break;
                       case 't': s.Append('\t'); break;
                       case 'u':
                           char unicodeChar = (char)0;
                           for (int i = 0; i < 4; ++i)
                           {
                               unicodeChar <<= 4;
                               c = reader.Read();
                               if (c == -1) throw new Exception();
                               else if (char.IsDigit((char)c))
                                   unicodeChar |= (char)(c - '0');
                               else if (c >= 'a' && c <= 'f')
                                   unicodeChar |= (char)(10 + (c - 'a'));
                               else if (c >= 'A' && c <= 'F')
                                   unicodeChar |= (char)(10 + (c - 'A'));
                               else throw new Exception();
                           }
                           s.Append(unicodeChar);
                           break;
                   }
                   break;
               case '"':
                   string result = s.ToString();
                   s.Clear();
                   return result;
               default:
                   s.Append((char)c);
                   break;

           }
       }
       throw new Exception();
   }
}
