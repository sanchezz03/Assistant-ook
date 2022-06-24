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

    public partial class StockView : UserControl
    {
        string path = @"D:\Саша\ЗДIА\2 курс\2 семестр\Практика\AssistantСook\AssistantСook\XmlFiles\ForStock.xml";
        int i = 0;
        string selectingID = "";
        string selectinCount = "";
        string selectingAmountOfProduct = "";
        string selectingNameOfProduct = "";
        public StockView()
        {
            InitializeComponent();

            ReadDataFromXML(); 
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            bool wasAdded = false;
            if (Id.Text != "" && Count.Text != "" && AmountOfProduct.Text != "" && NameOfProduct.Text != "")
            {
                XDocument doc = XDocument.Load(path);
                XElement? root = doc.Element("Сabinets");

                if (root != null)
                {
                    foreach (XElement node in root.Elements("Cabinet"))
                    {
                        if (node.Element("ID")?.Value == Id.Text)
                        {
                            node.Add(new XElement("NumberOfCell",
                         new XElement("Count", Count.Text),
                        new XElement("AmountOfProduct", AmountOfProduct.Text),
                        new XElement("NameOfProduct", NameOfProduct.Text)));
                            wasAdded = true;
                        }
                    }
                    if (wasAdded == false)
                    {
                        if (root != null)
                        {
                            root.Add(new XElement("Cabinet",
                                new XElement("ID", Id.Text),
                                new XElement("NumberOfCell",
                                  new XElement("Count", Count.Text),
                        new XElement("AmountOfProduct", AmountOfProduct.Text),
                        new XElement("NameOfProduct", NameOfProduct.Text))));
                        }
                    }
                    doc.Save(path);

                    ClearTextBox();

                    ReadDataFromXML();
                }

            }
            else
            {
                MessageBox.Show("Please fill all field in form");
            }  
        }

        private void dataGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if ((DataGridRow)dataGridView.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex) != null)
            {
                DataGridRow row = (DataGridRow)dataGridView.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
                
                DataGridCell RowColumnForId = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                DataGridCell RowColumnForCount = dataGrid.Columns[1].GetCellContent(row).Parent as DataGridCell;
                DataGridCell RowColumnForAmountOfProduct = dataGrid.Columns[2].GetCellContent(row).Parent as DataGridCell;
                DataGridCell RowColumnForNameOfProduct = dataGrid.Columns[3].GetCellContent(row).Parent as DataGridCell;

                selectingID = ((TextBlock)RowColumnForId.Content).Text;
                selectinCount = ((TextBlock)RowColumnForCount.Content).Text;
                selectingAmountOfProduct = ((TextBlock)RowColumnForAmountOfProduct.Content).Text;
                selectingNameOfProduct = ((TextBlock)RowColumnForNameOfProduct.Content).Text;
                labelSelectID.Content = selectingID;
            }
        }

        private void ReadDataFromXML()
        {
            XDocument doc = XDocument.Load(path);
            var resource = from c in doc.Descendants("Cabinet")
                           from m in c.Descendants("NumberOfCell")//doc.Descendants("NumberOfCell")
                           select new
                           {
                               id = c.Element("ID").Value,
                               count = m.Element("Count").Value,
                               amountOfProduct = m.Element("AmountOfProduct").Value,
                               nameOfProduct = m.Element("NameOfProduct").Value,
                           };

            dataGridView.ItemsSource = resource;
        }
        private void ClearTextBox()
        {
            Id.Text = "";
            Count.Text = "";
            AmountOfProduct.Text = "";
            NameOfProduct.Text = "";
        }
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xdoc = new XmlDocument();
            bool wasDeleted = false;
            int i = 0;
            xdoc.Load(path);

            if (selectingID != "")
            {

                foreach (XmlNode node in xdoc.SelectNodes("Сabinets/Cabinet"))
                {
                    foreach (XmlNode childNode in node.SelectNodes("NumberOfCell"))
                    {
                        i++;
                        if (node.SelectSingleNode("ID").InnerText == selectingID && childNode.SelectSingleNode("NameOfProduct").InnerText == selectingNameOfProduct && wasDeleted == false)
                        {
                           // if (i > 1)
                                childNode.ParentNode.RemoveChild(childNode);
                            //else
                            //    node.ParentNode.RemoveChild(node);
                            wasDeleted = true;
                        }
                    }
                    if(i == 1&&wasDeleted==true)
                    {
                        node.ParentNode.RemoveChild(node);
                    }
                    if (wasDeleted)
                        break;
                    i = 0;
                }

                xdoc.Save(path);

                ReadDataFromXML();
            }
            else
            {
                MessageBox.Show("Please choose a student to remove");
            }
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            Id.Text = selectingID;
            Count.Text = selectinCount;
            AmountOfProduct.Text = selectingAmountOfProduct;
            NameOfProduct.Text = selectingNameOfProduct;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(path);

            XmlNodeList idNodes = xdoc.SelectNodes("Сabinets/Cabinet");
            XmlNodeList numberOfCells = xdoc.SelectNodes("Сabinets/Cabinet/NumberOfCell");
            bool wasChange = false;

            foreach (XmlNode idNode in idNodes)
            {

                foreach (XmlNode numberOfCell in numberOfCells)
                {
                    if (wasChange)
                        break;

                    if (idNode.SelectSingleNode("ID").InnerText == selectingID && numberOfCell.SelectSingleNode("Count").InnerText == selectinCount && numberOfCell.SelectSingleNode("AmountOfProduct").InnerText == selectingAmountOfProduct && numberOfCell.SelectSingleNode("NameOfProduct").InnerText == selectingNameOfProduct)
                    {
                        idNode.SelectSingleNode("ID").InnerText = Id.Text;
                        numberOfCell.SelectSingleNode("Count").InnerText = Count.Text;
                        numberOfCell.SelectSingleNode("AmountOfProduct").InnerText = AmountOfProduct.Text;
                        numberOfCell.SelectSingleNode("NameOfProduct").InnerText = NameOfProduct.Text;
                        wasChange = true;
                    }
                }
                if (wasChange)
                    break;
            }

            xdoc.Save(path);

            ClearTextBox();

            ReadDataFromXML();
        }

    }
}
