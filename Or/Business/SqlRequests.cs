using Microsoft.Data.Sqlite;
using Or.Models;
using Or.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Or.Business
{
    static public class SqlRequests
    {
        static readonly string fileDb = "BaseAppBancaire.db";

        static readonly string queryComptesDispo = "SELECT IdtCpt, NumCarte, Solde, TypeCompte FROM COMPTE WHERE NOT IdtCpt=@IdtCpt";

        static readonly string queryComptesCarte = "SELECT IdtCpt, NumCarte, Solde, TypeCompte FROM COMPTE WHERE NumCarte=@Carte";

        static readonly string queryTransacCompte = "SELECT IdtTransaction, Horodatage, Montant, CptExpediteur, CptDestinataire, Statut FROM \"TRANSACTION\" WHERE Statut = 'O' AND (CptExpediteur=@IdtCptEx OR CptDestinataire=@IdtCptDest)";
        static readonly string queryCarte = "SELECT NumCarte, PrenomClient, NomClient, PlafondRetrait, IDCONSEILLER from CARTE WHERE NumCarte=@Carte";

        static readonly string queryConseiller = "SELECT c.IDCONSEILLER, co.NOM, co.PRENOM , co.NUM, co.MAIL from CONSEILLER co INNER JOIN CARTE c ON c.IDCONSEILLER = co.IDCON WHERE c.NumCarte=@Carte";
        static readonly string queryTransacCarte = "SELECT tr.IdtTransaction, tr.Horodatage, tr.Montant, tr.CptExpediteur, tr.CptDestinataire, tr.Statut FROM \"TRANSACTION\" tr INNER JOIN HISTTRANSACTION t ON t.IdtTransaction = tr.IdtTransaction WHERE tr.Statut = 'O' AND t.NumCarte=@Carte;";

        static readonly string queryInsertTransac = "INSERT INTO \"TRANSACTION\" (Horodatage, Montant, CptExpediteur, CptDestinataire, Statut) VALUES (@Horodatage,@Montant,@CptExp,@CptDest,\"O\")";
        static readonly string queryIdtTransac = "select seq from sqlite_sequence where name=\"TRANSACTION\"";
        static readonly string queryInsertHistTransac = "INSERT INTO HISTTRANSACTION (IdtTransaction,NumCarte) VALUES (@IdtTrans,@Carte)";

        static readonly string queryUpdateCompte = "UPDATE COMPTE SET Solde=Solde-@Montant WHERE IdtCpt=@IdtCompte";
        static readonly string queryComptesBenef = "SELECT B.IDBENEFICIAIRE,C.NomClient,C.PrenomClient,B.IDCOMPTE FROM BENEFICIAIRE B INNER JOIN  COMPTE CP ON B.IDCOMPTE=CP.idtcpt INNER JOIN  CARTE C ON C.NumCarte = CP.NumCarte WHERE  B.IDCARTE=@Carte ";
        static readonly string queryCreerCompte = "INSERT INTO CARTE (NumCarte, NomClient, PrenomClient, PlafondRetrait,IDCONSEILLER) VALUES (@NumCarte, @Nom, @Prenom, 1000,@IdCON)";
        static readonly string queryDalateBenef = "DELETE FROM BENEFICIAIRE WHERE IDBENEFICIAIRE = @IdBenef";

        static readonly string queryInsertBenef = "INSERT INTO BENEFICIAIRE (IdCarte, IdCompte) VALUES (@Carte, @Compte)";

        static readonly string querySuppBenef = "DELETE FROM BENEFICIAIRE WHERE IDCarte = @id";
        static readonly string querySuppCPTAss = "DELETE FROM COMPTE WHERE NumCarte = @id";
        static readonly string querySuppCarte = "DELETE FROM CARTE WHERE NumCarte = @id";

        /// <summary>
        /// Obtention des infos d'une carte
        /// </summary>
        /// <param name="numCarte"></param>
        /// <returns></returns>
        public static Carte InfosCarte(long numCarte)
        {
            Carte carte = null;

            string connectionString = ConstructionConnexionString(fileDb);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(queryCarte, connection))
                {
                    command.Parameters.AddWithValue("@Carte", numCarte);

                    using (var reader = command.ExecuteReader())
                    {
                        long idtCarte;
                        string prenom;
                        string nom;
                        int plafondRetrait;
                        int Idcon;

                        if (reader.Read())
                        {
                            idtCarte = reader.GetInt64(0);
                            prenom = reader.GetString(1);
                            nom = reader.GetString(2);
                            plafondRetrait = reader.GetInt32(3);
                            Idcon = reader.GetInt32(4);
                            carte = new Carte(idtCarte, prenom, nom, Idcon, plafondRetrait);
                        }
                    }
                }
            }

            return carte;
        }

        /// <summary>
        /// Obtention du dernier identifiant de transaction
        /// </summary>
        /// <returns></returns>
        public static int InfosIdtTrans()
        {
            int idtTransac = 0;

            string connectionString = ConstructionConnexionString(fileDb);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(queryIdtTransac, connection))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idtTransac = reader.GetInt32(0);
                        }
                    }
                }
            }

            return idtTransac;
        }


        /// <summary>
        /// Liste des comptes associés à une carte donnée
        /// </summary>
        /// <param name="numCarte"></param>
        /// <returns></returns>
        public static List<Compte> ListeComptesAssociesCarte(long numCarte)
        {
            List<Compte> comptes = new List<Compte>();

            string connectionString = ConstructionConnexionString(fileDb);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(queryComptesCarte, connection))
                {
                    command.Parameters.AddWithValue("@Carte", numCarte);

                    using (var reader = command.ExecuteReader())
                    {
                        int idtCpt;
                        long carte;
                        decimal solde;
                        string typeCompte;

                        while (reader.Read())
                        {
                            idtCpt = reader.GetInt32(0);
                            carte = reader.GetInt64(1);
                            solde = reader.GetDecimal(2);
                            typeCompte = reader.GetString(3);

                            Compte compte = new Compte(idtCpt, carte, typeCompte == "Courant" ? TypeCompte.Courant : TypeCompte.Livret, solde);
                            comptes.Add(compte);
                        }
                    }
                    
                }
                connection.Close();
            }
            
            return comptes;
            
        }


        /// <summary>
        /// Liste des comptes associés dispos
        /// </summary>
        /// <param name="idtCpt"></param>
        /// <returns></returns>
        public static List<Compte> ListeComptesDispo(int idtCpt)
        {
            List<Compte> comptes = new List<Compte>();

            string connectionString = ConstructionConnexionString(fileDb);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(queryComptesDispo, connection))
                {
                    command.Parameters.AddWithValue("@IdtCpt", idtCpt);

                    using (var reader = command.ExecuteReader())
                    {
                        int idt;
                        long carte;
                        decimal solde;
                        string typeCompte;

                        while (reader.Read())
                        {
                            idt = reader.GetInt32(0);
                            carte = reader.GetInt64(1);
                            solde = reader.GetDecimal(2);
                            typeCompte = reader.GetString(3);

                            Compte compte = new Compte(idt, carte, typeCompte == "Courant" ? TypeCompte.Courant : TypeCompte.Livret, solde);
                            comptes.Add(compte);
                        }
                    }
                }
            }

            return comptes;
        }

        /// <summary>
        /// Liste des transactions associées à une carte donnée
        /// </summary>
        /// <param name="numCarte"></param>
        /// <returns></returns>
        public static List<Transaction> ListeTransactionsAssociesCarte(long numCarte)
        {
            List<Transaction> transactions = new List<Transaction>();

            string connectionString = ConstructionConnexionString(fileDb);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(queryTransacCarte, connection))
                {
                    command.Parameters.AddWithValue("@Carte", numCarte);

                    using (var reader = command.ExecuteReader())
                    {
                        int idtTransaction;
                        string horodatage;
                        decimal montant;
                        int cptDest;
                        int cptExt;

                        while (reader.Read())
                        {
                            idtTransaction = reader.GetInt32(0);
                            horodatage = reader.GetString(1);
                            montant = reader.GetDecimal(2);
                            cptDest = reader.GetInt32(3);
                            cptExt = reader.GetInt32(4);

                            Transaction trans = new Transaction(idtTransaction, Tools.ConversionDate(horodatage), montant, cptDest, cptExt);
                            transactions.Add(trans);
                        }
                    }
                }
            }

            return transactions;
        }

        /// <summary>
        /// Liste des transactions associées à un compte donné
        /// </summary>
        /// <param name="numCarte"></param>
        /// <returns></returns>
        public static List<Transaction> ListeTransactionsAssociesCompte(int idtCpt)
        {
            List<Transaction> transactions = new List<Transaction>();

            string connectionString = ConstructionConnexionString(fileDb);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(queryTransacCompte, connection))
                {
                    command.Parameters.AddWithValue("@IdtCptEx", idtCpt);
                    command.Parameters.AddWithValue("@IdtCptDest", idtCpt);

                    using (var reader = command.ExecuteReader())
                    {
                        int idtTransaction;
                        string horodatage;
                        decimal montant;
                        int cptDest;
                        int cptExt;

                        while (reader.Read())
                        {
                            idtTransaction = reader.GetInt32(0);
                            horodatage = reader.GetString(1);
                            montant = reader.GetDecimal(2);
                            cptDest = reader.GetInt32(3);
                            cptExt = reader.GetInt32(4);

                            Transaction trans = new Transaction(idtTransaction, Tools.ConversionDate(horodatage), montant, cptDest, cptExt);
                            transactions.Add(trans);
                        }
                    }
                }
            }

            return transactions;
        }


        /// <summary>
        /// Procédure pour mettre à jour les données pour un retrait
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static bool EffectuerModificationOperationSimple(Transaction trans, long numCarte)
        {
            string connectionString = ConstructionConnexionString(fileDb);

            Operation typeOpe = Tools.TypeTransaction(trans.Expediteur, trans.Destinataire);

            if (typeOpe != Operation.DepotSimple && typeOpe != Operation.RetraitSimple)
            {
                return false;
            }

            int idtTrans = InfosIdtTrans() + 1;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Démarrer une transaction
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insertion de la transaction
                        var insertTransac = ConstructionInsertionTransaction(connection, trans);
                        insertTransac.Transaction = transaction;
                        insertTransac.ExecuteNonQuery();

                        // Insertion de l'historique de transaction
                        var insertHistTransac = ConstructionInsertionHistTransaction(connection, idtTrans, numCarte);
                        insertHistTransac.Transaction = transaction;
                        insertHistTransac.ExecuteNonQuery();

                        // Mise à jour du solde du compte de l'opération simple
                        decimal montant = typeOpe == Operation.RetraitSimple ? trans.Montant : -trans.Montant;
                        int idtCpt = typeOpe == Operation.DepotSimple ? trans.Destinataire : trans.Expediteur;

                        var updateCompte = ConstructionUpdateSolde(connection, idtCpt, montant);
                        updateCompte.Transaction = transaction;
                        updateCompte.ExecuteNonQuery();

                        // Valider la transaction
                        transaction.Commit();
                        Console.WriteLine("Transaction validée.");
                    }
                    catch (Exception ex)
                    {
                        // En cas d’erreur, annuler la transaction
                        Console.WriteLine("Erreur : " + ex.Message);
                        transaction.Rollback();
                        Console.WriteLine("Transaction annulée.");
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Procédure pour mettre à jour les données pour un retrait
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static bool EffectuerModificationOperationInterCompte(Transaction trans, long numCarteExp, long numCarteDest)
        {
            string connectionString = ConstructionConnexionString(fileDb);

            Operation typeOpe = Tools.TypeTransaction(trans.Expediteur, trans.Destinataire);

            if (typeOpe != Operation.InterCompte)
            {
                return false;
            }

            int idtTrans = InfosIdtTrans() + 1;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Démarrer une transaction
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insertion de la transaction
                        var insertTransac = ConstructionInsertionTransaction(connection, trans);
                        insertTransac.Transaction = transaction;
                        insertTransac.ExecuteNonQuery();

                        // Insertion de l'historique de transaction
                        var insertHistTransac = ConstructionInsertionHistTransaction(connection, idtTrans, numCarteExp);
                        insertHistTransac.Transaction = transaction;
                        insertHistTransac.ExecuteNonQuery();

                        if (numCarteDest != numCarteExp)
                        {
                            // Insertion de l'historique de transaction - côté destinataire
                            var insertHistTransacDest = ConstructionInsertionHistTransaction(connection, idtTrans, numCarteDest);
                            insertHistTransacDest.Transaction = transaction;
                            insertHistTransacDest.ExecuteNonQuery();
                        }

                        // Mise à jour du solde du compte de l'opération inter-compte 
                        // côté expéditeur
                        var updateCompteExp = ConstructionUpdateSolde(connection, trans.Expediteur, trans.Montant);
                        updateCompteExp.Transaction = transaction;
                        updateCompteExp.ExecuteNonQuery();

                        // côté destinataire
                        var updateCompteDest = ConstructionUpdateSolde(connection, trans.Destinataire, -trans.Montant);
                        updateCompteDest.Transaction = transaction;
                        updateCompteDest.ExecuteNonQuery();

                        // Valider la transaction
                        transaction.Commit();
                        Console.WriteLine("Transaction validée.");
                    }
                    catch (Exception ex)
                    {
                        // En cas d’erreur, annuler la transaction
                        Console.WriteLine("Erreur : " + ex.Message);
                        transaction.Rollback();
                        Console.WriteLine("Transaction annulée.");
                    }
                }
            }

            return true;
        }

        private static string ConstructionConnexionString(string fileDb)
        {
            string dossierRef = Directory.GetCurrentDirectory();
            string dossierProjet = Path.GetFullPath(Path.Combine(dossierRef, @"..\..\.."));

            string chemin = Path.Combine(dossierProjet, fileDb);
            return "Data Source=" + chemin;
        }

        private static SqliteCommand ConstructionInsertionTransaction(SqliteConnection connection, Transaction trans)
        {
            // Insertion de la transaction
            var insertTransac = connection.CreateCommand();
            insertTransac.CommandText = queryInsertTransac;

            insertTransac.Parameters.AddWithValue("@Horodatage", trans.Horodatage.ToString("dd/MM/yyyy hh:mm:ss"));
            insertTransac.Parameters.AddWithValue("@Montant", trans.Montant);
            insertTransac.Parameters.AddWithValue("@CptExp", trans.Expediteur);
            insertTransac.Parameters.AddWithValue("@CptDest", trans.Destinataire);

            return insertTransac;
        }

        private static SqliteCommand ConstructionInsertionHistTransaction(SqliteConnection connection, int idtTrans, long numCarte)
        {
            // Insertion de la transaction
            var insertHistTransac = connection.CreateCommand();
            insertHistTransac.CommandText = queryInsertHistTransac;

            insertHistTransac.Parameters.AddWithValue("@IdtTrans", idtTrans);
            insertHistTransac.Parameters.AddWithValue("@Carte", numCarte);

            return insertHistTransac;
        }

        /// <summary>
        /// COnstruction de la commande de mise à jour du solde du compte
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="idtCpt"></param>
        /// <param name="montant">Montant à soustraire au solde</param>
        /// <returns></returns>
        private static SqliteCommand ConstructionUpdateSolde(SqliteConnection connection, int idtCpt, decimal montant)
        {
            // Mise à jour du solde du compte
            var updateCompte = connection.CreateCommand();
            updateCompte.CommandText = queryUpdateCompte;
            updateCompte.Parameters.AddWithValue("@Montant", montant);
            updateCompte.Parameters.AddWithValue("@IdtCompte", idtCpt);

            return updateCompte;
        }

        // Récupérer la liste des bénéficiaires associé à ma carte 
        public static List<Beneficiaire> ListeBeneficiairesAssocieClient(long numCarte)
        {
            List<Beneficiaire> Beneficiaires = new List<Beneficiaire>();
            string connectionString = ConstructionConnexionString(fileDb);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(queryComptesBenef, connection))
                {
                    command.Parameters.AddWithValue("@Carte", numCarte);
                    using (var reader = command.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            Beneficiaire b = new Beneficiaire
                            {
                                IdBenef = reader.GetInt32(0),
                                Nom = reader.GetString(1),
                                Prenom = reader.GetString(2),
                                IdCompte = reader.GetInt32(3),
                                IdCarte = (int)numCarte
                            };
                            Beneficiaires.Add(b);

                        }
                    }


                }

            }

            return Beneficiaires;

        }
        // Ajouter un bénéficiaire 
        public static void AjouterBeneficiaire(long idCarte, int idCompte)
        {
            string connectionString = ConstructionConnexionString(fileDb);

            // Vérifie si ce bénéficiaire existe déjà
            if (ExisteBeneficiaire(idCarte, idCompte))
            {
                throw new Exception("Ce bénéficiaire existe déjà pour cette carte.");
            }

            string queryLastId = "SELECT last_insert_rowid();";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Insertion du bénéficiaire
                using (var command = new SqliteCommand(queryInsertBenef, connection))
                {
                    command.Parameters.AddWithValue("@Carte", idCarte);
                    command.Parameters.AddWithValue("@Compte", idCompte);
                    command.ExecuteNonQuery();
                }

                // Récupération de l’ID auto-généré
                using (var cmdLastId = new SqliteCommand(queryLastId, connection))
                {
                    long lastId = (long)cmdLastId.ExecuteScalar();

                }
            }
        }
        // Supprimer le bénéficiaire lié à l'id
        public static void SupprimerBeneficiaire(int idBenef)
        {
            string connectionString = ConstructionConnexionString(fileDb);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(queryDalateBenef, connection))
                {
                    command.Parameters.AddWithValue("@IdBenef", idBenef);
                    command.ExecuteNonQuery();
                }
            }
        }
        // trouver le Compte lié à l'idCompte s'il le trouve on return le sinon ça va etre null
        public static Compte ObtenirCompteParId(int idCompte)
        {
            string connectionString = ConstructionConnexionString(fileDb);
            string query = "SELECT IdtCpt, NumCarte, Solde, TypeCompte FROM COMPTE WHERE IdtCpt = @IdCompte";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCompte", idCompte);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            long carte = reader.GetInt64(1);
                            decimal solde = reader.GetDecimal(2);
                            string type = reader.GetString(3);

                            return new Compte(id, carte,
                            type == "Courant" ? TypeCompte.Courant : TypeCompte.Livret,
                            solde);
                        }
                    }
                }
            }

            return null;
        }
        // Vérifier si le Compte à ajouter il est déjà dans la liste des bénéficiaires ou pas 
        public static bool ExisteBeneficiaire(long idCarte, int idCompte)
        {
            string connectionString = ConstructionConnexionString(fileDb);
            string query = "SELECT COUNT(*) FROM BENEFICIAIRE WHERE IdCarte = @Carte AND IdCompte = @Compte";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Carte", idCarte);
                    command.Parameters.AddWithValue("@Compte", idCompte);

                    long count = (long)command.ExecuteScalar(); // Renvoie le nombre de lignes trouvées
                    return count > 0;
                }
            }
        }
        public static Conseiller InfoConseiller(long numCarte)
        {
            Conseiller Conseiller = new Conseiller();
            string connectionString = ConstructionConnexionString(fileDb);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(queryConseiller, connection))
                {
                    command.Parameters.AddWithValue("@Carte", numCarte);
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Conseiller = new Conseiller()
                            {
                                IdConseiller = reader.GetInt32(0),
                                Nom = reader.GetString(1),
                                Prenom = reader.GetString(2),
                                NTel = reader.GetString(3),
                                Mail = reader.GetString(4),
                                Idcarte = (int)numCarte
                            };

                        }
                    }


                }

            }

            return Conseiller;

        }

        // Vérifie si une carte existe déjà
        public static bool CarteExiste(long numCarte)
        {
            string connectionString = ConstructionConnexionString(fileDb);
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM CARTE WHERE NumCarte = @NumCarte";
                using (var cmd = new SqliteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@NumCarte", numCarte);
                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // Ajoute une carte
        public static void AjouterCarte(long numCarte, string nom, string prenom, int IdCon)
        {
            string connectionString = ConstructionConnexionString(fileDb);
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SqliteCommand(queryCreerCompte, connection))
                {
                    cmd.Parameters.AddWithValue("@NumCarte", numCarte);
                    cmd.Parameters.AddWithValue("@Nom", nom);
                    cmd.Parameters.AddWithValue("@Prenom", prenom);
                    cmd.Parameters.AddWithValue("@IdCON", IdCon);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Ajoute un compte lié à une carte
        public static void AjouterCompte(long numCarte, string type, decimal solde)
        {
            string connectionString = ConstructionConnexionString(fileDb);
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO COMPTE (NumCarte, TypeCompte, Solde) VALUES (@NumCarte, @Type, @Solde)";
                using (var cmd = new SqliteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@NumCarte", numCarte);
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@Solde", solde);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool SupprimerCompteLivret(int idCompteLivret, long idCarte)
        {
            string connectionString = ConstructionConnexionString(fileDb);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Récupérer le solde du livret
                decimal soldeLivret = 0;
                using (var cmd = new SqliteCommand("SELECT Solde FROM COMPTE WHERE IdtCpt=@id", connection))
                {
                    cmd.Parameters.AddWithValue("@id", idCompteLivret);
                    object result = cmd.ExecuteScalar();
                    if (result == null) return false;
                    soldeLivret = Convert.ToDecimal(result);
                }

                // Récupérer le compte courant de la même carte
                int idCompteCourant = 0;
                using (var cmd = new SqliteCommand(
                "SELECT IdtCpt FROM COMPTE WHERE NumCarte=@carte AND TypeCompte='Courant'", connection))
                {
                    cmd.Parameters.AddWithValue("@carte", idCarte);
                    object result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("Aucun compte courant associé à cette carte !");
                        return false;
                    }
                    idCompteCourant = Convert.ToInt32(result);
                }

                // Ajouter le solde du livret au solde du compte courant
                using (var cmd = new SqliteCommand(
                "UPDATE COMPTE SET Solde = Solde + @montant WHERE IdtCpt=@id", connection))
                {
                    cmd.Parameters.AddWithValue("@montant", soldeLivret);
                    cmd.Parameters.AddWithValue("@id", idCompteCourant);
                    cmd.ExecuteNonQuery();
                }

                // Supprimer l’historique de transaction lié au compte livret
                using (var cmd = new SqliteCommand(@"DELETE FROM HISTTRANSACTION WHERE IdtTransaction IN (SELECT IdtTransaction FROM 'TRANSACTION' WHERE CptExpediteur=@id OR CptDestinataire=@id)", connection))
                {
                    cmd.Parameters.AddWithValue("@id", idCompteLivret);
                    cmd.ExecuteNonQuery();
                }

                // Supprimer les transactions liées au compte livret
                using (var cmd = new SqliteCommand(@"DELETE FROM 'TRANSACTION' WHERE CptExpediteur=@id OR CptDestinataire=@id", connection))
                {
                    cmd.Parameters.AddWithValue("@id", idCompteLivret);
                    cmd.ExecuteNonQuery();
                }

                // Supprimer enfin le compte Livret
                using (var cmd = new SqliteCommand("DELETE FROM COMPTE WHERE IdtCpt=@id AND TypeCompte='Livret'", connection))
                {
                    cmd.Parameters.AddWithValue("@id", idCompteLivret);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
      

        public static void ModifierPlafondCarte(long idCarte, decimal nouveau)
        {
            string query = "UPDATE CARTE SET PlafondRetrait = @p WHERE NumCarte = @id";

            using (var conn = new SqliteConnection(ConstructionConnexionString(fileDb)))
            {
                conn.Open();

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@p", nouveau);
                    cmd.Parameters.AddWithValue("@id", idCarte);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void SupprimerCarteEtComptes(long idCarte)
        {
            // On récupère les comptes AVANT toute ouverture
            var comptes = ListeComptesAssociesCarte(idCarte).Select(c => new { c.Id }).ToList();

            var listeIdsComptes = comptes.Select(c => c.Id).ToList();

            string connStr = ConstructionConnexionString(fileDb);

            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();

                using (var pragmaTimeout = conn.CreateCommand())
                {
                    pragmaTimeout.CommandText = "PRAGMA busy_timeout = 3000";
                    pragmaTimeout.ExecuteNonQuery();
                }

                using (var pragmaFK = conn.CreateCommand())
                {
                    pragmaFK.CommandText = "PRAGMA foreign_keys = ON";
                    pragmaFK.ExecuteNonQuery();
                }

                try
                {
                    // Supprimer les bénéficiaires liés à cette carte
                    using (var cmdBenef = conn.CreateCommand())
                    {
                        cmdBenef.CommandText = "DELETE FROM BENEFICIAIRE WHERE IDCarte = @id";
                        cmdBenef.Parameters.AddWithValue("@id", idCarte);
                        cmdBenef.ExecuteNonQuery();
                    }

                    // Supprimer bénéficiaires où un compte supprimé est enregistré
                    if (listeIdsComptes.Count > 0)
                    {
                        using (var cmdSuppBenefCompte = conn.CreateCommand())
                        {
                            cmdSuppBenefCompte.CommandText =
                            $"DELETE FROM BENEFICIAIRE WHERE IDCompte IN ({string.Join(",", listeIdsComptes)})";
                            cmdSuppBenefCompte.ExecuteNonQuery();
                        }
                    }

                    // Supprimer historique
                    using (var cmdHTran = conn.CreateCommand())
                    {
                        cmdHTran.CommandText = "DELETE FROM HISTTRANSACTION WHERE NumCarte=@id";
                        cmdHTran.Parameters.AddWithValue("@id", idCarte);
                        cmdHTran.ExecuteNonQuery();
                    }

                    // Supprimer transactions liées
                    /*if (listeIdsComptes.Count > 0)
                    {
                        using (var cmdTransaction = conn.CreateCommand())
                        {
                            cmdTransaction.CommandText =
                            $"DELETE FROM \"TRANSACTION\" WHERE CptExpediteur IN ({string.Join(",", listeIdsComptes)}) " + $"OR CptDestinataire IN ({string.Join(",", listeIdsComptes)})";

                            cmdTransaction.ExecuteNonQuery();
                        }
                    }*/

                    // Supprimer les comptes
                    using (var cmdCompte = conn.CreateCommand())
                    {
                        cmdCompte.CommandText = "DELETE FROM COMPTE WHERE NumCarte = @id";
                        cmdCompte.Parameters.AddWithValue("@id", idCarte);
                        cmdCompte.ExecuteNonQuery();
                    }

                    // Supprimer la carte
                    using (var cmdCarte = conn.CreateCommand())
                    {
                        cmdCarte.CommandText = "DELETE FROM CARTE WHERE NumCarte=@id";
                        cmdCarte.Parameters.AddWithValue("@id", idCarte);
                        cmdCarte.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Erreur SQL : " + ex.Message);
                }
            }
        }

    }

}
