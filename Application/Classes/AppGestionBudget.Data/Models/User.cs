using AppGestionBudget.Data.Models;
using AppGestionBudget.Enum;
using AppGestionBudget.Data;

namespace AppGestionBudget.Data.Models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public int budget { get; set; }
        public int useMoney { get; set; }

        public List<Expenses> expenseList;

        public User(string name, string username, int budget, int id)
        {
            this.name = name;
            this.username = username;
            this.budget = budget;
            this.id = id;
            expenseList = new List<Expenses>();
        }
        public Expenses AddExpense(int sum, string label, Category category, DateOnly date, int userId)
        {
            Expenses expenses = new Expenses(sum, label, category, date, userId);
            expenseList.Add(expenses);
            return expenses;
        }
        public override string ToString() { return $"User name {name} {username} : as {budget} budget. (id = {id})"; }
    }
}
