using System;

namespace Ukesoppgave_3
{
    internal class Program
    {
        static void Main()
        {
            Game game = new();
            game.InitGameLoop();
        }
    }

    public class Game
    {
        private bool run = true;
        private VirtualPet[] pets = { };
        private string menuId = "none";
        private int petId = 0;
        public string[] Foods = {"Pizza","Pasta","Sandwich"};
        public static Random Rand = new();
        public void InitGameLoop()
        {
            while (run)
            {
                string action = "";
                if(menuId == "none")
                {
                    var actions = ShowActions();
                    ShowPets();
                    Console.WriteLine("Actions: " + string.Join(" ",actions));
                    bool actionInput = false;
                    while(action == null || action.ToLower().Replace(" ", "") == "" || !actions.Contains(action.ToLower())){
                        if (actionInput)
                        {
                            Console.WriteLine("Your action must be one of the following: " + string.Join(" ", actions));
                        }
                        action = Console.ReadLine();
                        actionInput = !actions.Contains(action.ToLower());
                    }
                    if(action == "add pet")
                    {
                        NewPet();
                    }
                    else if (action.Split(" ").Contains("goto"))
                    {
                        int numId;
                        bool parsed = Int32.TryParse(action.Split(" ")[1],out numId);
                        if (parsed)
                        {
                            GotoPet(numId);
                        }
                        else
                        {
                            Console.WriteLine("You must include pet ID when using goto command I.E goto 0");
                        }
                    }
                }
            }
        }
        private void GotoPet(int id)
        {
            if(id < pets.Length && id >=0)
            {
                petId = id;
                menuId = "pet";
            }
            else
            {
                Console.WriteLine("Pet ID doesn't exist, try again");
            }
        }
        private string[] ShowActions()
        {
            string[] actions = {"goto"};
            if (menuId == "none")
            {
                Array.Resize(ref actions, actions.Length+1);
                actions[1] = "add pet";
            }
            else if(menuId == "pet")
            {
                Array.Resize(ref actions, actions.Length+3);
                actions[1] = "feed";
                actions[2] = "play";
                actions[3] = "back";
            }
            return actions;
        }
        private void ShowPets()
        {
            if(pets.Length > 0)
            {

                for(int i = 0; i < pets.Length;i++)
                {
                    VirtualPet pet = pets[i];
                    Console.WriteLine(i+".");
                    Console.WriteLine("-------");
                    pet.DescribePet();
                    Console.WriteLine("-------");
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
            string newName = "";
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
                return;
            }
            else{
                Array.Resize(ref pets, pets.Length+1);
                pets[pets.Length-1] = new VirtualPet(newName, Foods[Rand.Next(0,Foods.Length)]);
                Console.WriteLine("New pet " + newName + "added!");
                return;
            }
        }
        private bool IsUnique(string name)
        {
            foreach (var pet in pets)
            {
                if (name.ToLower() == pet.Name.ToLower())
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class VirtualPet
    {
        private int fullness = 100;
        private int health = 100;
        private int closeness = 0;
        public string Name { get; }
        private string favouriteFood;
        public VirtualPet(string nameInput,string food)
        {
            Name = nameInput;
            favouriteFood = food;
        }
        public void DescribePet()
        {
            Console.WriteLine("Pet: " + Name);
            Console.WriteLine("Health: " + health);
            Console.WriteLine("Closeness: " + closeness);
            Console.WriteLine("Fullness: " + fullness);
            Console.WriteLine("Favourite food: " + favouriteFood);
        }
        public void FeedPet(string food)
        {

        }
    }
    public class Food
    {

    }
    public class Pizza : Food
    {

    }
}