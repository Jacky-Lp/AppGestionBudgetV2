using AppGestionBudget.Data.Models;
using AppGestionBudget.DataBase.DbEntities;
using AppGestionBudget.DataBase.Service;
using AppGestionBudget.Enum;
using Microsoft.EntityFrameworkCore;

namespace AppGestionBudget.Service
{
    public class GestionService {
        private  List<User> user;
        private  List<Expenses> expenses;
        private  List<Expenses> expensesExcel;

        GestionContext Db = new GestionContext();
        public GestionService() {
            user = new List<User>();
            expenses = new List<Expenses>();
            expensesExcel = new List<Expenses>();
            Init();
        }

        public List<User> GetUser(){ return this.user; }
        public List<Expenses> GetExpenses() {  return this.expenses; }

        public String GetExpense(Month month, User user) {
            int cal = 0;
            String text;
            var expense = user.expenseList.Where(x => x.date.Month == ((int)month)).ToList();

            foreach(var ele in expense) {
                cal += ele.sum;
            }
            if(cal > user.budget) {
                text = $"{user.name} spent {cal} in {month} and he spent too much because he has {user.budget} budget ";
            }
            else { 
                text = $"{user.name} spent {cal} in {month} and he didn't speend too much because he has {user.budget} budget ";
            }
            return text;
        }
        public String GetMuchCategory() {
            Dictionary<Category, int> cout = new Dictionary<Category, int>();

            foreach (var ele in expenses)
            {
                if (cout.Any(x => x.Key == ele.category)) {
                    cout[ele.category] += ele.sum;
                } else {
                    cout.Add(ele.category, ele.sum);
                }
            }
            var value = cout.Aggregate((x, y) => x.Value > y.Value ? x : y);
            return $"Everybody spent the most on {value.Key} with {value.Value} ";
        }
        public List<Expenses> checkCategory(User user, Category cate) { 
            var check = user.expenseList.Where(x => x.category == cate).ToList();
            if(check.Count == 0) {
                Console.WriteLine($"{user.name} {user.username} has no expense in {cate}");
            }
            return check;
        }
        public void ExpensesWithExel() {
            ExpensesExel data = new ExpensesExel("D:\\Dev\\C#\\Application\\Classes\\AppGestionBudget.Data\\Exel\\Expenses.xlsx", 1);
            
            List<Expenses> t = new List<Expenses>();
            Expenses expenses;
            try {
                for (int i = 2; i <= data.nbligne() ; i++) {
                    int nb = data.Sum(i);
                    string Label = data.label(i);
                    Category category = data.category(i);
                    string date = data.date(i);
                    string userExcel = data.user(i);
                    
                    try {
                        var user = this.user.First(x => x.name == userExcel);
                        expenses = new Expenses(nb, Label, category, DateOnly.Parse(date), user.id);
                        user.AddExpense(nb, Label, category, DateOnly.Parse(date), user.id);
                        this.expenses.Add(expenses);
                    }
                    catch(Exception) {
                        Console.WriteLine($"Error : Name in Exel don't exist (ligne {i})");
                    }

                } }catch (Exception ex) {
                Console.WriteLine($"Erreur : {ex.Message} Lors du chargement du fichier Excel");
            }
            data.close();
        }
        public String GetMuchCategoryUser(User user)
        {
            Dictionary<Category, int> cout = new Dictionary<Category, int>();

            foreach (var ele in user.expenseList)
            {
                if (cout.Any(x => x.Key == ele.category))
                {
                    cout[ele.category] += ele.sum;
                }
                else
                {
                    cout.Add(ele.category, ele.sum);
                }
            }
            try{ 
                var value = cout.Aggregate((x, y) => x.Value > y.Value ? x : y);
                return $"{user.name} spent the most on {value.Key} with {value.Value}";
            }catch(Exception ex) { Console.WriteLine(ex.Message); }

            return " ";
        }


        public User AddUserWithBdd(UserDb userDb) {
            User user = new User(userDb.name, userDb.username, userDb.budget, userDb.id);
            this.user.Add(user);
            return user;
        }

        public Expenses AddExpensesWithBdd(ExpensesDb expensesDb) {
            Expenses expenses = new Expenses(expensesDb.sum, expensesDb.label, expensesDb.category, expensesDb.date, expensesDb.userId);
            this.expenses.Add(expenses);
            return expenses;
        }

        public Expenses AddExpenses(int sum, string label, Category category, string date, int userId) {
            Expenses expenses = new Expenses(sum, label, category, DateOnly.Parse(date), userId);
            this.expenses.Add(expenses);
            return expenses;
        }
        public User AddUser(string  name, string username, int budget, int id) {
            User user = new User(name, username, budget, id);
            this.user.Add(user);
            return user;
        }
        public void Init() {
            //Avec la Bdd
            var i = Db.Users.Max(x => x.id);
            for (int a = 1; a <= i; a++){
                User t = AddUserWithBdd(Db.Users.First(x => x.id == a));
                foreach(var e in Db.Expenses.Where(x => x.userId == t.id)) { 
                    Expenses ex = t.AddExpense(e.sum, e.label, e.category, e.date, e.userId);
                    this.expenses.Add(ex);
                }
            }
            //ExpensesWithExel();
            //Sans la Bdd

            //var theo = AddUser("théo", "gg", 500, 0);
            //var leo = AddUser("léo", "gg", 500, 0);

            //Expenses play = AddExpenses(50, "gta 6", Category.Loisir, DateOnly.Parse("2023-5-8"));
            //Expenses eat = AddExpenses(75, "potato", Category.Alimentation, DateOnly.Parse("2023-6-8"));
            //Expenses eat2 = AddExpenses(50, "carrot", Category.Alimentation, DateOnly.Parse("2023-5-8"));
            //Expenses play2 = AddExpenses(50, "gta 4", Category.Loisir, DateOnly.Parse("2023-5-8"));
            //Expenses Voiture = AddExpenses(360, "Reparation", Category.Auto, DateOnly.Parse("2023-4-25"));
            //Expenses xBox = AddExpenses(556, "xBox", Category.Loisir, DateOnly.Parse("2023-5-1"));
        }
    }
}

