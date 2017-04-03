using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;

namespace SZJS
{
    public partial class Lottery
    {
        /// <summary>
        /// ����30ѡ7(���ֲ�)
        /// </summary>
        public partial class QLC : LotteryBase
        {
            #region ��̬����
            public const int PlayType_D = 1301;
            public const int PlayType_F = 1302;

            public const int ID = 13;
            public const string sID = "13";
            public const string Name = "���ֲ�";
            public const string Code = "QLC";
            public const double MaxMoney = 20000;
            #endregion

            public QLC()
            {
                id = 13;
                name = "���ֲ�";
                code = "QLC";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 1301) && (play_type <= 1302));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[2];

                Result[0] = new PlayType(PlayType_D, "��ʽ");
                Result[1] = new PlayType(PlayType_F, "��ʽ");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 13
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();
                ArrayList al = new ArrayList();

                for (int i = 0; i < Num; i++)
                {
                    al.Clear();
                    int Ball;
                    for (int j = 0; j < 7; j++)
                    {
                        Ball = 0;
                        while ((Ball == 0) || isExistBall(al, Ball))
                            Ball = rd.Next(1, 30 + 1);
                        al.Add(Ball.ToString().PadLeft(2, '0'));
                    }

                    CompareToAscii compare = new CompareToAscii();
                    al.Sort(compare);

                    string LotteryNumber = "";
                    for (int j = 0; j < al.Count; j++)
                        LotteryNumber += al[j].ToString() + " ";

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)	//��ʽȡ��ʽ, ���� ref �����ǽ���Ʊ�淶�����磺01 01 02... ��� 01 02...
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '));
                CanonicalNumber = "";

                if (strs.Length < 7)
                {
                    CanonicalNumber = "";
                    return null;
                }

                for (int i = 0; i < strs.Length; i++)
                    CanonicalNumber += (strs[i] + " ");
                CanonicalNumber = CanonicalNumber.Trim();

                ArrayList al = new ArrayList();

                #region ѭ��ȡ��ʽ

                int n = strs.Length;
                for (int i = 0; i < n - 6; i++)
                    for (int j = i + 1; j < n - 5; j++)
                        for (int k = j + 1; k < n - 4; k++)
                            for (int x = k + 1; x < n - 3; x++)
                                for (int y = x + 1; y < n - 2; y++)
                                    for (int z = y + 1; z < n - 1; z++)
                                        for (int a = z + 1; a < n; a++)
                                            al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[x] + " " + strs[y] + " " + strs[z] + " " + strs[a]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 25)	//25: "01 02 03 04 05 06 07 + 08"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;
                if ((WinMoneyList == null) || (WinMoneyList.Length < 14))
                    return -3;

                string Blue = WinNumber.Substring(23, 2);

                int Description1 = 0, Description2 = 0, Description3 = 0, Description4 = 0, Description5 = 0, Description6 = 0, Description7 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle(Lotterys[ii], ref t_str, PlayType);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 20)	//20: "01 02 03 04 05 06 07"
                            continue;

                        string[] Red = new string[7];
                        Regex regex = new Regex(@"(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d\s)(?<R4>\d\d\s)(?<R5>\d\d\s)(?<R6>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);

                        int RedRight = 0;
                        bool BlueRight = false;
                        bool Full = true;
                        for (int j = 0; j < 7; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.IndexOf(Red[j] + " ") >= 0)
                                RedRight++;
                            if (Blue == Red[j])
                                BlueRight = true;
                        }
                        if (!Full)
                            continue;

                        if (RedRight == 7)
                        {
                            Description1++;
                            WinMoney += WinMoneyList[0];
                            WinMoneyNoWithTax += WinMoneyList[1];
                            continue;
                        }
                        if ((RedRight == 6) && BlueRight)
                        {
                            Description2++;
                            WinMoney += WinMoneyList[2];
                            WinMoneyNoWithTax += WinMoneyList[3];
                            continue;
                        }
                        if (RedRight == 6)
                        {
                            Description3++;
                            WinMoney += WinMoneyList[4];
                            WinMoneyNoWithTax += WinMoneyList[5];
                            continue;
                        }
                        if ((RedRight == 5) && BlueRight)
                        {
                            Description4++;
                            WinMoney += WinMoneyList[6];
                            WinMoneyNoWithTax += WinMoneyList[7];
                            continue;
                        }
                        if (RedRight == 5)
                        {
                            Description5++;
                            WinMoney += WinMoneyList[8];
                            WinMoneyNoWithTax += WinMoneyList[9];
                            continue;
                        }
                        if ((RedRight == 4) && BlueRight)
                        {
                            Description6++;
                            WinMoney += WinMoneyList[10];
                            WinMoneyNoWithTax += WinMoneyList[11];
                            continue;
                        }
                        if (RedRight == 4)
                        {
                            Description7++;
                            WinMoney += WinMoneyList[12];
                            WinMoneyNoWithTax += WinMoneyList[13];
                            continue;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "һ�Ƚ�" + Description1.ToString() + "ע";
                if (Description2 > 0)
                {
                    if (Description != "")
                        Description += "��";
                    Description += "���Ƚ�" + Description2.ToString() + "ע";
                }
                if (Description3 > 0)
                {
                    if (Description != "")
                        Description += "��";
                    Description += "���Ƚ�" + Description3.ToString() + "ע";
                }
                if (Description4 > 0)
                {
                    if (Description != "")
                        Description += "��";
                    Description += "�ĵȽ�" + Description4.ToString() + "ע";
                }
                if (Description5 > 0)
                {
                    if (Description != "")
                        Description += "��";
                    Description += "��Ƚ�" + Description5.ToString() + "ע";
                }
                if (Description6 > 0)
                {
                    if (Description != "")
                        Description += "��";
                    Description += "���Ƚ�" + Description6.ToString() + "ע";
                }
                if (Description7 > 0)
                {
                    if (Description != "")
                        Description += "��";
                    Description += "�ߵȽ�" + Description7.ToString() + "ע";
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }

            public override string AnalyseScheme(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_D)
                    RegexString = @"(\d\d\s){6}\d\d";
                else
                    RegexString = @"(\d\d\s){6,29}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle(m.Value, ref CanonicalNumber, PlayType);
                        if (singles == null)
                            continue;
                        if ((singles.Length >= ((PlayType == PlayType_D) ? 1 : 2)))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override bool AnalyseWinNumber(string Number)
            {
                Regex regex = new Regex(@"(\d\d\s){7}[+]\s\d\d", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (!regex.IsMatch(Number))
                    return false;

                string t_str = "";
                string[] WinLotteryNumber = ToSingle(Number.Substring(0, 20), ref t_str, PlayType_D);

                if ((WinLotteryNumber == null) || (WinLotteryNumber.Length != 1))
                    return false;

                return true;
            }

            private string[] FilterRepeated(string[] NumberPart)
            {
                ArrayList al = new ArrayList();
                for (int i = 0; i < NumberPart.Length; i++)
                {
                    int Ball = Shove._Convert.StrToInt(NumberPart[i], -1);
                    if ((Ball >= 1) && (Ball <= 30) && (!isExistBall(al, Ball)))
                        al.Add(NumberPart[i]);
                }

                CompareToAscii compare = new CompareToAscii();
                al.Sort(compare);

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString().PadLeft(2, '0');

                return Result;
            }

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, "");
            }

            public override string GetPrintKeyList(string Number, int PlayTypeID, string LotteryMachine)    //���ݳ�Ʊ�����ͣ�ת������Ҫ���͵���Ʊ���� Key �б�
            {
                Number = Number.Trim();
                if (Number == "")
                {
                    return "";
                }

                string[] Numbers = Number.Split('\n');
                if ((Numbers == null) || (Numbers.Length < 1))
                {
                    return "";
                }

                switch (LotteryMachine)
                {
                    case "����Ͷעϵͳ2.2":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_F))
                        {
                            return GetPrintKeyList_FCTZST2_2(Numbers);
                        }
                        break;
                    case "FCR8000":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_F))
                        {
                            return GetPrintKeyList_FCR8000(Numbers);
                        }
                        break;
                    case "LT-E":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_LT_E_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_LT_E_F(Numbers);
                        }
                        break;
                    case "LT-E02":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_LT_E02_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_LT_E02_F(Numbers);
                        }
                        break;
                    case "SN-3000CQA":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_F))
                        {
                            return GetPrintKeyList_SN_3000CQA_D(Numbers);
                        }
                        break;
                    case "SN-2000":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_F))
                        {
                            return GetPrintKeyList_SN_3000CQA_D(Numbers);
                        }
                        break;
                    case "SN_3000CG":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_SN_3000CG_D(Numbers);
                        }
                        else if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_SN_3000CG_F(Numbers);
                        }
                        break;
                }

                return "";
            }
            #region GetPrintKeyList �ľ��巽��
            private string GetPrintKeyList_FCTZST2_2(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_FCR8000(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_LT_E_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {

                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7");

                return KeyList;
            }
            private string GetPrintKeyList_LT_E_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {

                    KeyList += "[" + Convert.ToString((Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").Length / 2)) + "]";
                    KeyList += "[ENTER]";

                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7");


                return KeyList;
            }

            private string GetPrintKeyList_LT_E02_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {

                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList.Replace("0", "O").Replace("1", "3").Replace("2", "A").Replace("3", "B").Replace("4", "6").Replace("5", "C").Replace("6", "D").Replace("7", "9").Replace("8", "E").Replace("9", "F");

                return KeyList;
            }
            private string GetPrintKeyList_LT_E02_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {

                    KeyList += "[" + Convert.ToString((Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").Length / 2)) + "]";
                    KeyList += "[ENTER]";

                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList.Replace("0", "O").Replace("1", "3").Replace("2", "A").Replace("3", "B").Replace("4", "6").Replace("5", "C").Replace("6", "D").Replace("7", "9").Replace("8", "E").Replace("9", "F");


                return KeyList;
            }

            private string GetPrintKeyList_SN_3000CQA_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_SN_2000_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_SN_3000CG_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_SN_3000CG_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList += "[ENTER]";

                return KeyList;
            }
            #endregion

            //����˵�� PlayTypeID:�淨���; Number:Ͷע����; Multiple: ����; MaxMultiple:�����;
            public override Ticket[] ToElectronicTicket_HPSH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_HPSH_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_HPSH_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_HPSH �ľ��巽��
            private Ticket[] ToElectronicTicket_HPSH_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i += 5)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        for (int M = 0; M < 5; M++)
                        {
                            if ((i + M) < strs.Length)
                            {
                                Numbers += strs[i + M].ToString().Split('|')[0] + "\n";
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(101, ConvertFormatToElectronTicket_HPSH(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPSH_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(102, ConvertFormatToElectronTicket_HPSH(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            // ת��Ϊ��Ҫ�Ĳ�Ʊ��ʽ
            private string ConvertFormatToElectronTicket_HPSH(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F)
                {
                    Ticket = Number.Replace(" ", ",");
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_HPJX(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_HPJX_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_HPJX_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_HPJX �ľ��巽��
            private Ticket[] ToElectronicTicket_HPJX_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i += 5)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        for (int M = 0; M < 5; M++)
                        {
                            if ((i + M) < strs.Length)
                            {
                                Numbers += strs[i + M].ToString().Split('|')[0] + "\n";
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(101, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPJX_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(102, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            // ת��Ϊ��Ҫ�Ĳ�Ʊ��ʽ
            private string ConvertFormatToElectronTicket_HPJX(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F)
                {
                    Ticket = Number.Replace(" ", ",");
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_ZCW(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_ZCW_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_ZCW_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_ZCW �ľ��巽��
            private Ticket[] ToElectronicTicket_ZCW_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i += 5)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        for (int M = 0; M < 5; M++)
                        {
                            if ((i + M) < strs.Length)
                            {
                                Numbers += strs[i + M].ToString().Split('|')[0] + "\n";
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_ZCW_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            // ת��Ϊ��Ҫ�Ĳ�Ʊ��ʽ
            private string ConvertFormatToElectronTicket_ZCW(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F)
                {
                    Ticket = Number.Replace(" ", "");
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_DYJ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_DYJ_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_DYJ_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_DYJ �ľ��巽��
            private Ticket[] ToElectronicTicket_DYJ_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i += 5)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        for (int M = 0; M < 5; M++)
                        {
                            if ((i + M) < strs.Length)
                            {
                                Numbers += strs[i + M].ToString().Split('|')[0] + ";\n";
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(101, ConvertFormatToElectronTicket_DYJ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_DYJ_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0] + ";";
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(102, ConvertFormatToElectronTicket_DYJ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            // ת��Ϊ��Ҫ�Ĳ�Ʊ��ʽ
            private string ConvertFormatToElectronTicket_DYJ(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F)
                {
                    Ticket = Number;
                }

                return Ticket;
            }
            #endregion
        }
    }
}