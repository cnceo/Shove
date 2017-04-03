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
        /// ������22ѡ5
        /// </summary>
        public partial class CTFC22X5 : LotteryBase
        {
            #region ��̬����
            public const int PlayType_D = 1201;
            public const int PlayType_F = 1202;

            public const int ID = 12;
            public const string sID = "12";
            public const string Name = "������22ѡ5";
            public const string Code = "CTFC22X5";
            public const double MaxMoney = 20000;
            #endregion

            public CTFC22X5()
            {
                id = 12;
                name = "������22ѡ5";
                code = "CTFC22X5";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 1201) && (play_type <= 1202));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[2];

                Result[0] = new PlayType(PlayType_D, "��ʽ");
                Result[1] = new PlayType(PlayType_F, "��ʽ");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 12
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();
                ArrayList al = new ArrayList();

                for (int i = 0; i < Num; i++)
                {
                    al.Clear();
                    for (int j = 0; j < 5; j++)
                    {
                        int Ball = 0;
                        while ((Ball == 0) || isExistBall(al, Ball))
                            Ball = rd.Next(1, 22 + 1);
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

                if (strs.Length < 5)
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
                for (int i = 0; i < n - 4; i++)
                    for (int j = i + 1; j < n - 3; j++)
                        for (int k = j + 1; k < n - 2; k++)
                            for (int x = k + 1; x < n - 1; x++)
                                for (int y = x + 1; y < n; y++)
                                    al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[x] + " " + strs[y]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;
                if ((WinMoneyList == null) || (WinMoneyList.Length < 6))
                    return -3;

                int Description1 = 0, Description2 = 0, Description3 = 0;
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
                        if (Lottery[i].Length < 14)
                            continue;

                        string[] Red = new string[5];
                        Regex regex = new Regex(@"(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d\s)(?<R4>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);
                        int j;
                        int RedRight = 0;
                        bool Full = true;
                        for (j = 0; j < 5; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.IndexOf(Red[j]) >= 0)
                                RedRight++;
                        }
                        if (!Full)
                            continue;

                        if (RedRight == 5)
                        {
                            Description1++;
                            WinMoney += WinMoneyList[0];
                            WinMoneyNoWithTax += WinMoneyList[1];
                            continue;
                        }
                        if (RedRight == 4)
                        {
                            Description2++;
                            WinMoney += WinMoneyList[2];
                            WinMoneyNoWithTax += WinMoneyList[3];
                            continue;
                        }
                        if (RedRight == 3)
                        {
                            Description3++;
                            WinMoney += WinMoneyList[4];
                            WinMoneyNoWithTax += WinMoneyList[5];
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
                    RegexString = @"(\d\d\s){4}\d\d";
                else
                    RegexString = @"(\d\d\s){4,21}\d\d";
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
                        if ((singles.Length >= ((PlayType == PlayType_D) ? 1 : 2)) && (singles.Length <= (MaxMoney / 2)))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override bool AnalyseWinNumber(string Number)
            {
                string t_str = "";
                string[] WinLotteryNumber = ToSingle(Number, ref t_str, PlayType_D);

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
                    if ((Ball >= 1) && (Ball <= 22) && (!isExistBall(al, Ball)))
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
                    case "LT-E02":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_F))
                        {
                            return GetPrintKeyList_LT_E02(Numbers);
                        }
                        break;
                }

                return "";
            }
            #region GetPrintKeyList �ľ��巽��
            private string GetPrintKeyList_LT_E02(string[] Numbers)
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
            #endregion
        }
    }
}