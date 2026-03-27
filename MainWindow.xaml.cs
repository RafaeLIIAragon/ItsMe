using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aragon101
{

    /// </summary>
    public partial class MainWindow : Window
    {
        //declare the array storage
        string[] storeName = new string[100];
        string[] product = new string[100];
        string[] quantity = new string[100];
        string[] pieces = new string[100];
        double[] price = new double[100];
        double[] total = new double[100];
       
        char status = 'A';
        int index = 0;
        int updatedIndex = -1;
        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
       
    
        public MainWindow()
        {
            InitializeComponent();
        }
            private void btnSave_Click(object sender, RoutedEventArgs e)
            {
                //string Quantity = "";
                string storeName = txtStoreName.Text;
                string product = comboBoxProduct.Text; ;
                string quantity = comboBoxQuantity.Text;
                string pieces = txtPcs.Text;




            // Convert price
            if (!double.TryParse(txtPrice.Text, out double pr))
            {
                MessageBox.Show("Invalid price!", "Error", MessageBoxButton.OK);
                return;
            }


            string data = $"{storeName} - {product} - {quantity} - {pieces}";
            
                SaveData(storeName, product, quantity, pr, pieces);
                ClearData();
            }
            /**
             * Clears the form data after the sava action
             */
            private void ClearData()
            {
                txtStoreName.Clear();
                txtPrice.Clear();
                txtPcs.Clear();
                comboBoxProduct.SelectedIndex = -1;
                comboBoxQuantity.SelectedIndex = -1;

            }

            private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                int index = dataGrid.SelectedIndex;
                if (index >= 0)
                {
                    txtStoreName.Text = storeName[index];
                    txtPcs.Text = pieces[index].ToString(); ;
                    comboBoxProduct.Text = product[index];
                    comboBoxQuantity.Text = quantity[index];
                    txtPrice.Text = price[index].ToString();
                  

                //make the delete data button enabled
                btnDeleteData.IsEnabled = true;
                    //change the status to E or Update
                    status = 'E';
                    updatedIndex = index;
                }
            }
        /**
         * Perform the save action. There are two save action here.
         * Add - Add the new data to the array
         * Update - updates the data from the array
         */

        private void SaveData(string sn, string p, string q, double pr, string pcs)
        {
            int qty = 1;

            // Extract number from "250mg", "1kg"
            if (!int.TryParse(q.Replace("g", "").Replace("kg", ""), out qty))
            {
                qty = 1;
            }
            double ptotal = pr * qty;
            if (!int.TryParse(pcs, out int pcsValue))
            {
                pcsValue = 1;
            }

            double computedTotal = pcsValue * pr;


            if (status == 'A')
            {
                storeName[index] = sn;
                product[index] = p;
                quantity[index] = q;
                price[index] = pr;
                pieces[index] = pcs;
                total[index] = computedTotal;

                dataGrid.Items.Add(new
                {
                    StoreName = sn,
                    Product = p,
                    Quantity = q,
                    Price = pr,
                    Pieces = pcs,
                    Total = computedTotal
                });

                index++;

                MessageBox.Show("Product added successfully!", "Product Form", MessageBoxButton.OK);
            }
            else if (status == 'E' && updatedIndex >= 0)
            {
                storeName[updatedIndex] = sn;
                product[updatedIndex] = p;
                quantity[updatedIndex] = q;
                price[updatedIndex] = pr;
                pieces[updatedIndex] = pcs;
                total[updatedIndex] = computedTotal;

                RefreshGrid();

                status = 'A';
                updatedIndex = -1;

                MessageBox.Show("Product updated successfully!", "Product Form", MessageBoxButton.OK);
            }
        }
        private void btnDeleteData_Click(object sender, RoutedEventArgs e)
            {
                int deleteIndex = dataGrid.SelectedIndex;

               
                ShiftElements(deleteIndex);
                //decrement the size
                index--;
                //update the grid
                RefreshGrid();
                //disables the delete button after deleting
                btnDeleteData.IsEnabled = false;
                //clears the data
                ClearData();
                MessageBox.Show("Student data deleted successfully!", "Product Form", MessageBoxButton.OK);
            }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            storeName = new string[100];
            product = new string[100];
            quantity = new string[100];
            pieces = new string[100];
            price = new double[100];
            total = new double[100];
            btnClear.IsEnabled = true;
            btnDeleteData.IsEnabled=false;

           ClearData() ;
        }
                

            /**
             * This function refreshes the data grid
             */
            private void RefreshGrid()
            {
                dataGrid.Items.Clear();
                for (int i = 0; i < index; i++)
                {
                    dataGrid.Items.Add(new
                    {
                        StoreName = storeName[i],
                        Product = product[i],
                        Quantity = quantity[i],
                        Price = price[i],
                        Pieces = pieces[i],
                        Total = total[i]

                    });
                }
            }

        /**
         * This function shifts the elements to the left after performing delete
         */
        private void ShiftElements(int deletedIndex)
        {
            for (int i = deletedIndex; i < index - 1; i++)
            {
                storeName[i] = storeName[i + 1];
                product[i] = product[i + 1];
                quantity[i] = quantity[i + 1];
                pieces[i] = pieces[i + 1];
                price[i] = price[i + 1];
                total[i] = total[i + 1];
            }
        }
    }
}
