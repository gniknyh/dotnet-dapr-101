namespace Link.Mydapr.Service.Ordering.Events
{
    public class BasketItem
    {
        public int Id {get; private set;}

        public string Name {get; private set;}

        public string Description {get; private set;}

        public BasketItem(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}