namespace System.Data
{
    using OleDb;
    using IO;

    public static class DataSetExtension
    {
        public static DataSet ImportExcel(this DataSet dataSet, String filename, bool hasHeaders = true)
        {
            if (filename.IsNotNullOrWhiteSpace()) return default(DataSet);
            var connectionString = String.Empty;
            var HDR = hasHeaders ? "Yes" : "No";
            var extension = Path.GetExtension(filename);
            if (extension.IsNotNullOrWhiteSpace())
                switch (extension.Substring(1))
                {
                    case "xls": //Excel 97-03
                        connectionString = String.Format(
                            @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1};IMEX=1'",
                            filename, HDR);
                        break;
                    case "xlsx": //Excel 07
                        connectionString = String.Format(
                            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1};IMEX=1'",
                            filename, HDR);
                        break;
                }

            dataSet = new DataSet(Path.GetFileNameWithoutExtension(filename));
            using (var conOleDb = new OleDbConnection(connectionString))
            {
                conOleDb.Open();
                //{TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE}
                var arrRestrict = new Object[] {null, null, null, "TABLE"};
                var dtSchema = conOleDb.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, arrRestrict);
                if (null != dtSchema)
                {
                    foreach (DataRow drSchema in dtSchema.Rows)
                    {
                        var nameSheet =
                            //drSchema[dtSchema.Columns[2].ColumnName].ToString();
                            drSchema["TABLE_NAME"].ToString();
                        var selectQuery = String.Format("SELECT * FROM [{0}]", nameSheet);
                        var table = new DataTable(nameSheet);
                        var cmdOleDb = new OleDbCommand(selectQuery, conOleDb) { CommandType = CommandType.Text };
                        new OleDbDataAdapter(cmdOleDb).Fill(table);
                        dataSet.Tables.Add(table);
                    }
                }
                conOleDb.Close();
            }
            RemoveNull(dataSet);
            return dataSet;
        }

        public static DataSet ImportXML(this DataSet dataSet, String filename)
        {
            if (filename.IsNullOrWhiteSpace()) return default(DataSet);
            dataSet = new DataSet(Path.GetFileNameWithoutExtension(filename));
            //dataSet.ReadXml(filename);
            dataSet.ReadXmlSchema(filename);
            foreach (DataTable table in dataSet.Tables)
                table.BeginLoadData();
            dataSet.ReadXml(filename);
            foreach (DataTable table in dataSet.Tables)
                table.EndLoadData();
            //-----------------------------------------------------
            //dataSet.ReadXml(XmlReader.Create(new StringReader(xmlDocument.InnerXml)));
            return dataSet;
        }

        public static void RemoveNull(this DataSet dataSet)
        {
            for (var t = 0; t < dataSet.Tables.Count; ++t)
                for (var r = 0; r < dataSet.Tables[t].Rows.Count; ++r)
                    if (dataSet.Tables[t].Rows[r].IsNull(0))
                        dataSet.Tables[t].Rows[r].Delete();

            dataSet.AcceptChanges();
        }

        // ---

        //public static void PrintDataSet(this DataSet set)
        //{
        //    foreach (DataTable table in set.Tables)
        //    {
        //        Console.WriteLine(table.TableName);
        //        //ConstraintCollection c= table.Constraints;
        //        foreach (DataRelation relation in table.ChildRelations)
        //        {
        //            Console.WriteLine(">" + relation.RelationName);
        //            foreach (DataRow row in table.Rows)
        //            {
        //                PrintRowValues("Parent Row", row);
        //                var rowsChild = row.GetChildRows(relation);
        //                //var rowsChild = row.GetChildRows(relation).Take(20);
        //                Console.WriteLine(rowsChild.Length);
        //                // Print values of rows.
        //                PrintRowValues("child rows", rowsChild);
        //            }
        //            Console.WriteLine("-");
        //        }
        //        Console.WriteLine("---");
        //    }
        //}

        //static void PrintRowValues(String label, params DataRow[] rows)
        //{
        //    Console.WriteLine("\n{0}", label);
        //    if (rows.Length <= 0)
        //    {
        //        Console.WriteLine("no rows found");
        //        return;
        //    }
        //    foreach (var row in rows)
        //    {
        //        foreach (DataColumn column in row.Table.Columns)
        //        {
        //            var value = row[column].ToString().Chop(20, "...");
        //            Console.Write("\t{0, 6}", value);
        //            //Console.Write("\t{0, -6}", value);
        //        }
        //        Console.WriteLine();
        //    }
        //}


        //public static DataSet GetExcel(String fileName)
        //{
        //    Application oXL;
        //    Workbook oWB;
        //    Worksheet oSheet;
        //    Range oRng;
        //    try
        //    {

        //        //  creat a Application object
        //        oXL = new ApplicationClass();
        //        //   get   WorkBook  object
        //        oWB = oXL.Workbooks.Open(fileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
        //                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
        //                Missing.Value, Missing.Value);
        //        //   get   WorkSheet object 
        //        oSheet = (Worksheet)oWB.Sheets[1];
        //        System.Data.DataTable dt = new System.Data.DataTable("dtExcel");
        //        DataSet ds = new DataSet();
        //        ds.Tables.Add(dt);
        //        DataRow dr;
        //        StringBuilder sb = new StringBuilder();
        //        int jValue = oSheet.UsedRange.Cells.Columns.Count;
        //        int iValue = oSheet.UsedRange.Cells.Rows.Count;
        //        //  get data columns
        //        for (int j = 1; j <= jValue; j++)
        //        {
        //            dt.Columns.Add("column" + j, System.Type.GetType("System.String"));
        //        }
        //        //String colString = sb.ToString().Trim();
        //        //String[] colArray = colString.Split(':');
        //        //  get data in cell
        //        for (int i = 1; i <= iValue; i++)
        //        {
        //            dr = ds.Tables["dtExcel"].NewRow();
        //            for (int j = 1; j <= jValue; j++)
        //            {
        //                oRng = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[i, j];
        //                String strValue = oRng.Text.ToString();
        //                dr["column" + j] = strValue;
        //            }
        //            ds.Tables["dtExcel"].Rows.Add(dr);
        //        }
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Label1.Text = "Error: ";
        //        //Label1.Text += ex.Message.ToString();
        //        return null;
        //    }
        //}

    }
}