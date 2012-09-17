namespace System.Windows.Forms
{
    public static class RichTextBoxExtensions
    {
        /// <summary>
        /// Adds basic coloured syntax to the text in a RichTextBox
        /// </summary>
        public static void AddColouredText(this RichTextBox richTextBox, string strTextToAdd)
        {
            //Use the RichTextBox to create the initial RTF code
            richTextBox.Clear();
            richTextBox.AppendText(strTextToAdd);
            var strRtf = richTextBox.Rtf;
            richTextBox.Clear();

            /* 
             * ADD COLOUR TABLE TO THE HEADER FIRST 
             * */

            // Search for colour table info, if it exists (which it shouldn't)
            // remove it and replace with our one
            int iCTableStart = strRtf.IndexOf("colortbl;", StringComparison.Ordinal);

            if (iCTableStart != -1) //then colortbl exists
            {
                //find end of colortbl tab by searching
                //forward from the colortbl tab itself
                int iCTableEnd = strRtf.IndexOf('}', iCTableStart);
                strRtf = strRtf.Remove(iCTableStart, iCTableEnd - iCTableStart);

                //now insert new colour table at index of old colortbl tag
                strRtf = strRtf.Insert(iCTableStart,
                    // CHANGE THIS STRING TO ALTER COLOUR TABLE
                    "colortbl ;\\red255\\green0\\blue0;\\red0\\green128\\blue0;\\red0\\green0\\blue255;}");
            }

            //colour table doesn't exist yet, so let's make one
            else
            {
                // find index of start of header
                var iRtfLoc = strRtf.IndexOf("\\rtf", StringComparison.Ordinal);
                // get index of where we'll insert the colour table
                // try finding opening bracket of first property of header first                
                var iInsertLoc = strRtf.IndexOf('{', iRtfLoc);

                // if there is no property, we'll insert colour table
                // just before the end bracket of the header
                if (iInsertLoc == -1) iInsertLoc = strRtf.IndexOf('}', iRtfLoc) - 1;

                // insert the colour table at our chosen location                
                strRtf = strRtf.Insert(iInsertLoc,
                    // CHANGE THIS STRING TO ALTER COLOUR TABLE
                    "{\\colortbl ;\\red128\\green0\\blue0;\\red0\\green128\\blue0;\\red0\\green0\\blue255;}");
            }

            /*
             * NOW PARSE THROUGH RTF DATA, ADDING RTF COLOUR TAGS WHERE WE WANT THEM
             * In our colour table we defined:
             * cf1 = red  
             * cf2 = green
             * cf3 = blue             
             * */

            for (var i = 0; i < strRtf.Length; i++)
            {
                switch (strRtf[i])
                {
                    case '<':
                        strRtf = (strRtf[i + 1] == '!')
                                     ? strRtf.Insert(i + 4, "\\cf2 ")
                                     : strRtf.Insert(i + 1, "\\cf1 ");

                        strRtf = strRtf.Insert(i, "\\cf3 ");
                        i += 6;
                        break;

                    case '>':
                        strRtf = strRtf.Insert(i + 1, "\\cf0 ");

                        switch (strRtf[i - 1])
                        {
                            case '-':
                                strRtf = strRtf.Insert(i - 2, "\\cf3 ");
                                i += 8;
                                break;
                            default:
                                strRtf = strRtf.Insert(i, "\\cf3 ");
                                i += 6;
                                break;
                        }
                        break;
                }
            }
            richTextBox.Rtf = strRtf;
        }
    }
}
