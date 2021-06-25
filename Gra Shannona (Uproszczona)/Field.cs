namespace Jipp_4_Projekt_Uproszczony_v2_2
{
    abstract class Abstract_Field
    {

        protected Abstract_Field up;
        protected Abstract_Field right;
        protected Abstract_Field down;
        protected Abstract_Field left;
        public abstract Abstract_Field Up { get; }
        public abstract Abstract_Field Right { get; }
        public abstract Abstract_Field Down { get; }
        public abstract Abstract_Field Left { get; }
        protected int which_end;
        protected int distance;
        public abstract int Distance { get; set; }
        public int Which_end
        {
            get => which_end;
            set
            {
                if (value == 1 || value == 2) which_end = value;
            }
        }
        public abstract void Connect_Ends(int i);
        public abstract bool Check_For_Connections();
        public abstract int Get_Connection(string side);

        public abstract int[] Get_Diffrecess();
    }
    class Field : Abstract_Field
    {
        public Field(int distance) { this.distance = distance; which_end = 0;}
        public Field(int distance, int end) { this.distance = distance; this.which_end = end;}
        protected int[] connections = new int[4];
        public override int Distance
        {
            get => distance;
            set
            {
                if (value >= 0) distance = value;
            }
        }


        public override Abstract_Field Up => up;
        public override Abstract_Field Right => right;
        public override Abstract_Field Down => down;
        public override Abstract_Field Left => left;
        public void Set_Neighbors(Abstract_Field g, Abstract_Field p, Abstract_Field d, Abstract_Field l)
        {
            if (up != null) return;
            up = g; if (up is Field) connections[0] = 1; else connections[0] = -1;
            down = d; if (down is Field) connections[2] = 1; else connections[2] = -1;
            if (which_end != 0)
            {
                right = p; connections[1] = -1;
                left = l; connections[3] = -1;
            }
            else
            {
                right = p; if (right is Field) connections[1] = 1; else connections[1] = -1;
                left = l; if (left is Field) connections[3] = 1; else connections[3] = -1;
            }

        }

        public void Connect(int kierunek)
        {
            if (connections[kierunek - 1] != 1) return;
            connections[kierunek - 1] = 0;
            switch (kierunek)
            {
                case 1: up.Connect_Ends(which_end); break;
                case 2: right.Connect_Ends(which_end); break;
                case 3: down.Connect_Ends(which_end); break;
                case 4: left.Connect_Ends(which_end); break;
            }
        }
        public virtual void Block(Blockade b, int kierunek)
        {
            if (connections[kierunek - 1] != 1) return;
            switch (kierunek)
            {
                case 1: up = b; break;
                case 2: right = b; break;
                case 3: down = b; break;
                case 4: left = b; break;
            }
            connections[kierunek - 1] = -1;
        }
        public override void Connect_Ends(int i)
        {
            if (i == 0 || which_end == i) return;
            else if (which_end != 0 && which_end != i) which_end = 3;
            else which_end = i;

            if (connections[0] == 0) up.Connect_Ends(which_end);
            if (connections[1] == 0) right.Connect_Ends(which_end);
            if (connections[2] == 0) down.Connect_Ends(which_end);
            if (connections[3] == 0) left.Connect_Ends(which_end);
        }
        public override int Get_Connection(string side)
        {
            switch (side)
            {
                case "up": return connections[0];
                case "right": return connections[1];
                case "down": return connections[2];
                default: return connections[3];
            }
        }
        public override bool Check_For_Connections()
        {
            for (int i = 0; i < 4; i++)
                if (connections[i] == 0)
                    return true;

            return false;
        }


        public void Temporary_Block(int kierunek)
        {
            connections[kierunek - 1] = -1;
        }
        public void Remove_Temporary_Block(int kierunek)
        {
            switch (kierunek)
            {
                case 1: if(up is Blockade) return; break;
                case 2: if(right is Blockade) return; break;
                case 3: if(down is Blockade) return; break;
                case 4: if(left is Blockade) return; break;
            }
            connections[kierunek - 1] = 1;
        }


        public override int[] Get_Diffrecess()
        {
            int[] wynik = new int[5];
            if (distance == 0) wynik[4] = 1;

            Abstract_Field sasiad = null;
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0: sasiad = up; break;
                    case 1: sasiad = right; break;
                    case 2: sasiad = down; break;
                    case 3: sasiad = left; break;
                }

                if(connections[i] == 0)
                {
                    if (sasiad.Distance == distance) wynik[i] = 0;
                    else wynik[i] = 3;
                }
                else if(connections[i] == 1)
                {
                    if(sasiad.Distance > distance + 1) wynik[i] = 3;
                    else if (sasiad.Distance == distance + 1) wynik[i] = 2;
                    else if (sasiad.Distance == distance) wynik[i] = 1;
                    else { wynik[i] = -1; wynik[4] = 1; }
                }
                else wynik[i] = -2;
            }

            return wynik;
        }
    }
    class Blockade : Abstract_Field
    {
        public override Abstract_Field Up => this;
        public override Abstract_Field Right => this;
        public override Abstract_Field Down => this;
        public override Abstract_Field Left => this;
        public override int Distance { get => -1; set { } }
        public override void Connect_Ends(int i) { return; }
        public override bool Check_For_Connections() { return false; }
        public override int Get_Connection(string side) { return -1; }

        public override int[] Get_Diffrecess() { return new int[0]; }
    }
}
