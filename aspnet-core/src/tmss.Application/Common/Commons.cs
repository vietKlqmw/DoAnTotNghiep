using FastMember;
using GemBox.Spreadsheet;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace tmss.Common
{
    public class Commons
    {
        public static string GetMessage(string code)
        {
            try
            {
                var sb = new StringBuilder();
                string path = path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot/Common/Message.xml");

                if (!(File.Exists(path)))
                {
                    return "";
                }

                var ds = new DataSet();
                ds.ReadXml(path);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        for (int j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
                        {
                            if (code.Equals(ds.Tables[0].Columns[j].ColumnName))
                            {
                                return ds.Tables[0].Rows[i][j].ToString();
                            }
                        }
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static void FillExcel<T>(List<T> data, ExcelWorksheet v_worksheet, int startRow, int startCol, string[] properties, string[] p_header)
        {
            IEnumerable<T> dataE = data.AsEnumerable();
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(dataE, properties))
            {
                table.Load(reader);
            }

            if (table.Rows.Count > 0)
            {
                v_worksheet.Cells.GetSubrange(v_worksheet.Cells[startRow, startCol].Name, v_worksheet.Cells[startRow + table.Rows.Count - 1, startCol + table.Columns.Count - 1].Name).Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), GemBox.Spreadsheet.LineStyle.Thin);

                InsertDataTableOptions ins = new InsertDataTableOptions(startRow, startCol);
                v_worksheet.InsertDataTable(table, ins);
            }

            for (int i = 0; i < p_header.Length; i++)
            {
                v_worksheet.Cells[0, i].Value = p_header[i];
            }

            SetBorder(v_worksheet, 0, 0, data.Count(), p_header.Count() - 1);
            SetColorHeader(v_worksheet, 0, 0, 0, p_header.Count() - 1);
        }

        public static void FillExcel2<T>(List<T> data, ExcelWorksheet v_worksheet, int startRow, int startCol, string[] properties, string[] p_header)
        {
            IEnumerable<T> dataE = data.AsEnumerable();
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(dataE, properties))
            {
                table.Load(reader);
            }

            if (table.Rows.Count > 0)
            {
                InsertDataTableOptions ins = new InsertDataTableOptions(startRow, startCol);
                v_worksheet.InsertDataTable(table, ins);
            }

            for (int i = 0; i < p_header.Length; i++)
            {
                v_worksheet.Cells[0, i].Value = p_header[i];
            }
        }

        public static void ExcelFormatDate(ExcelWorksheet v_worksheet, int columns)
        {
            v_worksheet.Columns[columns].Style.NumberFormat = "dd/MM/yyyy";
        }

        public static void ExcelFormatDateTime(ExcelWorksheet v_worksheet, int columns)
        {
            v_worksheet.Columns[columns].Style.NumberFormat = "dd/MM/yyyy hh:mm:ss";
        }

        public static void SetBorder(ExcelWorksheet v_worksheet, int p_start_rows, int p_start_columns, int p_end_rows, int p_end_columns)
        {
            v_worksheet.Cells.GetSubrange(v_worksheet.Cells[p_start_rows, p_start_columns].Name, v_worksheet.Cells[p_end_rows, p_end_columns].Name).Style.Borders.SetBorders(MultipleBorders.All, Color.Black, GemBox.Spreadsheet.LineStyle.Thin);

        }

        public static void SetColorHeader(ExcelWorksheet v_worksheet, int p_start_rows, int p_start_columns, int p_end_rows, int p_end_columns)
        {
            v_worksheet.Cells.GetSubrange(v_worksheet.Cells[p_start_rows, p_start_columns].Name, v_worksheet.Cells[p_end_rows, p_end_columns].Name).Style.FillPattern.SetPattern(FillPatternStyle.Solid, Color.LightBlue, Color.LightBlue);
        }

        public static void Merged(ExcelWorksheet v_worksheet, int p_start_rows, int p_start_columns, int p_end_rows, int p_end_columns)
        {
            v_worksheet.Cells.GetSubrange(v_worksheet.Cells[p_start_rows, p_start_columns].Name, v_worksheet.Cells[p_end_rows, p_end_columns].Name).Merged = true;
        }
        public static string SetAutoFit(string p_path, int p_num_column)
        {
            XSSFWorkbook xSSFWorkbook = null;
            using (FileStream file = new FileStream(p_path, FileMode.Open, FileAccess.ReadWrite))
            {
                xSSFWorkbook = new XSSFWorkbook(file);
                ISheet sheet = xSSFWorkbook.GetSheetAt(0);
                for (int i = 0; i < p_num_column; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
            }
            var tempfile2 = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            using (FileStream file = new FileStream(tempfile2, FileMode.Create, FileAccess.ReadWrite))
            {
                xSSFWorkbook.Write(file);
                file.Close();
            }

            return tempfile2;
        }

        public static string getConnectionString()
        {
            var appsettingsjson = JObject.Parse(File.ReadAllText("appsettings.json"));
            var connectionStrings = (JObject)appsettingsjson["ConnectionStrings"];
            return connectionStrings.Property("Default").Value.ToString();
        }

        //public static SessionOptions SftpConnect()
        //{
        //    SessionOptions sessionOptions = new SessionOptions
        //    {
        //        Protocol = Protocol.Sftp,
        //        HostName = "192.168.2.162",
        //        UserName = "Sysadmin",
        //        Password = "Nov@2022@6869",
        //        SshHostKeyFingerprint = "ssh-ed25519 255 Ecl1tDChtWi8VIBE0UjNxiLlnXOargOHpIBYBK1urUc"
        //    };

        //    return sessionOptions;
        //}

        //public static void GetFile(SessionOptions sessionOptions, string p_from_path, string p_to_path)
        //{
        //    p_to_path = @"Test";
        //    using (Session session = new Session())
        //    {
        //        TransferOptions transferOptions = new TransferOptions
        //        {
        //            TransferMode = TransferMode.Binary,
        //            OverwriteMode = OverwriteMode.Overwrite
        //        };
        //        session.Open(sessionOptions);
        //        session.PutFileToDirectory(@"D:\CheckWMI.xlsx",p_to_path,  false, transferOptions);
        //    }
        //}
    }

    public class CommonXml
    {
        public static string SerialSapXml<T>(T dto, XmlQualifiedName ns = null, string dateFormat = "yyyyMMdd", IDictionary<string, string> replaceKeyValuePairs = null)
        {
            var xml = string.Empty;
            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    writer.WriteStartDocument();
                    var t = typeof(T);
                    var tAttr = (DisplayAttribute)t.GetCustomAttribute(typeof(DisplayAttribute));
                    ConvertToSapFormat(writer, t, tAttr?.Name ?? t.Name, null, dto, dateFormat, ns);
                    writer.WriteEndDocument();
                    writer.Flush();
                    //
                    xml = sww.ToString();
                    xml = xml.Replace("utf-16", "UTF-8", StringComparison.CurrentCultureIgnoreCase);
                    if (replaceKeyValuePairs != null && replaceKeyValuePairs.Count > 0)
                    {
                        foreach (var pair in replaceKeyValuePairs)
                        {
                            xml = xml.Replace(pair.Key, pair.Value, StringComparison.CurrentCultureIgnoreCase);
                        }
                    }
                }
            }
            return xml;
        }

        private static void ConvertToSapFormat(XmlWriter writer, Type t, string tagName, string tagGroupName, object dto, string dateFormat, XmlQualifiedName ns = null)
        {
            if (IsSimpleType(t))
            {
                writer.WriteStartElement(tagName);
                writer.WriteString(ConvertToStringValue(dto, dateFormat));
                writer.WriteEndElement();
            }
            else
            {
                if (typeof(IEnumerable).IsAssignableFrom(t) && t != typeof(string))
                {
                    var objT = t.GetGenericArguments()?.Length > 0 ? t.GetGenericArguments()[0] : t.GetElementType();
                    if (!IsSimpleType(objT) && !(typeof(IEnumerable).IsAssignableFrom(objT) && objT != typeof(string)))
                    {
                        if (dto != null)
                        {
                            if (!string.IsNullOrWhiteSpace(tagName))
                            {
                                writer.WriteStartElement(ns?.Name, tagName, ns?.Namespace);
                            }
                            //
                            foreach (var obj in (dto as IEnumerable))
                            {
                                ConvertToSapFormat(writer, objT, tagGroupName ?? objT.Name, tagGroupName, obj, dateFormat);
                            }
                            //
                            if (!string.IsNullOrWhiteSpace(tagName))
                            {
                                writer.WriteEndElement();
                            }
                        }
                    }
                }
                else
                {
                    writer.WriteStartElement(ns?.Name, tagName, ns?.Namespace);
                    foreach (var p in t.GetProperties())
                    {
                        var pT = p.PropertyType;
                        var pAttr = (DisplayAttribute)p.GetCustomAttribute(typeof(DisplayAttribute));
                        bool pAutoGenerateField = true;
                        try { pAutoGenerateField = pAttr == null || pAttr.AutoGenerateField; } catch { }
                        if (pAutoGenerateField)
                        {
                            var pTagName = string.Empty;
                            if (!IsSimpleType(pT) && typeof(IEnumerable).IsAssignableFrom(pT) && pT != typeof(string)) { pTagName = pAttr?.Name; }
                            else { pTagName = pAttr?.Name ?? p.Name; }
                            //
                            ConvertToSapFormat(writer, p.PropertyType, pTagName, pAttr?.GroupName, p.GetValue(dto), dateFormat);
                        }
                    }
                    writer.WriteEndElement();
                }
            }
        }

        private static bool IsSimpleType(Type type)
        {
            var otherSimpleTypes = new Type[] {
                typeof(string),
                typeof(decimal),
                typeof(DateTime),
                typeof(DateTimeOffset),
                typeof(TimeSpan),
                typeof(Guid)
            };
            //
            return
                type.IsPrimitive ||
                otherSimpleTypes.Contains(type) ||
                type.IsEnum ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]));
        }

        private static string ConvertToStringValue(object value, string dateFormat)
        {
            string strValue = null;
            if (value != null)
            {
                var t = value.GetType();
                if (t == typeof(DateTime))
                {
                    strValue = ((DateTime)value).ToString(dateFormat);
                }
                else if (t == typeof(DateTimeOffset))
                {
                    strValue = ((DateTimeOffset)value).ToString(dateFormat);
                }
                else if (t == typeof(TimeSpan))
                {
                    strValue = ((TimeSpan)value).ToString(dateFormat);
                }
                else
                {
                    if (IsNumber(t))
                    {
                        var dotNumberFormat = (NumberFormatInfo)CultureInfo.InstalledUICulture.NumberFormat.Clone();
                        dotNumberFormat.NumberDecimalSeparator = ".";
                        strValue = string.Format(dotNumberFormat, "{0:0.##}", value);
                    }
                    else
                    {
                        strValue = value.ToString();
                    }
                }
            }
            return strValue;
        }

        private static bool IsNumber(Type type)
        {
            var numberTypes = new Type[] {
                typeof(sbyte),
                typeof(byte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(sbyte?),
                typeof(byte?),
                typeof(short?),
                typeof(ushort?),
                typeof(int?),
                typeof(uint?),
                typeof(long?),
                typeof(ulong?),
                typeof(float?),
                typeof(double?),
                typeof(decimal?)
            };
            return numberTypes.Contains(type);
        }

        public static T DeserialSapXml<T>(string xml, string dateFormat = "yyyyMMdd")
        {
            var t = typeof(T);
            var tAttr = (DisplayAttribute)t.GetCustomAttribute(typeof(DisplayAttribute));
            var dto = (T)Activator.CreateInstance(t);
            if (!string.IsNullOrWhiteSpace(xml))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                var root = xmlDoc.DocumentElement;
                if (root.LocalName == (tAttr?.Name ?? t.Name))
                {
                    var xpath = string.Empty;
                    foreach (var p in t.GetProperties())
                    {
                        ConvertFromSapFormat(root, p, dto, xpath, dateFormat);
                    }
                }
            }
            return dto;
        }

        private static void ConvertFromSapFormat(XmlElement root, PropertyInfo p, object dto, string xpath, string dateFormat)
        {
            if (dto != null)
            {
                var t = p.PropertyType;
                var pAttr = (DisplayAttribute)p.GetCustomAttribute(typeof(DisplayAttribute));
                bool pAutoGenerateField = true;
                try { pAutoGenerateField = pAttr == null || pAttr.AutoGenerateField; } catch { }
                if (pAutoGenerateField)
                {
                    var curXpath = string.Concat(xpath, (string.IsNullOrEmpty(xpath) ? "" : "/"), pAttr?.Name ?? p.Name);
                    if (IsSimpleType(t))
                    {
                        var value = ConvertFromStringValue(t, root.SelectSingleNode(curXpath)?.InnerText, dateFormat);
                        p.SetValue(dto, value);
                    }
                    else
                    {
                        if (typeof(IEnumerable).IsAssignableFrom(t) && t != typeof(string))
                        {
                            var objT = t.GetGenericArguments()?.Length > 0 ? t.GetGenericArguments()[0] : t.GetElementType();
                            if (!IsSimpleType(objT) && !(typeof(IEnumerable).IsAssignableFrom(objT) && objT != typeof(string)))
                            {
                                if (!string.IsNullOrWhiteSpace(pAttr?.Name))
                                {
                                    curXpath = string.Concat(xpath, (string.IsNullOrEmpty(xpath) ? "" : "/"), pAttr?.Name ?? p.Name, "/", pAttr?.GroupName ?? objT.Name);
                                }
                                else
                                {
                                    curXpath = string.Concat(xpath, (string.IsNullOrEmpty(xpath) ? "" : "/"), pAttr?.GroupName ?? objT.Name);
                                }
                                var nodes = root.SelectNodes(curXpath);
                                if (nodes?.Count > 0)
                                {
                                    if (t.IsArray)
                                    {
                                        var arr = (Array)Activator.CreateInstance(t, new object[] { nodes.Count });
                                        p.SetValue(dto, arr);
                                        var i = 0;
                                        foreach (var node in nodes)
                                        {
                                            var obj = Activator.CreateInstance(objT);
                                            arr.SetValue(obj, i);
                                            foreach (var sp in objT.GetProperties())
                                            {
                                                ConvertFromSapFormat(root, sp, obj, string.Concat(curXpath, "[", i + 1, "]"), dateFormat);
                                            }
                                            i++;
                                        }
                                    }
                                    else
                                    {
                                        var list = (IList)Activator.CreateInstance(t);
                                        p.SetValue(dto, list);
                                        var i = 0;
                                        foreach (var node in nodes)
                                        {
                                            var obj = Activator.CreateInstance(objT);
                                            list.Add(obj);
                                            foreach (var sp in objT.GetProperties())
                                            {
                                                ConvertFromSapFormat(root, sp, obj, string.Concat(curXpath, "[", i + 1, "]"), dateFormat);
                                            }
                                            i++;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (root.SelectSingleNode(curXpath) != null)
                            {
                                var obj = Activator.CreateInstance(t);
                                p.SetValue(dto, obj);
                                foreach (var sp in t.GetProperties())
                                {
                                    ConvertFromSapFormat(root, sp, obj, curXpath, dateFormat);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static object ConvertFromStringValue(Type t, string value, string dateFormat)
        {
            object obj = value;
            if (obj != null)
            {
                if (t == typeof(DateTime))
                {
                    obj = DateTime.ParseExact(value, dateFormat, CultureInfo.CreateSpecificCulture("vi-VN"));
                }
                else if (t == typeof(DateTimeOffset))
                {
                    obj = DateTime.ParseExact(value, dateFormat, CultureInfo.CreateSpecificCulture("vi-VN"));
                }
                else if (t == typeof(TimeSpan))
                {
                    obj = DateTime.ParseExact(value, dateFormat, CultureInfo.CreateSpecificCulture("vi-VN"));
                }
                else if (IsNumber(t))
                {
                    var dotNumberFormat = (NumberFormatInfo)CultureInfo.InstalledUICulture.NumberFormat.Clone();
                    dotNumberFormat.NumberDecimalSeparator = ".";
                    //                
                    if (t == typeof(sbyte))
                    {
                        obj = sbyte.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(byte))
                    {
                        obj = byte.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(short))
                    {
                        obj = short.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(ushort))
                    {
                        obj = ushort.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(int))
                    {
                        obj = int.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(uint))
                    {
                        obj = uint.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(long))
                    {
                        obj = long.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(ulong))
                    {
                        obj = ulong.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(float))
                    {
                        obj = float.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(double))
                    {
                        obj = double.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(decimal))
                    {
                        obj = decimal.Parse(value, dotNumberFormat);
                    }
                }
            }
            return obj;
        }
    }
}
