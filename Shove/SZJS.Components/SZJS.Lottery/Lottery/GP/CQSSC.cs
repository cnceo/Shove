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
        /// ����ʱʱ��
        /// </summary>
        public partial class CQSSC : LotteryBase
        {
            #region ��̬����
            public const int PlayType_Mixed = 2800; //���Ͷע�������������е��淨������ǰ�� [��ʽ]��[��ʽ]��[����淨]��[�´�С]��[������ѡ��ʽ]��[������ѡ��ʽ]��[������ѡ��λ]��[������ѡ����]��[������ѡ����]��[������ѡ����]...

            public const int PlayType_D = 2801;  //������,ע�⣬5�Ǹ�ʽ��ʾ��5�ǣ�3�ǣ�2�ǣ�1�ǹ�4ע����ʽ�ĸ���ʹ�ͳ��Ʊ��ͬ
            public const int PlayType_F = 2802;

            public const int PlayType_ZH = 2803;  //����淨���ʹ�ͳ��Ʊ��ʽ����һ��

            public const int PlayType_DX = 2804;  //�´�С

            public const int PlayType_5X_TXD = 2805; //����ͨѡ��ʽ
            public const int PlayType_5X_TXF = 2806; //����ͨѡ��ʽ

            public const int PlayType_2X_ZuD = 2807; //������ѡ��ʽ
            public const int PlayType_2X_ZuF = 2808; //������ѡ��ʽ
            public const int PlayType_2X_ZuFW = 2809; //������ѡ��λ
            public const int PlayType_2X_ZuB = 2810; // ������ѡ����
            public const int PlayType_2X_ZuBD = 2811; // ������ѡ����

            public const int PlayType_3X_B = 2812; //���ǰ���

            //stone 2009-06-30
            public const int PlayType_3X_Zu3D = 2813; //������3��ʽ
            public const int PlayType_3X_Zu3F = 2814; //������3��ʽ

            public const int PlayType_3X_Zu6D = 2815; //������6��ʽ
            public const int PlayType_3X_Zu6F = 2816; //������6��ʽ

            public const int PlayType_3X_ZHFS = 2817; //����ֱѡ��ϸ�ʽ
            public const int PlayType_3X_ZuB = 2818;  //������ѡ����
            public const int PlayType_3X_ZuBD = 2819; //������ѡ����

            public const int ID = 28;
            public const string sID = "28";
            public const string Name = "����ʱʱ��";
            public const string Code = "SHSSL";
            public const double MaxMoney = 200000;
            #endregion

            public CQSSC()
            {
                id = 28;
                name = "����ʱʱ��";
                code = "CQSSC";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 2800) && (play_type <= 2819));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[20];

                Result[0] = new PlayType(PlayType_Mixed, "���Ͷע");
                Result[1] = new PlayType(PlayType_D, "��ʽ");
                Result[2] = new PlayType(PlayType_F, "��ʽ");
                Result[3] = new PlayType(PlayType_ZH, "����淨");
                Result[4] = new PlayType(PlayType_DX, "�´�С");
                Result[5] = new PlayType(PlayType_5X_TXD, "����ͨѡ��ʽ");
                Result[6] = new PlayType(PlayType_5X_TXF, "����ͨѡ��ʽ");
                Result[7] = new PlayType(PlayType_2X_ZuD, "������ѡ��ʽ");
                Result[8] = new PlayType(PlayType_2X_ZuF, "������ѡ��ʽ");
                Result[9] = new PlayType(PlayType_2X_ZuFW, "������ѡ��λ");
                Result[10] = new PlayType(PlayType_2X_ZuB, "������ѡ����");
                Result[11] = new PlayType(PlayType_2X_ZuBD, "������ѡ����");
                Result[12] = new PlayType(PlayType_3X_B, "���ǰ���");
                Result[13] = new PlayType(PlayType_3X_Zu3D, "������3��ʽ");
                Result[14] = new PlayType(PlayType_3X_Zu3F, "������3��ʽ");
                Result[15] = new PlayType(PlayType_3X_Zu6D, "������6��ʽ");
                Result[16] = new PlayType(PlayType_3X_Zu6F, "������6��ʽ");
                Result[17] = new PlayType(PlayType_3X_ZHFS, "����ֱѡ��ϸ�ʽ");
                Result[18] = new PlayType(PlayType_3X_ZuB, "������ѡ����");
                Result[19] = new PlayType(PlayType_3X_ZuBD, "������ѡ����");

                return Result;
            }

            public override string BuildNumber(int Num, int Type)       //Type: 5 = 5��, 3 = 3��, 2 = 2��, 1 = 1��, -1 = ��С��˫
            {
                if ((Type != 5) && (Type != 3) && (Type != 2) && (Type != 1) && (Type != -1))
                    Type = 5;

                if (Type == -1) //��С��˫
                {
                    return BuildNumber_DX(Num);
                }

                return BuildNumber_5321(Num, Type);
            }
            #region BuilderNumber �ľ��巽��
            private string BuildNumber_5321(int Num, int Type)
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";

                    for (int j = Type; j < 5; j++)
                        LotteryNumber += "-";

                    for (int j = 0; j < Type; j++)
                        LotteryNumber += rd.Next(0, 9 + 1).ToString();

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);

                return Result;

            }
            private string BuildNumber_DX(int Num)   //��С��˫
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";

                    for (int j = 0; j < 2; j++)
                        LotteryNumber += "��С��˫".Substring(rd.Next(0, 3 + 1), 1);

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);

                return Result;
            }
            #endregion

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)   //��ʽȡ��ʽ, ���� ref �����ǽ���Ʊ�淶�����磺103(00)... ���1030...
            {
                if (PlayType == PlayType_Mixed)
                    return ToSingle_Mixed(Number, ref CanonicalNumber);

                if (PlayType == PlayType_D)
                    return ToSingle_D(Number, ref CanonicalNumber);

                if (PlayType == PlayType_F)
                    return ToSingle_F(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZH)
                    return ToSingle_ZH(Number, ref CanonicalNumber);

                if (PlayType == PlayType_DX)
                    return ToSingle_DX(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_5X_TXD) || (PlayType == PlayType_5X_TXF))
                    return ToSingle_5X_TX(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_2X_ZuD) || (PlayType == PlayType_2X_ZuF))
                    return ToSingle_2X_ZuD_ZuF(Number, ref CanonicalNumber);

                if (PlayType == PlayType_2X_ZuFW)
                    return ToSingle_2X_ZuFW(Number, ref CanonicalNumber);

                if (PlayType == PlayType_2X_ZuB)
                    return ToSingle_2X_ZuB(Number, ref CanonicalNumber);

                if (PlayType == PlayType_2X_ZuBD)
                    return ToSingle_2X_ZuBD(Number, ref CanonicalNumber);

                if (PlayType == PlayType_3X_B)
                    return ToSingle_3X_B(Number, ref CanonicalNumber);

                //stone 2009-06-30
                if (PlayType == PlayType_3X_Zu3D)
                    return ToSingle_3X_Zu3D(Number, ref CanonicalNumber);

                if (PlayType == PlayType_3X_Zu3F)
                    return ToSingle_3X_Zu3F(Number, ref CanonicalNumber);

                if (PlayType == PlayType_3X_Zu6D)
                    return ToSingle_3X_Zu6D(Number, ref CanonicalNumber);

                if (PlayType == PlayType_3X_Zu6F)
                    return ToSingle_3X_Zu6F(Number, ref CanonicalNumber);

                if (PlayType == PlayType_3X_ZHFS)
                    return ToSingle_3X_ZHFS(Number, ref CanonicalNumber);

                if (PlayType == PlayType_3X_ZuB)
                    return ToSingle_3X_ZuB(Number, ref CanonicalNumber);

                if (PlayType == PlayType_3X_ZuBD)
                    return ToSingle_3X_ZuBD(Number, ref CanonicalNumber);

                return null;
            }

            #region ToSingle �ľ��巽��

            private string[] ToSingle_Mixed(string Number, ref string CanonicalNumber)
            {
                CanonicalNumber = "";

                string PreFix = GetLotteryNumberPreFix(Number);

                if (Number.StartsWith("[��ʽ]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_D(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[��ʽ]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_F(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[����淨]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_ZH(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[�´�С]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_DX(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[����ͨѡ��ʽ]") || Number.StartsWith("[����ͨѡ��ʽ]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_5X_TX(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[������ѡ��ʽ]") || Number.StartsWith("[������ѡ��ʽ]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_2X_ZuD_ZuF(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[������ѡ��λ]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_2X_ZuFW(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[������ѡ����]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_2X_ZuB(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[������ѡ����]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_2X_ZuBD(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[���ǰ���]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_3X_B(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                return null;
            }
            private string[] ToSingle_D(string Number, ref string CanonicalNumber)
            {
                CanonicalNumber = "";

                Regex regex = new Regex(@"(?<L0>(\d)|(-))(?<L1>(\d)|(-))(?<L2>(\d)|(-))(?<L3>(\d)|(-))(?<L4>(\d))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 5; i++)
                {
                    string Locate = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    CanonicalNumber += Locate;
                }

                Regex[] regex5321 = new Regex[4];
                regex5321[0] = new Regex(@"(\d){5}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex5321[1] = new Regex(@"--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex5321[2] = new Regex(@"---(\d){2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex5321[3] = new Regex(@"----\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                bool isMatch = false;
                for (int i = 0; i < 4; i++)
                {
                    if (regex5321[i].IsMatch(CanonicalNumber))
                    {
                        isMatch = true;
                        break;
                    }
                }

                if (!isMatch)
                    return null;

                string[] Result = new string[1];
                Result[0] = CanonicalNumber;

                return Result;
            }
            private string[] ToSingle_F(string Number, ref string CanonicalNumber)
            {
                string[] t_strs = ToSingle_D(Number, ref CanonicalNumber);

                if ((t_strs == null) || (t_strs.Length != 1))
                    return null;

                Regex[] regex5321 = new Regex[4];
                regex5321[0] = new Regex(@"(\d){5}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex5321[1] = new Regex(@"--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex5321[2] = new Regex(@"---(\d){2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex5321[3] = new Regex(@"----\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                string[] Result = null;

                if (regex5321[0].IsMatch(CanonicalNumber))
                {
                    Result = new string[4];

                    Result[0] = CanonicalNumber;
                    Result[1] = "--" + CanonicalNumber.Substring(2, 3);
                    Result[2] = "---" + CanonicalNumber.Substring(3, 2);
                    Result[3] = "----" + CanonicalNumber.Substring(4, 1);
                }

                else if (regex5321[1].IsMatch(CanonicalNumber))
                {
                    Result = new string[3];

                    Result[0] = CanonicalNumber;
                    Result[1] = "---" + CanonicalNumber.Substring(3, 2);
                    Result[2] = "----" + CanonicalNumber.Substring(4, 1);
                }

                else if (regex5321[2].IsMatch(CanonicalNumber))
                {
                    Result = new string[2];

                    Result[0] = CanonicalNumber;
                    Result[1] = "----" + CanonicalNumber.Substring(4, 1);
                }

                else if (regex5321[3].IsMatch(CanonicalNumber))
                {
                    Result = new string[1];

                    Result[0] = CanonicalNumber;
                }

                return Result;
            }
            private string[] ToSingle_ZH(string Number, ref string CanonicalNumber)
            {
                string[] Locate = new string[5];
                CanonicalNumber = "";

                Regex regex = new Regex(@"(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 5; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);
                        if (Locate[i] == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }
                    if (Locate[i].Length > 1)
                        CanonicalNumber += "(" + Locate[i] + ")";
                    else
                        CanonicalNumber += Locate[i];
                }

                ArrayList al = new ArrayList();

                #region ѭ��ȡ��ʽ
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0][i_0].ToString();
                    for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                    {
                        string str_1 = str_0 + Locate[1][i_1].ToString();
                        for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                        {
                            string str_2 = str_1 + Locate[2][i_2].ToString();
                            for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                            {
                                string str_3 = str_2 + Locate[3][i_3].ToString();
                                for (int i_4 = 0; i_4 < Locate[4].Length; i_4++)
                                {
                                    string str_4 = str_3 + Locate[4][i_4].ToString();

                                    string str_4_Canonical = "";
                                    string[] strs_4 = ToSingle_D(str_4, ref str_4_Canonical);

                                    if ((strs_4 == null) || (strs_4.Length < 1))
                                        continue;

                                    al.Add(str_4);
                                }
                            }
                        }
                    }
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_DX(string Number, ref string CanonicalNumber)	//�´�С
            {
                CanonicalNumber = "";

                Regex regex = new Regex(@"(?<L0>([��С��˫]))(?<L1>([��С��˫]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 2; i++)
                {
                    string Locate = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    CanonicalNumber += Locate;
                }

                string[] Result = new string[1];
                Result[0] = CanonicalNumber;

                return Result;
            }
            private string[] ToSingle_5X_TX(string Number, ref string CanonicalNumber)  // ����ͨѡ ��ʽȡ��ʽ, ���� ref �����ǽ���Ʊ�淶�����磺10(223)45 ���10(23)45
            {
                string[] Locate = new string[5];
                CanonicalNumber = "";

                Regex regex = new Regex(@"(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 5; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);
                        if (Locate[i] == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }
                    if (Locate[i].Length > 1)
                        CanonicalNumber += "(" + Locate[i] + ")";
                    else
                        CanonicalNumber += Locate[i];
                }

                ArrayList al = new ArrayList();

                #region ѭ��ȡ��ʽ
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0][i_0].ToString();
                    for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                    {
                        string str_1 = str_0 + Locate[1][i_1].ToString();
                        for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                        {
                            string str_2 = str_1 + Locate[2][i_2].ToString();
                            for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                            {
                                string str_3 = str_2 + Locate[3][i_3].ToString();
                                for (int i_4 = 0; i_4 < Locate[4].Length; i_4++)
                                {
                                    string str_4 = str_3 + Locate[4][i_4].ToString();
                                    al.Add(str_4);
                                }
                            }
                        }
                    }
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_2X_ZuD_ZuF(string Number, ref string CanonicalNumber)//������ѡ ��ʽ����ʽ
            {
                CanonicalNumber = FilterRepeated(Number.Trim());

                if (CanonicalNumber.Length < 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                char[] strs = CanonicalNumber.ToCharArray();

                ArrayList al = new ArrayList();

                #region ѭ��ȡ��ʽ

                int n = strs.Length;
                for (int i = 0; i < n - 1; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        al.Add(strs[i].ToString() + strs[j].ToString());
                    }
                }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_2X_ZuFW(string Number, ref string CanonicalNumber) //������ѡ��λ
            {
                string[] Locate = new string[2];
                CanonicalNumber = "";

                Regex regex = new Regex(@"(?<L0>(\d)|([(][\d]{2,10}[)]))(?<L1>(\d)|([(][\d]{2,10}[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);

                if ((FilterRepeated(m.Groups["L0"].ToString()).Length < 2) && (FilterRepeated(m.Groups["L1"].ToString()).Length < 2))
                {
                    CanonicalNumber = "";

                    return null;
                }

                for (int i = 0; i < 2; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();

                    if (Locate[i].StartsWith("("))
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                        Locate[i] = FilterRepeated(Locate[i]);
                    }

                    if (Locate[i].Length < 1)
                    {
                        CanonicalNumber = "";

                        return null;
                    }

                    if (Locate[i].Length > 1)
                    {
                        CanonicalNumber += "(" + Locate[i] + ")";
                    }
                    else
                    {
                        CanonicalNumber += Locate[i];
                    }
                }

                ArrayList al = new ArrayList();

                #region ѭ��ȡ��ʽ
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0][i_0].ToString();
                    for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                    {
                        string str_1 = str_0 + Locate[1][i_1].ToString();
                        al.Add(str_1);
                    }
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_2X_ZuB(string sBill, ref string CanonicalNumber)//������ѡ����
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                ArrayList al = new ArrayList();

                #region ѭ��ȡ��ʽ

                if ((Bill < 0) || (Bill > 18))
                {
                    CanonicalNumber = "";
                    return null;
                }
                else
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        for (int j = i; j <= 9; j++)
                        {
                            if (i + j == Bill)
                                al.Add(i.ToString() + j.ToString());
                        }
                    }
                }

                #endregion

                CanonicalNumber = Bill.ToString();

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_2X_ZuBD(string sBill, ref string CanonicalNumber)//������ѡ����
            {
                CanonicalNumber = FilterRepeated(sBill.Trim());

                if (CanonicalNumber.Length < 1)
                {
                    CanonicalNumber = "";
                    return null;
                }

                if (CanonicalNumber.Length > 2)
                {
                    CanonicalNumber = CanonicalNumber.Substring(0, 2);
                }

                char[] strs = CanonicalNumber.ToCharArray();

                ArrayList al = new ArrayList();

                #region ѭ��ȡ��ʽ

                int n = strs.Length;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j <= 9; j++)
                    {
                        al.Add(strs[i].ToString() + j.ToString());
                    }
                }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_3X_B(string sBill, ref string CanonicalNumber)//���ǰ���
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                ArrayList al = new ArrayList();

                #region ѭ��ȡ��ʽ

                if ((Bill < 0) || (Bill > 27))
                {
                    CanonicalNumber = "";
                    return null;
                }
                else
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        for (int j = 0; j <= 9; j++)
                        {
                            for (int k = 0; k <= 9; k++)
                            {
                                if (i + j + k == Bill)
                                    al.Add(i.ToString() + j.ToString() + k.ToString());
                            }
                        }
                    }
                }

                #endregion

                CanonicalNumber = Bill.ToString();

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }

            //stone 2009-06-30
            private string[] ToSingle_3X_Zu3D(string Number, ref string CanonicalNumber) //������ʽ
            {
                CanonicalNumber = "";

                string tNumber = FilterRepeated(Number.Trim());
                if (tNumber.Length != 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Number;
                string[] Result = new string[1];
                Result[0] = CanonicalNumber;

                return Result;
            }
            private string[] ToSingle_3X_Zu3F(string Number, ref string CanonicalNumber) //������ʽ
            {
                CanonicalNumber = FilterRepeated(Number.Trim());
                if (CanonicalNumber.Length < 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                char[] strs = CanonicalNumber.ToCharArray();

                ArrayList al = new ArrayList();

                #region ѭ��ȡ��ʽ

                int n = strs.Length;
                for (int i = 0; i < n - 1; i++)
                    for (int j = i + 1; j < n; j++)
                    {
                        al.Add(strs[i].ToString() + strs[i].ToString() + strs[j].ToString());
                        al.Add(strs[i].ToString() + strs[j].ToString() + strs[j].ToString());
                    }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }
                return Result;
            }
            private string[] ToSingle_3X_Zu6D(string Number, ref string CanonicalNumber) //������ʽ
            {
                CanonicalNumber = "";

                string tNumber = FilterRepeated(Number.Trim());
                if (tNumber.Length != 3)
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Number;
                string[] Result = new string[1];
                Result[0] = CanonicalNumber;

                return Result;
            }
            private string[] ToSingle_3X_Zu6F(string Number, ref string CanonicalNumber) //������ʽ
            {
                //CanonicalNumber = "";

                //Regex regex = new Regex(@"(?<L0>(-))(?<L1>(-))(?<L2>(\d)|(-))(?<L3>(\d)|(-))(?<L4>(\d))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                //Match m = regex.Match(Number);
                //for (int i = 0; i < 5; i++)
                //{
                //    string Locate = m.Groups["L" + i.ToString()].ToString().Trim();
                //    if (Locate == "")
                //    {
                //        CanonicalNumber = "";
                //        return null;
                //    }

                //    CanonicalNumber += Locate;
                //}

                //Regex regex3 = new Regex(@"--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                //bool isMatch = false;
                //if (regex3.IsMatch(CanonicalNumber))
                //{
                //    isMatch = true;
                //}

                //if (!isMatch)
                //{
                //    return null;
                //}

                //string[] Result = new string[1];
                //Result[0] = CanonicalNumber;

                //return Result;

                return ToSingle_Zu3D_Zu6(Number, ref CanonicalNumber);
            }
            private string[] ToSingle_Zu3D_Zu6(string Number, ref string CanonicalNumber)	//��ʽȡ��ʽ, ���� ref �����ǽ���Ʊ�淶�����磺(10223) ���1023
            {
                CanonicalNumber = FilterRepeated(Number.Trim());
                if (CanonicalNumber.Length < 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                char[] strs = CanonicalNumber.ToCharArray();

                ArrayList al = new ArrayList();

                #region ѭ��ȡ��ʽ

                int n = strs.Length;

                if (n == 2)
                {
                    for (int i = 0; i < n - 1; i++)
                        for (int j = i + 1; j < n; j++)
                        {
                            al.Add(strs[i].ToString() + strs[i].ToString() + strs[j].ToString());
                            al.Add(strs[i].ToString() + strs[j].ToString() + strs[j].ToString());
                        }

                }
                else
                {
                    for (int i = 0; i < n - 2; i++)
                        for (int j = i + 1; j < n - 1; j++)
                            for (int k = j + 1; k < n; k++)
                                al.Add(strs[i].ToString() + strs[j].ToString() + strs[k].ToString());
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }

            private string[] ToSingle_3X_ZHFS(string Number, ref string CanonicalNumber) //����ֱѡ��ϸ�ʽ
            {
                CanonicalNumber = FilterRepeated(Number.Trim());
                if (CanonicalNumber.Length < 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                char[] strs = CanonicalNumber.ToCharArray();

                ArrayList al = new ArrayList();

                #region ѭ��ȡ��ʽ

                int n = strs.Length;

                for (int i = 0; i < strs.Length; i++)
                {
                    for (int j = 0; j < strs.Length; j++)
                    {
                        for (int k = 0; k < strs.Length; k++)
                        {
                            if (i != j && j != k && i != k && !al.Contains(strs[i].ToString() + strs[j].ToString() + strs[k].ToString()))
                            {
                                al.Add(strs[i].ToString() + strs[j].ToString() + strs[k].ToString());
                            }
                        }
                    }
                }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }
                return Result;
            }
            private string[] ToSingle_3X_ZuB(string Number, ref string CanonicalNumber) //������ѡ����
            {
                CanonicalNumber = Number;

                if ((CanonicalNumber.Length < 1) || (CanonicalNumber.Length > 2))
                {
                    CanonicalNumber = "";
                    return null;
                }

                char[] strs = CanonicalNumber.ToCharArray();

                ArrayList al = new ArrayList();

                #region ѭ��ȡ��ʽ

                int n = strs.Length;
                if (n == 1)
                {
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j <= 9; j++)
                        {
                            for (int k = 0; k <= 9; k++)
                            {
                                if (strs[i].ToString() == j.ToString() && !al.Contains(strs[i].ToString() + j.ToString() + k.ToString()))
                                {
                                    al.Add(strs[i].ToString() + j.ToString() + k.ToString());
                                }

                                if (j.ToString() == k.ToString() && !al.Contains(strs[i].ToString() + j.ToString() + k.ToString()))
                                {
                                    al.Add(strs[i].ToString() + j.ToString() + k.ToString());
                                }

                                if (strs[i].ToString() != j.ToString() && strs[i].ToString() != k.ToString() && j.ToString() != k.ToString() && !al.Contains(Sort(strs[i].ToString() + j.ToString() + k.ToString())))
                                {
                                    al.Add(Sort(strs[i].ToString() + j.ToString() + k.ToString()));
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j <= 9; j++)
                    {
                        al.Add(Number + j.ToString());
                    }
                }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_3X_ZuBD(string Number, ref string CanonicalNumber) //������ѡ����
            {
                int Bill = Shove._Convert.StrToInt(Number, -1);
                CanonicalNumber = "";

                ArrayList al = new ArrayList();

                #region ѭ��ȡ��ʽ

                //ѡ�������
                for (int i = 0; i <= 9; i++)
                    for (int j = 0; j <= 9; j++)
                        for (int k = 0; k <= 9; k++)
                            if (i + j + k == Bill && i == j && j == k)
                                al.Add(i.ToString() + j.ToString() + k.ToString());


                if ((Bill < 1) || (Bill > 26))
                {
                    if (al.Count > 0)
                    {
                        string[] ResultBZ = new string[al.Count];
                        for (int i = 0; i < al.Count; i++)
                        {
                            ResultBZ[i] = al[i].ToString();
                        }

                        CanonicalNumber = Bill.ToString();

                        return ResultBZ;
                    }

                    CanonicalNumber = "";
                    return null;
                }
                else
                {
                    //����
                    for (int i = 0; i <= 9; i++)
                        for (int j = 0; j <= 9; j++)
                        {
                            if (i == j)
                                continue;
                            if (i + i + j == Bill)
                                al.Add(i.ToString() + i.ToString() + j.ToString());
                        }
                }

                if ((Bill >= 3) && (Bill <= 24))
                {
                    //����
                    for (int i = 0; i <= 7; i++)
                        for (int j = i + 1; j <= 8; j++)
                            for (int k = j + 1; k <= 9; k++)
                                if (i + j + k == Bill)
                                    al.Add(i.ToString() + j.ToString() + k.ToString());
                }

                #endregion

                CanonicalNumber = Bill.ToString();

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }

            #endregion

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                Description = "";
                WinMoneyNoWithTax = 0;

                if ((WinMoneyList == null) || (WinMoneyList.Length < 20)) //�����������˳��(5 3 2 1�ǣ��´�С,ͨ������ѡ,������ѡ���Ӻ�,����ѡ123�Ƚ�
                    return -3;

                int WinCount5X = 0, WinCount3X = 0, WinCount2X = 0, WinCount1X = 0;
                int WinCountDX = 0;
                int WinCount_2X_Zu = 0, WinCount_2X_Zu_DZH = 0;
                int WinCount_5XTX_1 = 0, WinCount_5XTX_2 = 0, WinCount_5XTX_3 = 0, WinCount_3X_Zu3D = 0, WinCount_3X_Zu3F = 0, WinCount_3X_Zu6D = 0, WinCount_3X_Zu6F = 0, WinCount_3X_ZHFS = 0, WinCount_3X_ZX = 0;

                if (PlayType == PlayType_Mixed)
                    return ComputeWin_Mixed(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], WinMoneyList[8], WinMoneyList[9], WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13], WinMoneyList[14], WinMoneyList[15], WinMoneyList[16], WinMoneyList[17], WinMoneyList[18], WinMoneyList[19], WinMoneyList[20], WinMoneyList[21], WinMoneyList[22], WinMoneyList[23], WinMoneyList[24], WinMoneyList[25]);

                if (PlayType == PlayType_D)
                    return ComputeWin_D(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], ref WinCount5X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                if (PlayType == PlayType_F)
                    return ComputeWin_F(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], ref WinCount5X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                if (PlayType == PlayType_ZH)
                    return ComputeWin_ZH(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], ref WinCount5X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                if (PlayType == PlayType_DX)
                    return ComputeWin_DX(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[8], WinMoneyList[9], ref WinCountDX);

                if ((PlayType == PlayType_5X_TXD) || (PlayType == PlayType_5X_TXF))
                    return ComputeWin_5X_TX(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[14], WinMoneyList[15], WinMoneyList[16], WinMoneyList[17], WinMoneyList[18], WinMoneyList[19], ref WinCount_5XTX_1, ref WinCount_5XTX_2, ref WinCount_5XTX_3);

                if ((PlayType == PlayType_2X_ZuD) || (PlayType == PlayType_2X_ZuF))
                    return ComputeWin_2X_ZuD_ZuF(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[10], WinMoneyList[11], ref WinCount_2X_Zu);

                if (PlayType == PlayType_2X_ZuFW)
                    return ComputeWin_2X_ZuFW(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13], ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                if (PlayType == PlayType_2X_ZuB)
                    return ComputeWin_2X_ZuB(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13], ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                if (PlayType == PlayType_2X_ZuBD)
                    return ComputeWin_2X_ZuBD(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13], ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                if (PlayType == PlayType_3X_B)
                    return ComputeWin_3X_B(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], ref WinCount3X);

                //stone 2009-06-30
                if (PlayType == PlayType_3X_Zu3D)
                    return ComputeWin_3X_Zu3D(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[22], WinMoneyList[23], ref WinCount_3X_Zu3D);

                if (PlayType == PlayType_3X_Zu3F)
                    return ComputeWin_3X_Zu3F(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[22], WinMoneyList[23], ref WinCount_3X_Zu3F);

                if (PlayType == PlayType_3X_Zu6D)
                    return ComputeWin_3X_Zu6D(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[24], WinMoneyList[25], ref WinCount_3X_Zu6D);

                if (PlayType == PlayType_3X_Zu6F)
                    return ComputeWin_3X_Zu6F(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[24], WinMoneyList[25], ref WinCount_3X_Zu6F);

                if (PlayType == PlayType_3X_ZHFS)
                    return ComputeWin_3X_ZHFS(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[20], WinMoneyList[21], ref WinCount_3X_ZHFS);

                if (PlayType == PlayType_3X_ZuB)
                    return ComputeWin_3X_ZuB(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[20], WinMoneyList[21], WinMoneyList[22], WinMoneyList[23], WinMoneyList[24], WinMoneyList[25], ref WinCount_3X_ZX, ref WinCount_3X_Zu3D, ref WinCount_3X_Zu6D);

                if (PlayType == PlayType_3X_ZuBD)
                    return ComputeWin_3X_ZuBD(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[20], WinMoneyList[21], WinMoneyList[22], WinMoneyList[23], WinMoneyList[24], WinMoneyList[25], ref WinCount_3X_ZX, ref WinCount_3X_Zu3D, ref WinCount_3X_Zu6D);

                return -4;
            }

            #region ComputeWin  �ľ��巽��

            private double ComputeWin_Mixed(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, double WinMoney6, double WinMoneyNoWithTax6, double WinMoney7, double WinMoneyNoWithTax7, double WinMoney8, double WinMoneyNoWithTax8, double WinMoney9, double WinMoneyNoWithTax9, double WinMoney10, double WinMoneyNoWithTax10, double WinMoney11, double WinMoneyNoWithTax11, double WinMoney12, double WinMoneyNoWithTax12, double WinMoney13, double WinMoneyNoWithTax13)
            {
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                {
                    return -2;
                }

                if (Lotterys.Length < 1)
                {
                    return -2;
                }

                double WinMoney = 0;

                //�����������˳��(5 3 2 1�ǣ��´�С,������ѡ,������ѡ���Ӻ�,����ͨѡ123�Ƚ�)
                int WinCount1 = 0, WinCount2 = 0, WinCount3 = 0, WinCount4 = 0, WinCount5 = 0, WinCount6 = 0, WinCount7 = 0, WinCount8 = 0, WinCount9 = 0, WinCount10 = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    int WinCount5X = 0, WinCount3X = 0, WinCount2X = 0, WinCount1X = 0;
                    int WinCountDX = 0;
                    int WinCount_2X_Zu = 0, WinCount_2X_Zu_DZH = 0;
                    int WinCount_5XTX_1 = 0, WinCount_5XTX_2 = 0, WinCount_5XTX_3 = 0;

                    double t_WinMoneyNoWithTax = 0;

                    if (Lotterys[ii].StartsWith("[��ʽ]"))
                    {
                        WinMoney += ComputeWin_D(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney1, WinMoneyNoWithTax1, WinMoney2, WinMoneyNoWithTax2, WinMoney3, WinMoneyNoWithTax3, WinMoney4, WinMoneyNoWithTax4, ref WinCount5X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount1 += WinCount5X;
                        WinCount2 += WinCount3X;
                        WinCount3 += WinCount2X;
                        WinCount4 += WinCount1X;
                    }
                    else if (Lotterys[ii].StartsWith("[��ʽ]"))
                    {
                        WinMoney += ComputeWin_F(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney1, WinMoneyNoWithTax1, WinMoney2, WinMoneyNoWithTax2, WinMoney3, WinMoneyNoWithTax3, WinMoney4, WinMoneyNoWithTax4, ref WinCount5X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount1 += WinCount5X;
                        WinCount2 += WinCount3X;
                        WinCount3 += WinCount2X;
                        WinCount4 += WinCount1X;
                    }
                    else if (Lotterys[ii].StartsWith("[����淨]"))
                    {
                        WinMoney += ComputeWin_ZH(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney1, WinMoneyNoWithTax1, WinMoney2, WinMoneyNoWithTax2, WinMoney3, WinMoneyNoWithTax3, WinMoney4, WinMoneyNoWithTax4, ref WinCount5X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount1 += WinCount5X;
                        WinCount2 += WinCount3X;
                        WinCount3 += WinCount2X;
                        WinCount4 += WinCount1X;
                    }
                    else if (Lotterys[ii].StartsWith("[�´�С]"))
                    {
                        WinMoney += ComputeWin_DX(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney5, WinMoneyNoWithTax5, ref WinCountDX);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount5 += WinCountDX;
                    }
                    else if (Lotterys[ii].StartsWith("[����ͨѡ��ʽ]") || Lotterys[ii].StartsWith("[����ͨѡ��ʽ]"))
                    {
                        WinMoney += ComputeWin_5X_TX(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney8, WinMoneyNoWithTax8, WinMoney9, WinMoneyNoWithTax9, WinMoney10, WinMoneyNoWithTax10, ref WinCount_5XTX_1, ref WinCount_5XTX_2, ref WinCount_5XTX_3);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount8 += WinCount_5XTX_1;
                        WinCount9 += WinCount_5XTX_2;
                        WinCount10 += WinCount_5XTX_3;
                    }
                    else if (Lotterys[ii].StartsWith("[������ѡ��ʽ]") || Lotterys[ii].StartsWith("[������ѡ��ʽ]"))
                    {
                        WinMoney += ComputeWin_2X_ZuD_ZuF(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney6, WinMoneyNoWithTax6, ref WinCount_2X_Zu);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount6 += WinCount_2X_Zu;
                    }
                    else if (Lotterys[ii].StartsWith("[������ѡ��λ]"))
                    {
                        WinMoney += ComputeWin_2X_ZuFW(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney6, WinMoneyNoWithTax6, WinMoney7, WinMoneyNoWithTax7, ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount6 += WinCount_2X_Zu;
                        WinCount7 += WinCount_2X_Zu_DZH;
                    }
                    else if (Lotterys[ii].StartsWith("[������ѡ����]"))
                    {
                        WinMoney += ComputeWin_2X_ZuB(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney6, WinMoneyNoWithTax6, WinMoney7, WinMoneyNoWithTax7, ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount6 += WinCount_2X_Zu;
                        WinCount7 += WinCount_2X_Zu_DZH;
                    }
                    else if (Lotterys[ii].StartsWith("[������ѡ����]"))
                    {
                        WinMoney += ComputeWin_2X_ZuBD(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney6, WinMoneyNoWithTax6, WinMoney7, WinMoneyNoWithTax7, ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount6 += WinCount_2X_Zu;
                        WinCount7 += WinCount_2X_Zu_DZH;
                    }
                    else if (Lotterys[ii].StartsWith("[���ǰ���]"))
                    {
                        WinMoney += ComputeWin_3X_B(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney2, WinMoneyNoWithTax2, ref WinCount_5XTX_2);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount2 += WinCount_5XTX_2;
                    }
                }

                #region �����н�����

                Description = "";

                if (WinCount1 > 0)
                {
                    MergeWinDescription(ref Description, "���ǽ�" + WinCount1.ToString() + "ע");
                }
                if (WinCount2 > 0)
                {
                    MergeWinDescription(ref Description, "���ǽ�" + WinCount2.ToString() + "ע");
                }
                if (WinCount3 > 0)
                {
                    MergeWinDescription(ref Description, "���ǽ�" + WinCount3.ToString() + "ע");
                }
                if (WinCount4 > 0)
                {
                    MergeWinDescription(ref Description, "һ�ǽ�" + WinCount4.ToString() + "ע");
                }
                if (WinCount5 > 0)
                {
                    MergeWinDescription(ref Description, "�´�С��" + WinCount5.ToString() + "ע");
                }
                if (WinCount6 > 0)
                {
                    MergeWinDescription(ref Description, "������ѡ��" + WinCount6.ToString() + "ע");
                }
                if (WinCount7 > 0)
                {
                    MergeWinDescription(ref Description, "������ѡ��(���Ӻ�)" + WinCount7.ToString() + "ע");
                }
                if (WinCount8 > 0)
                {
                    MergeWinDescription(ref Description, "����ͨѡһ�Ƚ�" + WinCount8.ToString() + "ע");
                }
                if (WinCount9 > 0)
                {
                    MergeWinDescription(ref Description, "����ͨѡ���Ƚ�" + WinCount9.ToString() + "ע");
                }
                if (WinCount10 > 0)
                {
                    MergeWinDescription(ref Description, "����ͨѡ���Ƚ�" + WinCount10.ToString() + "ע");
                }

                #endregion

                return WinMoney;
            }
            private double ComputeWin_D(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, ref int WinCount5X, ref int WinCount3X, ref int WinCount2X, ref int WinCount1X)
            {
                WinCount5X = 0;
                WinCount3X = 0;
                WinCount2X = 0;
                WinCount1X = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//3: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_D(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    Regex[] regex5321 = new Regex[4];
                    regex5321[0] = new Regex(@"(\d){5}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[1] = new Regex(@"--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[2] = new Regex(@"---(\d){2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[3] = new Regex(@"----\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (regex5321[0].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i] == WinNumber)
                            {
                                WinCount5X++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;

                                continue;
                            }
                        }

                        if (regex5321[1].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(2, 3) == WinNumber.Substring(2, 3))
                            {
                                WinCount3X++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;

                                continue;
                            }
                        }

                        if (regex5321[2].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(3, 2) == WinNumber.Substring(3, 2))
                            {
                                WinCount2X++;
                                WinMoney += WinMoney3;
                                WinMoneyNoWithTax += WinMoneyNoWithTax3;

                                continue;
                            }
                        }

                        if (regex5321[3].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(4, 1) == WinNumber.Substring(4, 1))
                            {
                                WinCount1X++;
                                WinMoney += WinMoney4;
                                WinMoneyNoWithTax += WinMoneyNoWithTax4;

                                continue;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount5X > 0)
                {
                    MergeWinDescription(ref Description, "���ǽ�" + WinCount5X.ToString() + "ע");
                }

                if (WinCount3X > 0)
                {
                    MergeWinDescription(ref Description, "���ǽ�" + WinCount3X.ToString() + "ע");
                }

                if (WinCount2X > 0)
                {
                    MergeWinDescription(ref Description, "���ǽ�" + WinCount2X.ToString() + "ע");
                }

                if (WinCount1X > 0)
                {
                    MergeWinDescription(ref Description, "һ�ǽ�" + WinCount1X.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_F(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, ref int WinCount5X, ref int WinCount3X, ref int WinCount2X, ref int WinCount1X)
            {
                WinCount5X = 0;
                WinCount3X = 0;
                WinCount2X = 0;
                WinCount1X = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//3: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_F(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    Regex[] regex5321 = new Regex[4];
                    regex5321[0] = new Regex(@"(\d){5}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[1] = new Regex(@"--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[2] = new Regex(@"---(\d){2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[3] = new Regex(@"----\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (regex5321[0].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i] == WinNumber)
                            {
                                WinCount5X++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;

                                continue;
                            }
                        }

                        if (regex5321[1].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(2, 3) == WinNumber.Substring(2, 3))
                            {
                                WinCount3X++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;

                                continue;
                            }
                        }

                        if (regex5321[2].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(3, 2) == WinNumber.Substring(3, 2))
                            {
                                WinCount2X++;
                                WinMoney += WinMoney3;
                                WinMoneyNoWithTax += WinMoneyNoWithTax3;

                                continue;
                            }
                        }

                        if (regex5321[3].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(4, 1) == WinNumber.Substring(4, 1))
                            {
                                WinCount1X++;
                                WinMoney += WinMoney4;
                                WinMoneyNoWithTax += WinMoneyNoWithTax4;

                                continue;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount5X > 0)
                {
                    MergeWinDescription(ref Description, "���ǽ�" + WinCount5X.ToString() + "ע");
                }

                if (WinCount3X > 0)
                {
                    MergeWinDescription(ref Description, "���ǽ�" + WinCount3X.ToString() + "ע");
                }

                if (WinCount2X > 0)
                {
                    MergeWinDescription(ref Description, "���ǽ�" + WinCount2X.ToString() + "ע");
                }

                if (WinCount1X > 0)
                {
                    MergeWinDescription(ref Description, "һ�ǽ�" + WinCount1X.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_ZH(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, ref int WinCount5X, ref int WinCount3X, ref int WinCount2X, ref int WinCount1X)
            {
                WinCount5X = 0;
                WinCount3X = 0;
                WinCount2X = 0;
                WinCount1X = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//3: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                string[] Locate = new string[5];
                bool[] IsRight = new bool[5];
                int[] Num = new int[5];

                Regex regex = new Regex(@"(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    Match m = regex.Match(Lotterys[ii]);

                    for (int j = 0; j < 5; j++)
                    {
                        IsRight[j] = false;
                        Num[j] = 0;

                        Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                        if (Locate[j] == "")
                        {
                            continue;
                        }

                        if (Locate[j].Length > 1)
                        {
                            Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = FilterRepeated(Locate[j]);
                            }

                            if (Locate[j] == "")
                            {
                                continue;
                            }
                        }

                        Num[j] = Locate[j].Length;

                        IsRight[j] = Locate[j].IndexOf(WinNumber.Substring(j, 1)) >= 0;
                    }

                    if (IsRight[4] && IsRight[3] && IsRight[2] && IsRight[1] && IsRight[0])
                    {
                        WinCount5X++;
                        WinMoney += WinMoney1;
                        WinMoneyNoWithTax += WinMoneyNoWithTax1;
                    }

                    if (Locate[1] != "-")
                    {
                        continue;
                    }

                    if (IsRight[4] && IsRight[3] && IsRight[2])
                    {
                        WinCount3X++;
                        WinMoney += WinMoney2;
                        WinMoneyNoWithTax += WinMoneyNoWithTax2;
                    }

                    if (Locate[2] != "-")
                    {
                        continue;
                    }

                    if (IsRight[4] && IsRight[3])
                    {
                        WinCount2X++;
                        WinMoney += WinMoney3;
                        WinMoneyNoWithTax += WinMoneyNoWithTax3;
                    }

                    if (Locate[3] != "-")
                    {
                        continue;
                    }

                    if (IsRight[4])
                    {
                        WinCount1X++;
                        WinMoney += WinMoney4;
                        WinMoneyNoWithTax += WinMoneyNoWithTax4;
                    }
                }

                Description = "";

                if (WinCount5X > 0)
                {
                    MergeWinDescription(ref Description, "���ǽ�" + WinCount5X.ToString() + "ע");
                }

                if (WinCount3X > 0)
                {
                    MergeWinDescription(ref Description, "���ǽ�" + WinCount3X.ToString() + "ע");
                }

                if (WinCount2X > 0)
                {
                    MergeWinDescription(ref Description, "���ǽ�" + WinCount2X.ToString() + "ע");
                }

                if (WinCount1X > 0)
                {
                    MergeWinDescription(ref Description, "һ�ǽ�" + WinCount1X.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_DX(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCountDX)
            {
                WinCountDX = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//12345
                    return -1;

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                string WinNumber_1 = "", WinNumber_2 = "";
                int Num = Shove._Convert.StrToInt(WinNumber.Substring(3, 1), 0);
                WinNumber_1 += (Num <= 4) ? "С" : "��";
                WinNumber_1 += ((Num % 2) == 0) ? "˫" : "��";
                Num = Shove._Convert.StrToInt(WinNumber.Substring(4, 1), 0);
                WinNumber_2 += (Num <= 4) ? "С" : "��";
                WinNumber_2 += ((Num % 2) == 0) ? "˫" : "��";

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_DX(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 2)
                            continue;

                        if ((WinNumber_1.IndexOf(Lottery[i][0]) >= 0) && (WinNumber_2.IndexOf(Lottery[i][1]) >= 0))
                        {
                            WinCountDX++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                Description = "";

                if (WinCountDX > 0)
                {
                    MergeWinDescription(ref Description, "�´�С��" + WinCountDX.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_5X_TX(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, ref int WinCount_5XTX_1, ref int WinCount_5XTX_2, ref int WinCount_5XTX_3)
            {
                WinCount_5XTX_1 = 0;
                WinCount_5XTX_2 = 0;
                WinCount_5XTX_3 = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//3: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                string[] Locate = new string[5];
                bool[] IsRight = new bool[5];
                int[] Num = new int[5];

                Regex regex = new Regex(@"(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    Match m = regex.Match(Lotterys[ii]);

                    for (int j = 0; j < 5; j++)
                    {
                        IsRight[j] = false;
                        Num[j] = 0;

                        Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                        if (Locate[j] == "")
                        {
                            continue;
                        }

                        if (Locate[j].Length > 1)
                        {
                            Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = FilterRepeated(Locate[j]);
                            }

                            if (Locate[j] == "")
                            {
                                continue;
                            }
                        }

                        Num[j] = Locate[j].Length;

                        IsRight[j] = Locate[j].IndexOf(WinNumber.Substring(j, 1)) >= 0;
                    }

                    if (IsRight[4] && IsRight[3] && IsRight[2] && IsRight[1] && IsRight[0])
                    {
                        WinCount_5XTX_1++;
                        WinMoney += WinMoney1;
                        WinMoneyNoWithTax += WinMoneyNoWithTax1;
                    }

                    if (IsRight[4] && IsRight[3] && IsRight[2])
                    {
                        int Count = Num[0] * Num[1];

                        WinCount_5XTX_2 = WinCount_5XTX_2 + Count;
                        WinMoney += WinMoney2 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax2 * Count;
                    }

                    if (IsRight[0] && IsRight[1] && IsRight[2])
                    {
                        int Count = Num[4] * Num[3];

                        if (IsRight[4] && IsRight[3])
                        {
                            Count = Count - 1;
                        }

                        WinCount_5XTX_2 = WinCount_5XTX_2 + Count;
                        WinMoney += WinMoney2 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax2 * Count;
                    }

                    if (IsRight[4] && IsRight[3])
                    {
                        int Count = Num[0] * Num[1] * Num[2];

                        WinCount_5XTX_3 = WinCount_5XTX_3 + Count;
                        WinMoney += WinMoney3 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax3 * Count;
                    }

                    if (IsRight[0] && IsRight[1])
                    {
                        int Count = Num[2] * Num[3] * Num[4];

                        if (IsRight[4] && IsRight[3])
                        {
                            Count = Count - 1;
                        }

                        WinCount_5XTX_3 = WinCount_5XTX_3 + Count;
                        WinMoney += WinMoney3 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax3 * Count;
                    }
                }

                Description = "";

                if (WinCount_5XTX_1 > 0)
                {
                    MergeWinDescription(ref Description, "����ͨѡһ�Ƚ�" + WinCount_5XTX_1.ToString() + "ע");
                }

                if (WinCount_5XTX_2 > 0)
                {
                    MergeWinDescription(ref Description, "����ͨѡ���Ƚ�" + WinCount_5XTX_2.ToString() + "ע");
                }

                if (WinCount_5XTX_3 > 0)
                {
                    MergeWinDescription(ref Description, "����ͨѡ���Ƚ�" + WinCount_5XTX_3.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_2X_ZuD_ZuF(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_2X_Zu)
            {
                WinCount_2X_Zu = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//3: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinNumber = WinNumber.Substring(3, 2);  // ֻ��Ҫ����2λ

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_2X_ZuD_ZuF(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 2)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            WinCount_2X_Zu++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                Description = "";

                if (WinCount_2X_Zu > 0)
                {
                    MergeWinDescription(ref Description, "������ѡ��" + WinCount_2X_Zu.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_2X_ZuFW(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, ref int WinCount_2X_Zu, ref int WinCount_2X_Zu_DZH)
            {
                WinCount_2X_Zu = 0;
                WinCount_2X_Zu_DZH = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//3: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinNumber = WinNumber.Substring(3, 2);  // ֻ��Ҫ����2λ
                bool isDZH = (WinNumber[0] == WinNumber[1]); // �Ƿ���Ӻ�

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_2X_ZuFW(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 2)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            if (!isDZH)
                            {
                                WinCount_2X_Zu++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else
                            {
                                WinCount_2X_Zu_DZH++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount_2X_Zu > 0)
                {
                    MergeWinDescription(ref Description, "������ѡ��" + WinCount_2X_Zu.ToString() + "ע");
                }

                if (WinCount_2X_Zu_DZH > 0)
                {
                    MergeWinDescription(ref Description, "������ѡ��(���Ӻ�)" + WinCount_2X_Zu_DZH.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_2X_ZuB(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, ref int WinCount_2X_Zu, ref int WinCount_2X_Zu_DZH)
            {
                WinCount_2X_Zu = 0;
                WinCount_2X_Zu_DZH = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//5: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinNumber = WinNumber.Substring(3, 2);  // ֻ��Ҫ����2λ
                bool isDZH = (WinNumber[0] == WinNumber[1]); // �Ƿ���Ӻ�

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_2X_ZuB(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 2)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            if (!isDZH)
                            {
                                WinCount_2X_Zu++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else
                            {
                                WinCount_2X_Zu_DZH++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount_2X_Zu > 0)
                {
                    MergeWinDescription(ref Description, "������ѡ��" + WinCount_2X_Zu.ToString() + "ע");
                }

                if (WinCount_2X_Zu_DZH > 0)
                {
                    MergeWinDescription(ref Description, "������ѡ��(���Ӻ�)" + WinCount_2X_Zu_DZH.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_2X_ZuBD(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, ref int WinCount_2X_Zu, ref int WinCount_2X_Zu_DZH)
            {
                WinCount_2X_Zu = 0;
                WinCount_2X_Zu_DZH = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//5: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinNumber = WinNumber.Substring(3, 2);  // ֻ��Ҫ����2λ
                bool isDZH = (WinNumber[0] == WinNumber[1]); // �Ƿ���Ӻ�

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_2X_ZuBD(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 2)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            if (!isDZH)
                            {
                                WinCount_2X_Zu++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else
                            {
                                WinCount_2X_Zu_DZH++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount_2X_Zu > 0)
                {
                    MergeWinDescription(ref Description, "������ѡ��" + WinCount_2X_Zu.ToString() + "ע");
                }

                if (WinCount_2X_Zu_DZH > 0)
                {
                    MergeWinDescription(ref Description, "������ѡ��(���Ӻ�)" + WinCount_2X_Zu_DZH.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_3X_B(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_5XTX_2)
            {
                WinCount_5XTX_2 = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//5: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                int Sum = 0;
                int SumWinNumber = Convert.ToInt32(WinNumber.Substring(2, 1)) + Convert.ToInt32(WinNumber.Substring(3, 1)) + Convert.ToInt32(WinNumber.Substring(4, 1));

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    try
                    {
                        Sum = Convert.ToInt32(Lotterys[ii]);
                    }
                    catch
                    {
                        continue;
                    }

                    if (Sum == SumWinNumber)
                    {
                        WinCount_5XTX_2++;
                        WinMoney += WinMoney1;
                        WinMoneyNoWithTax += WinMoneyNoWithTax1;
                    }
                }

                Description = "";

                if (WinCount_5XTX_2 > 0)
                {
                    MergeWinDescription(ref Description, "���ǽ�" + WinCount_5XTX_2.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_3X_Zu3D(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_3X_Zu3D)
            {
                WinCount_3X_Zu3D = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//5: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                WinNumber = WinNumber.Substring(2, 3);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_3X_Zu3D(Lotterys[ii].Replace("-",""), ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            WinCount_3X_Zu3D++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                Description = "";

                if (WinCount_3X_Zu3D > 0)
                {
                    MergeWinDescription(ref Description, "������3��" + WinCount_3X_Zu3D.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_3X_Zu3F(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_3X_Zu3F)
            {
                WinCount_3X_Zu3F = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//5: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                WinNumber = WinNumber.Substring(2, 3);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_3X_Zu3F(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            WinCount_3X_Zu3F++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                Description = "";

                if (WinCount_3X_Zu3F > 0)
                {
                    MergeWinDescription(ref Description, "������3��" + WinCount_3X_Zu3F.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_3X_Zu6D(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_3X_Zu6D)
            {
                WinCount_3X_Zu6D = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//5: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                WinNumber = WinNumber.Substring(2, 3);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_3X_Zu6D(Lotterys[ii].Replace("-",""), ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            WinCount_3X_Zu6D++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                Description = "";

                if (WinCount_3X_Zu6D > 0)
                {
                    MergeWinDescription(ref Description, "������6��" + WinCount_3X_Zu6D.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_3X_Zu6F(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_3X_Zu6F)
            {
                WinCount_3X_Zu6F = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//5: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                WinNumber = WinNumber.Substring(2, 3);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_3X_Zu6F(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            WinCount_3X_Zu6F++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                Description = "";

                if (WinCount_3X_Zu6F > 0)
                {
                    MergeWinDescription(ref Description, "������6��" + WinCount_3X_Zu6F.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_3X_ZHFS(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_3X_ZHFS)
            {
                WinCount_3X_ZHFS = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//5: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                WinNumber = WinNumber.Substring(2, 3);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_3X_ZHFS(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Lottery[i] == WinNumber)
                        {
                            WinCount_3X_ZHFS++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                Description = "";

                if (WinCount_3X_ZHFS > 0)
                {
                    MergeWinDescription(ref Description, "����ֱѡ��" + WinCount_3X_ZHFS.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_3X_ZuB(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, ref int WinCount_3X_ZX, ref int WinCount_3X_Zu3D, ref int WinCount_3X_Zu6D)
            {
                WinCount_3X_ZX = 0;
                WinCount_3X_Zu3D = 0;
                WinCount_3X_Zu6D = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//5: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                WinNumber = WinNumber.Substring(2, 3);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_3X_ZuB(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (FilterRepeated(Sort(Lottery[i])).Length == 1)
                        {
                            if (Sort(Lottery[i]) == Sort(WinNumber))
                            {
                                WinCount_3X_ZX++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }

                            continue;
                        }

                        if (FilterRepeated(Sort(Lottery[i])).Length == 2)
                        {
                            if (Sort(Lottery[i]) == Sort(WinNumber))
                            {
                                WinCount_3X_Zu3D++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }

                            continue;
                        }

                        if (Lottery[i].Length < 3)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            WinCount_3X_Zu6D++;
                            WinMoney += WinMoney3;
                            WinMoneyNoWithTax += WinMoneyNoWithTax3;
                        }
                    }
                }

                Description = "";

                if (WinCount_3X_ZX > 0)
                {
                    MergeWinDescription(ref Description, "����ֱѡ��" + WinCount_3X_ZX.ToString() + "ע");
                }

                if (WinCount_3X_Zu3D > 0)
                {
                    MergeWinDescription(ref Description, "������3��" + WinCount_3X_Zu3D.ToString() + "ע");
                }

                if (WinCount_3X_Zu6D > 0)
                {
                    MergeWinDescription(ref Description, "������6��" + WinCount_3X_Zu6D.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }
            private double ComputeWin_3X_ZuBD(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, ref int WinCount_3X_ZX, ref int WinCount_3X_Zu3D, ref int WinCount_3X_Zu6D)
            {
                WinCount_3X_ZX = 0;
                WinCount_3X_Zu3D = 0;
                WinCount_3X_Zu6D = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//5: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                WinNumber = WinNumber.Substring(2, 3);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_3X_ZuBD(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (FilterRepeated(Sort(Lottery[i])).Length == 1)
                        {
                            if (Sort(Lottery[i]) == Sort(WinNumber))
                            {
                                WinCount_3X_ZX++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }

                            continue;
                        }

                        if (FilterRepeated(Sort(Lottery[i])).Length == 2)
                        {
                            if (Sort(Lottery[i]) == Sort(WinNumber))
                            {
                                WinCount_3X_Zu3D++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }

                            continue;
                        }

                        if (Lottery[i].Length < 3)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            WinCount_3X_Zu6D++;
                            WinMoney += WinMoney3;
                            WinMoneyNoWithTax += WinMoneyNoWithTax3;
                        }
                    }
                }

                Description = "";

                if (WinCount_3X_ZX > 0)
                {
                    MergeWinDescription(ref Description, "����ֱѡ��" + WinCount_3X_ZX.ToString() + "ע");
                }

                if (WinCount_3X_Zu3D > 0)
                {
                    MergeWinDescription(ref Description, "������3��" + WinCount_3X_Zu3D.ToString() + "ע");
                }

                if (WinCount_3X_Zu6D > 0)
                {
                    MergeWinDescription(ref Description, "������6��" + WinCount_3X_Zu6D.ToString() + "ע");
                }

                if (Description != "")
                    Description += "��";

                return WinMoney;
            }

            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if (PlayType == PlayType_Mixed)
                    return AnalyseScheme_Mixed(Content, PlayType);

                if (PlayType == PlayType_D)
                    return AnalyseScheme_D(Content, PlayType);

                if (PlayType == PlayType_F)
                    return AnalyseScheme_F(Content, PlayType);

                if (PlayType == PlayType_ZH)
                    return AnalyseScheme_ZH(Content, PlayType);

                if (PlayType == PlayType_DX)
                    return AnalyseScheme_DX(Content, PlayType);

                if ((PlayType == PlayType_5X_TXD) || (PlayType == PlayType_5X_TXF))
                    return AnalyseScheme_5X_TX(Content, PlayType);

                if ((PlayType == PlayType_2X_ZuD) || (PlayType == PlayType_2X_ZuF))
                    return AnalyseScheme_2X_ZuD_ZuF(Content, PlayType);

                if (PlayType == PlayType_2X_ZuFW)
                    return AnalyseScheme_2X_ZuFW(Content, PlayType);

                if (PlayType == PlayType_2X_ZuB)
                    return AnalyseScheme_2X_ZuB(Content, PlayType);

                if (PlayType == PlayType_2X_ZuBD)
                    return AnalyseScheme_2X_ZuBD(Content, PlayType);

                if (PlayType == PlayType_3X_B)
                    return AnalyseScheme_3X_B(Content, PlayType);

                //stone 2009-06-30
                if (PlayType == PlayType_3X_Zu3D)
                    return AnalyseScheme_3X_Zu3D(Content, PlayType);

                if (PlayType == PlayType_3X_Zu3F)
                    return AnalyseScheme_3X_Zu3F(Content, PlayType);

                if (PlayType == PlayType_3X_Zu6D)
                    return AnalyseScheme_3X_Zu6D(Content, PlayType);

                if (PlayType == PlayType_3X_Zu6F)
                    return AnalyseScheme_3X_Zu6F(Content, PlayType);

                if (PlayType == PlayType_3X_ZHFS)
                    return AnalyseScheme_3X_ZHFS(Content, PlayType);

                if (PlayType == PlayType_3X_ZuB)
                    return AnalyseScheme_3X_ZuB(Content, PlayType);

                if (PlayType == PlayType_3X_ZuBD)
                    return AnalyseScheme_3X_ZuBD(Content, PlayType);


                return "";
            }

            #region AnalyseScheme �ľ��巽��

            private string AnalyseScheme_Mixed(string Content, int PlayType)
            {
                string[] Lotterys = SplitLotteryNumber(Content);

                if (Lotterys == null)
                {
                    return "";
                }

                if (Lotterys.Length < 1)
                {
                    return "";
                }

                string Result = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string PreFix = GetLotteryNumberPreFix(Lotterys[ii]);
                    string t_Result = "";

                    if (Lotterys[ii].StartsWith("[��ʽ]"))
                    {
                        t_Result += AnalyseScheme_D(Lotterys[ii], PlayType_D);
                    }

                    if (Lotterys[ii].StartsWith("[��ʽ]"))
                    {
                        t_Result += AnalyseScheme_F(Lotterys[ii], PlayType_F);
                    }

                    if (Lotterys[ii].StartsWith("[����淨]"))
                    {
                        t_Result += AnalyseScheme_ZH(Lotterys[ii], PlayType_ZH);
                    }

                    if (Lotterys[ii].StartsWith("[�´�С]"))
                    {
                        t_Result += AnalyseScheme_DX(Lotterys[ii], PlayType_DX);
                    }

                    if (Lotterys[ii].StartsWith("[����ͨѡ��ʽ]"))
                    {
                        t_Result += AnalyseScheme_5X_TX(Lotterys[ii], PlayType_5X_TXD);
                    }

                    if (Lotterys[ii].StartsWith("[����ͨѡ��ʽ]"))
                    {
                        t_Result += AnalyseScheme_5X_TX(Lotterys[ii], PlayType_5X_TXF);
                    }

                    if (Lotterys[ii].StartsWith("[������ѡ��ʽ]"))
                    {
                        t_Result += AnalyseScheme_2X_ZuD_ZuF(Lotterys[ii], PlayType_2X_ZuD);
                    }

                    if (Lotterys[ii].StartsWith("[������ѡ��ʽ]"))
                    {
                        t_Result += AnalyseScheme_2X_ZuD_ZuF(Lotterys[ii], PlayType_2X_ZuF);
                    }

                    if (Lotterys[ii].StartsWith("[������ѡ��λ]"))
                    {
                        t_Result += AnalyseScheme_2X_ZuFW(Lotterys[ii], PlayType_2X_ZuFW);
                    }

                    if (Lotterys[ii].StartsWith("[������ѡ����]"))
                    {
                        t_Result += AnalyseScheme_2X_ZuB(Lotterys[ii], PlayType_2X_ZuB);
                    }

                    if (Lotterys[ii].StartsWith("[������ѡ����]"))
                    {
                        t_Result += AnalyseScheme_2X_ZuBD(Lotterys[ii], PlayType_2X_ZuBD);
                    }

                    if (Lotterys[ii].StartsWith("[���ǰ���]"))
                    {
                        t_Result += AnalyseScheme_3X_B(Lotterys[ii], PlayType_3X_B);
                    }

                    if (t_Result != "")
                    {
                        Result += PreFix + t_Result + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                {
                    Result = Result.Substring(0, Result.Length - 1);
                }

                return Result;
            }
            private string AnalyseScheme_D(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"(([\d])|([-])){4}[\d]";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_D(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_F(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"(([\d])|([-])){4}[\d]";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_F(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZH(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                int Num = 1;
                string CanonicalNumber = "";

                string[] Locate = new string[5];

                Regex regex = new Regex(@"(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);

                    CanonicalNumber = "";

                    for (int j = 0; j < 5; j++)
                    {
                        Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                        if (Locate[j] == "")
                        {
                            CanonicalNumber = "";
                            continue;
                        }

                        if (Locate[j].Length > 1)
                        {
                            Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = FilterRepeated(Locate[j]);
                            }

                            if (Locate[j] == "")
                            {
                                CanonicalNumber = "";
                                continue;
                            }
                        }

                        if (Locate[j].Length > 1)
                        {
                            CanonicalNumber += "(" + Locate[j] + ")";

                            Num = Num * Locate[j].Length;
                        }
                        else
                        {
                            CanonicalNumber += Locate[j];
                        }
                    }

                    Result += CanonicalNumber + "|" + Num.ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_DX(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"([[]�´�С[]])*?([��С��˫]){2}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_DX(m.Value.Replace("[�´�С]", ""), ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_5X_TX(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                int Num = 1;
                string CanonicalNumber = "";

                string[] Locate = new string[5];

                Regex regex = new Regex(@"(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);

                    CanonicalNumber = "";

                    for (int j = 0; j < 5; j++)
                    {
                        Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                        if (Locate[j] == "")
                        {
                            CanonicalNumber = "";
                            continue;
                        }

                        if (Locate[j].Length > 1)
                        {
                            Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);

                            if (Locate[j].Length > 1)
                            {
                                Locate[i] = FilterRepeated(Locate[j]);
                            }

                            if (Locate[j] == "")
                            {
                                CanonicalNumber = "";
                                continue;
                            }
                        }

                        if (Locate[j].Length > 1)
                        {
                            CanonicalNumber += "(" + Locate[j] + ")";

                            Num = Num * Locate[j].Length;
                        }
                        else
                        {
                            CanonicalNumber += Locate[j];
                        }
                    }

                    Result += CanonicalNumber + "|" + Num.ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_2X_ZuD_ZuF(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');

                if (strs == null)
                {
                    return "";
                }

                if (strs.Length == 0)
                {
                    return "";
                }

                string Result = "";

                string RegexString;

                if (PlayType == PlayType_2X_ZuD)
                {
                    RegexString = @"(\d){2}";
                }
                else
                {
                    RegexString = @"(\d){3,10}";
                }

                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_2X_ZuD_ZuF(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= ((PlayType == PlayType_2X_ZuD) ? 1 : 2))
                        {
                            if (FilterRepeated(Sort(m.Value)).Length == 2)
                            {
                                if (PlayType != PlayType_2X_ZuF)
                                {
                                    Result += m.Value + "|1\n";
                                }
                            }
                            else
                            {
                                Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                            }
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_2X_ZuFW(string Content, int PlayType)
            {
                string[] strs = Content.Trim().Split('\n');
                if (strs == null)
                {
                    return "";
                }

                if (strs.Length == 0)
                {
                    return "";
                }

                string Result = "";

                string RegexString = @"([(](\d){2,10}[)][(](\d){2,10}[)])|([\d][(](\d){2,10}[)])|([(](\d){2,10}[)][\d])";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    strs[i] = FilterPreFix(strs[i]);

                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_2X_ZuFW(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_2X_ZuB(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_2X_ZuB(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_2X_ZuBD(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_2X_ZuBD(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_3X_B(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_3X_B(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_3X_Zu3D(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"[\d]{3}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_3X_Zu3D(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_3X_Zu3F(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"([\d]){2,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_3X_Zu3F(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 2)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_3X_Zu6D(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"[\d]{3}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_3X_Zu6D(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_3X_Zu6F(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"([\d]){3,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_3X_Zu6F(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 2)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_3X_ZHFS(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"([\d]){3,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_3X_ZHFS(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 2)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_3X_ZuB(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_3X_ZuB(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 2)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_3X_ZuBD(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_3X_ZuBD(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
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
                Regex regex = new Regex(@"([\d]){5}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                return regex.IsMatch(Number);
            }

            public override int GetNum(int Number1, int Number2)
            {
                if (Number2 < Number1)
                {
                    return -1;
                }

                if (Number2 == Number1)
                {
                    return 1;
                }

                int i = 1;
                int j = 1;

                while (Number1 > 0)
                {
                    i = i * (Number2 + 1 - Number1);
                    j = j * (Number1);

                    Number1--;
                }

                return i / j;
            }

            private string FilterRepeated(string NumberPart)
            {
                string Result = "";
                for (int i = 0; i < NumberPart.Length; i++)
                {
                    if ((Result.IndexOf(NumberPart.Substring(i, 1)) == -1) && ("0123456789-".IndexOf(NumberPart.Substring(i, 1)) >= 0))
                        Result += NumberPart.Substring(i, 1);
                }
                return Sort(Result);
            }

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, " ");
            }

            //stone 2009-06-30 �ӵ��淨��Ʊ��ص�δ����
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
                    case "LT-E":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_LT_E_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_LT_E_F(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZH)
                        {
                            return GetPrintKeyList_LT_E_Zu(Numbers);
                        }
                        if (PlayTypeID == PlayType_DX)
                        {
                            return GetPrintKeyList_LT_E_DX(Numbers);
                        }
                        break;
                }

                return "";
            }
            #region GetPrintKeyList �ľ��巽��
            private string GetPrintKeyList_LT_E_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {

                    string PrintNumber = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("-", "");

                    PrintNumber = PrintNumber.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7");

                    if (PrintNumber.Length == 1)
                    {
                        KeyList += "[Q]";
                    }
                    if (PrintNumber.Length == 2)
                    {
                        KeyList += "[R]";
                    }
                    if (PrintNumber.Length == 3)
                    {
                        KeyList += "[3]";
                    }
                    if (PrintNumber.Length == 5)
                    {
                        KeyList += "[1]";
                    }

                    foreach (char ch in PrintNumber)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_LT_E_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {

                    string PrintNumber = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("-", "");

                    PrintNumber = PrintNumber.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7");

                    if (PrintNumber.Length == 2)
                    {
                        KeyList += "[Q]";
                    }
                    if (PrintNumber.Length == 3)
                    {
                        KeyList += "[R]";
                    }
                    if (PrintNumber.Length == 5)
                    {
                        KeyList += "[1]";
                    }

                    foreach (char ch in PrintNumber)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_LT_E_Zu(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    int PrintNumber = 0;


                    for (int i = 0; i < Number.Length; i++)
                    {
                        if (Number.Substring(i, 1) == "-")
                        {
                            PrintNumber++;
                        }
                    }

                    if (PrintNumber == 0)
                    {
                        KeyList += "[X]";
                    }
                    if (PrintNumber == 2)
                    {
                        KeyList += "[T]";
                    }
                    if (PrintNumber == 3)
                    {
                        KeyList += "[S]";
                    }

                    KeyList += PrintNumber.ToString();

                    foreach (char ch in Number)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7").Replace("X", "4");


                return KeyList;
            }
            private string GetPrintKeyList_LT_E_DX(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {

                    foreach (char ch in Number)
                    {
                        if (ch.ToString() == "С")
                        {
                            KeyList += "[Q]";
                        }
                        if (ch.ToString() == "��")
                        {
                            KeyList += "[R]";
                        }
                        if (ch.ToString() == "��")
                        {
                            KeyList += "[S]";
                        }
                        if (ch.ToString() == "˫")
                        {
                            KeyList += "[˫]";           //���ʻ�������3��Ӧ�ڵ��Լ�����1    
                        }

                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7").Replace("˫", "1");

                return KeyList;
            }
            #endregion

            //����˵�� PlayTypeID:�淨���; Number:Ͷע����; Multiple: ����; MaxMultiple:�����;
            public override Ticket[] ToElectronicTicket_HPCQ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_Mixed)
                {
                    return ToElectronicTicket_HPCQ_Mixed(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }

                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_HPCQ_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_HPCQ_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZH)
                {
                    return ToElectronicTicket_HPCQ_ZH(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_DX)
                {
                    return ToElectronicTicket_HPCQ_DX(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_5X_TXD)
                {
                    return ToElectronicTicket_HPCQ_5X_TXD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_5X_TXF)
                {
                    return ToElectronicTicket_HPCQ_5X_TXF(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2X_ZuD)
                {
                    return ToElectronicTicket_HPCQ_2X_ZuD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2X_ZuF)
                {
                    return ToElectronicTicket_HPCQ_2X_ZuF(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2X_ZuFW)
                {
                    return ToElectronicTicket_HPCQ_2X_ZuFW(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2X_ZuB)
                {
                    return ToElectronicTicket_HPCQ_2X_ZuB(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2X_ZuBD)
                {
                    return ToElectronicTicket_HPCQ_2X_ZuBD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3X_B)
                {
                    return ToElectronicTicket_HPCQ_3X_B(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }

                //stone 2009-06-30
                if (PlayTypeID == PlayType_3X_Zu3D)
                    return ToElectronicTicket_HPCQ_3X_Zu3D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);

                if (PlayTypeID == PlayType_3X_Zu3F)
                    return ToElectronicTicket_HPCQ_3X_Zu3F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);

                if (PlayTypeID == PlayType_3X_Zu6D)
                    return ToElectronicTicket_HPCQ_3X_Zu6D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);

                if (PlayTypeID == PlayType_3X_Zu6F)
                    return ToElectronicTicket_HPCQ_3X_Zu6F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);

                if (PlayTypeID == PlayType_3X_ZHFS)
                    return ToElectronicTicket_HPCQ_3X_ZHFS(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);

                if (PlayTypeID == PlayType_3X_ZuB)
                    return ToElectronicTicket_HPCQ_3X_ZuB(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);

                if (PlayTypeID == PlayType_3X_ZuBD)
                    return ToElectronicTicket_HPCQ_3X_ZuBD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);

                return null;
            }

            #region ToElectronicTicket_HPCQ �ľ��巽��

            private Ticket[] ToElectronicTicket_HPCQ_Mixed(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                {
                    return null;
                }

                if (Lotterys.Length < 1)
                {
                    return null;
                }

                Money = 0;

                string[] t_Result = new string[12] { "", "", "", "", "", "", "", "", "", "", "", "" };

                foreach (string str in Lotterys)
                {
                    if (str.StartsWith("[��ʽ]"))
                    {
                        t_Result[0] += str + "\n";
                    }
                    else if (str.StartsWith("[��ʽ]"))
                    {
                        t_Result[1] += str + "\n";
                    }
                    else if (str.StartsWith("[����淨]"))
                    {
                        t_Result[2] += str + "\n";
                    }
                    else if (str.StartsWith("[�´�С]"))
                    {
                        t_Result[3] += str + "\n";
                    }
                    else if (str.StartsWith("[����ͨѡ��ʽ]"))
                    {
                        t_Result[4] += str + "\n";
                    }
                    else if (str.StartsWith("[����ͨѡ��ʽ]"))
                    {
                        t_Result[5] += str + "\n";
                    }
                    else if (str.StartsWith("[������ѡ��ʽ]"))
                    {
                        t_Result[6] += str + "\n";
                    }
                    else if (str.StartsWith("[������ѡ��ʽ]"))
                    {
                        t_Result[7] += str + "\n";
                    }
                    else if (str.StartsWith("[������ѡ��λ]"))
                    {
                        t_Result[8] += str + "\n";
                    }
                    else if (str.StartsWith("[������ѡ����]"))
                    {
                        t_Result[9] += str + "\n";
                    }
                    else if (str.StartsWith("[������ѡ����]"))
                    {
                        t_Result[10] += str + "\n";
                    }
                    else if (str.StartsWith("[���ǰ���]"))
                    {
                        t_Result[11] += str + "\n";
                    }
                }

                ArrayList al = new ArrayList();
                Ticket[] t_ticket;

                if (t_Result[0].Length > 0)
                {
                    t_ticket = ToElectronicTicket_HPCQ_D(PlayType_D, t_Result[0].ToString(), Multiple, MaxMultiple, ref Money);

                    if (t_ticket != null)
                    {
                        foreach (Ticket t in t_ticket)
                        {
                            al.Add(t);
                        }
                    }
                }

                if (t_Result[1].Length > 0)
                {
                    t_ticket = ToElectronicTicket_HPCQ_F(PlayType_F, t_Result[1].ToString(), Multiple, MaxMultiple, ref Money);

                    if (t_ticket != null)
                    {
                        foreach (Ticket t in t_ticket)
                        {
                            al.Add(t);
                        }
                    }
                }

                if (t_Result[2].Length > 0)
                {
                    t_ticket = ToElectronicTicket_HPCQ_ZH(PlayType_ZH, t_Result[2].ToString(), Multiple, MaxMultiple, ref Money);

                    if (t_ticket != null)
                    {
                        foreach (Ticket t in t_ticket)
                        {
                            al.Add(t);
                        }
                    }
                }

                if (t_Result[3].Length > 0)
                {
                    t_ticket = ToElectronicTicket_HPCQ_DX(PlayType_DX, t_Result[3].ToString(), Multiple, MaxMultiple, ref Money);

                    if (t_ticket != null)
                    {
                        foreach (Ticket t in t_ticket)
                        {
                            al.Add(t);
                        }
                    }
                }

                if (t_Result[4].Length > 0)
                {
                    t_ticket = ToElectronicTicket_HPCQ_5X_TXD(PlayType_5X_TXD, t_Result[4].ToString(), Multiple, MaxMultiple, ref Money);

                    if (t_ticket != null)
                    {
                        foreach (Ticket t in t_ticket)
                        {
                            al.Add(t);
                        }
                    }
                }

                if (t_Result[5].Length > 0)
                {
                    t_ticket = ToElectronicTicket_HPCQ_5X_TXF(PlayType_5X_TXF, t_Result[5].ToString(), Multiple, MaxMultiple, ref Money);

                    if (t_ticket != null)
                    {
                        foreach (Ticket t in t_ticket)
                        {
                            al.Add(t);
                        }
                    }
                }

                if (t_Result[6].Length > 0)
                {
                    t_ticket = ToElectronicTicket_HPCQ_2X_ZuD(PlayType_2X_ZuD, t_Result[6].ToString(), Multiple, MaxMultiple, ref Money);

                    if (t_ticket != null)
                    {
                        foreach (Ticket t in t_ticket)
                        {
                            al.Add(t);
                        }
                    }
                }

                if (t_Result[7].Length > 0)
                {
                    t_ticket = ToElectronicTicket_HPCQ_2X_ZuF(PlayType_2X_ZuF, t_Result[7].ToString(), Multiple, MaxMultiple, ref Money);

                    if (t_ticket != null)
                    {
                        foreach (Ticket t in t_ticket)
                        {
                            al.Add(t);
                        }
                    }
                }

                if (t_Result[8].Length > 0)
                {
                    t_ticket = ToElectronicTicket_HPCQ_2X_ZuFW(PlayType_2X_ZuFW, t_Result[8].ToString(), Multiple, MaxMultiple, ref Money);

                    if (t_ticket != null)
                    {
                        foreach (Ticket t in t_ticket)
                        {
                            al.Add(t);
                        }
                    }
                }

                if (t_Result[9].Length > 0)
                {
                    t_ticket = ToElectronicTicket_HPCQ_2X_ZuB(PlayType_2X_ZuB, t_Result[9].ToString(), Multiple, MaxMultiple, ref Money);

                    if (t_ticket != null)
                    {
                        foreach (Ticket t in t_ticket)
                        {
                            al.Add(t);
                        }
                    }
                }

                if (t_Result[10].Length > 0)
                {
                    t_ticket = ToElectronicTicket_HPCQ_2X_ZuBD(PlayType_2X_ZuBD, t_Result[10].ToString(), Multiple, MaxMultiple, ref Money);

                    if (t_ticket != null)
                    {
                        foreach (Ticket t in t_ticket)
                        {
                            al.Add(t);
                        }
                    }
                }

                if (t_Result[11].Length > 0)
                {
                    t_ticket = ToElectronicTicket_HPCQ_3X_B(PlayType_3X_B, t_Result[11].ToString(), Multiple, MaxMultiple, ref Money);

                    if (t_ticket != null)
                    {
                        foreach (Ticket t in t_ticket)
                        {
                            al.Add(t);
                        }
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPCQ_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_D(Number, PlayTypeID).Split('\n');

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
                                Numbers += strs[i + M].ToString().Split('|')[0];
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(301, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPCQ_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_F(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(302, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPCQ_ZH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_ZH(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(303, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPCQ_DX(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_DX(Number, PlayTypeID).Split('\n');

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
                                Numbers += strs[i + M].ToString().Split('|')[0];
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(306, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPCQ_5X_TXD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_5X_TX(Number, PlayTypeID).Split('\n');

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
                                Numbers += strs[i + M].ToString().Split('|')[0];
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(312, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPCQ_5X_TXF(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                ArrayList al = new ArrayList();

                string strsNumbers = "";
                string CanonicalNumber = "";
                string[] strsNumber = ToSingle_5X_TX(Number, ref CanonicalNumber);

                for (int j = 0; j < strsNumber.Length; j++)
                {
                    strsNumbers += strsNumber[j] + "\n";
                }

                string[] strs = AnalyseScheme_5X_TX(strsNumbers, PlayType_5X_TXD).Split('\n');

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
                                Numbers += strs[i + M].ToString().Split('|')[0];
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(312, ConvertFormatToElectronTicket_HPCQ(PlayType_5X_TXD, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPCQ_2X_ZuD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_2X_ZuD_ZuF(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(307, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPCQ_2X_ZuF(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_2X_ZuD_ZuF(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(308, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPCQ_2X_ZuFW(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_2X_ZuFW(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(309, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPCQ_2X_ZuB(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_2X_ZuB(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(310, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPCQ_2X_ZuBD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_2X_ZuBD(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(311, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPCQ_3X_B(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_3X_B(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(304, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            //stone 2009-06-30
            private Ticket[] ToElectronicTicket_HPCQ_3X_Zu3D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_3X_Zu3D(Number, PlayTypeID).Split('\n');

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
                                Numbers += "--" + strs[i + M].ToString().Split('|')[0];
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(313, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPCQ_3X_Zu3F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_3X_Zu3F(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(315, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPCQ_3X_Zu6D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_3X_Zu6D(Number, PlayTypeID).Split('\n');

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
                                Numbers += "--" + strs[i + M].ToString().Split('|')[0];
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(314, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPCQ_3X_Zu6F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_3X_Zu6F(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(316, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPCQ_3X_ZHFS(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_3X_ZHFS(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(319, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPCQ_3X_ZuB(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_3X_ZuB(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(317, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPCQ_3X_ZuBD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_3X_ZuBD(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(318, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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
            private string ConvertFormatToElectronTicket_HPCQ(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F || PlayTypeID == PlayType_5X_TXD || PlayTypeID == PlayType_5X_TXF || PlayTypeID == PlayType_3X_Zu3D || PlayTypeID == PlayType_3X_Zu6D)
                {
                    for (int j = 0; j < Number.Length; j++)
                    {
                        if (j % 5 == 0 && j > 0)
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                            Ticket += "\n" + Number.Substring(j, 1) + ",";
                        }
                        else
                        {
                            Ticket += Number.Substring(j, 1) + ",";
                        }
                    }
                }

                if (PlayTypeID == PlayType_ZH)
                {
                    string[] Locate = new string[5];

                    Regex regex = new Regex(@"(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
                    {
                        Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate[i].Length > 1)
                        {
                            Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                        }

                        Ticket += Locate[i].ToString() + ",";
                    }
                }

                if (PlayTypeID == PlayType_DX)
                {
                    Number = Number.Replace("��", "2").Replace("С", "1").Replace("��", "5").Replace("˫", "4");

                    for (int j = 0; j < Number.Length; j++)
                    {
                        if (j % 2 == 0 && j > 0)
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                            Ticket += "\n" + Number.Substring(j, 1) + ",";
                        }
                        else
                        {
                            Ticket += Number.Substring(j, 1) + ",";
                        }
                    }
                }

                if (PlayTypeID == PlayType_2X_ZuD)
                {
                    string[] NumberList = Number.Split('\n');
                    for (int i = 0; i < NumberList.Length; i++)   // 12\n34\n13\n36
                    {
                        Ticket += "_,_,_,";
                        for (int j = 0; j < NumberList[i].Length; j++)
                        {
                            Ticket += NumberList[i].Substring(j, 1) + ",";
                        }

                        if (Ticket.EndsWith(","))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket += "\n";
                    }
                }

                if (PlayTypeID == PlayType_2X_ZuF || PlayTypeID == PlayType_3X_Zu3F || PlayTypeID == PlayType_3X_Zu6F || PlayTypeID == PlayType_3X_ZHFS || PlayTypeID == PlayType_3X_ZuB)
                {
                    for (int j = 0; j < Number.Length; j++)
                    {
                        Ticket += Number.Substring(j, 1) + ",";
                    }
                }

                if (PlayTypeID == PlayType_2X_ZuFW)
                {
                    Ticket += "_,_,_,";
                    string[] Locate = new string[2];

                    Regex regex = new Regex(@"(?<L0>([\d])|([(][\d]+?[)]))(?<L1>([\d])|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 2; i++)
                    {
                        Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate[i].Length > 1)
                        {
                            Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                        }

                        Ticket += Locate[i].ToString() + ",";
                    }
                }

                if (PlayTypeID == PlayType_2X_ZuB)
                {
                    Ticket += Number + ",";
                }

                if (PlayTypeID == PlayType_2X_ZuBD || PlayTypeID == PlayType_3X_ZuBD)
                {
                    Ticket += Number + ",";
                }

                if (PlayTypeID == PlayType_3X_B)
                {
                    Ticket += Number + ",";
                }

                Ticket = Ticket.Substring(0, Ticket.Length - 1);
                Ticket = Ticket.Replace("-", "_");
                return Ticket;
            }
            #endregion
        }
    }
}