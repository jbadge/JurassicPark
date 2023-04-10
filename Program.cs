using System;
using System.Collections.Generic;
using System.Linq;

namespace JurassicPark
{

    class Dinosaur
    {
        public string Name { get; set; }
        public string DietType { get; set; }
        public DateTime WhenAcquired { get; set; }
        public int Weight { get; set; }
        public int EnclosureNumber { get; set; }

        //Methods
        public string Description()
        {
            return $"The dinosaur in enclosure {EnclosureNumber} is known as a {Name}. \nIt is a {DietType} and weighs in at a whopping {Weight} pounds! \nThe dinosaur was acquired by the park on {WhenAcquired}";
        }
    }


    class Program
    {
        //QUESTION
        // Should/Could ANY of these methods be in Dinosaur class?
        // cannot use prop w tab (automatically goes to second word), needs to be fixed.

        static void WelcomeToJurassicPark()
        {
            Console.WriteLine("--------------------------");
            Console.WriteLine("Welcome...to Jurassic Park");
            Console.WriteLine("--------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true).Key.ToString();
        }

        static void Menu(List<Dinosaur> tempList)
        {
            WelcomeToJurassicPark();

            bool usingMenu = true;
            while (usingMenu)
            {
                var menuInput = PromptForChar("What do you want to do? (V)iew, (F)ind, (A)dd, (R)emove, (T)ransfer, (S)ummary, (Q)uit");

                if (menuInput == "V") { ViewDinosaurs(tempList); }
                else if (menuInput == "A") { AddDinosaur(tempList); }
                else if (menuInput == "F") { FindDinosaur(tempList); }
                else if (menuInput == "R") { RemoveDinosaur(tempList); }
                else if (menuInput == "T") { TransferDinosaur(tempList); }
                else if (menuInput == "S") { SummaryDinosaur(tempList); }
                else if (menuInput == "Q") { usingMenu = false; }
                else
                {
                    Console.WriteLine("Please pick a valid option.");
                    DialogueRefresher();
                }
            }
        }

        static void ViewDinosaurs(List<Dinosaur> tempList)
        {
            var menuInput = PromptForChar("Would you like to view dinosaurs in order by (N)ame or (E)nclosure Number?");
            if (tempList.Count == 0)
            {
                Console.WriteLine("There are currently no dinosaurs at the park.");
                DialogueRefresher();
            }
            else if (menuInput == "N") { SortDinosaurs(tempList, "N"); }
            else if (menuInput == "E") { SortDinosaurs(tempList, "E"); }
            else
            {
                Console.WriteLine("Please enter a valid selection.");
                DialogueRefresher();
            }
        }

        static void AddDinosaur(List<Dinosaur> tempList)
        {
            var dinosaur = new Dinosaur();
            dinosaur.Name = PromptForString("What is the name of the dinosaur?");
            dinosaur.DietType = PromptForString("What is the Diet Type of the dinosaur?");
            dinosaur.Weight = PromptForInt("How much does the dinosaur weigh?");
            dinosaur.EnclosureNumber = PromptForInt("What is the enclosure number of the dinosaur?");
            dinosaur.WhenAcquired = DateTime.Now;

            tempList.Add(dinosaur);
            Console.WriteLine($"The {dinosaur.Name} was successfully added to Jurassic Park!");
            DialogueRefresher();
        }

        // FINISH
        static void RemoveDinosaur(List<Dinosaur> tempList)
        {
            var deleteDinosaur = PromptForString("What is the name of the dinosaur you would like to remove from the park?");
            tempList.RemoveAll(x => x.Name == $"{deleteDinosaur}");
            Console.WriteLine($"The {deleteDinosaur} was deleted from the park.");
            DialogueRefresher();

            // MAKE SURE CASE IS NOT IMPORTANT (UPPER/LOWER)
            // ADD IF THE DINO IS NOT IN THE PARK ANYMORE!
        }

        // QUESTION
        static void TransferDinosaur(List<Dinosaur> tempList)
        {
            var name = PromptForString("What is the name of the dinosaur you would like to transfer?");

            if (tempList.Any(x => x.Name.Contains(name)))
            {
                Dinosaur dinoToTransfer = FindDinosaur(tempList, name);
                bool enclosureIsInUse = true;
                var newEnclosureNumber = 0;

                // IS THERE A SIMPLER WAY FOR THE LOOP BELOW OR FOR THIS METHOD?
                while (enclosureIsInUse)
                {
                    newEnclosureNumber = PromptForInt("What enclosure number would you like to move this dinosaur to?");
                    enclosureIsInUse = false;
                    foreach (var dino in tempList)
                    {
                        // MAKING SURE THE ENCLOSURE IS NOT ALREADY IN USE
                        if (newEnclosureNumber == dino.EnclosureNumber)
                        {
                            enclosureIsInUse = true;
                            Console.WriteLine("I am sorry, the enclosure is already taken");
                            DialogueRefresher();
                        }
                    }
                }
                dinoToTransfer.EnclosureNumber = newEnclosureNumber;
            }
            else
            {
                Console.WriteLine($"There does not appear to be a {name} at Jurassic Park.");
                DialogueRefresher();
            }
        }

        static void SummaryDinosaur(List<Dinosaur> tempList)
        {
            var carnivoreCount = 0;
            var herbivoreCount = 0;
            foreach (var dino in tempList)
            {
                if (dino.DietType == "carnivore")
                {
                    carnivoreCount += 1;
                }
                else if (dino.DietType == "herbivore")
                {
                    herbivoreCount += 1;
                }
            }
            Console.WriteLine($"There are currently {carnivoreCount} number of carnivores and {herbivoreCount} number of herbivores in Jurassic Park.");
            DialogueRefresher();
        }

        // QUESTION
        static Dinosaur FindDinosaur(List<Dinosaur> tempList, string dinosaur = null)
        {
            // IS THERE A SIMPLER WAY FOR FindDinosaur??

            // DEFAULT IS NULL AS TRANSFER FN CALLS USING A DINOSAUR ALREADY SELECTED, MENU SELECTION PROMPTS HERE

            // IF FIND OPTION WAS PICKED AT MENU
            if (dinosaur == null)
            {
                var menuInput = PromptForChar("Would you like find the dinosaur by (N)ame or (E)nclosure Number?");

                // CHECK TO SEE IF THE PARK IS EMPTY
                if (tempList.Count == 0)
                {
                    Console.WriteLine("There are currently no dinosaurs at the park.");
                    DialogueRefresher();
                }
                else if (menuInput == "N")
                {
                    bool found = false;
                    while (!found)
                    {
                        var name = PromptForString("What is the name of the dinosaur you are looking for?");
                        foreach (var dino in tempList)
                        {
                            if (name == dino.Name)
                            {
                                Console.WriteLine($"{dino.Description()}");
                                DialogueRefresher();
                                found = true;
                                return dino;
                            }
                        }
                        Console.WriteLine("Please check the spelling of the dinosaur and try again.");
                        DialogueRefresher();
                    }
                }
                else if (menuInput == "E")
                {
                    var enclosureNumber = PromptForInt("What is the enclosure number of the dinosaur you are looking for?");
                    foreach (var dino in tempList)
                    {
                        if (enclosureNumber == dino.EnclosureNumber)
                        {
                            Console.WriteLine($"{dino.Description()}");
                            DialogueRefresher();
                            return dino;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid selection.");
                    DialogueRefresher();
                }
            }

            // IF FIND WAS CALLED FROM TRANSFER
            Dinosaur foundDinosaur = null;
            if (dinosaur == null)
            {
                var name = PromptForString("What is the name of the dinosaur you are looking for?");
            }
            else
            {
                foreach (var dino in tempList)
                {
                    if (dino.Name == dinosaur)
                    {
                        foundDinosaur = dino;
                    }
                }
            }
            Console.WriteLine($"The {dinosaur} was found!");
            DialogueRefresher();
            return foundDinosaur;
        }

        static void SortDinosaurs(List<Dinosaur> tempList, string selection)
        {
            if (selection == "N") { PrintDinosaur(tempList.OrderBy(x => x.Name).ToList()); }
            else if (selection == "E") { PrintDinosaur(tempList.OrderBy(x => x.EnclosureNumber).ToList()); }
        }

        static void PrintDinosaur(List<Dinosaur> tempList)
        {
            foreach (var dino in tempList) { Console.WriteLine($"{dino.Name} is in enclosure {dino.EnclosureNumber}."); }
            DialogueRefresher();
        }

        static void DialogueRefresher()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true).Key.ToString();
            Console.Clear();
        }

        // QUESTION
        static int PromptForInt(string prompt)
        {
            var inputWasInteger = false;
            int inputAsInteger = 0;

            // IS THIS REALLY BEST WAY TO DO THIS?
            while (!inputWasInteger)
            {
                var userInput = PromptForString(prompt);
                var isThisGoodInput = int.TryParse(userInput, out inputAsInteger);

                if (isThisGoodInput == true)
                {
                    inputWasInteger = true;
                }
                else
                {
                    Console.WriteLine("This is not a valid number. Please try again");
                    DialogueRefresher();
                }
            }
            return inputAsInteger;
        }

        static string PromptForChar(string prompt)
        {
            Console.WriteLine(prompt);
            var userInput = Console.ReadKey(true).Key.ToString().ToUpper();
            Console.Clear();
            return userInput;
        }

        static string PromptForString(string prompt)
        {
            Console.WriteLine(prompt);
            var userInput = Console.ReadLine();
            Console.Clear();
            return userInput;
        }

        // QUESTION
        // I WOULD LIKE TO BE ABLE TO HAVE A PopulateJurassicPark METHOD! IS THIS POSSIBLE?
        // static List<Dinosaur> PopulateJurassicPark(List<Dinosaur> tempList)
        // {
        //     var listOfDinosaurs = new List<Dinosaur>()
        //     {
        //         new Dinosaur()
        //         {
        //             Name = "T-Rex",
        //             DietType = "carnivore",
        //             Weight = 100,
        //             EnclosureNumber = 1,
        //             WhenAcquired = DateTime.Now,

        //         },
        //         new Dinosaur()
        //         {
        //             Name = "steg",
        //             DietType = "herbivore",
        //             Weight = 200,
        //             EnclosureNumber = 3,
        //             WhenAcquired = DateTime.Now,

        //         },
        //         new Dinosaur()
        //         {
        //             Name = "alosaur",
        //             DietType = "omnivore",
        //             Weight = 150,
        //             EnclosureNumber = 2,
        //             WhenAcquired = DateTime.Now,

        //         },
        //     };
        //     return listOfDinosaurs;
        // }

        static void stubToBeDeleted()
        {
            // THIS WILL BE DELETED AND IS JUST HERE TO PREVENT ABOVE COMMENTED OUT FN FROM COLLAPSING MORE THAT I WANT IT TO
        }

        // QUESTION
        static void Main(string[] args)
        {
            // TRYING TO USE POPULATE HERE IN VARIOUS WAYS
            // var keepGoing = true;
            // var dinosaur = new Dinosaur();
            // var listOfDinosaurs = new List<Dinosaur>();
            // listOfDinosaurs.Add(null);
            // PopulateJurassicPark(listOfDinosaurs);

            var keepGoing = true;
            var dinosaur = new Dinosaur();
            var listOfDinosaurs = new List<Dinosaur>() {
                new Dinosaur()
                {
                    Name = "T-Rex",
                    DietType = "carnivore",
                    Weight = 100,
                    EnclosureNumber = 1,
                    WhenAcquired = DateTime.Now,
                },
                new Dinosaur()
                {
                    Name = "steg",
                    DietType = "herbivore",
                    Weight = 200,
                    EnclosureNumber = 3,
                    WhenAcquired = DateTime.Now,
                },
                new Dinosaur()
                {
                    Name = "alosaur",
                    DietType = "omnivore",
                    Weight = 150,
                    EnclosureNumber = 2,
                    WhenAcquired = DateTime.Now,
                },
            };

            while (keepGoing)
            {
                Menu(listOfDinosaurs);

                keepGoing = false;
            }
        }
    }
}
