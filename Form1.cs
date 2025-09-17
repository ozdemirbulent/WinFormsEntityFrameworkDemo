using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntityProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DbProjectEntities db = new DbProjectEntities();

        private void Form1_Load(object sender, EventArgs e)
        {
            /*kategori Sayısı */

            int CategoryCount = db.Category.Count();
            lblCategoryCount.Text = CategoryCount.ToString();


            /*Ürün Sayısı */

            int ProductCount = db.Product.Count();
            lblProductCount.Text = ProductCount.ToString();

            /*Müşteri Sayısı */

            int CustomerCount = db.Customer.Count();
            lblCustomerCount.Text = CustomerCount.ToString();

            /*Sipariş Sayısı */

            int OrderCount = db.TblOrder.Count();
            lblOrderCount.Text = OrderCount.ToString();

            /*Toplam Stok Sayısı 

            int TotalStock = db.Product.Sum(x => x.ProductStock).Value;
            lblSumStock.Text = TotalStock.ToString();
            */


            var productStockCountByCategoryNameIsSebzeAndStatusIsTrue = db.Product.Where(x => x.CategoryId
            == (db.Category.Where(q => q.CategoryName == "Sebze")
            .Select(t => t.CategoryId).FirstOrDefault())
            && x.ProductStatus == true).Sum(z => z.ProductStock);
            LblProductCountByCategoryAndStatusTrue.Text = productStockCountByCategoryNameIsSebzeAndStatusIsTrue.ToString();

            /*Ortalama Ürün Fiyatı */


            float AveragePrice = (float)db.Product.Average(x => x.ProductPrice);
            LblProductAveragePrice.Text = AveragePrice.ToString("0.00") + " ₺";


            /*Toplam Meyve Stok Sayısı */

            int TotalFruitStock = db.Product.Where(x => x.CategoryId == 1).Sum(y => y.ProductStock).Value;
            LblFruitStock.Text = TotalFruitStock.ToString();


            /* Stok sayısı 100 den küçük olan ürün sayısı*/

            int ProductStockLessThan100 = db.Product.Where(x => x.ProductStock < 100).Count();
            lblProductMin100ThenStock.Text = ProductStockLessThan100.ToString();


            /*Kola İşlem Hacmi */
            /*
             
            float Colaprocessingvolume = (float)db.Product.Where(x => x.ProductName == "Kola").
                Sum(y => y.ProductStock
                * y.ProductPrice).Value;
            LblcokeProcessingVolume.Text = Colaprocessingvolume.ToString("0.00") + " ₺";

            */
            /*En Düşük İşlem Hacmine Sahip Ürün*/

            var LowestProcessingVolume = db.Product.OrderBy(x => x.ProductPrice * x.ProductStock)
                .Select(y => y.ProductName).FirstOrDefault();
            LblProductProcesingMin.Text = LowestProcessingVolume;

            /*En Yüksek İşlem Hacmine Sahip Ürün*/

            var HighestProcessingVolume = db.Product.OrderByDescending(x => x.ProductPrice * x.ProductStock)
                .Select(y => y.ProductName).FirstOrDefault();

            LblProductProcesingMax.Text = HighestProcessingVolume;

            /*Türkiye den Verilen Sipariş Sayısı */
            /* 1.yol */
            /*  var TurkeyOrderCount = db.TblOrder
                  .Where(x =>
                      x.CustomerId == db.Customer
                          .Where(y => y.CustomerName == "Bülent Özdemir")
                          .Select(z => z.CustomerId)
                          .FirstOrDefault()
                      && x.Product.ProductStatus == true)
                  .Count();
              LblTurkeyOrder.Text = TurkeyOrderCount.ToString();*/


            /*    2.yol    */

            var values2 = (from x in db.TblOrder
                           join y in db.Customer on x.CustomerId equals y.CustomerId
                           where y.CustomerCountry == "Türkiye"
                           select x).Count();
            LblTurkeyOrder.Text = values2.ToString();

            /* 3.yol  */

            var values3 = db.Database.SqlQuery<int>(" select count(*) from TblOrder where CustomerId in \r\n(select CustomerId from Customer where CustomerCountry = 'Türkiye')").FirstOrDefault();
            LblTurkeyOrder2.Text = values3.ToString();


            /*4.Yol */

            /* var values = db.Customer.Where(x => x.CustomerCountry == "Türkiye")
          .Select(y => y.CustomerId).ToList();

             var result = db.TblOrder.Count(z => values.Contains(z.CustomerId.Value));
             LblTurkeyOrder2.Text = values2.ToString();*/


            /* Meyve Satışlarından Elde Edilen Ciro*/

            var FruitSalesRevenue = db.Database.SqlQuery<decimal>("select sum(o.TotalPrice) from TblOrder o\r\njoin\r\nProduct p on o.ProductId = p.ProductId\r\njoin\r\nCategory c on p.CategoryId = c.CategoryId\r\nwhere c.CategoryName ='Meyve'").FirstOrDefault();
            LblOrderTotalPriceByCategoryIsFruit.Text = FruitSalesRevenue.ToString();



            var FruitSalesRevenueWithEf = (from o in db.TblOrder
                                           join p in db.Product
                                           on o.ProductId equals p.ProductId
                                           join c in db.Category on
                                           p.CategoryId equals c.CategoryId
                                           where c.CategoryName == "Meyve"
                                           select o.TotalPrice).Sum();

            LblOrderTotalPriceByCategoryIsFruitWithEf.Text = FruitSalesRevenueWithEf.ToString();



            /* En Çok Sipariş Edilen Ürün*/

            var MostOrderedProduct = (from o in db.TblOrder
                                      join p in db.Product
                                      on o.ProductId equals p.ProductId
                                      group o by p.ProductName into g
                                      orderby g.Count() descending
                                      select g.Key).FirstOrDefault();

            LblOrderTotalPriceByCategoryIsWithEf.Text = MostOrderedProduct.ToString();


            /*En Çok Sipariş Veren Müşteri */

            var mostOrderCustomerName = (from o in db.Customer
                                         join p in db.TblOrder
                                         on o.CustomerId equals p.CustomerId
                                         group o by o.CustomerName into g
                                         orderby g.Count() descending
                                         select g.Key).FirstOrDefault();

            LblCustomerWhoTheMostOrders.Text = mostOrderCustomerName.ToString();


            /*En Çok Sipariş Veren Müşterinin Sipariş Sayısı*/

            var mostOrderCustomerOrderCount = (from o in db.Customer
                                               join p in db.TblOrder
                                               on o.CustomerId equals p.CustomerId
                                               group o by o.CustomerName into g
                                               orderby g.Count() descending
                                               select g.Count()).FirstOrDefault();

            LblfromOrdersTheCustomersMost.Text = mostOrderCustomerOrderCount.ToString();

            /*En Çok Sipariş Verilen Ülke      */

            var mostOrderCustomerCountry = (from o in db.Customer
                                            join p in db.TblOrder
                                            on o.CustomerId equals p.CustomerId
                                            group o by o.CustomerCountry into g
                                            orderby g.Count() descending
                                            select g.Key).FirstOrDefault();

            LblMostOrderedCountry.Text = mostOrderCustomerCountry.ToString();

            /*En Çok Sipariş Verilen Şehir      */

            var mostOrderCustomerCity = (from o in db.Customer
                                         join p in db.TblOrder
                                         on o.CustomerId equals p.CustomerId
                                         group o by o.CustomerCity into g
                                         orderby g.Count() descending
                                         select g.Key).FirstOrDefault();

            LblMostOrderedCity.Text = mostOrderCustomerCity.ToString();


            var leastOrderProduct = (from o in db.Product
                                     join p in db.TblOrder
                                     on o.ProductId equals p.ProductId
                                     group o by o.ProductName into g
                                     orderby g.Count() ascending
                                     select g.Key).FirstOrDefault();

            LblLeastOrderedProduct.Text = leastOrderProduct.ToString();
        }



    }
}
