using System;
using System.Collections.Generic;
using System.Linq;

namespace JurassicPark
{

    class Dinosaur
    {
        // finding name that does not exist says it exists
        // allows to add to full enclosure

        public string Name { get; set; }
        public string DietType { get; set; }
        public DateTime WhenAcquired { get; set; }
        public int Weight { get; set; }
        public int EnclosureNumber { get; set; }

        // METHODS
        public string Description()
        {
            return $"The dinosaur in enclosure {EnclosureNumber} is known as a {Name}. \nIt is a {DietType} and weighs in at a whopping {Weight.ToString("N0")} pounds! \nThe dinosaur was acquired by the park on {WhenAcquired}";
        }
    }

    class Program
    {

        static void WelcomeToJurassicPark()
        {
            Console.Clear();
            Console.WriteLine("--------------------------");
            Console.WriteLine("Welcome...to Jurassic Park");
            Console.WriteLine("--------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true).Key.ToString();
            Console.Clear();
        }

        static void Menu(List<Dinosaur> tempList)
        {
            WelcomeToJurassicPark();

            bool usingMenu = true;
            while (usingMenu)
            {
                var menuInput = PromptForChar("What do you want to do?\n(V)iew all dinosaurs in the park.\n(F)ind a specific dinosaur in the park.\n(A)dd a dinosaur to the park.\n(R)emove a dinosaur from the park.\n(T)ransfer a dinosaur to a new enclosure.\n(S)ummary of dinosaurs by diet in the park.\n(Q)uit the program.");

                switch (menuInput)
                {
                    case "V":
                        ViewDinosaurs(tempList);
                        break;
                    case "F":
                        FindDinosaur(tempList);
                        break;
                    case "A":
                        AddDinosaur(tempList);
                        break;
                    case "R":
                        RemoveDinosaur(tempList);
                        break;
                    case "T":
                        TransferDinosaur(tempList);
                        break;
                    case "S":
                        SummaryDinosaur(tempList);
                        break;
                    case "Q":
                        usingMenu = false;
                        break;
                    default:
                        Console.WriteLine("Please pick a valid option.");
                        DialogueRefresher();
                        break;
                }
            }
        }

        static void ViewDinosaurs(List<Dinosaur> tempList)
        {
            if (tempList.Count == 0)
            {
                Console.WriteLine("There are currently no dinosaurs at the park.");
                DialogueRefresher();
            }
            else
            {
                var menuInput = PromptForChar("Would you like to view dinosaurs in order by (N)ame or (E)nclosure Number?");
                if (menuInput == "N") { SortDinosaurs(tempList, "N"); }
                else if (menuInput == "E") { SortDinosaurs(tempList, "E"); }
                else
                {
                    Console.WriteLine("Please enter a valid selection.");
                    DialogueRefresher();
                }
            }
        }

        // QUESTION
        static Dinosaur FindDinosaur(List<Dinosaur> tempList, string dinosaur = null)
        {
            // DEFAULT IS NULL AS TRANSFER FN CALLS USING A DINOSAUR ALREADY SELECTED, MENU SELECTION PROMPTS HERE

            var foundDinosaur = new Dinosaur();

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
                    var name = PromptForString("What is the name of the dinosaur you are looking for?");
                    foreach (var dino in tempList)
                    {
                        if (name.ToLower() == dino.Name.ToLower())
                        {
                            Console.WriteLine($"{dino.Description()}");
                            DialogueRefresher();
                            // Q2
                            return dino;
                        }
                    }
                    Console.WriteLine($"There does not appear to be a {name} at Jurassic Park.");
                    DialogueRefresher();
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
                            // Q3
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
            else
            {
                foreach (var dino in tempList)
                {
                    if (dinosaur.ToLower() == dino.Name.ToLower())
                    {
                        Console.WriteLine($"The {dinosaur} was found!");
                        DialogueRefresher();
                        foundDinosaur = dino;
                        // Q1
                        return foundDinosaur;
                    }
                }
                Console.WriteLine($"There does not appear to be a {dinosaur} at Jurassic Park.");
                DialogueRefresher();
            }

            // WHY WON'T "Q1" ~ line 148 work, BUT "Q2" ~line 112 AND "Q3" ~line 128 DO?
            return foundDinosaur;
        }

        static void AddDinosaur(List<Dinosaur> tempList)
        {
            var dinosaur = new Dinosaur();

            dinosaur.Name = PromptForString("What is the name of the dinosaur?");

            bool correctDietType = true;
            while (correctDietType)
            {
                var newDietType = PromptForString($"Is {dinosaur.Name} a (h)erbivore or a (c)arnivore?").ToUpper();
                {
                    if (newDietType == "H" || newDietType == "C")
                    {
                        dinosaur.DietType = newDietType;
                        correctDietType = false;
                    }
                    else
                    {
                        Console.WriteLine("Please input a valid diet type");
                        DialogueRefresher();
                    }
                }
            }
            dinosaur.Weight = PromptForInt("How much does the dinosaur weigh?");

            bool enclosureIsInUse = true;
            var newEnclosureNumber = 0;
            while (enclosureIsInUse)
            {
                // SHOW UNAVAILABLE ENCLOSURE NUMBERS
                newEnclosureNumber = PromptForInt($"What enclosure would you like for the {dinosaur.Name}?");
                enclosureIsInUse = false;
                foreach (var dino in tempList)
                {
                    // MAKING SURE THE ENCLOSURE IS NOT ALREADY IN USE
                    if (newEnclosureNumber == dino.EnclosureNumber)
                    {
                        enclosureIsInUse = true;
                        Console.WriteLine("That enclosure is already taken.");
                        DialogueRefresher();
                    }
                }
            }
            dinosaur.EnclosureNumber = newEnclosureNumber;
            dinosaur.WhenAcquired = DateTime.Now;

            tempList.Add(dinosaur);
            Console.WriteLine($"The {dinosaur.Name} was successfully added to Jurassic Park!");
            DialogueRefresher();
        }

        static void RemoveDinosaur(List<Dinosaur> tempList)
        {
            var deleteDinosaur = PromptForString("What is the name of the dinosaur you would like to remove from the park?").ToLower();
            bool foundDinosaur = false;
            foreach (var dino in tempList)
            {
                if (deleteDinosaur == dino.Name.ToLower())
                {
                    foundDinosaur = true;
                }
            }
            if (foundDinosaur)
            {
                tempList.RemoveAll(x => x.Name.ToLower() == $"{deleteDinosaur}");
                Console.WriteLine($"The {deleteDinosaur} was removed from the park.");
                DialogueRefresher();
            }
            else
            {
                Console.WriteLine($"We do not have a {deleteDinosaur} at Jurassic Park!");
                DialogueRefresher();
            }
        }

        static void CheckEnclosureStatus(List<Dinosaur> tempList, Dinosaur tempDino)
        {
            if (tempDino.Name != null)
            {
                var newEnclosureNumber = PromptForInt($"What enclosure would you like to move the {tempDino.Name} to?");
                bool foundIt = true;

                //var count = 0;
                // Checking all dinosaurs to see if they are in that enclosure

                // USING BOOL WITH BREAK TO GET THROUGH LOGIC
                // BUT COULD ALSO USE COUNT METHOD...?
                // IF DOES NOT EXIST, COUNTS SAME AS .COUNT, IF FINDS NUMBER IT DOES NOT COUNT
                // SO IT IS LESS THAT COUNT BY 1.
                foreach (var dino in tempList)
                {
                    // Dino not in desired enclosure number
                    if (newEnclosureNumber != dino.EnclosureNumber)
                    {
                        foundIt = false;
                        // count += 1;
                    }
                    // Dino in that enclosure number but it is not the dino being moved
                    else if (newEnclosureNumber == dino.EnclosureNumber && dino.Name != tempDino.Name)
                    {
                        Console.WriteLine($"Enclosure {newEnclosureNumber} is currently occupied by {dino.Name}");
                        DialogueRefresher();
                        foundIt = true;
                        break;
                    }
                    // Dino in that enclosure number and it is the dino being moved
                    else if (newEnclosureNumber == dino.EnclosureNumber && dino.Name.ToLower() == tempDino.Name.ToLower())
                    {
                        Console.WriteLine($"This is {dino.Name}'s current enclosure. \nYou can only transfer a dinosaur to an empty enclosure.");
                        DialogueRefresher();
                        foundIt = true;
                        break;
                    }
                }

                // if (count > tempList.Count() - 1)
                if (!foundIt)
                {
                    // Enclosure not being used. Transferring now.
                    tempDino.EnclosureNumber = newEnclosureNumber;
                    Console.WriteLine($"Park rangers have successfully moved {tempDino.Name} to {tempDino.EnclosureNumber}");
                    DialogueRefresher();
                }
            }

        }

        static void TransferDinosaur(List<Dinosaur> tempList)
        {
            var name = PromptForString("What is the name of the dinosaur you would like to transfer?").ToLower();
            var dinoToTransfer = FindDinosaur(tempList, name);

            CheckEnclosureStatus(tempList, dinoToTransfer);
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

        // Helper Function for ViewDinosaurs
        static void SortDinosaurs(List<Dinosaur> tempList, string selection)
        {
            if (selection == "N") { PrintDinosaur(tempList.OrderBy(x => x.Name).ToList()); }
            else if (selection == "E") { PrintDinosaur(tempList.OrderBy(x => x.EnclosureNumber).ToList()); }
        }

        // Helper Function for SortDinosaurs for ViewDinosaurs
        static void PrintDinosaur(List<Dinosaur> tempList)
        {
            foreach (var dino in tempList) { Console.WriteLine($"{dino.Name} is in enclosure {dino.EnclosureNumber}."); }
            DialogueRefresher();
        }

        static int PromptForInt(string prompt)
        {
            var inputWasInteger = false;
            int inputAsInteger = 0;

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

        static void DialogueRefresher()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true).Key.ToString();
            Console.Clear();
        }

        static List<Dinosaur> PopulateJurassicPark()
        {
            var listOfDinosaurs = new List<Dinosaur>()
            {
                new Dinosaur()
                {
                    Name = "Dilophosaurus",
                    DietType = "carnivore",
                    Weight = 2000,
                    EnclosureNumber = 1,
                    WhenAcquired = DateTime.Now,
                },
                new Dinosaur()
                {
                    Name = "Gallimimus",
                    DietType = "herbivore",
                    Weight = 920,
                    EnclosureNumber = 2,
                    WhenAcquired = DateTime.Now,
                },
                new Dinosaur()
                {
                    Name = "Baryonyx",
                    DietType = "carnivore",
                    Weight = 3500,
                    EnclosureNumber = 3,
                    WhenAcquired = DateTime.Now,
                },
                    new Dinosaur()
                {
                    Name = "Tyrannosaurus Rex",
                    DietType = "carnivore",
                    Weight = 18500,
                    EnclosureNumber = 4,
                    WhenAcquired = DateTime.Now,
                },
                    new Dinosaur()
                {
                    Name = "Triceratops",
                    DietType = "herbivore",
                    Weight = 25200,
                    EnclosureNumber = 5,
                    WhenAcquired = DateTime.Now,
                },
                new Dinosaur()
                {
                    Name = "Brachiosaurus",
                    DietType = "herbivore",
                    Weight = 123700,
                    EnclosureNumber = 6,
                    WhenAcquired = DateTime.Now,
                },
                new Dinosaur()
                {
                    Name = "Pteranodon",
                    DietType = "carnivore",
                    Weight = 55,
                    EnclosureNumber = 7,
                    WhenAcquired = DateTime.Now,
                },
                new Dinosaur()
                {
                    Name = "Stegosaurus",
                    DietType = "herbivore",
                    Weight = 8000,
                    EnclosureNumber = 8,
                    WhenAcquired = DateTime.Now,
                },
                new Dinosaur()
                {
                    Name = "Velociraptor",
                    DietType = "carnivore",
                    Weight = 320,
                    EnclosureNumber = 9,
                    WhenAcquired = DateTime.Now,
                },
                new Dinosaur()
                {
                    Name = "V",
                    DietType = "carnivore",
                    Weight = 320,
                    EnclosureNumber = 10,
                    WhenAcquired = DateTime.Now,
                },
            };
            return listOfDinosaurs;
        }

        static void Main(string[] args)
        {
            var listOfDinosaurs = PopulateJurassicPark();

            var keepGoing = true;

            while (keepGoing)
            {
                Menu(listOfDinosaurs);

                keepGoing = false;
            }
        }
    }
}