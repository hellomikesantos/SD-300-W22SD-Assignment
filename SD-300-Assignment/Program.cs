// WEAPON INSTANCES
Weapon Axe = new Weapon("Axe", 115);
Weapon Sword = new Weapon("Sword", 222);
Weapon Flamethrower = new Weapon("Flamethrower", 201);

// ARMOR INSTANCES
Armor Bronze = new Armor("Bronze Shield", 10);
Armor Silver = new Armor("Silver Shield", 20);
Armor Gold = new Armor("Gold Shield", 35);

// MONSTER INSTANCES
Monster Demogorgon = new Monster("Demogorgon", 90);
Monster GhostRider = new Monster("Ghost Rider", 81);
Monster Thanos = new Monster("Thanos", 196);
Monster Voldemort = new Monster("Voldemort", 161);
Monster SilverSurfer = new Monster("Silver Surfer", 190);

Game Dungeons = new Game();
Dungeons.Start();

class Hero
{
    public Game Game { get; set; }
    public string Name { get; set; }
    public int Coins { get; set; } = 0;
    public int BaseStrength { get; set; } = 70;
    public int EnhancedStrength { get; set; }
    public int BaseDefence { get; set; } = 35;
    public int EnhancedDefence { get; set; } 
    public int OriginalHealth { get; set; } = 600;
    public int CurrentHealth { get; set; } = 600;
    public Inventory Inventory { get; set; }
    public Statistics Statistics { get; set; }
    public Weapon EquippedWeapon { get; set; }
    public Armor EquippedArmor { get; set; }
    public void Enhance()
    {
        if(Coins <= 0)
        {
            Console.WriteLine("Not enough coins... Gain 10 coins for each fight won.");
            Game.MainMenu.PromptMenu();
        }
        else
        {
            Console.WriteLine("Choose item to enhance:");
            Console.WriteLine($"STRENGTH - PRESS S");
            Console.WriteLine($"DEFENCE - PRESS D");
            Console.WriteLine("MAIN MENU - PRESS M");
            switch (Console.ReadKey(true).KeyChar)
            {
                case 's':
                    EnhancedStrength += Coins;
                    Coins = 0;
                    Console.WriteLine($"Your strength is now: {EnhancedStrength}");
                    Console.WriteLine($"You now have {Coins} coins");
                    break;
                case 'd':
                    EnhancedDefence += Coins;
                    Coins = 0;
                    Console.WriteLine($"Your defence is now: {EnhancedDefence}");
                    Console.WriteLine($"You now have {Coins} coins");
                    break;
                case 'm':
                    Game.MainMenu.PromptMenu();
                    break;
            }
        }
    }
    public string ShowStats()
    {
        return $"Name: {Name}, Base Strength: {BaseStrength}, Base Defence: {BaseDefence}, " +
            $"OriginalHealth: {OriginalHealth}, CurrentHealth: {CurrentHealth}";
    }
    public Hero(Statistics statistics)
    {
        EquippedWeapon = WeaponList.Weapons[0];
        EquippedArmor = ArmorList.Armors[0];
        Statistics = statistics;
        EnhancedStrength = BaseStrength;
        EnhancedDefence = BaseDefence;
    }
}
class Monster
{
    public string Name { get; set; }
    public int Strength { get; set; }
    public int Defence { get; set; }
    public int OriginalHealth { get; set; } = 600;
    public int CurrentHealth { get; set; } = 600;
    public bool Defeated { get; set; } = false;
    public Monster(string name, int strength)
    {
        Name = name;
        Strength = strength;

        MonsterList.ListMonster(this);
    }
}
class Weapon
{
    public string Name { get; set; }
    public int Power { get; set; }
    public Weapon(string name, int power)
    {
        Name = name;
        Power = power;

        WeaponList.ListWeapon(this);
    }
}
class Armor
{
    public string Name { get; set; }
    public int Power { get; set; }
    public Armor(string name, int power)
    {
        Name = name;
        Power = power;

        ArmorList.ListArmor(this);
    }
}
static class WeaponList
{
    public static List<Weapon> Weapons { get; set; } = new List<Weapon>();
    public static List<Weapon> ListWeapon(Weapon weapon)
    {
        Weapons.Add(weapon);
        return Weapons;
    }
}
static class ArmorList
{
    public static List<Armor> Armors { get; set; } = new List<Armor>();
    public static List<Armor> ListArmor(Armor armor)
    {
        Armors.Add(armor);
        return Armors;
    }
}
static class MonsterList
{
    public static int DefeatedCounter { get; set; } = 0;
    public static List<Monster> Monsters { get; set; } = new List<Monster>();
    public static List<Monster> ListMonster(Monster monster)
    {
        Monsters.Add(monster);
        return Monsters;
    }
}

//HeroTurn (calculates an handles “damage” to a monster as Hero BaseStrength + EquippedWeapon Power)
//MonsterTurn(calculates and handles “damage” to the Hero as Monster Strength – (Hero BaseDefence­ + EquippedArmour Power)
//Win(check and handle if the Monster CurrentHealth reaches 0.If the Hero wins, the Monster should no longer appear in the game, until the Hero Loses.)
//Lose(check and handle if the Player CurrentHealth eaches 0.If the Hero Loses, their CurrentHealth is set to equal their OriginalHealth, and any Monsters that were previously defeated can appear again).
class Fight
{
    public Game Game { get; set; }
    public Hero Hero { get; set; }
    public MainMenu MainMenu { get; set; }
    public int MonsterIndex { get; set; }
    public bool HeroWon { get; set; } = false;
    public bool HeroDied { get; set; } = false;
    public Monster CurrentMonster { get; set; }

    public void ChangeMonster (Monster currentMonster) // when a monster is defeated, another monster will be activated as new opponent
    {
        foreach (Monster monster in MonsterList.Monsters)
        {
            if (monster.Defeated)
            {
                MonsterList.DefeatedCounter++;
            }
            if (monster != currentMonster && !monster.Defeated && MonsterList.DefeatedCounter > 0)
            {
                CurrentMonster = monster;
                Console.WriteLine($"~~~~~~~~~~~~~~ Get Ready! New Monster is coming!");
                Console.WriteLine($"NEW MONSTER OPPONENT! : {monster.Name}");
                break;
            } else
            {
                CurrentMonster = currentMonster;
            }
        }
    }
    public void StartFight() // Initiate the fight
    {
        if (HeroDied) // reset the monsters and hero health
        {
            // if the hero's dead, reset its health and reset defeated status in monster
            Hero.CurrentHealth = Hero.OriginalHealth;
            HeroDied = false;
            HeroWon = false;
            foreach (Monster defeatedMonster in MonsterList.Monsters)
            {
                defeatedMonster.Defeated = false;
                defeatedMonster.CurrentHealth = defeatedMonster.OriginalHealth;
            }
        }
        Console.WriteLine($"BEGIN FIGHT!");
        ChangeMonster(CurrentMonster);
        Console.WriteLine($"Your opponent is: {CurrentMonster.Name}");
        this.HeroTurn(CurrentMonster);

    }
    public void HeroTurn(Monster monsterOpponent) // performs the hero attack sequence 
    {
        //this.Win(CurrentMonster); 
        this.Lose(CurrentMonster); 
        if (!HeroWon && !HeroDied) 
        {
                Console.WriteLine($"It's your turn, ATTACK!");
                int damage = Hero.BaseStrength + Hero.EquippedWeapon.Power;
                monsterOpponent.CurrentHealth -= damage;
                Console.WriteLine($"{monsterOpponent.Name} has been damaged {damage}");
                if (monsterOpponent.CurrentHealth < 0) // if monster died
                {
                    monsterOpponent.CurrentHealth = 0; // always sets to zero, to avoid negative ints.
                }
                Console.WriteLine($"It now has {monsterOpponent.CurrentHealth} health");
                this.MonsterTurn(CurrentMonster);
        }
        else
        {
            return;
        }
    }
    public void MonsterTurn(Monster monster) // performs the opponent attack sequence
    {
        this.Win(CurrentMonster); 
        this.Lose(CurrentMonster); 
        if (!HeroWon && !HeroDied)
        {
            Console.WriteLine($"It's {monster.Name}'s turn");
            int damage = monster.Strength - Hero.BaseDefence;
            Hero.CurrentHealth -= damage;

            Console.WriteLine($"Ouch! You've been hurt by {damage} damage!");
            if (Hero.CurrentHealth < 0)
            {
                Hero.CurrentHealth = 0;
            }
            Console.WriteLine($"Your current health is {Hero.CurrentHealth}");
            this.HeroTurn(CurrentMonster);
        }
        else
        {
            return;
        }
    }
    public bool Win(Monster monster) // checks the opponent's health - to determine if it's a lose
    {
        if (monster.CurrentHealth <= 0)
        {
            monster.Defeated = true;
            Console.WriteLine($"BAAAAAM!!!! You slayed {monster.Name}!!!");
            Console.WriteLine($"You WON!");
            Hero.Coins += 3;
            Console.WriteLine($"You just received 3 coins, you now have {Hero.Coins} coins!");
            Hero.Statistics.GamesPlayed++;
            Hero.Statistics.FightsWon++;
            ChangeMonster(monster);
            HeroTurn(CurrentMonster);
            //ChangeMonster(monster);
            if (MonsterList.DefeatedCounter == MonsterList.Monsters.Count)
            {
                Game.MainMenu.PromptMenu();
                HeroWon = true;
            }
            else
            {
                HeroWon = false;
            } 
        }
        return false;
    }
    public bool Lose(Monster monster)
    {
        if (Hero.CurrentHealth <= 0)
        {
            Console.WriteLine($"Oh boy, you just died! Revenge is key! {monster.Name} just killed you!!!");
            Console.WriteLine($"{monster.Name} Won!");
            Hero.Statistics.GamesPlayed++;
            Hero.Statistics.FightsLost++;
            HeroDied = true;
            Game.MainMenu.PromptMenu();
        }
        return false;
    }
    public Fight(Hero hero, Game game)
    {
        Hero = hero;
        Game = game;
        Random randomNum = new Random(); // makes a random number to be used as index to get random monster from the monster list
        MonsterIndex = randomNum.Next(MonsterList.Monsters.Count);
        CurrentMonster = MonsterList.Monsters[MonsterIndex];
    }
}
class Statistics
{
    public Game Game { get; }
    public Hero Hero { get; set; }
    public int GamesPlayed { get; set; }
    public int FightsWon { get; set; }
    public int FightsLost { get; set; }
}
class Inventory
{
    public Hero Hero { get; set; } 
    public void ChangeEquippedWeapon() // option to change equipped weapon by pressing cassigned character key
    {
        int number = 0;
        try
        {
            Console.WriteLine($"Your current weapon is: {Hero.EquippedWeapon.Name}");
            Console.WriteLine($"Select your new weapon:");
            foreach (Weapon weapon in WeaponList.Weapons)
            {
                Console.WriteLine($"PRESS {number} - Name: {weapon.Name} | Power: {weapon.Power}");
                number++;
            }
            char charKey = Console.ReadKey(true).KeyChar;
            for (int i = 0; i < WeaponList.Weapons.Count; i++)
            {
                if (charKey == char.Parse(i.ToString()))
                {
                    Hero.EquippedWeapon = WeaponList.Weapons[i];
                    break;
                }
            }
            Console.WriteLine($"Your NEW current weapon is: {Hero.EquippedWeapon.Name}");
            Console.WriteLine("Back to MAIN MENU - PRESS M");
            Console.WriteLine("Press any key to continue...");
            if (Console.ReadKey(true).KeyChar == 'm')
            {
                Hero.Game.MainMenu.PromptMenu();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public void ChangeEquippedArmor() // option to change equipped armor by pressing assigned character key
    {
        // Change Armor
        int number = 0;
        Console.WriteLine($"Your current armor is: {Hero.EquippedArmor.Name}");
        Console.WriteLine($"Select your new armor:");
        foreach (Armor armor in ArmorList.Armors)
        {
            Console.WriteLine($"PRESS {number} - Name: {armor.Name} | Power: {armor.Power}");
            number++;
        }
        char charKey = Console.ReadKey(true).KeyChar;
        for (int i = 0; i < ArmorList.Armors.Count; i++)
        {
            if (charKey == char.Parse(i.ToString()))
            {
                Hero.EquippedArmor = ArmorList.Armors[i];
                break;
            }
        }
        Console.WriteLine($"Your NEW current armor is: {Hero.EquippedArmor.Name}");
        Console.WriteLine("Back to MAIN MENU - PRESS M");
        if (Console.ReadKey(true).KeyChar == 'm')
        {
            Hero.Game.MainMenu.PromptMenu();
        }
    }
    public Inventory(Hero hero)
    {
        Hero = hero;
    } 
}
class MainMenu
{
    public Game Game { get; }
    public Statistics Statistics { get; set; }
    public Inventory Inventory { get; set; }
    public Fight Fight { get; set; }
    public void PromptMenu() // displays menu options - always in the game.
    {
        Console.WriteLine($"+++++++++++++++++ Main Menu +++++++++++++++++");
        Console.WriteLine($"PRESS S for Statistics");
        Console.WriteLine($"PRESS I for Inventory");
        Console.WriteLine($"PRESS E to Enhance");
        Console.WriteLine($"PRESS F to FIGHT!!! Grrr!!!");
        switch (Console.ReadKey(true).KeyChar)
        {
            case 's':
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>> Statistics:");
                Console.WriteLine($"Total games played: {Statistics.GamesPlayed}");
                Console.WriteLine($"Total fights won: {Statistics.FightsWon}");
                Console.WriteLine($"Total fights lost: {Statistics.FightsLost}");
                PromptMenu();
                break;
            case 'i':
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>> Inventory:");
                Inventory.ChangeEquippedWeapon();
                Inventory.ChangeEquippedArmor();
                PromptMenu();
                break;
            case 'e':
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>> Enhance:");
                Console.WriteLine($"Your current Base Strength: {Inventory.Hero.EnhancedStrength}");
                Console.WriteLine($"Your current Base Defence: {Inventory.Hero.EnhancedDefence}");
                Console.WriteLine($"Available Coins: {Inventory.Hero.Coins}");
                Inventory.Hero.Enhance();
                PromptMenu();
                break;
            case 'f':
                Console.WriteLine("()*()*()*()*()*() GET READY TO FIGHT! ()*()*()*()*()*()");
                Fight.StartFight();
                break;
            default:
                Console.WriteLine($"INCORRECT KEY - Try again.");
                PromptMenu();
                break;
        }
    }

    public MainMenu(Statistics statistics, Inventory inventory, Fight fight)
    {
        Statistics = statistics;
        Inventory = inventory;
        Fight = fight;
    }
}
class Game
{
    public Statistics Stats { get; set; } = new Statistics();
    public Inventory Inventory { get; set; }
    public Fight Fight { get; set; }
    public Hero Hero { get; set; }
    public MainMenu MainMenu { get; set; }
    public void Start()
    {
        // begin game sequence
        Console.WriteLine("You've been chosen as a player, (Press ENTER to continue)");
        Console.WriteLine("Enter your name:");
        string name = Console.ReadLine();
        Hero.Name = name;
        Console.WriteLine($"You've chosen the great name '{ Hero.Name}'");
            MainMenu.PromptMenu();
    }
    public Game()
    { 
        Hero = new Hero(Stats);
        Hero.Game = this;
        Fight = new Fight(Hero, this);
        Inventory = new Inventory(Hero);
        MainMenu = new MainMenu(Stats, Inventory, Fight);
    }
}