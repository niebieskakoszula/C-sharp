namespace Jipp_4_Projekt_Uproszczony_v2_2
{
    abstract class PunktAbstrakcyjny
    {

        protected PunktAbstrakcyjny gora;
        protected PunktAbstrakcyjny dol;
        protected PunktAbstrakcyjny prawo;
        protected PunktAbstrakcyjny lewo;
        public abstract PunktAbstrakcyjny Gora { get; }
        public abstract PunktAbstrakcyjny Prawo { get; }
        public abstract PunktAbstrakcyjny Dol { get; }
        public abstract PunktAbstrakcyjny Lewo { get; }
        protected int koniec;
        protected int odleglosc;
        public abstract int Odleglosc { get; set; }
        public int Koniec
        {
            get => koniec;
            set
            {
                if (value == 1 || value == 2) koniec = value;
            }
        }
        public virtual void PrzypiszSasiadow(PunktAbstrakcyjny g, PunktAbstrakcyjny p, PunktAbstrakcyjny d, PunktAbstrakcyjny l) { }
        public abstract void PolaczKonce(int i);
        public abstract bool CzyPolaczenia();
        public abstract int PokazPolaczenie(int kierunek);

        public abstract int[] PokazRoznice();
    }
    class Punkt : PunktAbstrakcyjny
    {
        public Punkt(int odleglosc) { this.odleglosc = odleglosc; koniec = 0;}
        public Punkt(int odleglosc, int koniec) { this.odleglosc = odleglosc; this.koniec = koniec;}
        protected int[] polaczenia = new int[4];
        public override int Odleglosc
        {
            get => odleglosc;
            set
            {
                if (value >= 0) odleglosc = value;
            }
        }


        public override PunktAbstrakcyjny Gora => gora;
        public override PunktAbstrakcyjny Prawo => prawo;
        public override PunktAbstrakcyjny Dol => dol;
        public override PunktAbstrakcyjny Lewo => lewo;
        public override void PrzypiszSasiadow(PunktAbstrakcyjny g, PunktAbstrakcyjny p, PunktAbstrakcyjny d, PunktAbstrakcyjny l)
        {
            if (gora != null) return;
            gora = g; if (gora is Punkt) polaczenia[0] = 1; else polaczenia[0] = -1;
            dol = d; if (dol is Punkt) polaczenia[2] = 1; else polaczenia[2] = -1;
            if (koniec != 0)
            {
                prawo = p; polaczenia[1] = -1;
                lewo = l; polaczenia[3] = -1;
            }
            else
            {
                prawo = p; if (prawo is Punkt) polaczenia[1] = 1; else polaczenia[1] = -1;
                lewo = l; if (lewo is Punkt) polaczenia[3] = 1; else polaczenia[3] = -1;
            }

        }

        public void Polacz(int kierunek)
        {
            if (polaczenia[kierunek - 1] != 1) return;
            polaczenia[kierunek - 1] = 0;
            switch (kierunek)
            {
                case 1: gora.PolaczKonce(koniec); break;
                case 2: prawo.PolaczKonce(koniec); break;
                case 3: dol.PolaczKonce(koniec); break;
                case 4: lewo.PolaczKonce(koniec); break;
            }
        }
        public virtual void Zablokuj(Blokada b, int kierunek)
        {
            if (polaczenia[kierunek - 1] != 1) return;
            switch (kierunek)
            {
                case 1: gora = b; break;
                case 2: prawo = b; break;
                case 3: dol = b; break;
                case 4: lewo = b; break;
            }
            polaczenia[kierunek - 1] = -1;
        }
        public override void PolaczKonce(int i)
        {
            if (i == 0 || koniec == i) return;
            else if (koniec != 0 && koniec != i) koniec = 3;
            else koniec = i;

            if (polaczenia[0] == 0) gora.PolaczKonce(koniec);
            if (polaczenia[1] == 0) prawo.PolaczKonce(koniec);
            if (polaczenia[2] == 0) dol.PolaczKonce(koniec);
            if (polaczenia[3] == 0) lewo.PolaczKonce(koniec);
        }
        public override int PokazPolaczenie(int kierunek)
        {
            return polaczenia[kierunek - 1];
        }
        public override bool CzyPolaczenia()
        {
            for (int i = 0; i < 4; i++)
                if (polaczenia[i] == 0)
                    return true;

            return false;
        }


        public void PolaczTymczasowo(int kierunek)
        {
            if (polaczenia[kierunek - 1] != 1) return;
            polaczenia[kierunek - 1] = 0;
        }
        public void ZablokujTymczasowo(int kierunek)
        {
            polaczenia[kierunek - 1] = -1;
        }
        public void Odblokuj(int kierunek)
        {
            switch (kierunek)
            {
                case 1: if(gora is Blokada) return; break;
                case 2: if(prawo is Blokada) return; break;
                case 3: if(dol is Blokada) return; break;
                case 4: if(lewo is Blokada) return; break;
            }
            polaczenia[kierunek - 1] = 1;
        }


        public override int[] PokazRoznice()
        {
            int[] wynik = new int[5];
            if (odleglosc == 0) wynik[4] = 1;

            PunktAbstrakcyjny sasiad = null;
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0: sasiad = gora; break;
                    case 1: sasiad = prawo; break;
                    case 2: sasiad = dol; break;
                    case 3: sasiad = lewo; break;
                }

                if(polaczenia[i] == 0)
                {
                    if (sasiad.Odleglosc == odleglosc) wynik[i] = 0;
                    else wynik[i] = 3;
                }
                else if(polaczenia[i] == 1)
                {
                    if(sasiad.Odleglosc > odleglosc + 1) wynik[i] = 3;
                    else if (sasiad.Odleglosc == odleglosc + 1) wynik[i] = 2;
                    else if (sasiad.Odleglosc == odleglosc) wynik[i] = 1;
                    else { wynik[i] = -1; wynik[4] = 1; }
                }
                else wynik[i] = -2;
            }

            return wynik;
        }
    }
    class Blokada : PunktAbstrakcyjny
    {
        public override PunktAbstrakcyjny Gora => this;
        public override PunktAbstrakcyjny Prawo => this;
        public override PunktAbstrakcyjny Dol => this;
        public override PunktAbstrakcyjny Lewo => this;
        public override int Odleglosc { get => -1; set { } }
        public override void PolaczKonce(int i) { return; }
        public override bool CzyPolaczenia() { return false; }
        public override int PokazPolaczenie(int kierunek) { return -1; }

        public override int[] PokazRoznice() { return new int[0]; }
    }
}
