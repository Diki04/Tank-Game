// Rizkillah Ramanda Sinyo
using System;
using System.Collections.Generic;
using static System.Random;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
    string[] Tank ={
	null!,
	// Atas
	@" _^_ " + "\n" +
	@"|___|" + "\n" +
	@"[ooo]" + "\n",
	// Bawah
	@"[ooo] " + "\n" +
	@"|_ _|" + "\n" +
	@"  |  " + "\n",
	// Kiri
	@"  __ " + "\n" +
	@"=|__|" + "\n" +
	@"[ooo]" + "\n",
	// Kanan
	@" __  " + "\n" +
	@"|__|=" + "\n" +
	@"[ooo]" + "\n",
	
};
string[] TankShooting ={
	null!,
	// Atas
	@" _█_ " + "\n" +
	@"|___|" + "\n" +
	@"[ooo]" + "\n",
	// Bawah
	@" ___ " + "\n" +
	@"|_█_|" + "\n" +
	@"[ooo]" + "\n",
	// Kiri
	@"  __ " + "\n" +
	@"█|__|" + "\n" +
	@"[ooo]" + "\n",
	// Kanan
	@" __  " + "\n" +
	@"|__|█" + "\n" +
	@"[ooo]" + "\n",
	
};

string[] TankExploding ={
	// Sebelum
	@" ___ " + "\n" +
	@"|___|" + "\n" +
	@"[ooo]" + "\n",
	// Animasi ledakan
	@"█████" + "\n" +
	@"█████" + "\n" +
	@"█████" + "\n",
	// Musnah
	@"     " + "\n" +
	@"     " + "\n" +
	@"     " + "\n",
	
};
char[] Bullet =
{
	default,
	'^', // Atas
	'v', // Bawah
	'<', // Kiri
	'>', // Kanan
};

string Map =
	
	@"╔═════════════════════════════════════════════════════════════════════════╗" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                    ║                                    ║" + "\n" +
	@"║                                    ║                                    ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║     ═════                                                     ═════     ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                    ║                                    ║" + "\n" +
	@"║                                    ║                                    ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"║                                                                         ║" + "\n" +
	@"╚═════════════════════════════════════════════════════════════════════════╝" + "\n";
	

var Tanks = new List<Tank>();
var AllTanks = new List<Tank>();
var Player = new Tank() { X = 08, Y = 05, IsPlayer = true };

Tanks.Add(Player);
Tanks.Add(new Tank() { X = 08, Y = 21, });
Tanks.Add(new Tank() { X = 66, Y = 05, });
Tanks.Add(new Tank() { X = 66, Y = 21, });
AllTanks.AddRange(Tanks);

Console.CursorVisible = false;
if (OperatingSystem.IsWindows())
{
	Console.WindowWidth = Math.Max(Console.WindowWidth, 90);
	Console.WindowHeight = Math.Max(Console.WindowHeight, 35);
}
Console.Clear();
Console.SetCursorPosition(0, 0);
Render(Map);
Console.WriteLine();
Console.WriteLine("Gunakan tombol (W, A, S, D) untuk bergerak dan tombol panah untuk menembak.");

static void Render(string @string, bool renderSpace = false)
{
	int x = Console.CursorLeft;
	int y = Console.CursorTop;
	foreach (char c in @string)
		if (c is '\n') Console.SetCursorPosition(x, ++y);
		else if (c is not ' ' || renderSpace) Console.Write(c);
		else Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
}

while (Tanks.Contains(Player) && Tanks.Count > 1)
{
	foreach (var tank in Tanks)
	{
		if (tank.IsShooting)
		{
			tank.Bullet = new Bullet()
			{
				X = tank.Arah switch
				{
					Arah.Left => tank.X - 3,
					Arah.Right => tank.X + 3,
					_ => tank.X,
				},
				Y = tank.Arah switch
				{
					Arah.Up => tank.Y - 2,
					Arah.Down => tank.Y + 2,
					_ => tank.Y,
				},
				Arah = tank.Arah,
			};
			tank.IsShooting = false;
			continue;
		}
		if (tank.IsExploding)
		{
			tank.ExplodingFrame++;
			Console.SetCursorPosition(tank.X - 2, tank.Y - 1);
			Render(tank.ExplodingFrame > 9
				? TankExploding[2]
				: TankExploding[tank.ExplodingFrame % 2], true);
			continue;
		}

		bool CekGerakan(Tank gerakTank, int X, int Y)
		{
			foreach (var tank in Tanks)
			{
				if (tank == gerakTank)
				{
					continue;
				}
				if (Math.Abs(tank.X - X) <= 4 && Math.Abs(tank.Y - Y) <= 2)
				{
					return false; // Tabrakan dengan tank
				}
			}
			if (X < 3 || X > 71 || Y < 2 || Y > 25)
			{
				return false; // Tabrakan dengan pembatas
			}
			if (3 < X && X < 13 && 11 < Y && Y < 15)
			{
				return false; // Tabrakan dengan pembatas kiri
			}
			if (34 < X && X < 40 && 2 < Y && Y < 8)
			{
				return false; // Tabrakan dengan pembatas atas
			}
			if (34 < X && X < 40 && 19 < Y && Y < 25)
			{
				return false; // Tabrakan dengan pembatas bawah
			}
			if (61 < X && X < 71 && 11 < Y && Y < 15)
			{
				return false; // Tabrakan dengan pembatas kanan
			}
			return true;
		}
		void GerakTank(Arah Arah)
		{
			switch (Arah)
			{
				case Arah.Up:
					if (CekGerakan(tank, tank.X, tank.Y - 1))
					{
						Console.SetCursorPosition(tank.X - 2, tank.Y + 1);
						Console.Write(' ');
						Console.SetCursorPosition(tank.X - 1, tank.Y + 1);
						Console.Write(' ');
						Console.SetCursorPosition(tank.X, tank.Y + 1);
						Console.Write(' ');
						Console.SetCursorPosition(tank.X + 1, tank.Y + 1);
						Console.Write(' ');
						Console.SetCursorPosition(tank.X + 2, tank.Y + 1);
						Console.Write(' ');
						tank.Y--;
					}
					break;
				case Arah.Down:
					if (CekGerakan(tank, tank.X, tank.Y + 1))
					{
						Console.SetCursorPosition(tank.X - 2, tank.Y - 1);
						Console.Write(' ');
						Console.SetCursorPosition(tank.X - 1, tank.Y - 1);
						Console.Write(' ');
						Console.SetCursorPosition(tank.X, tank.Y - 1);
						Console.Write(' ');
						Console.SetCursorPosition(tank.X + 1, tank.Y - 1);
						Console.Write(' ');
						Console.SetCursorPosition(tank.X + 2, tank.Y - 1);
						Console.Write(' ');
						tank.Y++;
					}
					break;
				case Arah.Left:
					if (CekGerakan(tank, tank.X - 1, tank.Y))
					{
						Console.SetCursorPosition(tank.X + 2, tank.Y - 1);
						Console.Write(' ');
						Console.SetCursorPosition(tank.X + 2, tank.Y);
						Console.Write(' ');
						Console.SetCursorPosition(tank.X + 2, tank.Y + 1);
						Console.Write(' ');
						tank.X--;
					}
					break;
				case Arah.Right:
					if (CekGerakan(tank, tank.X + 1, tank.Y))
					{
						Console.SetCursorPosition(tank.X - 2, tank.Y - 1);
						Console.Write(' ');
						Console.SetCursorPosition(tank.X - 2, tank.Y);
						Console.Write(' ');
						Console.SetCursorPosition(tank.X - 2, tank.Y + 1);
						Console.Write(' ');
						tank.X++;
					}
					break;
			}
		}
		if (tank.IsPlayer)
		{
			
			Arah? move = null;
			Arah? shoot = null;
			while (Console.KeyAvailable)
			{
				switch (Console.ReadKey(true).Key)
				{
					// pergerakan tank
					case ConsoleKey.W: move = move.HasValue ? Arah.Null : Arah.Up; break;
					case ConsoleKey.S: move = move.HasValue ? Arah.Null : Arah.Down; break;
					case ConsoleKey.A: move = move.HasValue ? Arah.Null : Arah.Left; break;
					case ConsoleKey.D: move = move.HasValue ? Arah.Null : Arah.Right; break;

					// Arah tembakan
					case ConsoleKey.UpArrow: shoot = shoot.HasValue ? Arah.Null : Arah.Up; break;
					case ConsoleKey.DownArrow: shoot = shoot.HasValue ? Arah.Null : Arah.Down; break;
					case ConsoleKey.LeftArrow: shoot = shoot.HasValue ? Arah.Null : Arah.Left; break;
					case ConsoleKey.RightArrow: shoot = shoot.HasValue ? Arah.Null : Arah.Right; break;

					// Game selesai
					case ConsoleKey.Escape:
						Console.Clear();
						Console.Write("Tanks was Done!.");
						return;
				}
				while (Console.KeyAvailable)
				{
					Console.ReadKey(true);
				}
			}

			tank.IsShooting = shoot.HasValue && shoot.Value is not Arah.Null && tank.Bullet is null;
			if (tank.IsShooting)
			{
				tank.Arah = shoot ?? tank.Arah;
			}

			if (move.HasValue)
				GerakTank(move.Value);
		    }
		else
		{
			int randomIndex = Random.Shared.Next(0, 6);
			if (randomIndex < 4)
				GerakTank((Arah)randomIndex + 1);

			if (tank.Bullet is null)
			{
				Arah shoot = Math.Abs(tank.X - Player.X) > Math.Abs(tank.Y - Player.Y)
					? (tank.X < Player.X ? Arah.Right : Arah.Left)
					: (tank.Y > Player.Y ? Arah.Up : Arah.Down);
				tank.Arah = shoot;
				tank.IsShooting = true;
			}
		}

		Console.SetCursorPosition(tank.X - 2, tank.Y - 1);
		Render(tank.IsExploding
			? TankExploding[tank.ExplodingFrame % 2]
			: tank.IsShooting
				? TankShooting[(int)tank.Arah]
				: Tank[(int)tank.Arah],
			true);
	}

	bool TabrakanBulletCek(Bullet bullet, out Tank tabrakanTank)
	{
		tabrakanTank = null!;
		foreach (var tank in Tanks)
		{
			if (Math.Abs(bullet.X - tank.X) < 3 && Math.Abs(bullet.Y - tank.Y) < 2)
			{
				tabrakanTank = tank;
				return true;
			}
		}
		if (bullet.X is 0 || bullet.X is 74 || bullet.Y is 0 || bullet.Y is 27)
		{
			return true;
		}
		if (5 < bullet.X && bullet.X < 11 && bullet.Y is 13)
		{
			return true; 
		}
		if (bullet.X is 37 && 3 < bullet.Y && bullet.Y < 7)
		{
			return true; 
		}
		if (bullet.X is 37 && 20 < bullet.Y && bullet.Y < 24)
		{
			return true; 
		}
		if (63 < bullet.X && bullet.X < 69 && bullet.Y is 13)
		{
			return true;
		}
		return false;
	}

	foreach (var tank in AllTanks)
	{
		if (tank.Bullet is not null)
		{
			var bullet = tank.Bullet;
			Console.SetCursorPosition(bullet.X, bullet.Y);
			Console.Write(' ');
			switch (bullet.Arah)
			{
				case Arah.Up: bullet.Y--; break;
				case Arah.Down: bullet.Y++; break;
				case Arah.Left: bullet.X--; break;
				case Arah.Right: bullet.X++; break;
			}
			Console.SetCursorPosition(bullet.X, bullet.Y);
			bool Tabrakan = TabrakanBulletCek(bullet, out Tank tabrakanTank);
			Console.Write(Tabrakan
				? '█'
				: Bullet[(int)bullet.Arah]);
			if (Tabrakan)
			{
				if (tabrakanTank is not null && --tabrakanTank.Health <= 0)
				{
					tabrakanTank.ExplodingFrame = 1;
				}
				tank.Bullet = null!;
			}
		}
	}

	for (int i = 0; i < Tanks.Count; i++)
	{
		if (Tanks[i].ExplodingFrame > 10)
		{
			Tanks.RemoveAt(i);
			i--;
		}
	}
	Console.SetCursorPosition(0, 0);
	Render(Map);
	Thread.Sleep(TimeSpan.FromMilliseconds(30));
}
Console.SetCursorPosition(0, 33);
Console.Clear();
Console.Write(Tanks.Contains(Player)
	? "You Win!."
	: "You Lose!.");
Console.ReadLine();
    }
}
enum Arah
{
	Null = 0,
	Up = 1,
	Down = 2,
	Left = 3,
	Right = 4,
}
class Tank
{
	public bool IsPlayer;
	public int Health = 4;
	public int X;
	public int Y;
	public Arah Arah = Arah.Down;
	public Bullet? Bullet;
	public bool IsShooting;
	public int ExplodingFrame;
	public bool IsExploding => ExplodingFrame > 0;
}

class Bullet
{
	public int X;
	public int Y;
	public Arah Arah;
}
