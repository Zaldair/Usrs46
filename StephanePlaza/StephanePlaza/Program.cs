using System;
using System.Text.RegularExpressions;

namespace StephanePlaza
{
    //Class Proprietaire --------------------------------------------
    public class Proprietaire
    {
        public string nom;
        public string prenom;
        public Bien[] biens = new Bien[0];

        public Proprietaire(string nom, string prenom, Bien[] biens)
        {
            this.nom = nom;
            this.prenom = prenom;
            this.biens = biens;
        }
        public override string ToString()
        {
            string toString = String.Format("{1} {0} {2}", this.nom, this.prenom, this.biens?.Length != 0 ? "possède\n" : "ne possède aucun bien");
            toString += ListeBiens();
            return toString;
        }

        private string ListeBiens()
        {
            string listeBiens = "";
            foreach (Bien B in this.biens)
            {
                listeBiens += String.Format("- {0} {1} au {2}\n", B.GetType().Name == "Maison" ? "Une" : "Un", B.GetType().Name, B.adresse);
            }
            return listeBiens;
        }

    }
    //Class Bien ----------------------------------------------------
    public class Bien
    {
        public string adresse;
        public float superficie;
        //Constructeur
        public Bien(string adresse, float supercifie)
        {
            this.adresse = adresse;
            this.superficie = supercifie;
        }
        //string qui est renvoyé quand l'objet est appelé
        public override string ToString()
        {
            string toString = String.Format("Adresse = {0}\n", this.adresse);
            toString += String.Format("Superficie = {0} m²\n", this.superficie);
            return toString;
        }
        public float EvalutationValeur()
        {
            int facteur = 3000;
            return this.superficie * facteur;
        }
    }
    //Class Maison --------------------------------------------------
    //L'objet Maison est un enfants de Bien qui est lui le parent
    public class Maison : Bien
    {
        public int nPieces;
        public bool jardin;
        public Piece[] pieces;
        public static readonly List<string> nomPiece = new List<string>() {"Cuisine","Salle de Bain","Chambre","Salon","Garage","Terrasse"};
        // base fait référence au constructeur du parent (ici Base)
        public Maison(string adresse, float superficie, int _nPiece, bool _Jardin) : base(adresse,superficie)
        {
            this.nPieces = _nPiece;
            this.jardin = _Jardin;
            this.superficie = 0;
            pieces = new Piece[nPieces];

            Random rand = new Random();

            for (int i = 0; i < nPieces; i++)
            {
                pieces[i] = new Piece(nomPiece[rand.Next(nomPiece.Count)], rand.Next(10, 30));
                this.superficie += pieces[i].superficie;
            }
        } 
        public override string ToString()
        {
            string toString = String.Format("Nombre de pièces = {0}\n", this.nPieces);
            for (int i = 0; i < this.nPieces; i++)
                toString += String.Format("- {0}\n", this.pieces[i]);
            toString += String.Format("Présence d'un jardin = {0}\n", this.jardin ? "Oui" : "Non");
            toString += String.Format("> VALEUR = {0}$\n\n", this.EvaluationValeur());
            return base.ToString()+ toString;
        }
        public float EvaluationValeur()
        {
            int facteur = 3000;

            if (this.jardin) { facteur += 500; }
            if (this.nPieces > 3) { facteur += 200; }

            if (Regex.IsMatch(this.adresse, @"\bParis\b")) { facteur += 7000; }
            else if (Regex.IsMatch(this.adresse, @"\bLyon\b")) { facteur += 2000; }

            return this.superficie * facteur;
        }
    }
    //Class Piece ---------------------------------------------------
    public class Piece
    {
        public string nom;
        public float superficie;

        public Piece(string nom, float superficie)
        {
            this.nom = nom;
            this.superficie = superficie;
        }
        public override string ToString()
        {
            string toString = String.Format("La pièce {0} fait {1} m²", this.nom, this.superficie);
            return toString;
        }
    }
    //Class Terrain -------------------------------------------------
    public class Terrain : Bien
    {
        public int nCoteCloture;
        public bool riviere;

        public Terrain (string adresse, float superficie, int nCoteCloture, bool riviere) : base(adresse,superficie)
        {
            this.nCoteCloture = nCoteCloture;
            this.riviere = riviere;
        }
        public override string ToString()
        {
            string toString = String.Format("Nombre de coté = {0}\n", this.nCoteCloture);
            toString += String.Format("Présence d'une rivière = {0}\n", this.riviere ? "Oui" : "Non");
            toString += String.Format("> VALEUR = {0}$\n", this.EvaluationValeur());
            toString += String.Format("> Estimation cout travaux = {0}$\n\n", this.CoutFinirClotures());
            return base.ToString() + toString;
        }
        public new float EvaluationValeur()
        {
            int facteur = 3000;

            if (this.riviere) { facteur += 1000; }

            if (Regex.IsMatch(this.adresse, @"\bParis\b")) { facteur += 7000; }
            else if (Regex.IsMatch(this.adresse, @"\bLyon\b")) { facteur += 2000; }

            return this.superficie * facteur;
        }
        public float CoutFinirClotures()
        {
            int facteur = 0;
            facteur += 1000 * nCoteCloture;
            return facteur;
        }
    }
    #region Main
    public class Program
    {
        static void Main(string[] args)
        {
            Maison maisonUn = new Maison("11 Rue des Chartreux, 69001 Lyon", 58f, 4, false);
            Maison maisonDeux = new Maison("4 place Saint Louis, 22100 Dinan", 86.5f, 5, true);
            Maison maisonTrois = new Maison("26 Boulevard Claude Lorrin, 40100 Dax", 25.2f, 2, false);

            Terrain terrainUn = new Terrain("12 Avenue des Coccinelles, 44850 Ligné", 120f, 3, false);
            Terrain terrainDeux = new Terrain("37 Boulevard Albert Einstein, 75012 Paris", 40f, 5, true);
            Terrain terrainTrois = new Terrain("14 Route de Manom, 57100 Thionville", 43.7f, 7, true);

            Bien[] agenceImmobiliere = new Bien[]{maisonUn,maisonDeux,maisonTrois, terrainUn, terrainDeux, terrainTrois };

            foreach (Bien B in agenceImmobiliere)
            {
                Console.Write(B);
            }

            Proprietaire Elodie = new Proprietaire("Martin", "Elodie", new Bien[] { maisonUn, terrainUn, terrainTrois });
            Console.WriteLine(Elodie);

            Proprietaire Marc = new Proprietaire("Dupont", "Marc", new Bien[] {maisonDeux,maisonTrois,terrainDeux });
            Console.WriteLine(Marc);

            Proprietaire Leo = new Proprietaire("Marin", "Leo", new Bien[0]);
            Console.WriteLine(Leo);
        }
    }
    #endregion
}