using System;

namespace ConsoleRPG
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    // Класс игры, управляющий основным процессом
    class Game
    {
        private Player player;
        private Enemy enemy;
        private BattleSystem battleSystem;

        public void Start()
        {
            Console.WriteLine("Добро пожаловать в консольную RPG!");
            Console.WriteLine("Создаем вашего персонажа...");
            
            player = new Player("Герой", 100, 15);
            enemy = new Enemy("Гоблин", 60, 10);
            battleSystem = new BattleSystem(player, enemy);
            
            Console.WriteLine($"Игрок: {player.Name} (Здоровье: {player.Health}, Урон: {player.Damage})");
            Console.WriteLine($"Враг: {enemy.Name} (Здоровье: {enemy.Health}, Урон: {enemy.Damage})");
            
            Console.WriteLine("\nНажмите любую клавишу для начала боя...");
            Console.ReadKey();
            
            battleSystem.StartBattle();
        }
    }

    // Базовый класс для всех персонажей
    abstract class Character
    {
        public string Name { get; private set; }
        public int Health { get; protected set; }
        public int Damage { get; protected set; }
        public bool IsAlive => Health > 0;

        protected Character(string name, int health, int damage)
        {
            Name = name;
            Health = health;
            Damage = damage;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }

        public abstract void Attack(Character target);
    }

    // Класс игрока
    class Player : Character
    {
        public Player(string name, int health, int damage) : base(name, health, damage) { }

        public override void Attack(Character target)
        {
            Console.WriteLine($"{Name} атакует {target.Name} и наносит {Damage} урона!");
            target.TakeDamage(Damage);
        }

        // Дополнительный метод для игрока (можно расширить)
        public void Heal(int amount)
        {
            Health += amount;
            Console.WriteLine($"{Name} восстанавливает {amount} здоровья!");
        }
    }

    // Класс врага
    class Enemy : Character
    {
        public Enemy(string name, int health, int damage) : base(name, health, damage) { }

        public override void Attack(Character target)
        {
            Console.WriteLine($"{Name} атакует {target.Name} и наносит {Damage} урона!");
            target.TakeDamage(Damage);
        }
    }

    // Класс системы боя
    class BattleSystem
    {
        private readonly Player player;
        private readonly Enemy enemy;
        private int round = 1;

        public BattleSystem(Player player, Enemy enemy)
        {
            this.player = player;
            this.enemy = enemy;
        }

        public void StartBattle()
        {
            Console.WriteLine("\n=== БОЙ НАЧАЛСЯ ===");
            
            while (player.IsAlive && enemy.IsAlive)
            {
                Console.WriteLine($"\nРаунд {round++}");
                Console.WriteLine($"Игрок: {player.Health} HP | Враг: {enemy.Health} HP");
                
                // Ход игрока
                player.Attack(enemy);
                if (!enemy.IsAlive) break;
                
                // Ход врага
                enemy.Attack(player);
                
                Console.ReadKey();
            }

            EndBattle();
        }

        private void EndBattle()
        {
            Console.WriteLine("\n=== БОЙ ОКОНЧЕН ===");
            
            if (player.IsAlive)
            {
                Console.WriteLine($"Победа! {player.Name} победил {enemy.Name}!");
            }
            else
            {
                Console.WriteLine($"Поражение! {enemy.Name} победил {player.Name}!");
            }
        }
    }
}
