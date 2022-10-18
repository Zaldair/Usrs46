using System;


namespace QuatreDeuxUn
{
    #region Class
    #region ClassDe
    //Class De -------------------------------------------
    public class De
    {
        public string nFace;
        public Random rand;
        public int face { get; private set; }

        public De()
        {
            this.rand = new Random();
        }
        public override string ToString()
        {
            string toString = String.Format("+---+  \n");
            toString += String.Format("| {0} |  \n", nFace);
            toString += String.Format("+---+  \n");
            return toString;
        }
        public virtual int Lancer()
        {
            int rand = this.rand.Next(1, 7);
            nFace = rand.ToString();
            return rand;
        }

    }
    //Class DeTruque ----------------------------------------
    public class DeTruque : De
    {
        public override string ToString()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            return base.ToString();
        }
        public override int Lancer()
        {
            int rand = this.rand.Next(1, 13);
            if (rand > 4 && rand < 7)
                nFace = "5";
            else if (rand > 7)
                nFace = "6";
            else
                nFace = rand.ToString();
            return int.Parse(nFace);
        }
    }
    #endregion
    #region Jeu
    public class Jeu
    {
        public readonly int nManche;
        public readonly int nDe;
        public readonly int nDeTruque;
        private List<De> des = new List<De>();

        public Jeu(int nManche = 5, int nDe = 5, int nDeTruque = 0)
        {
            this.nManche = nManche;

            this.nDe = nDe;
            for (int i = 0; i < nDe; i++)
                des.Add(new De());

            this.nDeTruque = nDeTruque;
            for (int i = 0; i < nDeTruque; i++)
                des.Add(new DeTruque());

            foreach (De D in des)
                D.Lancer();
        }

        public override string ToString()
        {
            (int, int) cursorPos = Console.GetCursorPosition();
            for (int i = 0; i < des.Count; i++)
            {
                string[] dString = des[i].ToString().Split('\n');
                for (int y = 0; y < dString.Length; y++)
                {
                    Console.SetCursorPosition(cursorPos.Item1 + 1 + 6 * i, cursorPos.Item2 + y);
                    Console.Write(dString[y] + "\r");
                }
            }
            Console.ResetColor();
            return base.ToString();
        }

        public int Relancer(int index)
        {
            return des[index].Lancer();
        }
        public int Score()
        {
            int score = 0;
            foreach (De D in des)
                score += int.Parse(D.nFace);
            return score;
        }
        public string Run()
        {
            Console.Write("(" + Score() + " pts) Quel.s dé.s voulez vous relancer ? (a,b,..) : ");
            string toReturn = Console.ReadLine();
            return toReturn;
        }
    }
    #endregion
    #endregion
    public class Program
    {
        static void Main(string[] args)
        {
            //Setup
            Console.Write("En combien de manche voulez vous jouer ? : ");
            int thisManche = int.Parse(Console.ReadLine());
            Console.Write("Avec combien de dé voulez vous jouer ? : ");
            int nDe = int.Parse(Console.ReadLine());
            Console.Write("Avec combien de dé truqué voulez vous jouer ? : ");
            int nDeTruque = int.Parse(Console.ReadLine());
            //Debut de la partie
            Jeu mainGame = new Jeu(thisManche,nDe,nDeTruque);
            thisManche = 0;
            #region BoucleDeJeu
            while (thisManche < mainGame.nManche)
            {
                thisManche++;
                Console.WriteLine("Manche {0} :", thisManche);
                mainGame.ToString();
                string toRoll = mainGame.Run();
                string[] reroll = toRoll.Split(',');
                foreach (string s in reroll)
                {
                    mainGame.Relancer(int.Parse(s) - 1);
                }
            }
            #endregion 
            mainGame.ToString();
        }
    }
}