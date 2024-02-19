using AppGestionBudget.Enum;

namespace AppGestionBudget.Data.Models
{
    public class Expenses{
        public int sum {  get; set; }
        public string label { get; set; }
        public Category category { get; set; }
        public DateOnly date {  get; set; }
        public int userId { get; set; }

        public Expenses(int sum, string label, Category category, DateOnly date, int userId) {
            this.sum = sum;
            this.label = label;
            this.category = category;
            this.date = date;
            this.userId = userId;
        }

        public override string ToString()
        {
            return $"Expenses {sum} for {label} {category} on {date} (user id : {userId})";
        }

    }
}
