namespace ExampleService.Model
{
    public class Animal
    {
        public string Name { get; }
        public string Species { get; }

        protected Animal(string name)
        {
            Name = name;
            Species = GetType().Name;
        }
    }
}
