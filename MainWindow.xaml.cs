using AutoLotModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

namespace Cristutiu_Andreea_Lab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
        CollectionViewSource customerViewSource;
        CollectionViewSource inventoryViewSource;
        CollectionViewSource customerOrdersViewSource;

        //Binding txtFirstNameBinding = new Binding(); 
        //Binding txtLastNameBinding = new Binding();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
           // txtFirstNameBinding.Path = new PropertyPath("First Name"); 
           // txtLastNameBinding.Path = new PropertyPath("Last Name"); 
           //txtFirstName.SetBinding(TextBox.TextProperty, txtFirstNameBinding); 
           // txtLastName.SetBinding(TextBox.TextProperty, txtLastNameBinding);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // customerViewSource.Source = [generic data source]
            customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            ctx.Customers.Load();
            customerViewSource.Source = ctx.Customers.Local;
            //System.Windows.Data.CollectionViewSource customerOrdersViewSource= ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));
            customerOrdersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));
            //customerOrdersViewSource.Source = ctx.Orders.Local;
            BindDataGrid();
           
            ctx.Orders.Load();
            
            
            System.Windows.Data.CollectionViewSource inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // inventoryViewSource.Source = [generic data source]
            inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            inventoryViewSource.Source = ctx.Inventories.Local;
            ctx.Inventories.Load();


            cmbCustomers.ItemsSource = ctx.Customers.Local;
            //cmbCustomers.DisplayMemberPath = "FirstName";
            cmbCustomers.SelectedValuePath = "CustId";
            cmbInventory.ItemsSource = ctx.Inventories.Local;
            // cmbInventory.DisplayMemberPath = "Make";
            cmbInventory.SelectedValuePath = "CarId";


        }
        private void SetValidationBinding()
        {
            customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            ctx.Customers.Load();
            customerViewSource.Source = ctx.Customers.Local;
            Binding firstNameValidationBinding = new Binding();
            firstNameValidationBinding.Source = customerViewSource;
            firstNameValidationBinding.Path = new PropertyPath("FirstName");
            firstNameValidationBinding.NotifyOnValidationError = true;
            firstNameValidationBinding.Mode = BindingMode.TwoWay;
            firstNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //string required
            firstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
            txtFirstName.SetBinding(TextBox.TextProperty, firstNameValidationBinding);
            Binding lastNameValidationBinding = new Binding();
            lastNameValidationBinding.Source = customerViewSource;
            lastNameValidationBinding.Path = new PropertyPath("LastName");
            lastNameValidationBinding.NotifyOnValidationError = true;
            lastNameValidationBinding.Mode = BindingMode.TwoWay;
            lastNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //string min length validator
            lastNameValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            txtLastName.SetBinding(TextBox.TextProperty, lastNameValidationBinding); //setare binding nou
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New; 
            

            btnNew.IsEnabled = false; 
            btnEdit.IsEnabled = false; 
            btnDelete.IsEnabled = false; 
            //btnSave.IsEnabled = true; 
            btnCancel.IsEnabled = true; 
            customerDataGrid.IsEnabled = false; 
            btnPrevious.IsEnabled = false; 
            btnNext.IsEnabled = false; 
            txtFirstName.IsEnabled = true; 
            txtLastName.IsEnabled = true;

            BindingOperations.ClearBinding(txtFirstName, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtLastName, TextBox.TextProperty);
            customerDataGrid.SelectedItem = null;
            SetValidationBinding();
            txtFirstName.Text = ""; 
            txtLastName.Text = "";
            
            Keyboard.Focus(txtFirstName);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            

            string tempFirstName = txtFirstName.Text.ToString(); 
            string tempLastName = txtLastName.Text.ToString();

            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            //btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            customerDataGrid.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;
            txtFirstName.IsEnabled = true;
            txtLastName.IsEnabled = true;
            BindingOperations.ClearBinding(txtFirstName, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtLastName, TextBox.TextProperty);
            SetValidationBinding();

            txtFirstName.Text = tempFirstName;
            txtLastName.Text = tempLastName;

            Keyboard.Focus(txtFirstName);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempFirstName = txtFirstName.Text.ToString();
            string tempLastName = txtLastName.Text.ToString();

            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            //btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            customerDataGrid.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;

            txtFirstName.Text = tempFirstName;
            txtLastName.Text = tempLastName;
            Keyboard.Focus(txtFirstName);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnDelete.IsEnabled = true;
            //btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;
            customerDataGrid.IsEnabled =false;
            btnPrevious.IsEnabled =true;
            btnNext.IsEnabled = true;
            txtFirstName.IsEnabled = false;
            txtLastName.IsEnabled = false;
            //txtFirstName.SetBinding(TextBox.TextProperty, txtFirstNameBinding); 
            //txtLastName.SetBinding(TextBox.TextProperty, txtLastNameBinding);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // customerViewSource.Source = [generic data source]
            customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            customerViewSource.Source = ctx.Customers.Local;
            ctx.Customers.Load();
            Customer customer = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Customer entity
                    customer = new Customer()
                    {
                        FirstName = txtFirstName.Text.Trim(),
                        LastName = txtLastName.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Customers.Add(customer);
                    customerViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                //btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = true;
                txtFirstName.IsEnabled = false;
                txtLastName.IsEnabled = false;
                //SetValidationBinding();
            }
            else
            if (action == ActionState.Edit)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    customer.FirstName = txtFirstName.Text.Trim();
                    customer.LastName = txtLastName.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                customerViewSource.View.MoveCurrentTo(customer);
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                //btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = true;
                txtFirstName.IsEnabled = false;
                txtLastName.IsEnabled = false;
                //txtFirstName.SetBinding(TextBox.TextProperty, txtFirstNameBinding);
               // txtLastName.SetBinding(TextBox.TextProperty, txtLastNameBinding);
                SetValidationBinding();
                

            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    ctx.Customers.Remove(customer);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                //btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = true;
                txtFirstName.IsEnabled = false;
                txtLastName.IsEnabled = false;
                //txtFirstName.SetBinding(TextBox.TextProperty, txtFirstNameBinding);
                //txtLastName.SetBinding(TextBox.TextProperty, txtLastNameBinding);
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // customerViewSource.Source = [generic data source]
            customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            ctx.Customers.Load();
            customerViewSource.Source = ctx.Customers.Local;
            customerViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // customerViewSource.Source = [generic data source]
            customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            ctx.Customers.Load();
            customerViewSource.Source = ctx.Customers.Local;
            customerViewSource.View.MoveCurrentToNext();
        }

        


        private void btnNewI_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNewI.IsEnabled = false;
            btnEditI.IsEnabled = false;
            btnDeleteI.IsEnabled = false;
            btnSaveI.IsEnabled = true;
            btnCancelI.IsEnabled = true;
            inventoryDataGrid.IsEnabled = false;
            btnPreviousI.IsEnabled = false;
            btnNextI.IsEnabled = false;
            colorTextBox.IsEnabled = true;
            makeTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            colorTextBox.Text = "";
            makeTextBox.Text = "";
            Keyboard.Focus(colorTextBox);
        }

        private void btnEditI_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempColor = colorTextBox.Text.ToString();
            string tempMake = makeTextBox.Text.ToString();

            btnNewI.IsEnabled = false;
            btnEditI.IsEnabled = false;
            btnDeleteI.IsEnabled = false;
            btnSaveI.IsEnabled = true;
            btnCancelI.IsEnabled = true;
            inventoryDataGrid.IsEnabled = false;
            btnPreviousI.IsEnabled = false;
            btnNextI.IsEnabled = false;
            colorTextBox.IsEnabled = true;
            makeTextBox.IsEnabled = true;

            colorTextBox.Text = tempColor;
            makeTextBox.Text = tempMake;
            Keyboard.Focus(colorTextBox);
        }

        private void btnDeleteI_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempColor = colorTextBox.Text.ToString();
            string tempMake = makeTextBox.Text.ToString();

            btnNewI.IsEnabled = false;
            btnEditI.IsEnabled = false;
            btnDeleteI.IsEnabled = false;
            btnSaveI.IsEnabled = true;
            btnCancelI.IsEnabled = true;
            inventoryDataGrid.IsEnabled = false;
            btnPreviousI.IsEnabled = false;
            btnNextI.IsEnabled = false;

            colorTextBox.Text = tempColor;
            makeTextBox.Text = tempMake;
            Keyboard.Focus(colorTextBox);
        }

        private void btnCancelI_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNewI.IsEnabled = true;
            btnEditI.IsEnabled = true;
            btnDeleteI.IsEnabled = true;
            btnSaveI.IsEnabled = false;
            btnCancelI.IsEnabled = false;
            inventoryDataGrid.IsEnabled = false;
            btnPreviousI.IsEnabled = true;
            btnNextI.IsEnabled = true;
            colorTextBox.IsEnabled = false;
            makeTextBox.IsEnabled = false;
        }
        private void btnSaveI_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            inventoryViewSource.Source = ctx.Inventories.Local;
            ctx.Inventories.Load();
            Inventory inventory = null;
            if (action == ActionState.New)
            {
                try
                {
                    inventory = new Inventory()
                    {
                        Color = colorTextBox.Text.Trim(),
                        Make = makeTextBox.Text.Trim()
                    };
                    ctx.Inventories.Add(inventory);
                    inventoryViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNewI.IsEnabled = true;
                btnEditI.IsEnabled = true;
                btnDeleteI.IsEnabled = true;
                btnSaveI.IsEnabled = false;
                btnCancelI.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPreviousI.IsEnabled = true;
                btnNextI.IsEnabled = true;
                colorTextBox.IsEnabled = false;
                makeTextBox.IsEnabled = false;
            }
            else
            if (action == ActionState.Edit)
            {
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    inventory.Color = colorTextBox.Text.Trim();
                    inventory.Make = makeTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                inventoryViewSource.View.MoveCurrentTo(inventory);
                btnNewI.IsEnabled = true;
                btnEditI.IsEnabled = true;
                btnDeleteI.IsEnabled = true;
                btnSaveI.IsEnabled = false;
                btnCancelI.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPreviousI.IsEnabled = true;
                btnNextI.IsEnabled = true;
                colorTextBox.IsEnabled = false;
                makeTextBox.IsEnabled = false;

            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    ctx.Inventories.Remove(inventory);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
                btnNewI.IsEnabled = true;
                btnEditI.IsEnabled = true;
                btnDeleteI.IsEnabled = true;
                btnSaveI.IsEnabled = false;
                btnCancelI.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPreviousI.IsEnabled = true;
                btnNextI.IsEnabled = true;
                colorTextBox.IsEnabled = false;
                makeTextBox.IsEnabled = false;
            }
        }

        private void btnNextI_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            inventoryViewSource.Source = ctx.Inventories.Local;
            ctx.Inventories.Load();
            inventoryViewSource.View.MoveCurrentToNext();
        }

        private void btnPreviousI_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            inventoryViewSource.Source = ctx.Inventories.Local;
            ctx.Inventories.Load();
            inventoryViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNewO_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNewO.IsEnabled = false;
            btnEditO.IsEnabled = false;
            btnDeleteO.IsEnabled = false;
            btnSaveO.IsEnabled = true;
            btnCancelO.IsEnabled = true;
            ordersDataGrid.IsEnabled = false;
            btnPreviousO.IsEnabled = false;
            btnNextO.IsEnabled = false;
            
        }

        private void btnEditO_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
           

            btnNewO.IsEnabled = false;
            btnEditO.IsEnabled = false;
            btnDeleteO.IsEnabled = false;
            btnSaveO.IsEnabled = true;
            btnCancelO.IsEnabled = true;
            ordersDataGrid.IsEnabled = true;
            btnPreviousO.IsEnabled = false;
            btnNextO.IsEnabled = false;
          
        }

        private void btnDeleteO_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            

            btnNewO.IsEnabled = false;
            btnEditO.IsEnabled = false;
            btnDeleteO.IsEnabled = false;
            btnSaveO.IsEnabled = true;
            btnCancelO.IsEnabled = true;
            ordersDataGrid.IsEnabled = true;
            btnPreviousO.IsEnabled = false;
            btnNextO.IsEnabled = false;
        }

        private void btnCancelO_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNewO.IsEnabled = true;
            btnEditO.IsEnabled = true;
            btnDeleteO.IsEnabled = true;
            btnSaveO.IsEnabled = false;
            btnCancelO.IsEnabled = false;
            ordersDataGrid.IsEnabled = true;
            btnPreviousO.IsEnabled = true;
            btnNextO.IsEnabled = true;
        }

        private void btnSaveO_Click(object sender, RoutedEventArgs e)
        {
            ctx.Orders.Load();
     
            //cmbInventory.ItemsSource = ctx.Inventories.Local;
            Order order = null;
            if (action == ActionState.New)
            {
                try
                {
                    Customer customer = (Customer)cmbCustomers.SelectedItem;
                    Inventory inventory = (Inventory)cmbInventory.SelectedItem;
                    //instantiem Order entity
                    order = new Order()
                    {
                        CustId = customer.CustId,
                        CarId = inventory.CarId
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Orders.Add(order);
                    BindDataGrid();
                    //customerOrdersViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNewO.IsEnabled = true;
                btnEditO.IsEnabled = true;
                btnDeleteO.IsEnabled = true;
                btnSaveO.IsEnabled = false;
                btnCancelO.IsEnabled = false;
                ordersDataGrid.IsEnabled = true;
                btnPreviousO.IsEnabled = true;
                btnNextO.IsEnabled = true;
            }
            if (action == ActionState.Edit)
            {
                /* try
                 {
                     order = (Order)ordersDataGrid.SelectedItem;
                     Customer customer = (Customer)cmbCustomers.SelectedItem;
                     Inventory inventory = (Inventory)cmbInventory.SelectedItem;
                     order.CustId = customer.CustId;
                     order.CarId = inventory.CarId;
                     //salvam modificarile
                     ctx.SaveChanges();
                 }
                 catch (DataException ex)
                 {
                     MessageBox.Show(ex.Message);
                 }
                 customerOrdersViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                customerOrdersViewSource.View.MoveCurrentTo(order);
                */
                dynamic selectedOrder = ordersDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedOrder.OrderId;
                    var editedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (editedOrder != null)
                    {
                        editedOrder.CustId = Int32.Parse(cmbCustomers.SelectedValue.ToString());
                        editedOrder.CarId = Convert.ToInt32(cmbInventory.SelectedValue.ToString());
                        //salvam modificarile
                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
                // pozitionarea pe item-ul curent
                customerOrdersViewSource.View.MoveCurrentTo(selectedOrder);

                btnNewO.IsEnabled = true;
                btnEditO.IsEnabled = true;
                btnDeleteO.IsEnabled = true;
                btnSaveO.IsEnabled = false;
                btnCancelO.IsEnabled = false;
                ordersDataGrid.IsEnabled = true;
                btnPreviousO.IsEnabled = true;
                btnNextO.IsEnabled = true;


            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedOrder = ordersDataGrid.SelectedItem;
                    int curr_id = selectedOrder.OrderId;
                    var deletedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (deletedOrder != null)
                    {
                        ctx.Orders.Remove(deletedOrder);
                        ctx.SaveChanges();
                        MessageBox.Show("Order Deleted Successfully", "Message");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNewO.IsEnabled = true;
                btnEditO.IsEnabled = true;
                btnDeleteO.IsEnabled = true;
                btnSaveO.IsEnabled = false;
                btnCancelO.IsEnabled = false;
                ordersDataGrid.IsEnabled = true;
                btnPreviousO.IsEnabled = true;
                btnNextO.IsEnabled = true;
            }

        }

        private void btnPreviousO_Click(object sender, RoutedEventArgs e)
        {
            
            customerOrdersViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNextO_Click(object sender, RoutedEventArgs e)
        {
            
            customerOrdersViewSource.View.MoveCurrentToNext();
        }
        private void BindDataGrid()
        {
            ctx.Customers.Load();
           
            ctx.Inventories.Load();

            customerOrdersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));
            ctx.Orders.Load();
            var queryOrder =  from ord in ctx.Orders
                                              join cust in ctx.Customers on ord.CustId equals
                                              cust.CustId
                                              join inv in ctx.Inventories on ord.CarId
                                  equals inv.CarId
                                              select new
                                              {
                                                  ord.OrderId,
                                                  ord.CarId,
                                                  ord.CustId,
                                                  cust.FirstName,
                                                  cust.LastName,
                                                  inv.Make,
                                                  inv.Color
                                              };

            customerOrdersViewSource.Source = queryOrder.ToList();
        }


    }
   
    }
