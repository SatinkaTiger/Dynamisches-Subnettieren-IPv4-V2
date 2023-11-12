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
                1
            };
        static void Main(string[] args)
        {
            //User Interaktion
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
                if (CheckInput(Networks))
                    break;
                else
                    Networks.Clear();
            }
            //Verarbeitung
            foreach (var Item in Networks)
            {
                SubNetworks.Add(NetworkJump(GetNewPräfix(Item.Item2)));
            }
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
            Console.WriteLine($"Dynamisches Subnettieren IPv4 V2\n\nBy Rick Kummer\n\nNetzadresse: {NetAdress}\nPräfix: {StringPräfix}\nSubnetzmaske: {StringSubnetMask}\n");
        }
        static Tuple<int[], int[]> NetworkJump(int TempPräfix)
        {
            //Ermittelt die Größe des Teilnetzwerks und setzt den Wert für das nächste Netzwerk
            int JumpBit = Bits[TempPräfix % 8 + 1];
            int TargetOktett = TempPräfix / 8;
            int[] NewIP = IPOktettsGlobal;
            int[] Temp = IPOktettsGlobal;
            NewIP[TargetOktett] += JumpBit;
            if (NewIP[TargetOktett] > 255)
            {
                NewIP[TargetOktett] = 0;
                NewIP[TargetOktett - 1] += 1;
            }
            IPOktettsGlobal = NewIP;
            return new Tuple<int[], int[]>(Temp, NewIP);
        }
        static int GetNewPräfix(int Hosts)
        {
            //Ermittelt den neuen Präfix anhand der angegebenen Hostanzahl
            int Count = 0;
            while (true)
            {
                if (Math.Pow(2, Count) < Hosts + 2)
                    Count++;
                else
                    return Count + Präfix;
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
                ProgramHead(Networks);
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
            result.Add(SetBitInOktett(InputPräfix % 8));
            for (int i = result.Count; i < 4; i++)
                result.Add(0);
            return new Tuple<int, int, int, int>(result[0], result[1], result[2], result[3]);
        }
        static int SetBitInOktett(int SetBit)
        {
            //Berechen der Adresse im Oktett durch gesetzte Bits
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
            bool Error;
            int[] Temp = new int[4];
            while (true)
            {
                Error = false;
                ProgramHead();
                Console.Write("Bitte geben sie die Netzadresse an: ");
                NetAdress = Console.ReadLine() ?? "";
                Tuple<bool, int[]> Oktette = GetOktetts(NetAdress);
                if (Oktette.Item1)
                {
                    ErrorInputIp();
                    continue;
                }
                for (int i = 0; i < Oktette.Item2.Length; i++)
                {
                    Error = CheckOktett(Oktette.Item2[i]);
                    if (Error)
                        break;
                }
                if (Error)
                {
                    ErrorInputIp();
                    continue;
                }
                Temp = Oktette.Item2;
                break;
            }
            return Temp;
        }
        static bool CheckOktett(int Input)
        {
            //Checkt auf den richtigen Bereich der Adressierung
            if (Input > 0 && Input < 256)
                return true;
            else
                return false;
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
                            Count++;
                            continue;
                        }
                        else
                            return new Tuple<bool, int[]>(true, Oktestts);
                    }
                    else
                    {
                        Temp += InputIP[j];
                        Count++;
                    }
                    if (j == InputIP.Length - 1)
                    {
                        if (int.TryParse(Temp, out Oktestts[i]))
                            continue;
                        else
                            return new Tuple<bool, int[]>(true, Oktestts);
                    }
                }
            }
            return new Tuple<bool, int[]>(false, Oktestts);
        }
    }
}