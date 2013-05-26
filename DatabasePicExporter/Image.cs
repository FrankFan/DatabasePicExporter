using DatabasePicExporter.DPE.Entity;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DatabasePicExporter
{
    public static class Image
    {
        public static void DrawPhoto(string database, Table table, string dir, int counter)
        {
            string fontFamily = "SimSun";
            float fontSize = 1.5F;
            int vFontSize = 9;
            int imageWidth = 0;
            int imageHeight = 0;
            int rowHeight = 20;
            int borderWidth = 4;

            int nameWidth = 0;
            int typeWidth = 0;
            int descWidth = 0;

            Font titleFont = new Font(fontFamily, fontSize, FontStyle.Bold);
            Font vTitleFont = new Font(fontFamily, vFontSize, FontStyle.Bold);

            string number = string.Format("[{0}]", counter.ToString());
            int numberWidth = TextRenderer.MeasureText(number, vTitleFont).Width;
            int RecordeCount = 0;

            //

            RecordeCount = (int)SqlHelper.ExecuteScalar(Dbinfo.ConnString, System.Data.CommandType.Text, "select count(*) from " + table.TableName);
            //

            string title = string.Format("{0} ({1}) [记录行数:{2}] ", table.TableName, table.Description, RecordeCount);

            string fullTitle = number + title;

            int titleWidth = TextRenderer.MeasureText(fullTitle, vTitleFont).Width;

            // 计算图高
            imageHeight = rowHeight + rowHeight * table.Columns.Count + borderWidth * 2;

            // 计算列宽、图宽
            foreach (Column column in table.Columns)
            {
                Font font = new Font(fontFamily, vFontSize);
                int tmpNameWidth = TextRenderer.MeasureText(column.ColumnName, font).Width + borderWidth;
                int tmpTypeWidth = TextRenderer.MeasureText(column.Type, font).Width + borderWidth;
                int tmpDescWidth = TextRenderer.MeasureText(column.Description, font).Width + borderWidth;

                if (tmpNameWidth > nameWidth)
                    nameWidth = tmpNameWidth;
                if (tmpTypeWidth > typeWidth)
                    typeWidth = tmpTypeWidth;
                if (tmpDescWidth > descWidth)
                    descWidth = tmpDescWidth;
            }

            imageWidth = nameWidth + typeWidth + descWidth + borderWidth * 2;

            // 标题宽度较大
            if (titleWidth > imageWidth)
                imageWidth = titleWidth;

            Bitmap bitmap = new Bitmap(imageWidth, imageHeight);
            bitmap.SetResolution(600, 600);
            Graphics graphics = Graphics.FromImage(bitmap);

            // 填充背景
            graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, imageWidth, imageHeight);

            // 画竖线
            graphics.DrawLine(new Pen(Color.LightGray, 1), new Point(nameWidth, rowHeight + borderWidth), new Point(nameWidth, imageHeight));
            graphics.DrawLine(new Pen(Color.LightGray, 1), new Point(nameWidth + typeWidth, rowHeight + borderWidth), new Point(nameWidth + typeWidth, imageHeight));

            // 画横线
            for (int i = 0; i < table.Columns.Count; i++)
                graphics.DrawLine(new Pen(Color.LightGray, 1), new Point(0, rowHeight + borderWidth + rowHeight * i), new Point(imageWidth, rowHeight + borderWidth + rowHeight * i));

            // 画外边框
            graphics.DrawLine(new Pen(Color.Gray, borderWidth), new Point(0, 0), new Point(imageWidth, 0));
            graphics.DrawLine(new Pen(Color.Gray, borderWidth), new Point(0, imageHeight), new Point(imageWidth, imageHeight));
            graphics.DrawLine(new Pen(Color.Gray, borderWidth), new Point(0, 0), new Point(0, imageHeight));
            graphics.DrawLine(new Pen(Color.Gray, borderWidth), new Point(imageWidth, 0), new Point(imageWidth, imageHeight));

            // 画表名
            graphics.DrawString(number, titleFont, new SolidBrush(Color.Green), borderWidth, borderWidth);
            graphics.DrawString(title, titleFont, new SolidBrush(Color.Black), borderWidth + numberWidth, borderWidth);

            // 画字段
            for (int i = 0; i < table.Columns.Count; i++)
            {
                SolidBrush brush = new SolidBrush(Color.Black);
                Font font = new Font(fontFamily, fontSize);

                // 存在索引就将文字变成蓝色
                if (table.Columns[i].IndexName != "")
                    brush = new SolidBrush(Color.Blue);

                // 是标识就将文字加下划线
                if (table.Columns[i].IsIdentity)
                    font = new Font(fontFamily, fontSize, FontStyle.Underline);

                // 是主键就将文字颜色改为红色
                if (table.Columns[i].IsPrimaryKey)
                    brush = new SolidBrush(Color.Red);


                graphics.DrawString(table.Columns[i].ColumnName, font, brush, borderWidth, rowHeight + borderWidth * 2 + rowHeight * i);
                graphics.DrawString(table.Columns[i].Type, font, brush, nameWidth + borderWidth, rowHeight + borderWidth * 2 + rowHeight * i);
                graphics.DrawString(table.Columns[i].Description, font, brush, nameWidth + typeWidth + borderWidth, rowHeight + borderWidth * 2 + rowHeight * i);
            }

            string path = string.Format("{0}", dir);
            string fullPath = string.Format("{0}\\{1}.png", path, table.TableName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            bitmap.Save(fullPath);
        }
    }
}
