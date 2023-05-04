namespace Ukesoppgave_3
{
    internal class Program
    {
        private static void Main()
        {
            Game game = new();
            game.InitGameLoop();
        }
    }

    public class Game
    {
        private bool _run = true;
        private VirtualPet[] _pets = Array.Empty<VirtualPet>();
        private string _menuId = "none";
        private int _petId = 0;
        public Dictionary<string,Food> Foods = new() { {"pizza",new Food("Pizza",23)}, { "pasta",new Food("Pasta",19) }, { "sandwich", new Food("15",15) } };
        private static Random _rand = new();
        public void InitGameLoop()
        {
            while (_run)
            {
                string? action = "";
                if(_menuId == "none")
                {
                    var actions = ShowActions();
                    ShowPets();
                    Console.WriteLine("Actions: " + string.Join(" ",actions));
                    bool actionInput = false;
                    while(string.IsNullOrEmpty(action) || action.ToLower().Replace(" ", "") == "" || !actions.Contains(action.Split(' ')[0])){
                        if (actionInput)
                        {
                            Console.WriteLine("Your action must be one of the following: " + string.Join(" ", actions));
                        }
                        action = Console.ReadLine();
                        actionInput = !actions.Contains(action?.ToLower());
                    }
                    if(action == "add")
                    {
                        NewPet();
                    }
                    else if (action.ToLower().Contains("goto"))
                    {
                        string[] comms = action.Split(' ');
                        if(comms.Length > 1)
                        {
                            bool parsed = int.TryParse(comms[1], out int numId);
                            if (parsed)
                            {
                                GotoPet(numId);
                            }
                            else
                            {
                                Console.WriteLine("ID must be a number");
                            }
                        }
                        else
                        {
                            Console.WriteLine("You must include pet ID when using goto command I.E goto 0");
                        }
                    }
                    else if (action.ToLower().Contains("rename"))
                    {
                        string[] comms = action.Split(' ');
                        if (comms.Length > 1)
                        {
                            bool parsed = int.TryParse(comms[1], out int numId);
                            if (parsed)
                            {
                                RenamePet(numId);
                            }
                            else
                            {
                                Console.WriteLine("ID must be a number");
                            }
                        }
                        else
                        {
                            Console.WriteLine("You must include pet ID when using rename command from menu");
                        }
                    }
                }
                else if(_menuId == "pet")
                {
                    var actions = ShowActions();
                    _pets[_petId].DescribePet();
                    Console.WriteLine("Available foods: " + string.Join(" ",Foods.Keys.Select(k=>k).ToArray()));
                    Console.WriteLine("Actions: " + string.Join(" ", actions));
                    bool actionInput = false;
                    while (string.IsNullOrWhiteSpace(action) || !actions.Contains(action.Split(' ')[0]))
                    {
                        if (actionInput)
                        {
                            Console.WriteLine("Your action must be one of the following: " + string.Join(" ", actions));
                        }
                        action = Console.ReadLine().ToLower();
                        actionInput = !actions.Contains(action) && !string.IsNullOrEmpty(action);
                    }
                    if (action.Contains("rename"))
                    {
                        string[] comms = action.Split(' ');
                        if (comms.Length > 1)
                        {
                            if (int.TryParse(comms[^1], out int numId) && numId<_pets.Length)
                            {
                                RenamePet(numId);
                            }
                            else
                            {
                                Console.WriteLine("invalid ID");
                            }
                        }
                        else
                        {
                            RenamePet();
                        }
                        
                    }
                    else if (action.Contains("play"))
                    {
                        _pets[_petId].Play();
                    }
                    else if (action.ToLower().Contains("feed"))
                    {
                        string[] coms = action.Split(" ");
                        if (coms.Length > 1 && Foods.ContainsKey(coms[^1]))
                        {
                            if (coms[^1] == _pets[_petId].FavouriteFood.ToLower())
                                _pets[_petId].FeedPet(Foods[coms[^1]],true);
                            else
                                _pets[_petId].FeedPet(Foods[coms[^1]]);
                        }
                        else
                        {
                            Console.WriteLine("Invalid food, use from list of available foods");
                        }
                        
                    }
                    else if (action.ToLower() == "back")
                    {
                        _menuId = "none";
                    }
                    else if (action.ToLower() == "show")
                    {
                        _pets[_petId].ShowPet(_pets[_petId].Art);
                    }
                }
            }
        }
        private void GotoPet(int id)
        {
            if(id < _pets.Length && id >=0)
            {
                _petId = id;
                _menuId = "pet";
            }
            else
            {
                Console.WriteLine("Pet ID doesn't exist, try again");
            }
        }
        private void RenamePet()
        {
            Console.WriteLine("Please enter new name for " + _pets[_petId].Name + " or cancel to stop name change");
            while(true)
            {
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid, try again.");
                }
                else if(!IsUnique(input)){
                    Console.WriteLine("Name must be unique, try again.");
                }
                else if (input.ToLower() == "cancel")
                {
                    Console.WriteLine("Cancelling name change.");
                    break;
                }
                else {
                    _pets[_petId].Name = input;
                    break;
                }
            }

        }
        private void RenamePet( int id)
        {
            Console.WriteLine("Please enter new name for " + _pets[id].Name);
            while (true)
            {
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid, try again.");
                }
                else if (!IsUnique(input))
                {
                    Console.WriteLine("Name must be unique, try again.");
                }
                else { 
                    _pets[id].RenamePet(input);
                    break; 
                }
            }
        }
        private string[] ShowActions()
        {
            string[] actions = {"goto","rename"};
            if (_menuId == "none")
            {
                Array.Resize(ref actions, actions.Length+1);
                actions[2] = "add";
            }
            else if(_menuId == "pet")
            {
                Array.Resize(ref actions, actions.Length+5);
                actions[2] = "feed";
                actions[3] = "play";
                actions[4] = "rename";
                actions[5] = "show";
                actions[6] = "back";
            }
            return actions;
        }
        private void ShowPets()
        {
            if(_pets.Length > 0)
            {

                for(int i = 0; i < _pets.Length;i++)
                {
                    VirtualPet pet = _pets[i];
                    Console.WriteLine(i+".");
                    pet.DescribePet();
                }
            }
            else
            {
                Console.WriteLine("You currently have no pets");
            }
        }
        private void NewPet()
        {
            Console.WriteLine("Enter new pet name: ");
            string? newName = "";
            bool nameEntered = false;
            while (newName == null || newName.Replace(" ","") == "" || !IsUnique(newName))
            {
                if (nameEntered)
                {
                    Console.WriteLine("Invalid, name must be unique.");
                }
                newName = Console.ReadLine();
                nameEntered = true;
            }
            if (newName.ToLower() == "cancel")
            {
               
            }
            else{
                Array.Resize(ref _pets, _pets.Length+1);
                string[] animalStrings = {"Rabbit","Cat","Dog" };
                string selectedAnimal = animalStrings[_rand.Next(animalStrings.Length-1)];
                switch (selectedAnimal)
                {
                    case "Rabbit":
                        _pets[^1] = new Rabbit(newName, "         ,\r\n        /|      __\r\n       / |   ,-~ /\r\n      Y :|  //  /\r\n      | jj /( .^\r\n      >-\"~\"-v\"\r\n     /       Y\r\n    jo  o    |\r\n   ( ~T~     j\r\n    >._-' _./\r\n   /   \"~\"  |\r\n  Y     _,  |\r\n /| ;-\"~ _  l\r\n/ l/ ,-\"~    \\\r\n\\//\\/      .- \\\r\n Y        /    Y    -Row\r\n l       I     !\r\n ]\\      _\\    /\"\\\r\n(\" ~----( ~   Y.  )");
                        break;
                    case "Cat":
                        _pets[^1] = new Cat(newName, "(\"`-''-/\").___..--''\"`-._ \r\n `6_ 6  )   `-.  (     ).`-.__.`) \r\n (_Y_.)'  ._   )  `._ `. ``-..-' \r\n   _..`--'_..-_/  /--'_.'\r\n  ((((.-''  ((((.'  (((.-' ");
                        break;
                    case "Dog":
                        _pets[^1] = new Dog(newName, "   |\\|\\\r\n  ..    \\       .\r\no--     \\\\    / @)\r\n v__///\\\\\\\\__/ @\r\n   {           }\r\n    {  } \\\\\\{  }\r\n    <_|      <_|");
                        break;
                }
                Console.WriteLine("New " + selectedAnimal +  " " + newName + " added!");
            }
        }
        private bool IsUnique(string name)
        {
            return _pets.All(pet => !string.Equals(name, pet.Name, StringComparison.CurrentCultureIgnoreCase));
        }
    }

    public class VirtualPet
    {
        public string FavouriteFood;
        public string Art;
        protected int Fullness = 100;
        protected int Health = 100;
        protected int Closeness = 0;
        public string? Name { get; set; }
        public VirtualPet(string? nameInput, string ascii)
        {
            Name = nameInput;
            Art = ascii;
            FavouriteFood = "";
        }
        public void DescribePet()
        {
            Console.WriteLine("----------");
            Console.WriteLine("Pet: " + Name);
            Console.WriteLine("Health: " + Health);
            Console.WriteLine("Closeness: " + Closeness);
            Console.WriteLine("Fullness: " + Fullness);
            Console.WriteLine("----------");
        }
        public void FeedPet(Food food)
        {
            if (Fullness == 100)
            {
                Console.WriteLine(Name + " is already full! closeness -1, health -10");
                Closeness -= 1;
                Health -= 10;
            }
            else
            {
                Console.WriteLine("You fed the " + food.Name + " to " + Name + "fullness +" + food.Filling);
                Fullness += food.Filling;
            }

            if (Fullness > 100) Fullness = 100;
        }
        public void FeedPet(Food food, bool isFavourite)
        {
            if (Fullness == 100)
            {
                Console.WriteLine(Name + " is already full! closeness -1, health -10");
                Closeness -= 1;
                Health -= 10;
            }
            else
            {
                Console.WriteLine(food.Name + " is " + Name + "'s favourite food! closeness +2, fullness +" + food.Filling);
                Closeness += 2;
                Fullness += food.Filling;
            }
            if (Fullness > 100) Fullness = 100;
        }
        public void RenamePet(string? name)
        {
            Name = name;
        }

        public void Play()
        {
            Closeness += 2;
            Fullness -= 20;
            if (Fullness < 0)
            {
                Fullness = 0;
            }
            Console.WriteLine("You played with "+ Name + " closeness +2, fullness -20");
            if (Fullness < 50)
            {
                Health -= 10;
                Console.WriteLine(Name + " is too hungry to play: health -10");
                if (Health < 0)
                {
                    Health = 0;
                }
            }
            else
            {
                Health += 10;
                Console.WriteLine(Name + " became stronger, health +10");
                if (Health > 0)
                {
                    Health = 100;
                }
            }
        }

        public void ShowPet(string ascii)
        {
            Console.WriteLine(ascii);
        }
    }
    public class Rabbit : VirtualPet
    {
        public Rabbit(string? nameInput,string ascii) : base(nameInput, ascii)
        {
            FavouriteFood = "Pizza";
        }
    }

    public class Cat : VirtualPet
    {
        public Cat(string? nameInput, string ascii) : base(nameInput, ascii)
        {
            FavouriteFood = "Pasta";
        }
    }

    public class Dog : VirtualPet
    {
        public Dog(string? nameInput, string ascii) : base(nameInput, ascii)
        {
            FavouriteFood = "Sandwich";
        }
    }

    public class Food
    {
        public string Name;
        public int Filling;
        public Food(string name, int filling)
        {
            Name = name;
            Filling = filling;
        }
    }
}