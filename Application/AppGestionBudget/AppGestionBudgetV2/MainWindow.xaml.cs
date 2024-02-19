using AppGestionBudget.Data.Models;
using AppGestionBudget.Enum;
using AppGestionBudget.Service;
using Microsoft.Data.Sqlite;
using System.Windows;
using AppGestionBudget.DataBase.DbEntities;
using System.Windows.Controls;
namespace AppGestionBudgetV2
{
    public partial class MainWindow : Window
    {
        public GestionService service = new GestionService();
        public MainWindow()
        {
            InitializeComponent();
            Ini();
        }
        private void BtnAddUser(object sender, RoutedEventArgs e) {

            try {
                var name = Name.Text;
                var username = Username.Text;
                var budget = int.Parse(Budget.Text);
                var id = int.Parse(Id.Text);
                User t = service.AddUser(name, username, budget, id);
                AddDb(t);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            
            Name.Clear();
            Username.Clear();
            Budget.Clear();
            Id.Clear();


            


            AddConfUserOrExpenses.Visibility = Visibility.Hidden;
            AddUser.Visibility = Visibility.Hidden;

            ChooseUser.Items.Clear();
            Ini();
        }
        private void AddExpenses_Click(object sender, RoutedEventArgs e)
        {
            AddConfUserOrExpenses.Visibility = Visibility.Visible;
            AddUser.Visibility = Visibility.Visible;
            namee.Text = "Entrer un nom";
            usernamee.Text = "Prénom utilisateur";
            budgett.Text = "Budget de l'utilisateur";
            idd.Text = "Id utilisateur";
        }
        private void Ini()
        {
            //Add sum of all Expenses 
            NbSumExpenses.Text = service.GetMuchCategory();
            
            //Add Expenses in ListBox
            foreach (var item in service.GetExpenses()) { Expenses.Items.Add($"Dépenses : {item.label} dans la catégorie {item.category} prix {item.sum} euro a la date {item.date}"); }
            var t = service.GetUser().Count();

            //Add User in ComboBox User
            foreach (var ele in service.GetUser()) { ChooseUser.Items.Add(ele); }

            //Add User in ComboBox Month
            foreach (var ele in Enum.GetValues(typeof(Month))) { ChooseMonth.Items.Add(ele); }

            //Add User in ComboBox Category
            foreach (var ele in Enum.GetValues(typeof(Category))) { ChooseCategory.Items.Add(ele); }
        }

        private void CheckExpensesByUser(object sender, RoutedEventArgs e)
        {

            Expenses.Items.Clear();
            var user = service.GetUser();
            User takeUser;
            Month? CheckMonth;
            Category? CheckCategory;
            if (ChooseUser.SelectedIndex == 0) {
                takeUser = null;
                MuchCategoryOfUser.Text = " ";
            }
            else {
                takeUser = (User)ChooseUser.SelectedValue;
                MuchCategoryOfUser.Text = service.GetMuchCategoryUser(takeUser);
            }
            if (ChooseMonth.SelectedIndex == 0) {
                CheckMonth = null;
                TextCost.Visibility = Visibility.Hidden;
            }
            else {
                CheckMonth = (Month)ChooseMonth.SelectedValue;
                if (takeUser != null) {
                    TextCost.Text = service.GetExpense((Month)CheckMonth, takeUser);
                }
            }
            if (ChooseCategory.SelectedIndex == 0) {
                CheckCategory = null;
            }
            else
            {
                CheckCategory = (Category)ChooseCategory.SelectedValue;
            }
            
            if (CheckMonth != null & takeUser != null & CheckCategory != null)
            {
                foreach (var item in service.GetExpenses().Where(x => x.userId == takeUser.id & x.date.Month == ((int)CheckMonth) & x.category == CheckCategory))
                {
                    Expenses.Items.Add(item);
                }
            }
            else if (CheckMonth == null & takeUser != null & CheckCategory != null)
            {
                foreach (var item in service.GetExpenses().Where(x => x.userId == takeUser.id & x.category == CheckCategory))
                {
                    Expenses.Items.Add(item);
                }
            }
            else if (CheckMonth != null & takeUser == null & CheckCategory != null)
            {
                foreach (var item in service.GetExpenses().Where(x => x.date.Month == ((int)CheckMonth) & x.category == CheckCategory))
                {
                    Expenses.Items.Add(item);
                }
            }
            else if (CheckMonth != null & takeUser != null & CheckCategory == null)
            {
                foreach (var item in service.GetExpenses().Where(x => x.date.Month == ((int)CheckMonth) & x.userId == takeUser.id))
                {
                    Expenses.Items.Add(item);
                }
            }
            else if (CheckMonth == null & takeUser == null & CheckCategory != null)
            {
                foreach (var item in service.GetExpenses().Where(x => x.category == CheckCategory))
                {
                    Expenses.Items.Add(item);
                }
            }
            else if (CheckMonth != null & takeUser == null & CheckCategory == null)
            {
                foreach (var item in service.GetExpenses().Where(x => x.date.Month == ((int)CheckMonth)))
                {
                    Expenses.Items.Add(item);
                }
            }
            else if (CheckMonth == null & takeUser != null & CheckCategory == null)
            {
                foreach (var item in service.GetExpenses().Where(x => x.userId == takeUser.id))
                {
                    Expenses.Items.Add(item);
                }
            }
            else {
                foreach (var item in service.GetExpenses())
                {
                    Expenses.Items.Add(item);
                }
            }
        }
        public void AddDb(User user) {

            GestionContext Db = new GestionContext();
            var userDb = new UserDb();

            userDb.expenseList = new List<ExpensesDb>();
            userDb.budget = user.budget;
            userDb.name = user.name;
            userDb.budget = user.budget;
            userDb.useMoney = user.useMoney;
            userDb.username = user.username;
            userDb.id = user.id;
            userDb.expenseList = new List<ExpensesDb>();

            foreach (var expense in user.expenseList.ToList()) {
                var expensesDb = new ExpensesDb();
                expensesDb.date = expense.date;
                expensesDb.sum = expense.sum;
                expensesDb.label = expense.label;
                expensesDb.category = expense.category;
                userDb.expenseList.Add(expensesDb);
            } try {
                Db.Add(userDb);
                Db.SaveChanges();
            } catch (SqliteException e) { MessageBox.Show(e.SqlState); }
        }

        private void AddExpenses(object sender, RoutedEventArgs e) {
            MessageBox.Show("Coming soon");
            
        }
    }
}
