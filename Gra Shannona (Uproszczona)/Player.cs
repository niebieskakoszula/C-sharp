using System;
using System.Collections.Generic;

namespace Jipp_4_Projekt_Uproszczony_v2_2
{
    abstract class Player
    {
        protected int size;
        protected Abstract_Field[,] mine_fields;
        protected Abstract_Field[,] enemy_fields;
        protected Blockade blokada = new Blockade();
        public Player(int size, ref Abstract_Field[,] mine, ref Abstract_Field[,] enemy) { this.size = size; mine_fields = mine; enemy_fields = enemy; }
        public abstract int[] Move();
        protected void Connect_Fields(int[] directions)
        {
            if (directions[0] > directions[2] || directions[1] > directions[3])
            {
                int x = directions[0];
                directions[0] = directions[2];
                directions[2] = x;
                x = directions[1];
                directions[1] = directions[3];
                directions[3] = x;
            }

            if (directions[0] != directions[2])
            {
                ((Field)mine_fields[directions[0], directions[1]]).Connect(2);
                ((Field)enemy_fields[directions[3] - 1, directions[2]]).Block(blokada, 2);
                ((Field)mine_fields[directions[2], directions[3]]).Connect(4);
                ((Field)enemy_fields[directions[3], directions[2]]).Block(blokada, 4);
            }
            else
            {
                ((Field)mine_fields[directions[0], directions[1]]).Connect(3);
                ((Field)enemy_fields[directions[1], directions[0]]).Block(blokada, 3);
                ((Field)mine_fields[directions[2], directions[3]]).Connect(1);
                ((Field)enemy_fields[directions[1], directions[0] + 1]).Block(blokada, 1);
            }
        }
    }
    class Person : Player
    {
        public Person(int size, ref Abstract_Field[,] mine, ref Abstract_Field[,] enemy) : base(size, ref mine, ref enemy) { }
        public override int[] Move()
        {
            int[] vs = new int[4];
            do
            {
                Console.WriteLine("Podaj wiersz/kolumnę pierwszej koordynaty: ");
                vs[0] = Check_Input();
                if (vs[0] == -int.MaxValue) break;
                Console.WriteLine("Podaj numer pierwszej koordynaty: ");
                vs[1] = Check_Input();
                if (vs[1] == -int.MaxValue) break;
                Console.WriteLine("Podaj wiersz/kolumnę drugiej koordynaty: ");
                vs[2] = Check_Input();
                if (vs[2] == -int.MaxValue) break;
                Console.WriteLine("Podaj numer drugiej koordynaty: ");
                vs[3] = Check_Input();
                if (vs[3] == -int.MaxValue) break;
            } while (Check_Move(vs));
            Connect_Fields(vs);
            return vs;
        }
        bool Check_Move(int[] directions)
        {
            for (int i = 0; i < 4; i++)
                if (directions[i] > size || directions[i] < 1)
                {
                    Console.WriteLine("Dane spoza zakresu");
                    return false;
                }

            if ((directions[1] == 1 && directions[3] == 1) || (directions[1] == size && directions[3] == size))
            {
                Console.WriteLine("Nie możesz łączyć punktów na ścianach");
                return false;
            }

            int a = Math.Abs(directions[0] - directions[2]), b = Math.Abs(directions[1] - directions[3]);
            if (a > 1 || b > 1) { Console.WriteLine("Punkty zbyt daleko od siebie"); return false; }
            if (a == 0 && b == 0) { Console.WriteLine("To jest ten sam punkt"); return false; }
            if (a == 1 && b == 1) { Console.WriteLine("Nie możesz łączyć na skos"); return false; }

            if (directions[0] > directions[2] || directions[1] > directions[3])
            {
                int x = directions[0];
                directions[0] = directions[2];
                directions[2] = x;
                x = directions[1];
                directions[1] = directions[3];
                directions[3] = x;
            }


            if (a == 1 && mine_fields[directions[0], directions[1]].Get_Connection("right") == 1)
                return true;
            else if (b == 1 && mine_fields[directions[0], directions[1]].Get_Connection("down") == 1)
                return true;
            
            Console.WriteLine("Połączenie już zajęte");
            return false;
        }
        int Check_Input()
        {
            int number;
            string input;
            while (true)
            {
                input = Console.ReadLine();
                if(input == "poddaję się") return -int.MaxValue;
                if (int.TryParse(input, out number)) break;
                Console.WriteLine("Niepoprawne wejście, spróbuj ponownie.");
            }
            return number;
        }
    }
    abstract class Bot : Player
    {
        protected bool[,] mine_path;
        protected Tuple<int, int> mine_start;
        protected bool[,] enemy_path;
        protected Tuple<int, int> enemy_start;
        protected int[] reaction;
        public Bot(int n, ref Abstract_Field[,] p, ref Abstract_Field[,] w) : base(n, ref p, ref w) { }
        public override abstract int[] Move();
        protected void Correct_Distance_Of_Fields(ref Abstract_Field[,] fields)
        {
            Queue<int> priority_queue = new Queue<int>();
            Queue<int> queue = new Queue<int>();

            for (int i = 1; i < size; i++)
            {
                priority_queue.Enqueue(i);
                priority_queue.Enqueue(1);
                fields[i, 1].Distance = 0;
                for (int j = 2; j <= size; j++)
                {
                    fields[i, j].Distance = int.MaxValue;
                }
            }

            int x, y;
            int[] diffrecess;
            while (priority_queue.Count != 0 || queue.Count != 0)
            {
                if (priority_queue.Count != 0)
                {
                    x = priority_queue.Peek(); priority_queue.Dequeue();
                    y = priority_queue.Peek(); priority_queue.Dequeue();
                }
                else
                {
                    x = queue.Peek(); queue.Dequeue();
                    y = queue.Peek(); queue.Dequeue();
                }

                diffrecess = fields[x, y].Get_Diffrecess();
                if (diffrecess[0] == 3)
                {
                    if (fields[x, y].Get_Connection("up") == 0) { fields[x, y - 1].Distance = fields[x, y].Distance; priority_queue.Enqueue(x); priority_queue.Enqueue(y - 1); }
                    else { fields[x, y - 1].Distance = fields[x, y].Distance + 1; queue.Enqueue(x); queue.Enqueue(y - 1); }
                }
                if (diffrecess[1] == 3)
                {
                    if (fields[x, y].Get_Connection("right") == 0) { fields[x + 1, y].Distance = fields[x, y].Distance; priority_queue.Enqueue(x + 1); priority_queue.Enqueue(y); }
                    else { fields[x + 1, y].Distance = fields[x, y].Distance + 1; queue.Enqueue(x + 1); queue.Enqueue(y); }
                }
                if (diffrecess[2] == 3)
                {
                    if (fields[x, y].Get_Connection("down") == 0) { fields[x, y + 1].Distance = fields[x, y].Distance; priority_queue.Enqueue(x); priority_queue.Enqueue(y + 1); }
                    else { fields[x, y + 1].Distance = fields[x, y].Distance + 1; queue.Enqueue(x); queue.Enqueue(y + 1); }
                }
                if (diffrecess[3] == 3)
                {
                    if (fields[x, y].Get_Connection("left") == 0) { fields[x - 1, y].Distance = fields[x, y].Distance; priority_queue.Enqueue(x - 1); priority_queue.Enqueue(y); }
                    else { fields[x - 1, y].Distance = fields[x, y].Distance + 1; queue.Enqueue(x - 1); queue.Enqueue(y); }
                }
            }
        }
        protected void Search_For_Path(ref bool[,] path, ref Abstract_Field[,] fields, ref Tuple<int, int> start_point)
        {
            int min = int.MaxValue, x = 0, y = size;
            Random r = new Random();

            for (int i = 1; i < size; i++)
            {
                if (fields[i, size].Distance < min || (fields[i, size].Distance == min && r.NextDouble() < 0.5f))
                {
                    min = fields[i, size].Distance;
                    x = i;
                }
            }

            int lenght = fields[x, y].Distance;
            path = new bool[size + 1, size + 2];
            path[x, y] = true;

            Abstract_Field current;
            List<int> path_of_connected;
            int[] diffrecess;
            while (lenght > 0)
            {
                current = fields[x, y];
                diffrecess = current.Get_Diffrecess();
                if (diffrecess[4] == 1)
                {
                    if (diffrecess[0] == -1) y--;
                    else if (diffrecess[1] == -1) x++;
                    else if (diffrecess[2] == -1) y++;
                    else if (diffrecess[3] == -1) x--;

                    path[x, y] = true;
                    lenght--;
                }
                else
                {
                    path_of_connected = Search_For_Path_Of_Connected(x, y, ref fields);
                    for (int i = 0; i < path_of_connected.Count; i += 2)
                        path[path_of_connected[i], path_of_connected[i + 1]] = true;
                    x = path_of_connected[path_of_connected.Count - 2];
                    y = path_of_connected[path_of_connected.Count - 1];
                }
            }

            start_point = new Tuple<int, int>(x, y);
        }
        protected List<int> Search_For_Path_Of_Connected(int x, int y, ref Abstract_Field[,] punkty)
        {
            List<int> path = new List<int>();
            bool[,] visited = new bool[size + 1, size + 2];
            path.Add(x); path.Add(y);
            Abstract_Field current;
            int[] diffrecess;
            int previus = 0;
            while (true)
            {
                current = punkty[x, y];
                diffrecess = current.Get_Diffrecess();
                if (diffrecess[4] == 1) return path;

                if (diffrecess[0] == 0 && !visited[x, y - 1] && previus != 1) { y--; path.Add(x); path.Add(y); previus = 3; }
                else if (diffrecess[1] == 0 && !visited[x + 1, y] && previus != 2) { x++; path.Add(x); path.Add(y); previus = 4; }
                else if (diffrecess[2] == 0 && !visited[x, y + 1] && previus != 3) { y++; path.Add(x); path.Add(y); previus = 1; }
                else if (diffrecess[3] == 0 && !visited[x - 1, y] && previus != 4) { x--; path.Add(x); path.Add(y); previus = 2; }
                else
                {
                    visited[x, y] = true;
                    path.RemoveAt(path.Count - 1);
                    path.RemoveAt(path.Count - 1);
                    x = path[path.Count - 2];
                    y = path[path.Count - 1];
                    previus = 0;
                }
            }
        }
    }
    class BotBloker : Bot
    {
        public BotBloker(int n, ref Abstract_Field[,] p, ref  Abstract_Field[,] w) : base(n, ref p, ref w) { }
        public override int[] Move()
        {
            Search_For_Dangers();
            Connect_Fields(reaction);
            return reaction;
        }
        protected void Search_For_Dangers()
        {
            Correct_Distance_Of_Fields(ref mine_fields);
            Correct_Distance_Of_Fields(ref enemy_fields);
            Search_For_Path(ref mine_path, ref mine_fields, ref mine_start);
            Search_For_Path(ref enemy_path, ref enemy_fields, ref enemy_start);

            int x = mine_start.Item1, y = mine_start.Item2;
            bool[,] visited = new bool[size + 1, size + 2];
            reaction = null;
            List<int[]> suggestions = new List<int[]>();
            while (true)
            {
                if (!visited[x, y + 1] && mine_path[x, y + 1] && mine_fields[x, y].Get_Connection("down") != -1)
                {
                    if (enemy_path[y, x] && enemy_path[y, x + 1] && mine_fields[x, y].Get_Connection("down") == 1)
                        suggestions.Add(new int[4] { x, y, x, y + 1 });

                    visited[x, y] = true;
                    y++;
                }
                else if (!visited[x + 1, y] && mine_path[x + 1, y] && mine_fields[x, y].Get_Connection("right") != -1)
                {
                    if (enemy_path[y - 1, x + 1] && enemy_path[y, x + 1] && mine_fields[x, y].Get_Connection("right") == 1)
                        suggestions.Add(new int[4] { x, y, x + 1, y });

                    visited[x, y] = true;
                    x++;
                }
                else if (!visited[x - 1, y] && mine_path[x - 1, y] && mine_fields[x, y].Get_Connection("left") != -1)
                {
                    if (enemy_path[y - 1, x] && enemy_path[y, x] && mine_fields[x, y].Get_Connection("left") == 1)
                        suggestions.Add(new int[4] { x, y, x - 1, y });

                    visited[x, y] = true;
                    x--;
                }
                else if (!visited[x, y - 1] && mine_path[x, y - 1] && mine_fields[x, y].Get_Connection("up") != -1)
                {
                    if (enemy_path[y - 1, x] && enemy_path[y - 1, x + 1] && mine_fields[x, y].Get_Connection("up") == 1)
                        suggestions.Add(new int[4] { x, y, x, y - 1 });

                    visited[x, y] = true;
                    y--;
                }
                else break;
            }

            Choose_Greatest_Danger(suggestions);
        }
        void Choose_Greatest_Danger(List<int[]> suggestions)
        {
            int minn, best = 0;
            Random r = new Random();
            foreach(int[] connection in suggestions)
            {
                minn = int.MaxValue;
                if (connection[0] > connection[2] || connection[1] > connection[3])
                {
                    int x = connection[0];
                    connection[0] = connection[2];
                    connection[2] = x;
                    x = connection[1];
                    connection[1] = connection[3];
                    connection[3] = x;
                }

                if (connection[0] != connection[2])
                {
                    ((Field)enemy_fields[connection[3] - 1, connection[2]]).Temporary_Block(2);
                    ((Field)enemy_fields[connection[3], connection[2]]).Temporary_Block(4);
                }
                else
                {
                    ((Field)enemy_fields[connection[1], connection[0]]).Temporary_Block(3);
                    ((Field)enemy_fields[connection[1], connection[0] + 1]).Temporary_Block(1);
                }

                Correct_Distance_Of_Fields(ref enemy_fields);
                for (int i = 1; i < size; i++)
                {
                    minn = Math.Min(minn, enemy_fields[i, size].Distance);
                }
                if (connection[0] != connection[2])
                {
                    ((Field)enemy_fields[connection[3] - 1, connection[2]]).Remove_Temporary_Block(2);
                    ((Field)enemy_fields[connection[3], connection[2]]).Remove_Temporary_Block(4);
                }
                else
                {
                    ((Field)enemy_fields[connection[1], connection[0]]).Remove_Temporary_Block(3);
                    ((Field)enemy_fields[connection[1], connection[0] + 1]).Remove_Temporary_Block(1);
                }

                if (minn > best || (minn == best && r.NextDouble() < 0.5f) )
                {
                    reaction = connection;
                    best = minn;
                }
            }
        }
    }
}

