namespace Dynamisches_Subnettieren_IPv4_V2
{
    internal class Program
    {
        static int[] IPOktettsGlobal = new int[4];
        static string NetAdress = "";
        static int Präfix;
        static string StringPräfix = "";
        static List<Tuple<string, int>> Networks = new List<Tuple<string, int>>();
        static List<Tuple<int[], int[]>> SubNetworks = new List<Tuple<int[], int[]>>();
        static int[] Bits = new int[]
            {
                128,
                64,
                32,
                16,
                8,
                4,
                2,
                1,
                0
            };
        static void Main(string[] args)
        {
            //User Interaktion
            UserInput();
            //Verarbeitung
            NetworkJump(Networks);
            for (int i = 0; i < SubNetworks.Count; i++)
                Console.WriteLine($"{Networks[i].Item1}\nNetzadesse: {ConvertIntArrayToString(SubNetworks[i].Item1)}\nNeues Netz: {SubNetworks[i].Item2}");
            Console.ReadKey();
        }
        static void UserInput()
        {
            //Fordert den User zum eingeben der Daten auf und prüft und Speichert diese
            while (true)
            {
                IPOktettsGlobal = GetIPFromUser();
                Präfix = GetPräfixFormUser();
                if (CheckInput())
                    break;
            }
            while (true)
            {
                GetSubnetHostsFormUser();
                NetworksSort(Networks);
                if (CheckInput(Networks))
                    break;
                else
                    Networks.Clear();
            }
        }
        static List<Tuple<string, int>> NetworksSort(List<Tuple<string, int>> NetworkItem)
        {
            //Die Teilnetzwerke werden absteigend Sortiert
            for (int i = 0; i < NetworkItem.Count; i++)
            {
                string TempString;
                int TempInt;
                for (int j = i; j < NetworkItem.Count; j++)
                {
                    TempString = NetworkItem[j].Item1;
                    TempInt = NetworkItem[j].Item2;
                    if (NetworkItem[i].Item2 < NetworkItem[j].Item2)
                    {
                        NetworkItem[j] = new Tuple<string, int>(NetworkItem[i].Item1, NetworkItem[i].Item2);
                        NetworkItem[i] = new Tuple<string, int>(TempString, TempInt);
                    }
                }
            }
            return NetworkItem;
        }
        static void ProgramHead()
        {
            //Programmkopf mit Netzadresse und Präfix
            Console.Clear();
            Console.WriteLine($"Dynamisches Subnettieren IPv4 V2\n\nBy Rick Kummer\n\n\nNetzadresse: {NetAdress}\nPräfix: {StringPräfix}\n");
        }
        static void ProgramHead(List<Tuple<string, int>> NetworkItem)
        {
            //Programmkopf mit Teilnetzwerken
            Console.Clear();
            Console.WriteLine($"Dynamisches Subnettieren IPv4 V2\n\nBy Rick Kummer\n\n\n");
            foreach (var item in NetworkItem)
                Console.WriteLine($"{item.Item1}: {item.Item2} Host's");
            Console.WriteLine();
        }
        static void ProgramHead(Tuple<int, int, int, int> SubnetMask)
        {
            //Programmkopf mit kompletten Netzadressdaten
            string StringSubnetMask = $"{SubnetMask.Item1}.{SubnetMask.Item2}.{SubnetMask.Item3}.{SubnetMask.Item4}";
            Console.Clear();
            Console.WriteLine($"Dynamisches Subnettieren IPv4 V2\n\nBy Rick Kummer\n\n\nNetzadresse: {NetAdress}\nPräfix: {StringPräfix}\nSubnetzmaske: {StringSubnetMask}\n");
        }
        static void ProgramHead(List<Tuple<string, int>> TempNetworks,List<Tuple<int[], int[]>> TempSubnetworks)
        {
            //Programmkopf mit Netzadresse, Präfix und Teilnetzwrken
            Console.Clear();
            Console.WriteLine($"Dynamisches Subnettieren IPv4 V2\n\nBy Rick Kummer\n\n\nNetzadresse: {NetAdress}\nPräfix: {StringPräfix}\n");
            for (int i = 0; i < TempSubnetworks.Count; i++)
            {
                Console.WriteLine($"{TempNetworks[i].Item1}\nNetzadresse: {ConvertIntArrayToString(TempSubnetworks[i].Item1)}\nBrodcastardesse: {ConvertIntArrayToString(TempSubnetworks[i].Item2)}");
            }
                
        }
        static string ConvertIntArrayToString(int[] Array)
        {
            //Setzt die vier Oktetten aus dem Array zu einer Netzadresse zusammen und gibt diese als String wieder aus
            string TempString = "";
            for (int i = 0; i < Array.Length; i++)
            {
                TempString += $"{Array[i]}";
                if (i < 3)
                    TempString += ".";
            }
            return TempString;
        }
        static void NetworkJump(List<Tuple<string, int>> TempNetworks)
        {
            //Ermittelt die Größe des Teilnetzwerks und gibt den Wert für die Netzadresse des Teilnetzwerks und das neue Teilnetzwerk zurück

            int[] TempNetwokAdress = new List<int>(IPOktettsGlobal).ToArray();
            foreach (var Item in TempNetworks)
            {
                int[] NextNetwork = TempNetwokAdress;
                int NewPräfix = Präfix + GetBitToAdress(Item.Item2);
                for (int i = 0; i < NextNetwork.Length; i++)
                {
                    if (i == GetTargetOktett((int)NewPräfix))
                    {
                        int TempInt = GetSumOfBits(TempNetworks[i].Item2 + 2);
                        if (CheckOktett(TempInt))
                        {
                            NextNetwork[i - 1]++;
                            NextNetwork[i] = 0;
                        }
                        else
                        {
                            NextNetwork[i] = TempInt;
                        }
                        break;
                    }
                }
                SubNetworks.Add(new Tuple<int[], int[]>(TempNetwokAdress, NextNetwork));
                TempNetwokAdress = NextNetwork;
            }
        }
        static int GetSumOfBits(int temp)
        {
            //Gibt die Summe der gesetzen Bits zurück
            return -1;
        }
        static int GetTargetOktett(int TempPräfix)
        {
            double Temp = TempPräfix / 8;
            if (TempPräfix % 8 > 0)
                Temp++;
            return (int)Math.Floor(Temp);
        }
        static void ErrorProcessing()
        {
            //Rehlermeldung unerwarteter Fehler
            Console.WriteLine("Es ist ein Unerwateter Fehler Aufgetreten");
            Console.ReadLine();
            Environment.Exit(0);
        }
        static int GetBitToAdress(int Hosts)
        {
            //Ermittelt die Betötigten Bits anhand der angegebenen Hostanzahl
            int Count = 0;
            while (true)
            {
                if (Math.Pow(2, Count) < Hosts + 2)
                    Count++;
                else
                    return Count;
            }
        }
        static void GetSubnetHostsFormUser()
        {
            //Fordert den User auf, den Namen des Teilnetzwerkes und die Anzahl der Host's einzugeben. Gibt es als Liste aus Tuple zurück
            while (true)
            {
                ProgramHead(Networks);
                Console.Write("Bitte geben Sie den Namen Ihres Teilnezwerkes an: ");
                string TempNetwworkName = Console.ReadLine() ?? "";
                if (string.IsNullOrEmpty(TempNetwworkName))
                {
                    Console.WriteLine("Sie haben Keinen Namen angegeben\nWeiter mit beliebiger Taste");
                    Console.ReadKey();
                    continue;
                }
                Console.Write("Bitte geben Sie die Anzahl der Host's an: ");
                int Hosts;
                if (!int.TryParse(Console.ReadLine(), out Hosts))
                {
                    Console.WriteLine("Sie haben keine valide Zahl eingegeben\nWeiter mit beliebiger Taste\n");
                    continue;
                }
                Networks.Add(new Tuple<string, int>(TempNetwworkName, Hosts));
                ProgramHead(Networks);
                Console.Write("Mochten Sie ein weiteres Teilnetzwerk hinzufügen? [j/n]: ");
                if (Console.ReadLine() == "n")
                    break;
            }
        }
        static bool CheckInput()
        {
            //User überprüft Input der Netzadresse auf Richtigkeit
            ProgramHead(GetSubnetMask(Präfix));
            Console.Write("Überprüfen Sie Ihre Eingabe\nIst Ihre Eingabe korrekt? [j/n]: ");
            if (Console.ReadLine() == "j")
                return true;
            else
                return false;
        }
        static bool CheckInput(List<Tuple<string, int>> TempNetworks)
        {
            //User überprüft Input von Teilnetzwerken auf Richtigkeit
            ProgramHead(TempNetworks);
            Console.Write("Überprüfen Sie Ihre Eingabe\nIst Ihre Eingabe korrekt? [j/n]: ");
            if (Console.ReadLine() == "j")
                return true;
            else
                return false;
        }
        static Tuple<int, int, int, int> GetSubnetMask(int InputPräfix)
        {
            //Ermitteln der Subnetzmaske über den Präfix
            List<int> result = new List<int>();
            int IndexFullOktett = InputPräfix / 8;
            for (int i = 0; i < IndexFullOktett; i++)
                result.Add(255);
            result.Add(ValueBitInOktett(InputPräfix % 8));
            for (int i = result.Count; i < 4; i++)
                result.Add(0);
            return new Tuple<int, int, int, int>(result[0], result[1], result[2], result[3]);
        }
        static int ValueBitInOktett(int SetBit)
        {
            //Berechnen der Adresse im Oktett durch gesetzte Bits
            int OutAdress = 0;
            for (int i = 0; i < SetBit; i++)
                OutAdress += Bits[i];
            return OutAdress;
        }
        static void ErrorInputPräfix()
        {
            //Fehlermeldung falscher Präfix
            ProgramHead();
            Console.WriteLine("Der Präfix wurde nicht korrek angegeben\nWeiter mit beliebiger Taste");
            Console.ReadKey();
        }
        static int GetPräfixFormUser()
        {
            //Präfix Abfragen, prüfen und zurückgeben
            while (true)
            {
                int IntPräfix;
                ProgramHead();
                Console.Write("Bitte geben Sie ihren Präfix an: ");
                string Temp = Console.ReadLine() ?? "";
                if (int.TryParse(Temp, out IntPräfix))
                {
                    if (IntPräfix >= 8 || IntPräfix <= 32)
                    {
                        StringPräfix = Temp;
                        return IntPräfix;
                    }
                    else
                        ErrorInputPräfix();
                }
            }
        }
        static int[] GetIPFromUser()
        {
            //Fordert den Nutzer auf die Netzadresse einzugeben und gibt ein Tuple der Oktetten zurück
            int[] Temp = new int[4];
            while(true)
            {
                ProgramHead();
                Console.Write("Bitte geben sie die Netzadresse an: ");
                NetAdress = Console.ReadLine() ?? "";
                Tuple<bool, int[]> Oktette = GetOktetts(NetAdress);
                if (Oktette.Item1)
                {
                    ErrorInputIp();
                    continue;
                }
                return Oktette.Item2;
            }
        }
        static bool CheckOktett(int Input)
        {
            //Checkt auf den richtigen Bereich der Adressierung
            if (Input >= 0 && Input < 256)
                return false;
            else
                return true;
        }
        static void ErrorInputIp()
        {
            //Fehlerbehandlung bei Falscher Ip
            ProgramHead();
            Console.WriteLine("Sie haben die Adresse nicht korrekt angegeben.\nWeiter mit beliebiger Taste");
            Console.ReadKey();
        }
        static Tuple<bool, int[]> GetOktetts(string InputIP)
        {
            //Zerlegt den String in Vier Oktetten und gibt eien Tuple mit Bool (Eror) und einen Array mit den Oktetten zurück
            int[] Oktestts = new int[4];
            int Count = 0;
            string Temp = "";
            for (int i = 0; i < Oktestts.Length; i++)
            {
                for (int j = Count; j < InputIP.Length; j++)
                {
                    if (InputIP[j] == '.')
                    {
                        if (int.TryParse(Temp, out Oktestts[i]))
                        {
                            if (CheckOktett(Oktestts[i]))
                                return new Tuple<bool, int[]>(true, Oktestts);
                            else
                            {
                                Temp = "";
                                Count++;
                                break;
                            }
                        }
                    }
                    else if (j == 3)
                    {
                        if (int.TryParse(Temp, out Oktestts[i]))
                        {
                            if (CheckOktett(Oktestts[i]))
                                return new Tuple<bool, int[]>(true, Oktestts);
                            else
                            {
                                Temp = "";
                                Count++;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Temp += InputIP[j];
                        Count++;
                    }
                }
            }
            return new Tuple<bool, int[]>(false, Oktestts);
        }
    }
}