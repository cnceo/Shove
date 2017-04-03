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
        /// ���Ϸ��22ѡ5
        /// </summary>
        public partial class HNFC22X5 : LotteryBase
        {
            #region ��̬����
            public const int PlayType_D = 2301;
            public const int PlayType_F = 2302;

            public const int PlayType_HY1_D = 2303;
            public const int PlayType_HY1_F = 2304;

            public const int PlayType_HY2_D = 2305;
            public const int PlayType_HY2_F = 2306;

            public const int PlayType_HY3_D = 2307;
            public const int PlayType_HY3_F = 2308;

            public const int PlayType_HY4_D = 2309;
            public const int PlayType_HY4_F = 2310;

            public const int ID = 23;
            public const string sID = "23";
            public const string Name = "���Ϸ��22ѡ5";
            public const string Code = "HNFC22X5";
            public const double MaxMoney = 52668;
            #endregion

            public HNFC22X5()
            {
                id = 23;
                name = "���Ϸ��22ѡ5";
                code = "HNFC22X5";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 2301) && (play_type <= 2310));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[10];

                Result[0] = new PlayType(PlayType_D, "��ʽ");
                Result[1] = new PlayType(PlayType_F, "��ʽ");
                Result[2] = new PlayType(PlayType_HY1_D, "����һ��ʽ");
                Result[3] = new PlayType(PlayType_HY1_F, "����һ��ʽ");
                Result[4] = new PlayType(PlayType_HY2_D, "���˶���ʽ");
                Result[5] = new PlayType(PlayType_HY2_F, "���˶���ʽ");
                Result[6] = new PlayType(PlayType_HY3_D, "��������ʽ");
                Result[7] = new PlayType(PlayType_HY3_F, "��������ʽ");
                Result[8] = new PlayType(PlayType_HY4_D, "�����ĵ�ʽ");
                Result[9] = new PlayType(PlayType_HY4_F, "�����ĸ�ʽ");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 23
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
                if ((PlayType == PlayType_D) || (PlayType == PlayType_F))
                    return ToSingle_Zhi(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_HY1_D) || (PlayType == PlayType_HY1_F))
                    return ToSingle_HY1(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_HY2_D) || (PlayType == PlayType_HY2_F))
                    return ToSingle_HY2(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_HY3_D) || (PlayType == PlayType_HY3_F))
                    return ToSingle_HY3(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_HY4_D) || (PlayType == PlayType_HY4_F))
                    return ToSingle_HY4(Number, ref CanonicalNumber);

                return null;
            }
            #region ToSingle �ľ��巽��
            private string[] ToSingle_Zhi(string Number, ref string CanonicalNumber)
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
            private string[] ToSingle_HY1(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '));
                CanonicalNumber = "";

                if (strs.Length < 1)
                {
                    CanonicalNumber = "";
                    return null;
                }

                for (int i = 0; i < strs.Length; i++)
                    CanonicalNumber += (strs[i] + " ");
                CanonicalNumber = CanonicalNumber.Trim();

                return strs;
            }
            private string[] ToSingle_HY2(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '));
                CanonicalNumber = "";

                if (strs.Length < 2)
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
                for (int i = 0; i < n - 1; i++)
                    for (int j = i + 1; j < n; j++)
                        al.Add(strs[i] + " " + strs[j]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_HY3(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '));
                CanonicalNumber = "";

                if (strs.Length < 3)
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
                for (int i = 0; i < n - 2; i++)
                    for (int j = i + 1; j < n - 1; j++)
                        for (int k = j + 1; k < n; k++)
                            al.Add(strs[i] + " " + strs[j] + " " + strs[k]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_HY4(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '));
                CanonicalNumber = "";

                if (strs.Length < 4)
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
                for (int i = 0; i < n - 3; i++)
                    for (int j = i + 1; j < n - 2; j++)
                        for (int k = j + 1; k < n - 1; k++)
                            for (int m = k + 1; m < n; m++)
                                al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[m]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            #endregion

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                if ((WinMoneyList == null) || (WinMoneyList.Length < 14))   //����˳�� 1-3�Ƚ�������1-4
                    return -3;

                if ((PlayType == PlayType_D) || (PlayType == PlayType_F))
                    return ComputeWin_Zhi(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);

                if ((PlayType == PlayType_HY1_D) || (PlayType == PlayType_HY1_F))
                    return ComputeWin_HY1(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[6], WinMoneyList[7]);

                if ((PlayType == PlayType_HY2_D) || (PlayType == PlayType_HY2_F))
                    return ComputeWin_HY2(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[8], WinMoneyList[9]);

                if ((PlayType == PlayType_HY3_D) || (PlayType == PlayType_HY3_F))
                    return ComputeWin_HY3(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[10], WinMoneyList[11]);

                if ((PlayType == PlayType_HY4_D) || (PlayType == PlayType_HY4_F))
                    return ComputeWin_HY4(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[12], WinMoneyList[13]);

                return -4;
            }
            #region ComputeWin �ľ��巽��
            private double ComputeWin_Zhi(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3)	//�����н�
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int Description1 = 0, Description2 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_Zhi(Lotterys[ii], ref t_str);
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
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            continue;
                        }
                        if (RedRight == 4)
                        {
                            Description2++;
                            WinMoney += WinMoney2;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2;
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
                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_HY1(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//�����н�
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//22: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                string Blue = WinNumber.Substring(0, 2);    //��һ�������ĺ�

                int Description1 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_HY1(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 2)	//2: "01"��"02"...������
                            continue;

                        if (Lottery[i] == Blue)
                        {
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "����һ��" + Description1.ToString() + "ע";

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_HY2(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//�����н�
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int Description1 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_HY2(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 5)	//5: "01 02"
                            continue;

                        string[] Red = new string[2];
                        Regex regex = new Regex(@"(?<R0>\d\d\s)(?<R1>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);

                        int RedRight = 0;
                        bool Full = true;
                        for (int j = 0; j < 2; j++)
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

                        if (RedRight == 2)
                        {
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "���˶���" + Description1.ToString() + "ע";

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_HY3(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//�����н�
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int Description1 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_HY3(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 8)	//5: "01 02 03"
                            continue;

                        string[] Red = new string[3];
                        Regex regex = new Regex(@"(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);

                        int RedRight = 0;
                        bool Full = true;
                        for (int j = 0; j < 3; j++)
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

                        if (RedRight == 3)
                        {
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "��������" + Description1.ToString() + "ע";

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_HY4(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//�����н�
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int Description1 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_HY4(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 11)	//5: "01 02 03 04"
                            continue;

                        string[] Red = new string[4];
                        Regex regex = new Regex(@"(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);

                        int RedRight = 0;
                        bool Full = true;
                        for (int j = 0; j < 4; j++)
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

                        if (RedRight == 4)
                        {
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "�����Ľ�" + Description1.ToString() + "ע";

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if ((PlayType == PlayType_D) || (PlayType == PlayType_F))
                    return AnalyseScheme_Zhi(Content, PlayType);

                if ((PlayType == PlayType_HY1_D) || (PlayType == PlayType_HY1_F))
                    return AnalyseScheme_HY1(Content, PlayType);

                if ((PlayType == PlayType_HY2_D) || (PlayType == PlayType_HY2_F))
                    return AnalyseScheme_HY2(Content, PlayType);

                if ((PlayType == PlayType_HY3_D) || (PlayType == PlayType_HY3_F))
                    return AnalyseScheme_HY3(Content, PlayType);

                if ((PlayType == PlayType_HY4_D) || (PlayType == PlayType_HY4_F))
                    return AnalyseScheme_HY4(Content, PlayType);

                return "";
            }
            #region AnalyseScheme �ľ��巽��
            private string AnalyseScheme_Zhi(string Content, int PlayType)
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
                    RegexString = @"(\d\d\s){4,25}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zhi(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_HY1(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_HY1_D)
                    RegexString = @"(\d\d)";
                else
                    RegexString = @"((\d\d\s){1,21}\d\d)|(\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_HY1(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if ((singles.Length >= ((PlayType == PlayType_HY1_D) ? 1 : 2)) && (singles.Length <= (MaxMoney / 2)))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_HY2(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_HY2_D)
                    RegexString = @"(\d\d\s\d\d)";
                else
                    RegexString = @"(\d\d\s){1,25}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_HY2(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if ((singles.Length >= ((PlayType == PlayType_HY2_D) ? 1 : 2)) && (singles.Length <= (MaxMoney / 2)))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_HY3(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_HY3_D)
                    RegexString = @"(\d\d\s){2}\d\d";
                else
                    RegexString = @"(\d\d\s){2,25}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_HY3(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if ((singles.Length >= ((PlayType == PlayType_HY3_D) ? 1 : 2)) && (singles.Length <= (MaxMoney / 2)))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_HY4(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_HY4_D)
                    RegexString = @"(\d\d\s){3}\d\d";
                else
                    RegexString = @"(\d\d\s){3,21}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_HY4(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if ((singles.Length >= ((PlayType == PlayType_HY4_D) ? 1 : 2)) && (singles.Length <= (MaxMoney / 2)))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            #endregion

            public override bool AnalyseWinNumber(string Number)
            {
                string t_str = "";
                string[] WinLotteryNumber = ToSingle_Zhi(Number, ref t_str);

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
                    case "SN-2000":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_HY1_D) || (PlayTypeID == PlayType_HY2_D) || (PlayTypeID == PlayType_HY3_D) ||
                             (PlayTypeID == PlayType_HY4_D))
                        {
                            return GetPrintKeyList_SN_2000_D(Numbers);
                        }
                        if ((PlayTypeID == PlayType_F) || (PlayTypeID == PlayType_HY1_F) || (PlayTypeID == PlayType_HY2_F) || (PlayTypeID == PlayType_HY3_F) ||
                            (PlayTypeID == PlayType_HY4_F))
                        {
                            return GetPrintKeyList_SN_2000_F(Numbers);
                        }
                        break;
                }

                return "";
            }
            #region GetPrintKeyList �ľ��巽��
            private string GetPrintKeyList_SN_2000_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace("+", "").Replace(" ", "").Replace("\r", "").Replace("\n", "");

                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_SN_2000_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace("+", "").Replace(" ", "").Replace("\r", "").Replace("\n", "");

                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList + "[��][E]";
                return KeyList;
            }
            #endregion
        }
    }
}