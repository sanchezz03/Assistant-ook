using System;
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
using System.Xml;
using System.Xml.Linq;

namespace AssistantСook.MVVM.View
{
    /// <summary>
    /// Interaction logic for CookBook.xaml
    /// </summary>
    public partial class CookBookView : UserControl
    {
        string pathForCookBook = @"D:\Саша\ЗДIА\2 курс\2 семестр\Практика\AssistantСook\AssistantСook\XmlFiles\ForCookBook.xml";
        int i = 0;
        string selectingName = "";
        string selectingAmountOfProduct = "";
        string selectingNameOfProduct = "";
        public CookBookView()
        {
            InitializeComponent();

            ReadDataFromXML();
        }

        private void dataGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if ((DataGridRow)dataGridView.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex) != null)
            {
                DataGridRow row = (DataGridRow)dataGridView.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);

                DataGridCell RowColumnForName = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                DataGridCell RowColumnForNameOfProduct = dataGrid.Columns[1].GetCellContent(row).Parent as DataGridCell;
                DataGridCell RowColumnForAmountOfProduct = dataGrid.Columns[2].GetCellContent(row).Parent as DataGridCell;
               
                selectingName = ((TextBlock)RowColumnForName.Content).Text;
                selectingNameOfProduct= ((TextBlock)RowColumnForNameOfProduct.Content).Text;
                selectingAmountOfProduct = ((TextBlock)RowColumnForAmountOfProduct.Content).Text;

                labelSelectID.Content = selectingName;
            }
        }
        private void ReadDataFromXML()
        {
            XDocument doc = XDocument.Load(pathForCookBook);
            var resource = from c in doc.Descendants("Recipe")
                           from m in c.Descendants("ListOfIngredients")
                           select new
                           {
                               nameOfDish = c.Element("NameOfDish").Value,
                               nameOfProduct = m.Element("NameOfProduct").Value,
                               amountOfProduct = m.Element("AmountOfProduct").Value,
                               gram = c.Element("Gram").Value,
                               calorieContentOfFood = c.Element("CalorieContentOfFood").Value
                           };

            dataGridView.ItemsSource = resource;
        }

        private void ClearTextBox()
        {
            NameOfDish.Text = "";
            NameOfProduct.Text = "";
            AmountOfProduct.Text = "";
            Gram.Text = "";
            CalorieContentOfFood.Text = "";
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            bool wasAdded = false;
            if (NameOfDish.Text != "" && NameOfProduct.Text != "" && AmountOfProduct.Text != "")
            {
                XDocument doc = XDocument.Load(pathForCookBook);
                XElement? root = doc.Element("Recipes");

                if (root != null)
                {
                    foreach (XElement node in root.Elements("Recipe"))
                    {
                        if (node.Element("NameOfDish")?.Value == NameOfDish.Text)
                        {
                            node.Add(
                                new XElement("ListOfIngredients",
                                  new XElement("NameOfProduct", NameOfProduct.Text),
                        new XElement("AmountOfProduct", AmountOfProduct.Text)));
                            wasAdded = true;
                        }
                    }
                    if (wasAdded == false)
                    {
                        if (root != null)
                        {
                            root.Add(new XElement("Recipe",
                                new XElement("NameOfDish", NameOfDish.Text),
                                new XElement("ListOfIngredients",
                                  new XElement("NameOfProduct", NameOfProduct.Text),
                        new XElement("AmountOfProduct", AmountOfProduct.Text)),
                                new XElement("Gram",Gram.Text),
                                new XElement("CalorieContentOfFood", CalorieContentOfFood.Text)));
                        }
                    }
                    doc.Save(pathForCookBook);

                    ClearTextBox();

                    ReadDataFromXML();
                }
            }
            else
            {
                MessageBox.Show("Please fill all field in form");
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xdoc = new XmlDocument();
            bool wasDeleted = false;
            int i = 0;
            xdoc.Load(pathForCookBook);

            if (selectingName != "")
            {

                foreach (XmlNode node in xdoc.SelectNodes("Recipes/Recipe"))
                {
                    foreach (XmlNode childNode in node.SelectNodes("ListOfIngredients"))
                    {
                        i++;
                        if (node.SelectSingleNode("NameOfDish").InnerText == selectingName && childNode.SelectSingleNode("NameOfProduct").InnerText == selectingNameOfProduct && childNode.SelectSingleNode("AmountOfProduct").InnerText == selectingAmountOfProduct && wasDeleted == false)
                        {
                            //if (i > 1)
                                childNode.ParentNode.RemoveChild(childNode);
                            //else
                               // node.ParentNode.RemoveChild(node);
                            wasDeleted = true;
                        }
                    }
                    if (i == 1 && wasDeleted == true)
                    {
                        node.ParentNode.RemoveChild(node);
                    }
                    if (wasDeleted)
                        break;
                    i = 0;
                }

                xdoc.Save(pathForCookBook);

                ReadDataFromXML();
            }
            else
            {
                MessageBox.Show("Please choose a student to remove");
            }
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            NameOfDish.Text = selectingName;
            AmountOfProduct.Text = selectingAmountOfProduct;
            NameOfProduct.Text = selectingNameOfProduct;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(pathForCookBook);

            XmlNodeList nameNodes = xdoc.SelectNodes("Recipes/Recipe");
            XmlNodeList childNodes = xdoc.SelectNodes("Recipes/Recipe/ListOfIngredients");
            bool wasChange = false;

            foreach (XmlNode nameNode in nameNodes)
            {

                foreach (XmlNode childNode in childNodes)
                {
                    if (wasChange)
                        break;

                    if (nameNode.SelectSingleNode("NameOfDish").InnerText == selectingName && childNode.SelectSingleNode("AmountOfProduct").InnerText == selectingAmountOfProduct && childNode.SelectSingleNode("NameOfProduct").InnerText == selectingNameOfProduct)
                    {
                        nameNode.SelectSingleNode("NameOfDish").InnerText = NameOfDish.Text;
                        childNode.SelectSingleNode("AmountOfProduct").InnerText = AmountOfProduct.Text;
                        childNode.SelectSingleNode("NameOfProduct").InnerText = NameOfProduct.Text;
                        wasChange = true;
                    }
                }
                if (wasChange)
                    break;
            }

            xdoc.Save(pathForCookBook);

            ClearTextBox();

            ReadDataFromXML();
        }
    }
}
