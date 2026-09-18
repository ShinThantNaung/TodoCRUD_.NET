namespace Todo.Model
{
    public class TodoItem
    {
        public int Id { get; private set; }
        public String? Name { get; private set; } = String.Empty;
        public Boolean IsComplete { get; private set; }
        public String? Secret { get; private set; }

        public DateTime CreatedAt { get; private set; }

        private TodoItem() { }

        public TodoItem(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("The name of the todo cannot be empty");
            }
            Name = name;
            IsComplete = false;
            CreatedAt = DateTime.UtcNow;
        }
        public void MarkAsComplete()
        {
            IsComplete = true;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new Exception("The name of the todo cannot be empty");
            }
            Name = newName;
        }
    }

}