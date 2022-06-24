using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace AssistantСook.MVVM.View
{
    class Orders
    {
        public string NameOfDish { get; set; }
        public string NumberOfPortions { get; set; }

        public Orders(string _nameOfDish, string _numberOfPortions)
        {
            NameOfDish = _nameOfDish;
            NumberOfPortions = _numberOfPortions;
        }

        public Orders()
        {
            NameOfDish = null;
            NumberOfPortions = null;
        }
    }
    public partial class OrdersView : UserControl
    {
        string pathForCookBook = @"D:\Саша\ЗДIА\2 курс\2 семестр\Практика\AssistantСook\AssistantСook\XmlFiles\ForCookBook.xml";
        string pathForStock = @"D:\Саша\ЗДIА\2 курс\2 семестр\Практика\AssistantСook\AssistantСook\XmlFiles\ForStock.xml";
        int sum = 0;
        public OrdersView()
        {
            InitializeComponent();

            CalculateOrders();
        }

        private void CalculateOrders()
        {
            int division = 0;
            int result = 0;
            int prevValue = 0;
            bool wasAdded = false;
            bool alreadyAdded = false;
            int i = 0;

            ArrayList sumListOfPortions = new ArrayList();
            ArrayList sumListOfDishes = new ArrayList();

            XDocument docForCookBook = XDocument.Load(pathForCookBook);
            XDocument docForStock = XDocument.Load(pathForStock);

            XElement? rootForBook = docForCookBook.Element("Recipes");
            XElement? rootForStock = docForStock.Element("Сabinets");

            if (rootForBook != null)
            {
                foreach (XElement nodeForBook in rootForBook.Elements("Recipe"))
                {
                    i = 0;
                    foreach (XElement childNodeForBook in nodeForBook.Elements("ListOfIngredients"))
                    {
                        i++;
                        if (wasAdded)
                            prevValue = division;
                        wasAdded = false;
                        alreadyAdded = false;
                        foreach (XElement nodeForStock in rootForStock.Elements("Cabinet"))
                        {
                            foreach (XElement childNodeForStock in nodeForStock.Elements("NumberOfCell"))
                            {
                                if (childNodeForBook.Element("NameOfProduct").Value == childNodeForStock.Element("NameOfProduct").Value)
                                {
                                    division = Int32.Parse(childNodeForStock.Element("AmountOfProduct").Value) / Int32.Parse(childNodeForBook.Element("AmountOfProduct").Value);
                                    if (i > 1)
                                    {
                                        if (division < prevValue)
                                            sumListOfPortions.Add(division);
                                        else
                                            sumListOfPortions.Add(prevValue);
                                        result = prevValue;
                                        alreadyAdded = true;
                                    }
                                    wasAdded = true;
                                }
                            }
                        }
                    }
                    if (wasAdded && alreadyAdded == false)
                    {
                        sumListOfPortions.Add(division);
                        prevValue = 0;
                    }
                    else if (wasAdded == false && alreadyAdded == false)
                    {
                        division = 0;
                        sumListOfPortions.Add(division);
                    }
                }

                i = 0;
                foreach (var item in docForCookBook.Descendants("Recipe"))
                {
                    sumListOfDishes.Add(item.Element("NameOfDish").Value);
                    i++;
                }

                ReadDataFromXML(sumListOfPortions, sumListOfDishes, i);
            }
        }
        private void ReadDataFromXML(ArrayList sumListOfPortions, ArrayList sumListOfDishes,int count)
        {
            Orders[] orders = new Orders[count];
            int p = 0;

            foreach (var numberOfPortions in sumListOfPortions)
            {
                foreach (var nameOfDish in sumListOfDishes)
                {
                    if (sumListOfDishes.IndexOf(nameOfDish) == p)
                    {
                        Orders order = new Orders() { NameOfDish = nameOfDish.ToString(), NumberOfPortions = numberOfPortions.ToString() };
                        orders[p] = order;
                        break;
                    }
                }
                p++;
            }

            var resource = from m in orders
                           select new
                           {
                               Dish = m.NameOfDish,
                               Portions = m.NumberOfPortions
                           };

            dataGridView.ItemsSource = resource;
        }
    }
}
