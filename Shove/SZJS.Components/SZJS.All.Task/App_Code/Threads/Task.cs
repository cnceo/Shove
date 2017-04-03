﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

using Shove.Database;

namespace SZJS.All.Task
{
    public class Task
    {
        private long gCount1 = 0;
        private long gCount2 = 0;
        private long gCount3 = 0;
        private long gCount4 = 0;
        private long gCount5 = 0;
        private long gCount6 = 0;
        private System.Threading.Thread thread;
        private string ConnectionString;

        public string EmailServer_From = "";
        public string EmailServer_EmailServer = "";
        public string EmailServer_User = "";
        public string EmailServer_Password = "";

        private Message msg = new Message("Task");
        private Log log = new Log("Task");

        public int State = 0;   // 0 停止 1 运行中 2 置为停止

        public Task(string connectionstring)
        {
            ConnectionString = connectionstring;
        }

        public void Run()
        {
            // 已经启动
            if (State == 1)
            {
                return;
            }

            lock (this) // 确保临界区被一个 Thread 所占用
            {
                State = 1;

                gCount1 = 0;
                gCount2 = 0;
                gCount3 = 0;
                gCount4 = 0;
                gCount5 = 0;
                gCount6 = 0;

                thread = new System.Threading.Thread(new System.Threading.ThreadStart(Do));
                thread.IsBackground = true;

                thread.Start();

                msg.Send("Task Start.");
                log.Write("Task Start.");
            }
        }

        public void Exit()
        {
            State = 2;
        }

        public void Do()
        {
            while (true)
            {
                if (State == 2)
                {
                    msg.Send("Task Stop.");
                    log.Write("Task Stop.");

                    State = 0;

                    Stop();

                    return;
                }

                System.Threading.Thread.Sleep(1000);   // 1秒为单位

                gCount1++;
                gCount2++;
                gCount3++;
                gCount4++;
                gCount5++;
                gCount6++;

                #region 30 秒, 使用保底使方案满员，未成功方案系统撤单

                if (gCount1 > 30)
                {
                    gCount1 = 0;

                    try
                    {
                        SchemeSystemDeal();

                        msg.Send("SchemeSystemDeal ...... OK.");
                    }
                    catch (Exception e)
                    {
                        msg.Send("SchemeSystemDeal is Fail: " + e.Message);
                        log.Write("SchemeSystemDeal is Fail: " + e.Message);
                    }

                    try
                    {
                        QuashSchemeNoLotteryNumber();

                        msg.Send("QuashSchemeNoLotteryNumber ...... OK.");
                    }
                    catch (Exception e)
                    {
                        msg.Send("QuashSchemeNoLotteryNumber is Fail: " + e.Message);
                        log.Write("QuashSchemeNoLotteryNumber is Fail: " + e.Message);
                    }
                }

                #endregion

                #region 1 分, 执行追号任务 & 临近系统结束时间使用保底使方案满员

                if (gCount2 > 60)
                {
                    gCount2 = 0;

                    try
                    {
                        ExecChaseTasks();

                        msg.Send("ExecChaseTasks ...... OK.");
                    }
                    catch (Exception e)
                    {
                        msg.Send("ExecChaseTasks is Fail: " + e.Message);
                        log.Write("ExecChaseTasks is Fail: " + e.Message);
                    }

                    try
                    {
                        //ExecIsuseEndTime(); //官方时间截止撤单

                        msg.Send("ExecIsuseEndTime ...... OK.");
                    }
                    catch (Exception e)
                    {
                        msg.Send("ExecIsuseEndTime is Fail: " + e.Message);
                        log.Write("ExecIsuseEndTime is Fail: " + e.Message);
                    }

                    try
                    {
                        SchemeAssure(); //时时乐比 SystemEndTime 提前 2 分钟，其他彩种提前 20 分钟

                        msg.Send("SchemeAssure ...... OK.");
                    }
                    catch (Exception e)
                    {
                        msg.Send("SchemeAssure is Fail: " + e.Message);
                        log.Write("SchemeAssure is Fail: " + e.Message);
                    }
                }

                #endregion

                #region 5 分钟, 检测追号过期未执行的任务，以撤消此明细任务。

                if (gCount3 > 60 * 5)
                {
                    gCount3 = 0;

                    try
                    {
                        CheckChaseTasks();

                        msg.Send("CheckChaseTasks ...... OK.");
                    }
                    catch (Exception e)
                    {
                        msg.Send("CheckChaseTasks is Fail: " + e.Message);
                        log.Write("CheckChaseTasks is Fail: " + e.Message);
                    }
                }

                #endregion

                #region 5 分钟, 检测单场未满员的方案，未成功方案系统撤单，足彩单场专用(系统撤单)。

                if (gCount4 > 60 * 5)
                {
                    gCount4 = 0;
                }

                #endregion

                #region 1 小时, 发送沉淀资金邮件

                if (gCount5 > 60 * 60)
                {
                    gCount5 = 0;

                    try
                    {
                        ExecChases();

                        msg.Send("ExecChases ...... OK.");
                    }
                    catch (Exception e)
                    {
                        msg.Send("ExecChases is Fail: " + e.Message);
                        log.Write("ExecChases is Fail: " + e.Message);
                    }
                }

                #endregion

                #region 10 小时, 检查追号套餐的随机选号

                if (gCount6 > 60 * 600)
                {
                    gCount6 = 0;

                    try
                    {
                        CheckLotteryNumberCountForCustomChase();

                        msg.Send("CheckLotteryNumberCountForCustomChase ...... OK.");
                    }
                    catch (Exception e)
                    {
                        msg.Send("CheckLotteryNumberCountForCustomChase is Fail: " + e.Message);
                        log.Write("CheckLotteryNumberCountForCustomChase is Fail: " + e.Message);
                    }
                }

                #endregion
            }
        }

        private void Stop()
        {
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }

        #region 定时器执行的事件

        private void SchemeSystemDeal()	//使用保底使方案满员，未成功方案系统撤单
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            int Return = DAL.Procedures.P_SystemEnd(ConnectionString, ref ReturnValue, ref ReturnDescription);

            if (Return < 0)
            {
                msg.Send("Exec SchemeSystemDeal: Procedure \"P_SystemEnd\" Fail." + Return.ToString());
                log.Write("Exec SchemeSystemDeal: Procedure \"P_SystemEnd\" Fail." + Return.ToString());

                return;
            }

            if (ReturnValue < 0)
            {
                msg.Send("Exec SchemeSystemDeal: Procedure \"P_SystemEnd\" Return: " + ReturnDescription);
                log.Write("Exec SchemeSystemDeal: Procedure \"P_SystemEnd\" Return: " + ReturnDescription);
            }
        }

        private void ExecChaseTasks()	//执行追号任务
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            if (DAL.Procedures.P_ExecChaseTasks(ConnectionString, ref ReturnValue, ref ReturnDescription) < 0)
            {
                msg.Send("第一次执行：Exec ExecChaseTasks: Procedure \"P_ExecChaseTasks\" Fail.");
                log.Write("第一次执行：Exec ExecChaseTasks: Procedure \"P_ExecChaseTasks\" Fail.");

                if (DAL.Procedures.P_ExecChaseTasks(ConnectionString, ref ReturnValue, ref ReturnDescription) < 0)
                {
                    msg.Send("第二次执行：Exec ExecChaseTasks: Procedure \"P_ExecChaseTasks\" Fail.");
                    log.Write("第二次执行：Exec ExecChaseTasks: Procedure \"P_ExecChaseTasks\" Fail.");

                    return;
                }
            }

            if (ReturnValue < 0)
            {
                msg.Send("Exec ExecChaseTasks: Procedure \"P_ExecChaseTasks\" Return: " + ReturnDescription);
                log.Write("Exec ExecChaseTasks: Procedure \"P_ExecChaseTasks\" Return: " + ReturnDescription);
            }
        }

        private void SchemeAssure()     //临近系统结束时间使用保底使方案满员
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            if (DAL.Procedures.P_SchemeAssure(ConnectionString, ref ReturnValue, ref ReturnDescription) < 0)
            {
                msg.Send("Exec SchemeAssureTask: Procedure \"P_SchemeAssure\" Fail.");
                log.Write("Exec SchemeAssureTask: Procedure \"P_SchemeAssure\" Fail.");

                return;
            }

            if (ReturnValue < 0)
            {
                msg.Send("Exec SchemeAssureTask: Procedure \"P_SchemeAssure\" Return: " + ReturnDescription);
                log.Write("Exec SchemeAssureTask: Procedure \"P_SchemeAssure\" Return: " + ReturnDescription);
            }
        }

        private void CheckChaseTasks()    // 检测追号过期未执行的任务，以撤消此明细任务。
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            if (DAL.Procedures.P_CheckChaseTasks(ConnectionString, ref ReturnValue, ref ReturnDescription) < 0)
            {
                msg.Send("Exec CheckChaseTasks: Procedure \"P_CheckChaseTasks\" Fail.");
                log.Write("Exec CheckChaseTasks: Procedure \"P_CheckChaseTasks\" Fail.");

                return;
            }

            if (ReturnValue < 0)
            {
                msg.Send("Exec CheckChaseTasks: Procedure \"P_CheckChaseTasks\" Return: " + ReturnDescription);
                log.Write("Exec CheckChaseTasks: Procedure \"P_CheckChaseTasks\" Return: " + ReturnDescription);
            }
        }

        private void ExecChases()
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            if (DAL.Procedures.P_ExecChases(ConnectionString, ref ReturnValue, ref ReturnDescription) < 0)
            {
                msg.Send("Exec ExecChases: Procedure \"P_ExecChases\" Fail.");
                log.Write("Exec ExecChases: Procedure \"P_ExecChases\" Fail.");

                return;
            }

            if (ReturnValue < 0)
            {
                msg.Send("Exec ExecChases: Procedure \"P_ExecChases\" Return: " + ReturnDescription);
                log.Write("Exec ExecChases: Procedure \"P_ExecChases\" Return: " + ReturnDescription);
            }
        }               //执行追号套餐

        private void QuashSchemeNoLotteryNumber()   //足彩预投中没有上传投注内容的方案进行撤单
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            if (DAL.Procedures.P_QuashSchemeNoLotteryNumber(ConnectionString, ref ReturnValue, ref ReturnDescription) < 0)
            {
                msg.Send("Exec QuashSchemeNoLotteryNumber: Procedure \"P_QuashSchemeNoLotteryNumber\" Fail.");
                log.Write("Exec QuashSchemeNoLotteryNumber: Procedure \"P_QuashSchemeNoLotteryNumber\" Fail.");

                return;
            }

            if (ReturnValue < 0)
            {
                msg.Send("Exec QuashSchemeNoLotteryNumber: Procedure \"P_QuashSchemeNoLotteryNumber\" Return: " + ReturnDescription);
                log.Write("Exec QuashSchemeNoLotteryNumber: Procedure \"P_QuashSchemeNoLotteryNumber\" Return: " + ReturnDescription);
            }
        }

        private void CheckLotteryNumberCountForCustomChase()//检查追号套餐中随机选号的号码是否少
        {
            DataTable dt = Shove.Database.MSSQL.Select(ConnectionString, "select ID,LotteryID,Nums,Counts from (select ID,LotteryID,Nums,Cast(Money as int)/(Multiple*Nums*Price) - (select count(ID) as NumberCount from T_ChaseLotteryNumber where ChaseID = a.ID) as Counts from T_Chases a where BetType = 1 and QuashStatus = 0 and Money > 0)a where Counts > 0");

            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            int LotteryID = 0;
            string lotteryNumber;
            int IsuseCount = 0;
            int Nums = 0;
            foreach (DataRow dr in dt.Rows)
            {
                LotteryID = Shove._Convert.StrToInt(dr["LotteryID"].ToString(), 0);
                IsuseCount = Shove._Convert.StrToInt(dr["Counts"].ToString(), 0);
                Nums = Shove._Convert.StrToInt(dr["Nums"].ToString(), 0);

                for (int i = 0; i < IsuseCount; i++)
                {
                    if (LotteryID == 5)
                    {
                        lotteryNumber = new Lottery()[5].BuildNumber(6, 1, Nums);
                    }
                    else if (LotteryID == 39)
                    {
                        lotteryNumber = new Lottery()[39].BuildNumber(5, 2, Nums);
                    }
                    else
                    {
                        lotteryNumber = new Lottery()[LotteryID].BuildNumber(Nums);
                    }

                    DAL.Tables.T_ChaseLotteryNumber t = new DAL.Tables.T_ChaseLotteryNumber();

                    t.ChaseID.Value = dr["ID"].ToString();
                    t.LotteryNumber.Value = lotteryNumber;

                    t.Insert(ConnectionString);
                }
            }
        }

        #endregion
    }
}
