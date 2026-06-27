namespace ToDoList.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public DateTime TimeDue { get; set; }
        public bool IsCompleted { get; set; }
        public String Details { get; set; }

        public ToDoItem()
        {

        }

    }
}
