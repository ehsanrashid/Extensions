namespace System.Text
{
    public class StringBuilderWrapper
    {
        public StringBuilder StringBuilder { get; private set; }

        public StringBuilderWrapper(StringBuilder sb)
        {
            StringBuilder = sb;
        }

        public static implicit operator StringBuilderWrapper(StringBuilder sb)
        {
            return new StringBuilderWrapper(sb);
        }

        public static StringBuilderWrapper operator +(StringBuilderWrapper sbw, String s)
        {
            sbw.StringBuilder.Append(s);
            return sbw;
        }

        public static StringBuilderWrapper operator -(StringBuilderWrapper sbw, String s)
        {
            //sbw.StringBuilder.Remove(s);
            return sbw;
        }

    }
}
