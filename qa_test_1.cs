using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace selenium_test_01
{
    class Program
    {


        static void Main(string[] args)
        {
            IWebDriver driver = new FirefoxDriver();
            StringBuilder verificationErrors = new StringBuilder();

            driver.Navigate().GoToUrl("http://demo.opencart.com/");
            //ad.1 the command above opens browser and required page

            driver.FindElement(By.XPath("(//button[@type='button'])[11]")).Click();
            //probably the command above should be removed, but I can not verify it now (the website is offline)

            driver.FindElement(By.XPath("//form[@id='currency']/div/button")).Click();
            driver.FindElement(By.Name("GBP")).Click();
            //ad.2 currency is changed to GBP now

            driver.FindElement(By.Name("search")).Clear();
            driver.FindElement(By.Name("search")).SendKeys("iPod");
            driver.FindElement(By.XPath(".//*[@id='search']/span/button")).Click();
            //ad.3 program is looking for items which contains "iPod" in title

            int NumOfElem = 0;
            var result = driver.FindElements(By.XPath("//*[contains(@data-original-title, 'Compare this Product')] "));
            foreach (IWebElement element in result)
            {
                element.Click();
                NumOfElem += 1;
            }
            //ad.4 the function adds all iPods returned in search results to product comparison

            driver.FindElement(By.XPath(".//*[@id='compare-total']")).Click();
            //ad.5 the command above opens product comparison page


            for (int column = 1; column <= NumOfElem; column++)
            {

                var element = driver.FindElement(By.XPath(".//*[@id='content']/table/tbody[1]/tr[6]/td[" + column + "]")).Text.ToString();
                if (element == "Out Of Stock")
                {
                    driver.FindElement(By.XPath(".//*[@id='content']/table/tbody[2]/tr/td[" + column + "]/a")).Click();
                    NumOfElem = NumOfElem - 1;
                    column = column - 1;
                }
            }
            //ad.6 Because I know that the text "out of string" is always in table row number 6, I am looking for td number which contains it.
            //The loop is looking for "out of stock" text, if finds, puts the "td number" into variable and clicks the "remove" button in the same table column

            Random rnumber = new Random();
            int buyproduct = rnumber.Next(2, NumOfElem + 1);


            var price = driver.FindElement(By.XPath(".//*[@id='content']/table/tbody[1]/tr[3]/td[" + buyproduct + "]")).Text.ToString();
            // take the product price and puts it into variable
            driver.FindElement(By.XPath(".//*[@id='content']/table/tbody[2]/tr/td[" + buyproduct + "]/input")).Click();
            //ad.7 Adds a random avaible iPod to shopping cart

            driver.FindElement(By.XPath("html/body/div[2]/div[1]/a[2]")).Click();
            // while executing this command the shopping cart should be opened, unfortunatelly xPath doesn't work correctly (can not change - website offline)

            var totalprice = driver.FindElement(By.XPath(".//*[@id='content']/form/div/table/tbody/tr/td[6]")).Text.ToString();


            if (string.Equals(price, totalprice))
            {
                Console.WriteLine("Everything is OK, the product price is same as total price");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("There is something wrong! Total price is different than the price of your product!");
                Console.ReadKey();
            }

            // here program compares two variables ("price" and "totalprice" if they are equal or not

        }
    }
}
