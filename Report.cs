using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using TelephoneSpravochnik;
using Word = Microsoft.Office.Interop.Word;

namespace BuildPCServrice
{
    class Report
    {
        Word.Application app = new Word.Application();
        Word.Document doc;

        ~Report()
        {
            doc.Saved = true;
            try { app.Quit(); }
            catch { }
        }

        public void Phone_category(IList<Phone_category> phone_Categories)
        {
            if (phone_Categories != null)
            {
                doc = app.Documents.Add(Template: $@"{Environment.CurrentDirectory}\Templates\Категории телефона.docx", Visible: true);

                Word.Range dateTime = doc.Bookmarks["DateTime"].Range;
                dateTime.Text = DateTime.Now.ToString();

                Word.Table table = doc.Bookmarks["Table"].Range.Tables[1];
                int currPage = 1;
                foreach (var item in phone_Categories)
                {
                    int page = doc.ComputeStatistics(Word.WdStatistic.wdStatisticPages);

                    Word.Row row = table.Rows.Add();
                    if (page > currPage) //Если запись не влазеет на текущею страницу
                    {
                        row.Range.InsertBreak();
                        table = doc.Tables[doc.Tables.Count];

                        doc.Tables[1].Rows[1].Range.Copy();
                        row.Range.Paste();
                        table.Rows[2].Delete(); //Удаляем пустую строку после заголовка

                        currPage = page;
                        row = table.Rows.Add();
                    }

                    row.Cells[1].Range.Text = item.Name;

                }
                doc.Bookmarks["Table"].Range.Tables[1].Rows[2].Delete(); //Удаляем строку [текст] [текст] [текст] [текст] в таблице

                app.Visible = true;
            }
        }


        public void District(IList<District> districts)
        {
            if (districts != null)
            {
                doc = app.Documents.Add(Template: $@"{Environment.CurrentDirectory}\Templates\Районы.docx", Visible: true);

                Word.Range dateTime = doc.Bookmarks["DateTime"].Range;
                dateTime.Text = DateTime.Now.ToString();

                Word.Table table = doc.Bookmarks["Table"].Range.Tables[1];
                int currPage = 1;
                foreach (var item in districts)
                {
                    int page = doc.ComputeStatistics(Word.WdStatistic.wdStatisticPages);

                    Word.Row row = table.Rows.Add();
                    if (page > currPage) //Если запись не влазеет на текущею страницу
                    {
                        row.Range.InsertBreak();
                        table = doc.Tables[doc.Tables.Count];

                        doc.Tables[1].Rows[1].Range.Copy();
                        row.Range.Paste();
                        table.Rows[2].Delete(); //Удаляем пустую строку после заголовка

                        currPage = page;
                        row = table.Rows.Add();
                    }

                    row.Cells[1].Range.Text = item.Name;
                }
                doc.Bookmarks["Table"].Range.Tables[1].Rows[2].Delete(); //Удаляем строку [текст] [текст] [текст] [текст] в таблице

                app.Visible = true;
            }
        }

        public void Lgotnaya_category(IList<Lgotnaya_category> lgotnaya_Categories)
        {
            if (lgotnaya_Categories != null)
            {
                doc = app.Documents.Add(Template: $@"{Environment.CurrentDirectory}\Templates\Льготные категории.docx", Visible: true);

                Word.Range dateTime = doc.Bookmarks["DateTime"].Range;
                dateTime.Text = DateTime.Now.ToString();

                Word.Table table = doc.Bookmarks["Table"].Range.Tables[1];
                int currPage = 1;
                foreach (var item in lgotnaya_Categories)
                {
                    int page = doc.ComputeStatistics(Word.WdStatistic.wdStatisticPages);

                    Word.Row row = table.Rows.Add();
                    if (page > currPage) //Если запись не влазеет на текущею страницу
                    {
                        row.Range.InsertBreak();
                        table = doc.Tables[doc.Tables.Count];

                        doc.Tables[1].Rows[1].Range.Copy();
                        row.Range.Paste();
                        table.Rows[2].Delete(); //Удаляем пустую строку после заголовка

                        currPage = page;
                        row = table.Rows.Add();
                    }

                    row.Cells[1].Range.Text = item.Name;

                }
                doc.Bookmarks["Table"].Range.Tables[1].Rows[2].Delete(); //Удаляем строку [текст] [текст] [текст] [текст] в таблице

                app.Visible = true;
            }
        }

        public void Abonents(IList<Abonent> abonents)
        {
            if (abonents != null)
            {
                doc = app.Documents.Add(Template: $@"{Environment.CurrentDirectory}\Templates\Абоненты.docx", Visible: true);

                Word.Range dateTime = doc.Bookmarks["DateTime"].Range;
                dateTime.Text = DateTime.Now.ToString();

                Word.Table table = doc.Bookmarks["Table"].Range.Tables[1];
                int currPage = 1;
                foreach (var item in abonents)
                {
                    int page = doc.ComputeStatistics(Word.WdStatistic.wdStatisticPages);

                    Word.Row row = table.Rows.Add();
                    if (page > currPage) //Если запись не влазеет на текущею страницу
                    {
                        row.Range.InsertBreak();
                        table = doc.Tables[doc.Tables.Count];

                        doc.Tables[1].Rows[1].Range.Copy();
                        row.Range.Paste();
                        table.Rows[2].Delete(); //Удаляем пустую строку после заголовка

                        currPage = page;
                        row = table.Rows.Add();
                    }

                    row.Cells[1].Range.Text = item.FIO;
                    row.Cells[2].Range.Text = item.Phone_Number;
                    row.Cells[3].Range.Text = Convert.ToString(item.Date);
                    row.Cells[4].Range.Text = item.Adress;
                    row.Cells[5].Range.Text = item.Districts.Name;
                    row.Cells[6].Range.Text = item.Phone_category.Name;
                    row.Cells[7].Range.Text = item.Lgotnaya_category.Name;
                }
                doc.Bookmarks["Table"].Range.Tables[1].Rows[2].Delete(); //Удаляем строку [текст] [текст] [текст] [текст] в таблице

                app.Visible = true;
            }
        }
    }
}