using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Windows.Shared;
using Syncfusion.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Paws.Services
{
    public class GetPDFReport
    {
        private BaseFont baseFont;
        private Font font;
        public GetPDFReport()
        {
            string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
            baseFont = baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
        }

        public string GetFullReport()
        {
            try
            {
                iTextSharp.text.Document doc = CreatePDFDocument();

                doc.Open();

                using (var context = new ApplicationContext())
                {
                    PdfPTable table = new PdfPTable(8);
                    table.AddCell(new PdfPCell(new Phrase("ID", font)));
                    table.AddCell(new PdfPCell(new Phrase("ПІБ клієнта", font)));
                    table.AddCell(new PdfPCell(new Phrase("Товар", font)));
                    table.AddCell(new PdfPCell(new Phrase("Дата замовлення", font)));
                    table.AddCell(new PdfPCell(new Phrase("Сума", font)));
                    table.AddCell(new PdfPCell(new Phrase("Адреса доставки", font)));
                    table.AddCell(new PdfPCell(new Phrase("Спосіб оплати", font)));
                    table.AddCell(new PdfPCell(new Phrase("Коментар до замовлення", font)));
                    foreach (var item in context.Orders.Include(u => u.Goods).Include(u => u.Customer).Include(u => u.Employee).ToList())
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Id.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.CustomerName, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.GoodsNames, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.OrderDateTime.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Amount.ToString() + "₴", font)));
                        table.AddCell(new PdfPCell(new Phrase(item.DeliveryAdress, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.PaymentMethod.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Comment, font)));

                    }

                    doc.Add(new Phrase("Звіт \"Замовлення в базі даних\"\n", font));
                    doc.Add(new Phrase("Нижче надано список замовлень внесених до бази даних системи управління Зоомагазином PawsomePets", font));
                    doc.Add(table);
                    doc.Add(new Phrase($"Всього замовлень: {context.Goods.Count()}", font));

                    doc.Add(Chunk.NEXTPAGE);

                    table = new PdfPTable(6);
                    table.AddCell(new PdfPCell(new Phrase("ID", font)));
                    table.AddCell(new PdfPCell(new Phrase("Назва", font)));
                    table.AddCell(new PdfPCell(new Phrase("Категорія", font)));
                    table.AddCell(new PdfPCell(new Phrase("Ціна", font)));
                    table.AddCell(new PdfPCell(new Phrase("Кількість", font)));
                    table.AddCell(new PdfPCell(new Phrase("Наявність На складі", font)));
                    foreach (var item in context.Goods.ToList())
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Id.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.ProductName, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.ProductCategory, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Price.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.AvailabilityInStock.ToString(), font)));


                    }

                    doc.Add(new Phrase("Звіт \"Товари в базі даних\"\n", font));
                    doc.Add(new Phrase("Нижче надано список товарів внесених до бази даних системи управління Зоомагазином PawsomePets", font));
                    doc.Add(table);
                    doc.Add(new Phrase($"Всього товарів: {context.Goods.Count()}", font));

                    doc.Add(Chunk.NEXTPAGE);

                    table = new PdfPTable(6);
                    table.AddCell(new PdfPCell(new Phrase("ID", font)));
                    table.AddCell(new PdfPCell(new Phrase("ПІБ співробітника", font)));
                    table.AddCell(new PdfPCell(new Phrase("Посада", font)));
                    table.AddCell(new PdfPCell(new Phrase("Зарплата", font)));
                    table.AddCell(new PdfPCell(new Phrase("Телефон", font)));
                    table.AddCell(new PdfPCell(new Phrase("Email", font)));
                    foreach (var item in context.Employees.ToList())
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Id.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Fullname, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Position.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Salary.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Phone, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Email, font)));
                    }

                    doc.Add(new Phrase("Звіт \"Співробітники PawsomePets\"\n", font));
                    doc.Add(new Phrase("Нижче надано список співробітників компанії PawsomePets, внесених в базу даних системи управління", font));
                    doc.Add(table);
                    doc.Add(new Phrase($"Всього співробітників: {context.Employees.Count()}", font));

                    doc.Add(Chunk.NEXTPAGE);

                    table = new PdfPTable(6);
                    table.AddCell(new PdfPCell(new Phrase("ID", font)));
                    table.AddCell(new PdfPCell(new Phrase("ПІБ клієнта", font)));
                    table.AddCell(new PdfPCell(new Phrase("Адрес", font)));
                    table.AddCell(new PdfPCell(new Phrase("Телефон", font)));
                    table.AddCell(new PdfPCell(new Phrase("Email", font)));
                    table.AddCell(new PdfPCell(new Phrase("Дисконтна карта", font)));
                    foreach (var item in context.Customers.ToList())
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Id.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Fullname, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Adress, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Phone, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Email, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.DiscountCard, font)));
                    }

                    doc.Add(new Phrase("Звіт \"Клієнти компанії PawsomePets\"\n", font));
                    doc.Add(new Phrase("Нижче надано список клієнтів компанії PawsomePets, внесених в базу даних системи управління", font));
                    doc.Add(table);
                    doc.Add(new Phrase($"Всього клієнтів: {context.Customers.Count()}", font));
                }
                doc.Close();
                return "Повний звіт успішно збережений!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetOrdersPDF()
        {
            try
            {
                iTextSharp.text.Document doc = CreatePDFDocument();

                doc.Open();

                using (var context = new ApplicationContext())
                {

                    PdfPTable table = new PdfPTable(8);
                    table.AddCell(new PdfPCell(new Phrase("ID", font)));
                    table.AddCell(new PdfPCell(new Phrase("ПІБ клієнта", font)));
                    table.AddCell(new PdfPCell(new Phrase("Товар", font)));
                    table.AddCell(new PdfPCell(new Phrase("Дата замовлення", font)));
                    table.AddCell(new PdfPCell(new Phrase("Сума", font)));
                    table.AddCell(new PdfPCell(new Phrase("Адреса доставки", font)));
                    table.AddCell(new PdfPCell(new Phrase("Спосіб оплати", font)));
                    table.AddCell(new PdfPCell(new Phrase("Коментар до замовлення", font)));
                    foreach (var item in context.Orders.Include(u => u.Goods).Include(u => u.Customer).Include(u => u.Employee).ToList())
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Id.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.CustomerName, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.GoodsNames, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.OrderDateTime.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Amount.ToString() + "₴", font)));
                        table.AddCell(new PdfPCell(new Phrase(item.DeliveryAdress, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.PaymentMethod.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Comment, font)));

                    }

                    doc.Add(new Phrase("Звіт \"Замовлення в базі даних\"\n", font));
                    doc.Add(new Phrase("Нижче надано список замовлень внесених до бази даних системи управління Зоомагазином PawsomePets", font));
                    doc.Add(table);
                    doc.Add(new Phrase($"Всього замовлень: {context.Goods.Count()}", font));
                }
                doc.Close();
                return "Звіт по замовленнях успішно збережений!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string GetGoodsPDF()
        {
            try
            {
                iTextSharp.text.Document doc = CreatePDFDocument();

                doc.Open();

                using (var context = new ApplicationContext())
                {

                    PdfPTable table = new PdfPTable(6);
                    table.AddCell(new PdfPCell(new Phrase("ID", font)));
                    table.AddCell(new PdfPCell(new Phrase("Назва", font)));
                    table.AddCell(new PdfPCell(new Phrase("Категорія", font)));
                    table.AddCell(new PdfPCell(new Phrase("Ціна", font)));
                    table.AddCell(new PdfPCell(new Phrase("Кількість", font)));
                    table.AddCell(new PdfPCell(new Phrase("Наявність На складі", font)));
                    foreach (var item in context.Goods)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Id.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.ProductName, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.ProductCategory, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Price.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.AvailabilityInStock.ToString(), font)));


                    }

                    doc.Add(new Phrase("Звіт \"Товари в базі даних\"\n", font));
                    doc.Add(new Phrase("Нижче надано список товарів внесених до бази даних системи управління Зоомагазином PawsomePets", font));
                    doc.Add(table);
                    doc.Add(new Phrase($"Всього товарів: {context.Goods.Count()}", font));
                }
                doc.Close();
                return "Звіт по товарах успішно збережено!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetEmployeesPDF()
        {
            try
            {
                iTextSharp.text.Document doc = CreatePDFDocument();

                doc.Open();

                using (var context = new ApplicationContext())
                {

                    PdfPTable table = new PdfPTable(6);
                    table.AddCell(new PdfPCell(new Phrase("ID", font)));
                    table.AddCell(new PdfPCell(new Phrase("ПІБ співробітника", font)));
                    table.AddCell(new PdfPCell(new Phrase("Посада", font)));
                    table.AddCell(new PdfPCell(new Phrase("Зарплата", font)));
                    table.AddCell(new PdfPCell(new Phrase("Телефон", font)));
                    table.AddCell(new PdfPCell(new Phrase("Email", font)));
                    foreach (var item in context.Employees)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Id.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Fullname, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Position.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Salary.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Phone, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Email, font)));


                    }

                    doc.Add(new Phrase("Звіт \"Співробітники PawsomePets\"\n", font));
                    doc.Add(new Phrase("Нижче надано список співробітників компанії PawsomePets, внесених в базу даних системи управління", font));
                    doc.Add(table);
                    doc.Add(new Phrase($"Всього співробітників: {context.Employees.Count()}", font));
                }
                doc.Close();
                return "Звіт по співробітникам успішно збережений!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetCustomersPDF()
        {
            try
            {
                iTextSharp.text.Document doc = CreatePDFDocument();

                doc.Open();

                using (var context = new ApplicationContext())
                {

                    PdfPTable table = new PdfPTable(6);
                    table.AddCell(new PdfPCell(new Phrase("ID", font)));
                    table.AddCell(new PdfPCell(new Phrase("ПІБ клієнта", font)));
                    table.AddCell(new PdfPCell(new Phrase("Адрес", font)));
                    table.AddCell(new PdfPCell(new Phrase("Телефон", font)));
                    table.AddCell(new PdfPCell(new Phrase("Email", font)));
                    table.AddCell(new PdfPCell(new Phrase("Дисконтна карта", font)));
                    foreach (var item in context.Customers)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Id.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Fullname, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Adress, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Phone, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.Email, font)));
                        table.AddCell(new PdfPCell(new Phrase(item.DiscountCard, font)));


                    }

                    doc.Add(new Phrase("Звіт \"Клієнти компанії PawsomePets\"\n", font));
                    doc.Add(new Phrase("Нижче надано список клієнтів компанії PawsomePets, внесених в базу даних системи управління", font));
                    doc.Add(table);
                    doc.Add(new Phrase($"Всього клієнтів: {context.Customers.Count()}", font));
                }
                doc.Close();
                return "Звіт по клієнтам успішно збережений!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        private iTextSharp.text.Document CreatePDFDocument()
        {

            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 10, 10, 10, 10);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PDF File (*.pdf)|*.pdf|Все файлы(*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            dialog.FileName = "Report";
            if (dialog.ShowDialog().Value == true)
            {
                PdfWriter.GetInstance(doc, new FileStream(dialog.FileName, FileMode.Create));
            }
            else
            {
                throw new Exception("Операція скасована Користувачем!");
            }

            return doc;
        }
    }
}
